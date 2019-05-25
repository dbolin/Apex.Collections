using Apex.Collections;
using Apex.Collections.Immutable;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;
using BenchmarkDotNet.Validators;
using Sasa.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Benchmarks
{
    public class Config : ManualConfig
    {
        public Config()
        {
            Add(JitOptimizationsValidator.DontFailOnError);
            Add(DefaultConfig.Instance.GetLoggers().ToArray()); // manual config has no loggers by default
            Add(DefaultConfig.Instance.GetExporters().ToArray()); // manual config has no exporters by default
            Add(DefaultConfig.Instance.GetColumnProviders().ToArray()); // manual config has no columns by default

            Add(Job.Core.With(CsProjCoreToolchain.NetCoreApp30).WithGcServer(true));
            //Add(Job.Core.With(CsProjCoreToolchain.NetCoreApp22).WithGcServer(true));
            //Add(Job.Clr.With(CsProjClassicNetToolchain.Net472));
            //Add(Job.CoreRT);
            //Add(HardwareCounter.BranchMispredictions, HardwareCounter.BranchInstructions, HardwareCounter.CacheMisses, HardwareCounter.LlcMisses);

            Add(MemoryDiagnoser.Default);
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


            var summaries = BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).Run(config: new Config());
        }

        static void Test()
        {
            var x = new PriorityQueue { Count = 10000 };
            x.Init();

            while (true)
            {
                x.PQApexAddHighest();
            }
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
