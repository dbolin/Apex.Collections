
# Benchmarked Collections

|                    Name |                                              Source|
| ----------------------- | -------------------------------------------------- |
|     ImmutableDictionary |                                        .NET Core 3 |
|                SasaTrie |                         Sasa.Collections 1.0.0-RC4 |
| ImmutableTrieDictionary |                          ImmutableTrie 1.0.0-alpha |
| ImmutableTreeDictionary |  TunnelVisionLabs.Collections.Trees 1.0.0-alpha.74 |
|             ApexHashMap |                                    this Repository |

```
BenchmarkDotNet=v0.11.5, OS=Windows 10.0.18362
Intel Core i5-4690 CPU 3.50GHz (Haswell), 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=3.0.100-preview8-013656
  [Host] : .NET Core 3.0.0-preview8-28405-07 (CoreCLR 4.700.19.37902, CoreFX 4.700.19.40503), 64bit RyuJIT
  Core   : .NET Core 3.0.0-preview8-28405-07 (CoreCLR 4.700.19.37902, CoreFX 4.700.19.40503), 64bit RyuJIT

Job=Core  Runtime=Core  Server=True
Toolchain=.NET Core 3.0
```

# Integer key

## Add


|                  Method | Count |            Mean |          Error |         StdDev | Ratio | RatioSD |     Gen 0 |    Gen 1 | Gen 2 |  Allocated |
|------------------------ |------ |----------------:|---------------:|---------------:|------:|--------:|----------:|---------:|------:|-----------:|
|     ImmutableDictionary |     5 |      1,035.7 ns |       9.004 ns |       8.422 ns |  1.00 |    0.00 |    0.0542 |        - |     - |      872 B |
|                SasaTrie |     5 |        253.0 ns |       2.139 ns |       2.001 ns |  0.24 |    0.00 |    0.0282 |        - |     - |      440 B |
| ImmutableTrieDictionary |     5 |        363.0 ns |       3.574 ns |       3.168 ns |  0.35 |    0.00 |    0.0473 |        - |     - |      728 B |
| ImmutableTreeDictionary |     5 |      1,128.1 ns |       9.981 ns |       8.848 ns |  1.09 |    0.01 |    0.1439 |        - |     - |     2240 B |
|             ApexHashMap |     5 |        183.9 ns |       1.789 ns |       1.585 ns |  0.18 |    0.00 |    0.0447 |        - |     - |      680 B |
|                         |       |                 |                |                |       |         |           |          |       |            |
|     ImmutableDictionary |   100 |     49,129.4 ns |     308.061 ns |     273.088 ns |  1.00 |    0.00 |    2.5312 |        - |     - |    39504 B |
|                SasaTrie |   100 |     12,303.1 ns |     257.804 ns |     241.150 ns |  0.25 |    0.01 |    2.6897 |        - |     - |    41456 B |
| ImmutableTrieDictionary |   100 |     19,159.8 ns |     222.450 ns |     185.755 ns |  0.39 |    0.00 |    2.7174 |        - |     - |    41936 B |
| ImmutableTreeDictionary |   100 |     92,609.3 ns |     793.721 ns |     742.447 ns |  1.88 |    0.02 |    4.4118 |        - |     - |    65640 B |
|             ApexHashMap |   100 |      8,949.0 ns |     102.612 ns |      90.963 ns |  0.18 |    0.00 |    2.1465 |        - |     - |    32384 B |
|                         |       |                 |                |                |       |         |           |          |       |            |
|     ImmutableDictionary | 10000 | 12,097,449.0 ns | 258,426.883 ns | 215,798.152 ns |  1.00 |    0.00 |  454.5455 |  90.9091 |     - |  7713376 B |
|                SasaTrie | 10000 |  3,574,270.0 ns |  59,323.120 ns |  55,490.883 ns |  0.30 |    0.01 | 1014.4928 | 115.9420 |     - | 15333552 B |
| ImmutableTrieDictionary | 10000 |  4,558,248.2 ns | 108,672.790 ns | 162,656.315 ns |  0.37 |    0.02 |  520.8333 | 104.1667 |     - |  8084512 B |
| ImmutableTreeDictionary | 10000 | 25,530,520.0 ns | 136,824.400 ns | 127,985.629 ns |  2.11 |    0.04 |  600.0000 | 100.0000 |     - |  9649888 B |
|             ApexHashMap | 10000 |  2,690,112.2 ns |  43,520.302 ns |  40,708.918 ns |  0.22 |    0.01 |  510.4167 |  83.3333 |     - |  7707952 B |

