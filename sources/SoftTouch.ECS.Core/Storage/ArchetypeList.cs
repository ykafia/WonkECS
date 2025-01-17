namespace SoftTouch.ECS.Storage;



public record struct ArchetypeItem(ArchetypeID Key, Archetype Value);

public class ArchetypeList
{
    public ArchetypeListKeys Keys => new(this);
    public ArchetypeListValues Values => new(this);
    public int Count => list.Count;
    readonly List<ArchetypeItem> list = [];
    readonly Dictionary<ArchetypeID, int> lookup = [];

    public Archetype this[ArchetypeID id]
    {
        get => list[lookup[id]].Value;
        set
        {
            if (ContainsKey(id))
            {
                list[lookup[id]] = list[lookup[id]] with { Value = value };
            }
            else
            {
                list.Add(new(id, value));
                lookup.Add(id, list.Count - 1);
            }
        }
    }

    public bool Has(RefArchetypeID id)
    {
        foreach (var key in Keys)
            if (id == key)
                return true;
        return false;
    }


    public bool TryGetValue(ArchetypeID id, out Archetype arch)
    {
        var result = lookup.TryGetValue(id, out var index);
        if (!result)
        {
            arch = null!;
            return false;
        }
        else
        {
            arch = list[index].Value;
            return true;
        }
    }


    public bool ContainsKey(ArchetypeID Key)
        => lookup.ContainsKey(Key);

    public void Add(ArchetypeID key, Archetype value)
    {
        list.Add(new(key, value));
        lookup.Add(key, list.Count - 1);
    }

    public Enumerator GetEnumerator() => new(this);

    public ref struct Enumerator(ArchetypeList list)
    {
        List<ArchetypeItem>.Enumerator enumerator = list.list.GetEnumerator();
        public ArchetypeItem Current => enumerator.Current;
        public bool MoveNext() => enumerator.MoveNext();
    }

    public readonly ref struct ArchetypeListKeys(ArchetypeList list)
    {
        readonly ArchetypeList list = list;

        public Enumerator GetEnumerator() => new(list);

        public ref struct Enumerator(ArchetypeList list)
        {
            List<ArchetypeItem>.Enumerator enumerator = list.list.GetEnumerator();
            public ArchetypeID Current => enumerator.Current.Key;
            public bool MoveNext() => enumerator.MoveNext();

        }
    }
    public readonly ref struct ArchetypeListValues(ArchetypeList list)
    {
        readonly ArchetypeList list = list;

        public Archetype this[int index] => list.list[index].Value;

        public Enumerator GetEnumerator() => new(list);

        public ref struct Enumerator(ArchetypeList list)
        {
            List<ArchetypeItem>.Enumerator enumerator = list.list.GetEnumerator();
            public Archetype Current => enumerator.Current.Value;
            public bool MoveNext() => enumerator.MoveNext();
        }
    }
}