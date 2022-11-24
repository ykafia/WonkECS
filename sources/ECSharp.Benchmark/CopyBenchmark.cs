using System;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using ECSharp.Components;
using System.Linq;
using ECSharp.Processors;
using ECSharp.Arrays;
using System.Collections.Generic;

namespace ECSharp.Benchmark;


[MemoryDiagnoser]
[SimpleJob(launchCount: 3, warmupCount: 10, targetCount: 15)]
public class CopyBench
{
    public class Person : ICloneable
    {
        public int Age {get;set;}
        public float Height {get;set;}

        public Person(int a, float h)
        {
            Age = a;
            Height = h;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
    public ComponentList<HealthComponent> comps1 = new(Size);
    public List<Person> compo1 = new(Size);

    public ComponentList<HealthComponent> comps2 = new(Size);
    public List<Person> compo2 = new(Size);

    static int Size = 10;

    public CopyBench()
    {
        for (int i = 0; i < Size; i++)
        {
            comps1.Add(new(i,i));
            compo1.Add(new(i,i));
        }
    }
    [Benchmark]
    public void CopyRangeObjects()
    {
        compo2.AddRange(compo1.Select(x => x.Clone()).Cast<Person>());
    }
    [Benchmark]
    public void CopyRangeStructs()
    {
        comps1.AddRange(comps1);
    }
}