## Remove

|                  Method | Count |            Mean |          Error |         StdDev | Ratio | RatioSD |     Gen 0 |    Gen 1 | Gen 2 |  Allocated |
|------------------------ |------ |----------------:|---------------:|---------------:|------:|--------:|----------:|---------:|------:|-----------:|
|     ImmutableDictionary |     5 |        646.1 ns |       5.932 ns |       5.548 ns |  1.00 |    0.00 |    0.0308 |        - |     - |      496 B |
|                SasaTrie |     5 |        246.3 ns |       3.440 ns |       3.218 ns |  0.38 |    0.01 |    0.0238 |        - |     - |      376 B |
| ImmutableTrieDictionary |     5 |        319.0 ns |       2.633 ns |       2.463 ns |  0.49 |    0.01 |    0.0369 |        - |     - |      568 B |
| ImmutableTreeDictionary |     5 |        984.5 ns |      14.141 ns |      13.228 ns |  1.52 |    0.03 |    0.1267 |        - |     - |     1928 B |
|             ApexHashMap |     5 |        171.1 ns |       1.467 ns |       1.372 ns |  0.26 |    0.00 |    0.0406 |        - |     - |      616 B |
|                         |       |                 |                |                |       |         |           |          |       |            |
|     ImmutableDictionary |   100 |     38,553.4 ns |     241.883 ns |     188.846 ns |  1.00 |    0.00 |    2.0111 |        - |     - |    31008 B |
|                SasaTrie |   100 |     14,657.1 ns |      98.046 ns |      86.916 ns |  0.38 |    0.00 |    3.7175 |        - |     - |    56400 B |
| ImmutableTrieDictionary |   100 |     19,254.3 ns |     113.684 ns |      94.932 ns |  0.50 |    0.00 |    2.6677 |        - |     - |    40672 B |
| ImmutableTreeDictionary |   100 |     85,678.6 ns |     818.896 ns |     765.996 ns |  2.23 |    0.02 |    4.0984 |        - |     - |    63168 B |
|             ApexHashMap |   100 |      9,956.5 ns |     119.989 ns |     100.196 ns |  0.26 |    0.00 |    2.3001 |        - |     - |    34832 B |
|                         |       |                 |                |                |       |         |           |          |       |            |
|     ImmutableDictionary | 10000 | 10,760,832.6 ns | 102,613.542 ns |  90,964.232 ns |  1.00 |    0.00 |  434.7826 |  86.9565 |     - |  6779368 B |
|                SasaTrie | 10000 |  4,264,354.7 ns |  40,841.865 ns |  36,205.250 ns |  0.40 |    0.00 | 1046.8750 | 109.3750 |     - | 15961016 B |
| ImmutableTrieDictionary | 10000 |  5,222,232.5 ns |  86,338.080 ns |  80,760.693 ns |  0.49 |    0.01 |  500.0000 |  62.5000 |     - |  7838856 B |
| ImmutableTreeDictionary | 10000 | 23,985,619.4 ns | 461,237.293 ns | 452,996.694 ns |  2.23 |    0.05 |  555.5556 | 111.1111 |     - |  9981216 B |
|             ApexHashMap | 10000 |  3,159,114.5 ns |  36,182.727 ns |  32,075.045 ns |  0.29 |    0.00 |  512.5000 |  62.5000 |     - |  7775704 B |

