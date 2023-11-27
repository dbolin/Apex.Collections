using Apex.Collections.Immutable;
using Apex.Runtime;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
//using BenchmarkDotNet.Horology;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;
using BenchmarkDotNet.Validators;
using Perfolizer.Horology;
using Sasa.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TunnelVisionLabs.Collections.Trees.Immutable;

namespace Benchmarks
{
    public class Config : ManualConfig
    {
        public Config()
        {
            AddValidator(JitOptimizationsValidator.DontFailOnError);
            AddLogger(DefaultConfig.Instance.GetLoggers().ToArray()); // manual config has no loggers by default
            AddExporter(DefaultConfig.Instance.GetExporters().ToArray()); // manual config has no exporters by default
            AddColumnProvider(DefaultConfig.Instance.GetColumnProviders().ToArray()); // manual config has no columns by default

            AddJob(Job.Default.WithToolchain(CsProjCoreToolchain.NetCoreApp80).WithGcServer(true).WithIterationTime(new Perfolizer.Horology.TimeInterval(250, TimeUnit.Millisecond)).WithMaxIterationCount(30));
            //Add(Job.Core.With(CsProjCoreToolchain.NetCoreApp22).WithGcServer(true));
            //Add(Job.Clr.With(CsProjClassicNetToolchain.Net472));
            //Add(Job.CoreRT);
            //Add(HardwareCounter.BranchMispredictions, HardwareCounter.BranchInstructions, HardwareCounter.CacheMisses, HardwareCounter.LlcMisses);

            AddDiagnoser(MemoryDiagnoser.Default);
        }
    }

    public sealed class T
    {
    }

    class Program
    {
        static void Main(string[] args)
        {
            /*
            Task.Run(() =>
            {
                Test();
            });

            Test();
            */

            //Test();

            //TestSizes();

            var summaries = BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).Run(config: new Config());
        }

        private static void TestSizes()
        {
            var count = 10000;
            var rn = new Random(4);
            var a = new List<int>();
            for (int i = 0; i < count; ++i)
            {
                a.Add(i);
            }

            int n = count;
            while (n > 1)
            {
                n--;
                int k = rn.Next(n + 1);
                var value = a[k];
                a[k] = a[n];
                a[n] = value;
            }

            var x = new List<object>();
            //var d = ImmutableTrieDictionary.Create<int, int>();
            //var d = ImmutableDictionary<int, int>.Empty;
            //var d = HashMap<int, int>.Empty;
            var d = ImmutableTreeDictionary<int, int>.Empty;
            for (int i = 0; i < 10000; ++i)
            {
                d = d.SetItem(a[i], a[i]);
                //if (i % 1000 == 0)
                {
                    x.Add(d);
                }
            }

            var r = new Memory(Memory.Mode.Detailed);
            var s = r.DetailedSizeOf(x);
            Console.WriteLine(s.TotalSize);
        }

        static void Test()
        {
            /*
            var x = new DictionariesBuilderAdd<int>();
            x.Count = 5;
            x.Init2();

            while (true)
            {
                x.ApexHashMap();
            }
            */
        }

        static void Test2()
        {
            var r = new Random();
            var i = 1;
            var t = HashMap<string, int>.Empty.WithComparer(Apex.Collections.StringComparer.NonRandomOrdinalIgnoreCase);
            var t2 = new Dictionary<string, int>();

            while (true)
            {
                var s = $"t{i}";

                if (r.Next(0, 100) < 50)
                {
                    t = t.SetItem(s, i);
                    if (!t2.ContainsKey(s))
                    {
                        t2.Add(s, i);
                    }

                    i++;
                }
                else
                {
                    t = t.Remove(s);
                    t2.Remove(s);
                }

                if (t.Count != t2.Count)
                {
                    throw new InvalidOperationException();
                }

                foreach (var kvp in t2)
                {
                    t.TryGetValue(kvp.Key, out var res);

                    if (res != kvp.Value)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }
    }
}
