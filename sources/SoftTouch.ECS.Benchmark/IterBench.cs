﻿using System;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using SoftTouch.ECS.Shared.Processors;
using SoftTouch.ECS.Shared.Components;
using System.Linq;

namespace SoftTouch.ECS.Benchmark
{
    [MemoryDiagnoser]
    [SimpleJob(launchCount: 3, warmupCount: 10, targetCount: 15)]
    public class IterBench
    {

        public World W1 = new();
        public World W2 = new();
        public World W3 = new();

        ArchetypeID aid = new(typeof(HealthComponent));

        static int Size = 32;

        public IterBench()
        {
            for (int i = 0; i < Size; i++)
            {
                W1.CreateEntity()
                    .With<HealthComponent>();
                W1.CreateEntity()
                    .With<NameComponent>()
                    .With<HealthComponent>();
                W1.CreateEntity()
                    .With<NameComponent>()
                    .With<HealthComponent>()
                    .With<ModelComponent>();
                W1.CreateEntity()
                    .With<int>()
                    .With<HealthComponent>()
                    .With<uint>();
                W1.CreateEntity()
                    .With<int>()
                    .With<HealthComponent>();
                W1.CreateEntity()
                    .With<float>()
                    .With<HealthComponent>()
                    .With<uint>();
                W1.CreateEntity()
                    .With<float>()
                    .With<HealthComponent>()
                    .With<double>();
                W1.CreateEntity()
                    .With<float>()
                    .With<HealthComponent>();
            }
        }
        [Benchmark]
        public void DoNothing()
        {
            
        }
        [Benchmark]
        public void IterForeach()
        {
            foreach(var a in W1.QueryArchetypes(aid))
            {
                var x = 0;
                // if(a.HasEntities)
                //     a.GetComponentList<HealthComponent>()[0] = new(127,127);
            }
        }

        [Benchmark]
        public void IterForEachToList()
        {
            var l = W1.QueryArchetypes(aid).ToList();
            foreach(Archetype a in l)
            {
                var x = 0;
                // if(a.HasEntities)
                //     a.GetComponentList<HealthComponent>()[0] = new(128,128);
            }
        }

        [Benchmark]
        public void IterForToList()
        {
            var l = W1.QueryArchetypes(aid).ToList();
            for(int i = 0; i < l.Count; i ++)
            {
                var x = 0;
                // if(l[i].HasEntities)
                //     l[i].GetComponentList<HealthComponent>()[0] = new(129,129);
            }
        }
        
    }
}