## Lookup

|                  Method | Count |             Mean |          Error |         StdDev | Ratio | RatioSD |   Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------------ |------ |-----------------:|---------------:|---------------:|------:|--------:|--------:|------:|------:|----------:|
|     ImmutableDictionary |     5 |         73.41 ns |      0.2462 ns |      0.2303 ns |  1.00 |    0.00 |       - |     - |     - |         - |
|                SasaTrie |     5 |         38.39 ns |      0.5530 ns |      0.5172 ns |  0.52 |    0.01 |       - |     - |     - |         - |
| ImmutableTrieDictionary |     5 |         96.47 ns |      1.0393 ns |      0.9721 ns |  1.31 |    0.01 |  0.0078 |     - |     - |     120 B |
| ImmutableTreeDictionary |     5 |        479.01 ns |      4.1935 ns |      3.9226 ns |  6.53 |    0.06 |  0.0437 |     - |     - |     680 B |
|             ApexHashMap |     5 |         48.04 ns |      0.1911 ns |      0.1788 ns |  0.65 |    0.00 |       - |     - |     - |         - |
|                         |       |                  |                |                |       |         |         |       |       |           |
|     ImmutableDictionary |   100 |      1,916.50 ns |     28.1015 ns |     26.2862 ns |  1.00 |    0.00 |       - |     - |     - |         - |
|                SasaTrie |   100 |      1,364.12 ns |      7.4901 ns |      7.0063 ns |  0.71 |    0.01 |       - |     - |     - |         - |
| ImmutableTrieDictionary |   100 |      2,104.41 ns |     17.6968 ns |     16.5536 ns |  1.10 |    0.02 |  0.1504 |     - |     - |    2400 B |
| ImmutableTreeDictionary |   100 |     42,711.26 ns |    240.6352 ns |    225.0903 ns | 22.29 |    0.31 |  0.8446 |     - |     - |   13600 B |
|             ApexHashMap |   100 |      1,169.04 ns |     12.4076 ns |     11.6060 ns |  0.61 |    0.01 |       - |     - |     - |         - |
|                         |       |                  |                |                |       |         |         |       |       |           |
|     ImmutableDictionary | 10000 |  1,005,373.29 ns |  3,516.5572 ns |  3,289.3898 ns |  1.00 |    0.00 |       - |     - |     - |         - |
|                SasaTrie | 10000 |    248,800.94 ns |  1,884.5813 ns |  1,670.6322 ns |  0.25 |    0.00 |       - |     - |     - |         - |
| ImmutableTrieDictionary | 10000 |    423,498.57 ns |  2,578.4592 ns |  2,411.8923 ns |  0.42 |    0.00 | 15.2027 |     - |     - |  240000 B |
| ImmutableTreeDictionary | 10000 | 11,514,730.26 ns | 63,310.3714 ns | 59,220.5607 ns | 11.45 |    0.06 | 76.9231 |     - |     - | 1360000 B |
|             ApexHashMap | 10000 |    228,860.75 ns |    863.9169 ns |    808.1084 ns |  0.23 |    0.00 |       - |     - |     - |         - |

## Enumeration

