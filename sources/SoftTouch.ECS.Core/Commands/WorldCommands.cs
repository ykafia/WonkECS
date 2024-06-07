using CommunityToolkit.HighPerformance.Buffers;
using SoftTouch.ECS.Arrays;
using SoftTouch.ECS.Storage;

namespace SoftTouch.ECS;

public partial class WorldCommands(World world)
{
    readonly World world = world;
    internal List<EntityUpdate> Updates { get; } = [];


    public void AddComponent<T>(in Entity e, in T component) where T : struct
    {
        foreach (var u in Updates)
        {
            if (u.Entity == e)
            {
                u.AddedComponents.Add(ComponentBox<T>.Create(component));
                return;
            }
        }
        var update = new ArchUpdate(e);
        update.AddedComponents.Add(ComponentBox<T>.Create(component));
        Updates.Add(update);
    }
    public void RemoveComponent<T>(in Entity e) where T : struct
    {
        foreach (var u in Updates)
        {
            if (u.Entity == e)
            {
                u.RemovedComponents.Add(ComponentBox<T>.Create(default));
                return;
            }
        }
        var update = new ArchUpdate(e);
        update.RemovedComponents.Add(ComponentBox<T>.Create(default));
        Updates.Add(update);
    }

    public void ExecuteUpdates()
    {
        foreach (var update in Updates)
        {
            if (update is SpawnEntity spawn)
            {
                // Find the archetype
                Archetype arch = null!;
                foreach (var a in world.Archetypes.Values)
                {
                    arch = a;
                    foreach (var t in spawn.AddedComponents.Span)
                        if (!a.ID.Contains(t.ComponentType))
                        {
                            arch = null!;
                            break;
                        }
                    if (arch != null)
                        break;
                }
                if (arch is null)
                {
                    var idArr = new Type[spawn.AddedComponents.Length];
                    for (int i = 0; i < spawn.AddedComponents.Length; i++)
                        idArr[i] = spawn.AddedComponents.Span[i].ComponentType;
                    var aid = new ArchetypeID(idArr);
                    world.Archetypes.Add(aid, new Archetype(aid, spawn.AddedComponents, world));
                    arch = world.Archetypes[aid];

                    world.Entities.ReservedIds.Remove(spawn.Entity);

                    var meta = new EntityMeta() { Entity = spawn.Entity, Location = new(arch, 0) };
                    world.Entities.Meta.Add(meta);
                    arch.AddEntity(spawn.Entity.Index);
                    
                }
                else
                {
                    // Does our world have the archetype we need?

                    // Add the meta data

                    // First Step remove the reserved ids since we're spawing the entity again
                    world.Entities.ReservedIds.Remove(spawn.Entity);

                    // If the size of the entities is 0, Add the meta information
                    if (world.Entities.Meta.Count == 0)
                    {
                        var meta = new EntityMeta() { Entity = spawn.Entity, Location = new(arch, 0) };
                        world.Entities.Meta.Add(meta);
                        arch.AddEntity(spawn.Entity.Index);
                        foreach (var comp in spawn.AddedComponents.Span)
                            arch.Storage[comp.ComponentType].TryAdd(comp);
                    }
                    // Else check if the entity id already exists and increment its generation
                    else if (spawn.Entity.Index < world.Entities.Meta.Count)
                        world.Entities.Meta[spawn.Entity] = world.Entities.Meta[spawn.Entity] with { Entity = spawn.Entity };
                    // Else add the entity
                    else if (world.Entities.Meta.Count == spawn.Entity.Index)
                    {
                        var meta = new EntityMeta() { Entity = spawn.Entity, Location = new(arch, arch.Length) };
                        world.Entities.Meta.Add(meta);
                        arch.AddEntity(spawn.Entity.Index);
                        foreach (var comp in spawn.AddedComponents.Span)
                            arch.Storage[comp.ComponentType].TryAdd(comp);
                    }
                    else throw new Exception("Entity index were skipped");
                }


            }
            else if (update is DespawnEntity despawn)
            {
                // Remove all components
                var location = world.Entities[despawn.Entity].Location;
                location.Archetype.RemoveEntity(despawn.Entity);
                // Put the entity to FreeIds                
                world.Entities.SetFree(despawn.Entity);
            }
            else if (update is ArchUpdate archUpdate)
            {
                var oldArch = world.Entities[archUpdate.Entity].Location.Archetype;
                // Create the list of components
                using ReusableList<ComponentBox> comps = [];
                foreach (var t in oldArch.ID.Types)
                {
                    bool toAdd = true;
                    foreach (var e in update.RemovedComponents.Span)
                        if (t == e.ComponentType)
                            toAdd = false;
                    if (toAdd && oldArch.GetComponent(update.Entity, t, out var c))
                        comps.Add(c);
                    else
                        throw new Exception("Could not add component");
                }
                foreach (var r in archUpdate.AddedComponents.Span)
                    comps.Add(r);


                Archetype newArch = null!;
                foreach (var a in world.Archetypes.Values)
                {
                    newArch = a;
                    foreach (var t in comps.Span)
                        if (!a.ID.Contains(t.ComponentType))
                        {
                            newArch = null!;
                            break;
                        }
                    if (newArch != null)
                        break;
                }
                if (newArch is null)
                {

                    // Create the archetype id
                    var idArr = new Type[comps.Length];
                    for (int i = 0; i < comps.Length; i++)
                        idArr[i] = comps.Span[i].ComponentType;
                    var aid = new ArchetypeID(idArr);
                    newArch = new Archetype(aid, comps, world);
                    world.Archetypes.Add(aid, newArch);
                }

                // Remove all components
                oldArch.RemoveEntity(archUpdate.Entity);


                world.Entities.Meta[archUpdate.Entity.Index] =
                    world.Entities.Meta[archUpdate.Entity.Index]
                    with
                    {
                        Location = new(newArch, 0)
                    };
                newArch.AddEntity(update.Entity.Index);
            }
            update.Dispose();
        }
        Updates.Clear();
    }
}