using Apex.Collections.Immutable;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;
using BenchmarkDotNet.Validators;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

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

            Add(Job.Core.With(CsProjCoreToolchain.NetCoreApp30).WithGcServer(false));
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
            var x = new LRUCache();
            x.Capacity = 1000;
            x.HitRate = 95;

            while (true)
            {
                var lru = x.LRUCacheST();
            }
            */
            /*
            var sw = new Stopwatch();

            var x = HashMap<int, int>.Empty;
            for (int i = 0; i < 1000000; ++i)
            {
                x = x.SetItem(i, i);
            }

            for (int j = 0; j < 100000; ++j)
            {
                sw.Restart();
                //while(true)
                {
                    for (int i = 0; i < 1000000; ++i)
                    {
                        x.TryGetValue(i, out _);
                    }
                }
                sw.Stop();

                if (sw.Elapsed.TotalMilliseconds > 3)
                {
                    Console.WriteLine("{0}: {1}", j, sw.Elapsed.TotalMilliseconds);
                }
            }

            return;
            */

            var summaries = BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).Run(config: new Config());
        }
    }
}