|                  Method | Count |          Mean |         Error |        StdDev | Ratio |   Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------------ |------ |--------------:|--------------:|--------------:|------:|--------:|------:|------:|----------:|
|     ImmutableDictionary |     5 |     572.89 ns |     1.5775 ns |     1.4756 ns |  1.00 |       - |     - |     - |         - |
|                SasaTrie |     5 |     104.94 ns |     0.2806 ns |     0.2624 ns |  0.18 |  0.0058 |     - |     - |      88 B |
| ImmutableTrieDictionary |     5 |     210.68 ns |     1.4072 ns |     1.3163 ns |  0.37 |  0.0157 |     - |     - |     240 B |
|             ApexHashMap |     5 |      85.90 ns |     0.3755 ns |     0.3513 ns |  0.15 |       - |     - |     - |         - |
|                         |       |               |               |               |       |         |       |       |           |
|     ImmutableDictionary |   100 |   8,460.74 ns |    34.7266 ns |    32.4833 ns |  1.00 |       - |     - |     - |         - |
|                SasaTrie |   100 |   4,698.80 ns |    21.3403 ns |    19.9618 ns |  0.56 |  0.1907 |     - |     - |    2912 B |
| ImmutableTrieDictionary |   100 |   4,996.05 ns |    16.8366 ns |    15.7490 ns |  0.59 |  0.2975 |     - |     - |    4560 B |
|             ApexHashMap |   100 |   1,403.70 ns |     3.7516 ns |     3.5092 ns |  0.17 |       - |     - |     - |         - |
|                         |       |               |               |               |       |         |       |       |           |
|     ImmutableDictionary | 10000 | 872,223.85 ns | 1,654.7570 ns | 1,547.8608 ns |  1.00 |       - |     - |     - |         - |
|                SasaTrie | 10000 | 412,359.56 ns | 1,402.0439 ns | 1,311.4727 ns |  0.47 |  5.8594 |     - |     - |   93280 B |
| ImmutableTrieDictionary | 10000 | 589,376.42 ns | 4,825.2410 ns | 4,513.5334 ns |  0.68 | 23.4375 |     - |     - |  362320 B |
|             ApexHashMap | 10000 | 103,975.76 ns |   210.3447 ns |   196.7566 ns |  0.12 |       - |     - |     - |         - |

## Builder - Add

|                  Method | Count |           Mean |          Error |         StdDev | Ratio |   Gen 0 |   Gen 1 | Gen 2 | Allocated |
|------------------------ |------ |---------------:|---------------:|---------------:|------:|--------:|--------:|------:|----------:|
|     ImmutableDictionary |     5 |       712.3 ns |      2.5966 ns |      2.4289 ns |  1.00 |  0.0238 |       - |     - |     360 B |
| ImmutableTrieDictionary |     5 |       439.9 ns |      8.4326 ns |      7.4753 ns |  0.62 |  0.0434 |       - |     - |     664 B |
|             ApexHashMap |     5 |       185.3 ns |      0.9486 ns |      0.8409 ns |  0.26 |  0.0257 |       - |     - |     392 B |
|                         |       |                |                |                |       |         |         |       |           |
|     ImmutableDictionary |   100 |    25,356.1 ns |    156.0610 ns |    145.9796 ns |  1.00 |  0.3662 |       - |     - |    5680 B |
| ImmutableTrieDictionary |   100 |     8,905.6 ns |    202.2438 ns |    262.9741 ns |  0.35 |  0.6256 |       - |     - |    9576 B |
|             ApexHashMap |   100 |     7,004.0 ns |     26.8398 ns |     25.1060 ns |  0.28 |  1.1063 |  0.0076 |     - |   16704 B |
|                         |       |                |                |                |       |         |         |       |           |
|     ImmutableDictionary | 10000 | 5,431,158.3 ns | 13,119.1346 ns | 12,271.6467 ns |  1.00 | 23.4375 |  7.8125 |     - |  560080 B |
| ImmutableTrieDictionary | 10000 | 1,275,004.0 ns | 25,368.4327 ns | 23,729.6477 ns |  0.23 | 33.2031 | 15.6250 |     - | 1059240 B |
|             ApexHashMap | 10000 |   775,319.2 ns |  3,420.6710 ns |  3,199.6978 ns |  0.14 | 69.3359 | 27.3438 |     - | 1055608 B |

## Builder - Remove

