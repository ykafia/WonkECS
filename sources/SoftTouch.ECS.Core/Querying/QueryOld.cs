using System;
using System.Collections;
using System.Collections.Generic;
using SoftTouch.ECS.Arrays;
using SoftTouch.ECS.Processors;
using SoftTouch.ECS.Storage;

namespace SoftTouch.ECS;

public abstract class Query : IQuery
{
    protected World world;
    public ArchetypeID ID;
    //protected WorldCommands Commands => world.Commands;

    public virtual int Length
    {
        get
        {
            var sum = 0;
            for (int i = 0; i < world.Archetypes.Values.Count; i++)
                if (world.Archetypes.Values[i].IsSupersetOf(ID.Span))
                    sum += world.Archetypes.Values[i].Length;
            return sum;
        }
    }

    public Query() { }
    public Query(World w)
    {
        world = w;
    }

    public virtual Query With(World w)
    {
        world = w;
        return this;
    }
    public void WithWorld(World w)
    {
        world = w;
    }
}




public class Res<T> : Query, IQuery
    where T : class
{
    public Res()
    {
    }

    public static implicit operator T(Res<T> r) => r.world.Resources.Get<T>();

    public override Res<T> With(World w)
    {
        world = w;
        ID = new(typeof(T));
        return (Res<T>)this;
    }
}



public class Query<T> : Query, IQuery
    where T : struct, IEquatable<T>
{
    public Query()
    {
    }
    public Query(World w) : base(w)
    {
    }

    public QueryEnumerator<T> GetEnumerator() => new(world);


    public override Query<T> With(World w)
    {
        world = w;
        ID = new(typeof(T));
        return (Query<T>)this;
    }


}
public class Query<T1, T2> : Query, IQuery
    where T1 : struct, IEquatable<T1>
    where T2 : struct, IEquatable<T2>
{
    public Query()
    {
    }

    public QueryEnumerator<T1, T2> GetEnumerator() => new(world);

    public override Query<T1, T2> With(World w)
    {
        world = w;
        ID = new ArchetypeID(typeof(T1), typeof(T2));
        return this;
    }
}
public class Query<T1, T2, T3> : Query, IQuery
    where T1 : struct, IEquatable<T1>
    where T2 : struct, IEquatable<T2>
    where T3 : struct, IEquatable<T3>
{
    public QueryEnumerator<T1, T2, T3> GetEnumerator() => new(world);


    public override Query<T1, T2, T3> With(World w)
    {
        world = w;
        ID = new ArchetypeID(typeof(T1), typeof(T2), typeof(T3));
        return this;
    }
}
public class Query<T1, T2, T3, T4> : Query, IQuery
    where T1 : struct, IEquatable<T1>
    where T2 : struct, IEquatable<T2>
    where T3 : struct, IEquatable<T3>
    where T4 : struct, IEquatable<T4>
{
    public QueryEnumerator<T1, T2, T3, T4> GetEnumerator() => new(world);


    public override Query<T1, T2, T3, T4> With(World w)
    {
        world = w;
        ID = new ArchetypeID(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        return this;
    }
}
public class Query<T1, T2, T3, T4, T5> : Query, IQuery
    where T1 : struct, IEquatable<T1>
    where T2 : struct, IEquatable<T2>
    where T3 : struct, IEquatable<T3>
    where T4 : struct, IEquatable<T4>
    where T5 : struct, IEquatable<T5>
{
    public QueryEnumerator<T1, T2, T3, T4, T5> GetEnumerator() => new(world);

    public override Query<T1, T2, T3, T4, T5> With(World w)
    {
        world = w;
        ID = new ArchetypeID(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
        return this;
    }

}
public class Query<T1, T2, T3, T4, T5, T6> : Query, IQuery
    where T1 : struct, IEquatable<T1>
    where T2 : struct, IEquatable<T2>
    where T3 : struct, IEquatable<T3>
    where T4 : struct, IEquatable<T4>
    where T5 : struct, IEquatable<T5>
    where T6 : struct, IEquatable<T6>
{
    public QueryEnumerator<T1, T2, T3, T4, T5, T6> GetEnumerator() => new(world);

    public override Query<T1, T2, T3, T4, T5, T6> With(World w)
    {
        world = w;
        ID = new ArchetypeID(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
        return this;
    }
}
public class Query<T1, T2, T3, T4, T5, T6, T7> : Query, IQuery
    where T1 : struct, IEquatable<T1>
    where T2 : struct, IEquatable<T2>
    where T3 : struct, IEquatable<T3>
    where T4 : struct, IEquatable<T4>
    where T5 : struct, IEquatable<T5>
    where T6 : struct, IEquatable<T6>
    where T7 : struct, IEquatable<T7>
{
    public QueryEnumerator<T1, T2, T3, T4, T5, T6, T7> GetEnumerator() => new(world);

    public override Query<T1, T2, T3, T4, T5, T6, T7> With(World w)
    {
        world = w;
        ID = new ArchetypeID(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
        return this;
    }
}
public class Query<T1, T2, T3, T4, T5, T6, T7, T8> : Query, IQuery
    where T1 : struct, IEquatable<T1>
    where T2 : struct, IEquatable<T2>
    where T3 : struct, IEquatable<T3>
    where T4 : struct, IEquatable<T4>
    where T5 : struct, IEquatable<T5>
    where T6 : struct, IEquatable<T6>
    where T7 : struct, IEquatable<T7>
    where T8 : struct, IEquatable<T8>
{
    public QueryEnumerator<T1, T2, T3, T4, T5, T6, T7, T8> GetEnumerator() => new(world);

    public override Query<T1, T2, T3, T4, T5, T6, T7, T8> With(World w)
    {
        world = w;
        ID = new ArchetypeID(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
        return this;
    }
}