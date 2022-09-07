using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ECSharp.Arrays;
using ECSharp.ComponentData;

namespace ECSharp
{
    public class World
    {
        public Dictionary<long, ArchetypeRecord> Entities = new();

        public SortedList<ArchetypeID, Archetype> Archetypes = new(new ArchetypeIDComparer());
        public List<ProcessorBase> Processors { get; set; } = new();

        public bool IsRunning { get; private set; }

        public long FrameCount { get; private set; } = 0;

        UpdateQueue UpdateQueue = new();

        public ArchetypeRecord this[long id]
        {
            get { return Entities[id]; }
            set { Entities[id] = value; }
        }
        public ArchetypeRecord this[Entity e]
        {
            get { return Entities[e.Index]; }
            set { Entities[e.Index] = value; }
        }

        public World()
        {
            Archetypes.Add(new(), Archetype.Empty);
        }

        public EntityBuilder CreateEntity(string name = "")
        {
            var e = new EntityBuilder(new Entity(Entities.Count, this, name));
            Archetype.Empty.AddEntity(e.Entity);
            Entities[e.Entity.Index] = new ArchetypeRecord { Entity = e.Entity, Archetype = Archetype.Empty };
            return e;
        }

        public ArchetypeRecord GetOrCreateRecord(ArchetypeID types, EntityBuilder e)
        {
            if (Archetypes.TryGetValue(types, out Archetype? a) && e.Entity != null)
            {
                return new ArchetypeRecord { Entity = e.Entity, Archetype = a };
            }
            else
            {
                throw new NotImplementedException("Cannot generate record");
            }
        }

        internal Archetype GenerateArchetype(ArchetypeID types, IEnumerable<ComponentList> components)
        {
            if (!Archetypes.ContainsKey(types))
            {
                Archetypes.Add(types, new Archetype(components));
                return Archetypes[types];
            }
            else
                return Archetypes[types];
        }
        internal Archetype GenerateArchetype(ArchetypeID types, List<ComponentBase> components)
        {
            if (!Archetypes.ContainsKey(types))
            {
                Archetypes.Add(types, new Archetype(components));
                return Archetypes[types];
            }
            else
                return Archetypes[types];
        }

        public void BuildGraph()
        {
            var stor = Archetypes.Values;
            foreach (var arch in Archetypes.Values)
            {
                foreach (var x in stor
                    .Where(x => x.ID.IsAddedType(arch.ID))
                    .Select(other => (arch.TypeExcept(other).First(), other)))
                {
                    arch.Edges.Add[x.Item1] = x.other;
                }
                foreach (var x in stor
                    .Where(x => x.ID.IsRemovedType(arch.ID))
                    .Select(other => (other.TypeExcept(arch).First(), other))
                )
                {
                    arch.Edges.Remove[x.Item1] = x.other;
                }
            }
        }

        public IEnumerable<Archetype> QueryArchetypes(ArchetypeID types)
        {
            // return Archetypes
            //     .Where(arch => arch.Value.ID.IsSupersetOf(types))
            //     .Select(arch => arch.Value)
            //     .AsSpan();
            throw new NotImplementedException();
        }
        public IEnumerable<Archetype> QueryArchetypes(params Type[] types)
        {
            return Archetypes
                .Where(arch => arch.Value.ID.IsSupersetOf(new ArchetypeID(types.ToHashSet())))
                .Select(arch => arch.Value);
        }

        public void Add(ProcessorBase p)
        {
            p.World = this;
            Processors.Add(p);
        }
        public void Add<T>() where T : ProcessorBase, new()
        {
            var p = new T
            {
                World = this
            };
            Processors.Add(p);
        }
        public void Remove(Processor p) => Processors.Remove(p);


        public void AddArchetypeUpdate(ComponentUpdate update)
        {
            UpdateQueue.Enqueue(update);
        }


        public void Start()
        {
            foreach (ProcessorAsync pa in Processors.Where(p => p is ProcessorAsync))
                pa.Execute();
        }
        public void Update()
        {
            foreach (Processor p in Processors.Where(p => p is Processor))
                p.Update();
            UpdateQueue.ExecuteUpdates();
            FrameCount += 1;
        }

        public void Run(int framesToRun = 0)
        {
            IsRunning = true;
            Start();
            while (IsRunning)
            {
                Update();
                if (framesToRun != 0 && FrameCount >= framesToRun)
                    IsRunning = false;
            }
            IsRunning = false;
        }


        internal class ArchetypeIDComparer : Comparer<ArchetypeID>
        {
            public override int Compare(ArchetypeID x, ArchetypeID y)
            {
                if (x.Id.CompareTo(y.Id) != 0)
                {
                    return x.Count.CompareTo(y.Count);
                }
                else if (x.Count.CompareTo(y.Count) != 0)
                {
                    return x.Count.CompareTo(y.Count);
                }
                else
                {
                    return 0;
                }
            }
        }
        // public void Add(Entity entity) => Entities.Add(entity,new ArchetypeRecord{Archetype = new Archetype(entity.Archetype)});
        // public void Remove(Entity entity) => Entities.Remove(entity);

    }
}