|                  Method | Count |           Mean |         Error |        StdDev | Ratio |   Gen 0 |  Gen 1 | Gen 2 | Allocated |
|------------------------ |------ |---------------:|--------------:|--------------:|------:|--------:|-------:|------:|----------:|
|     ImmutableDictionary |     5 |       362.4 ns |      1.414 ns |      1.322 ns |  1.00 |  0.0119 |      - |     - |     184 B |
| ImmutableTrieDictionary |     5 |       361.0 ns |      1.598 ns |      1.495 ns |  1.00 |  0.0262 |      - |     - |     392 B |
|             ApexHashMap |     5 |       225.5 ns |      4.350 ns |      4.069 ns |  0.62 |  0.0236 |      - |     - |     360 B |
|                         |       |                |               |               |       |         |        |       |           |
|     ImmutableDictionary |   100 |    15,610.3 ns |     34.149 ns |     31.943 ns |  1.00 |  0.2136 |      - |     - |    3152 B |
| ImmutableTrieDictionary |   100 |     7,279.0 ns |     23.224 ns |     21.723 ns |  0.47 |  0.2365 |      - |     - |    3576 B |
|             ApexHashMap |   100 |     6,370.4 ns |    131.469 ns |    122.976 ns |  0.41 |  0.6027 |      - |     - |    9208 B |
|                         |       |                |               |               |       |         |        |       |           |
|     ImmutableDictionary | 10000 | 4,323,630.3 ns | 20,288.126 ns | 16,941.504 ns |  1.00 | 23.4375 |      - |     - |  373032 B |
| ImmutableTrieDictionary | 10000 |   803,960.6 ns |  3,958.348 ns |  3,702.641 ns |  0.19 | 10.7422 | 0.9766 |     - |  160344 B |
|             ApexHashMap | 10000 |   779,754.0 ns | 15,236.382 ns | 14,252.121 ns |  0.18 | 51.7578 | 8.7891 |     - |  782392 B |

# String key

## Add

|                  Method | Count |            Mean |          Error |         StdDev | Ratio | RatioSD |     Gen 0 |    Gen 1 | Gen 2 |  Allocated |
|------------------------ |------ |----------------:|---------------:|---------------:|------:|--------:|----------:|---------:|------:|-----------:|
|     ImmutableDictionary |     5 |      1,715.2 ns |      17.700 ns |      16.557 ns |  1.00 |    0.00 |    0.0613 |        - |     - |      968 B |
|                SasaTrie |     5 |        368.9 ns |       4.576 ns |       4.280 ns |  0.22 |    0.00 |    0.0370 |        - |     - |      560 B |
| ImmutableTrieDictionary |     5 |        495.5 ns |       4.136 ns |       3.869 ns |  0.29 |    0.00 |    0.0490 |        - |     - |      768 B |
| ImmutableTreeDictionary |     5 |      1,887.4 ns |      20.958 ns |      19.605 ns |  1.10 |    0.02 |    0.1670 |        - |     - |     2560 B |
|             ApexHashMap |     5 |        374.5 ns |       7.212 ns |       6.746 ns |  0.22 |    0.00 |    0.0625 |        - |     - |      952 B |
|                         |       |                 |                |                |       |         |           |          |       |            |
|     ImmutableDictionary |   100 |     73,749.1 ns |   1,240.794 ns |   1,160.640 ns |  1.00 |    0.00 |    2.9206 |        - |     - |    44576 B |
|                SasaTrie |   100 |     18,649.0 ns |     300.800 ns |     281.368 ns |  0.25 |    0.01 |    3.1574 |        - |     - |    48872 B |
| ImmutableTrieDictionary |   100 |     22,254.1 ns |     218.770 ns |     204.638 ns |  0.30 |    0.00 |    2.7443 |        - |     - |    42408 B |
| ImmutableTreeDictionary |   100 |    130,214.4 ns |   1,114.584 ns |   1,042.582 ns |  1.77 |    0.04 |    4.6560 |        - |     - |    73128 B |
|             ApexHashMap |   100 |     13,452.7 ns |     167.007 ns |     148.048 ns |  0.18 |    0.00 |    2.4636 |        - |     - |    37624 B |
|                         |       |                 |                |                |       |         |           |          |       |            |
|     ImmutableDictionary | 10000 | 16,511,283.5 ns | 320,626.464 ns | 356,375.465 ns |  1.00 |    0.00 |  571.4286 | 142.8571 |     - |  8812032 B |
|                SasaTrie | 10000 |  5,026,913.9 ns |  54,201.126 ns |  48,047.887 ns |  0.30 |    0.01 | 1060.0000 | 140.0000 |     - | 15888344 B |
| ImmutableTrieDictionary | 10000 |  5,582,820.7 ns |  74,051.270 ns |  69,267.604 ns |  0.34 |    0.01 |  541.6667 | 145.8333 |     - |  8169968 B |
| ImmutableTreeDictionary | 10000 | 32,558,181.7 ns | 598,000.192 ns | 559,369.750 ns |  1.97 |    0.05 |  500.0000 |        - |     - | 10369200 B |
|             ApexHashMap | 10000 |  3,183,709.5 ns |  61,036.838 ns |  77,192.057 ns |  0.19 |    0.01 |  537.5000 | 100.0000 |     - |  8224560 B |

## Remove

|                  Method | Count |            Mean |          Error |         StdDev | Ratio | RatioSD |     Gen 0 |    Gen 1 | Gen 2 |  Allocated |
|------------------------ |------ |----------------:|---------------:|---------------:|------:|--------:|----------:|---------:|------:|-----------:|
|     ImmutableDictionary |     5 |      1,112.2 ns |       6.133 ns |       5.436 ns |  1.00 |    0.00 |    0.0352 |        - |     - |      544 B |
|                SasaTrie |     5 |        435.8 ns |      25.867 ns |      26.564 ns |  0.39 |    0.03 |    0.0296 |        - |     - |      456 B |
| ImmutableTrieDictionary |     5 |        502.6 ns |       1.610 ns |       1.506 ns |  0.45 |    0.00 |    0.0402 |        - |     - |      616 B |
| ImmutableTreeDictionary |     5 |      1,628.6 ns |      14.228 ns |      12.613 ns |  1.46 |    0.01 |    0.1418 |        - |     - |     2184 B |
|             ApexHashMap |     5 |        285.6 ns |       2.213 ns |       1.962 ns |  0.26 |    0.00 |    0.0454 |        - |     - |      696 B |
|                         |       |                 |                |                |       |         |           |          |       |            |
|     ImmutableDictionary |   100 |     53,832.0 ns |     337.620 ns |     315.810 ns |  1.00 |    0.00 |    2.1404 |        - |     - |    34296 B |
|                SasaTrie |   100 |     20,040.7 ns |     159.891 ns |     141.739 ns |  0.37 |    0.00 |    3.8265 |        - |     - |    57912 B |
| ImmutableTrieDictionary |   100 |     21,887.7 ns |     199.277 ns |     186.404 ns |  0.41 |    0.00 |    2.6006 |        - |     - |    39560 B |
| ImmutableTreeDictionary |   100 |    113,691.1 ns |     968.850 ns |     906.263 ns |  2.11 |    0.02 |    4.4964 |        - |     - |    70536 B |
|             ApexHashMap |   100 |     12,939.1 ns |     149.827 ns |     125.112 ns |  0.24 |    0.00 |    2.2959 |        - |     - |    35168 B |
|                         |       |                 |                |                |       |         |           |          |       |            |
|     ImmutableDictionary | 10000 | 14,107,252.1 ns | 394,399.529 ns | 387,353.073 ns |  1.00 |    0.00 |  500.0000 |  55.5556 |     - |  7731416 B |
|                SasaTrie | 10000 |  5,800,077.3 ns | 112,592.023 ns | 129,661.141 ns |  0.41 |    0.01 | 1062.5000 | 104.1667 |     - | 16180528 B |
| ImmutableTrieDictionary | 10000 |  5,323,591.8 ns |  29,557.953 ns |  26,202.355 ns |  0.38 |    0.01 |  500.0000 |  62.5000 |     - |  7840352 B |
| ImmutableTreeDictionary | 10000 | 28,485,757.8 ns | 226,150.340 ns | 211,541.168 ns |  2.02 |    0.05 |  666.6667 | 111.1111 |     - | 10160120 B |
|             ApexHashMap | 10000 |  3,782,613.6 ns |  52,857.755 ns |  49,443.177 ns |  0.27 |    0.01 |  525.0000 |  87.5000 |     - |  7983128 B |

## Lookup

|                  Method | Count |            Mean |           Error |         StdDev | Ratio | RatioSD |   Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------------ |------ |----------------:|----------------:|---------------:|------:|--------:|--------:|------:|------:|----------:|
|     ImmutableDictionary |     5 |        162.8 ns |       1.3866 ns |      1.2970 ns |  1.00 |    0.00 |       - |     - |     - |         - |
|                SasaTrie |     5 |        138.3 ns |       0.6276 ns |      0.5871 ns |  0.85 |    0.01 |       - |     - |     - |         - |
| ImmutableTrieDictionary |     5 |        206.0 ns |       0.8802 ns |      0.7803 ns |  1.27 |    0.01 |  0.0076 |     - |     - |     120 B |
| ImmutableTreeDictionary |     5 |        826.2 ns |       2.3727 ns |      1.9813 ns |  5.08 |    0.04 |  0.0429 |     - |     - |     680 B |
|             ApexHashMap |     5 |        130.0 ns |       0.5318 ns |      0.4974 ns |  0.80 |    0.01 |       - |     - |     - |         - |
|                         |       |                 |                 |                |       |         |         |       |       |           |
|     ImmutableDictionary |   100 |      3,948.4 ns |      52.5114 ns |     49.1192 ns |  1.00 |    0.00 |       - |     - |     - |         - |
|                SasaTrie |   100 |      5,142.5 ns |      39.3276 ns |     32.8403 ns |  1.31 |    0.02 |       - |     - |     - |         - |
| ImmutableTrieDictionary |   100 |      4,799.5 ns |      52.9189 ns |     46.9112 ns |  1.22 |    0.02 |  0.1525 |     - |     - |    2400 B |
| ImmutableTreeDictionary |   100 |     56,047.1 ns |     801.9705 ns |    669.6817 ns | 14.23 |    0.29 |  0.8865 |     - |     - |   13600 B |
|             ApexHashMap |   100 |      2,997.6 ns |      13.6840 ns |     12.8000 ns |  0.76 |    0.01 |       - |     - |     - |         - |
|                         |       |                 |                 |                |       |         |         |       |       |           |
|     ImmutableDictionary | 10000 |  1,323,395.0 ns |  10,622.7752 ns |  9,936.5505 ns |  1.00 |    0.00 |       - |     - |     - |         - |
|                SasaTrie | 10000 |    937,562.5 ns |  21,486.1134 ns | 19,046.8799 ns |  0.71 |    0.01 |       - |     - |     - |         - |
| ImmutableTrieDictionary | 10000 |    842,392.0 ns |  11,463.6194 ns | 10,723.0767 ns |  0.64 |    0.01 | 13.1579 |     - |     - |  240000 B |
| ImmutableTreeDictionary | 10000 | 14,142,611.9 ns | 103,662.7023 ns | 91,894.2855 ns | 10.69 |    0.08 | 55.5556 |     - |     - | 1360000 B |
|             ApexHashMap | 10000 |    542,377.3 ns |   4,302.0872 ns |  3,813.6882 ns |  0.41 |    0.01 |       - |     - |     - |         - |
