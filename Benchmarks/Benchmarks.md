
## Benchmarked Collections

|                    Name |                      Source|
| ----------------------- | -------------------------- |
|     ImmutableDictionary |                .NET Core 3 |
|                SasaTrie | Sasa.Collections 1.0.0-RC4 |
| ImmutableTrieDictionary |  ImmutableTrie 1.0.0-alpha |
|             ApexHashMap |            this Repository |

```
BenchmarkDotNet=v0.11.5, OS=Windows 10.0.18362
Intel Core i5-4690 CPU 3.50GHz (Haswell), 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=3.0.100-preview8-013656
  [Host] : .NET Core 3.0.0-preview8-28405-07 (CoreCLR 4.700.19.37902, CoreFX 4.700.19.40503), 64bit RyuJIT
  Core   : .NET Core 3.0.0-preview8-28405-07 (CoreCLR 4.700.19.37902, CoreFX 4.700.19.40503), 64bit RyuJIT

Job=Core  Runtime=Core  Server=True
Toolchain=.NET Core 3.0
```

# Immutable.HashMap<K, V>

## Add

|                  Method | Count |            Mean |         Error |        StdDev | Ratio | RatioSD |     Gen 0 |    Gen 1 | Gen 2 |  Allocated |
|------------------------ |------ |----------------:|--------------:|--------------:|------:|--------:|----------:|---------:|------:|-----------:|
|     ImmutableDictionary |     5 |      1,063.2 ns |      5.162 ns |      4.576 ns |  1.00 |    0.00 |    0.0610 |        - |     - |      928 B |
|                SasaTrie |     5 |        206.4 ns |      1.275 ns |      1.130 ns |  0.19 |    0.00 |    0.0291 |        - |     - |      440 B |
| ImmutableTrieDictionary |     5 |        357.0 ns |     12.305 ns |     15.561 ns |  0.34 |    0.02 |    0.0477 |        - |     - |      728 B |
|             ApexHashMap |     5 |        172.6 ns |      1.300 ns |      1.216 ns |  0.16 |    0.00 |    0.0451 |        - |     - |      680 B |
|                         |       |                 |               |               |       |         |           |          |       |            |
|     ImmutableDictionary |   100 |     50,571.0 ns |    196.604 ns |    183.904 ns |  1.00 |    0.00 |    2.7466 |        - |     - |    41688 B |
|                SasaTrie |   100 |     12,179.7 ns |    241.244 ns |    287.184 ns |  0.24 |    0.01 |    3.8757 |        - |     - |    58696 B |
| ImmutableTrieDictionary |   100 |     17,351.4 ns |     80.642 ns |     75.432 ns |  0.34 |    0.00 |    2.7466 |        - |     - |    41880 B |
|             ApexHashMap |   100 |      9,909.3 ns |     55.799 ns |     52.195 ns |  0.20 |    0.00 |    2.6245 |        - |     - |    39880 B |
|                         |       |                 |               |               |       |         |           |          |       |            |
|     ImmutableDictionary | 10000 | 10,258,457.3 ns | 52,860.241 ns | 41,269.799 ns |  1.00 |    0.00 |  515.6250 | 109.3750 |     - |  7882552 B |
|                SasaTrie | 10000 |  2,841,877.0 ns | 17,478.803 ns | 15,494.503 ns |  0.28 |    0.00 | 1058.5938 |  89.8438 |     - | 15968616 B |
| ImmutableTrieDictionary | 10000 |  3,765,494.0 ns | 25,032.971 ns | 23,415.856 ns |  0.37 |    0.00 |  527.3438 | 128.9063 |     - |  8015992 B |
|             ApexHashMap | 10000 |  1,925,236.0 ns | 37,806.120 ns | 42,021.402 ns |  0.19 |    0.00 |  525.3906 |  87.8906 |     - |  7920488 B |

## Remove

|                  Method | Count |            Mean |          Error |          StdDev | Ratio | RatioSD |     Gen 0 |   Gen 1 | Gen 2 |  Allocated |
|------------------------ |------ |----------------:|---------------:|----------------:|------:|--------:|----------:|--------:|------:|-----------:|
|     ImmutableDictionary |     5 |        577.6 ns |       6.098 ns |       5.4058 ns |  1.00 |    0.00 |    0.0286 |       - |     - |      440 B |
|                SasaTrie |     5 |        258.1 ns |       1.648 ns |       1.4609 ns |  0.45 |    0.00 |    0.0248 |       - |     - |      376 B |
| ImmutableTrieDictionary |     5 |        324.0 ns |       1.581 ns |       1.4012 ns |  0.56 |    0.00 |    0.0372 |       - |     - |      568 B |
|             ApexHashMap |     5 |        172.8 ns |       1.040 ns |       0.9220 ns |  0.30 |    0.00 |    0.0410 |       - |     - |      616 B |
|                         |       |                 |                |                 |       |         |           |         |       |            |
|     ImmutableDictionary |   100 |     35,883.4 ns |     517.274 ns |     458.5503 ns |  1.00 |    0.00 |    1.8921 |       - |     - |    29384 B |
|                SasaTrie |   100 |     15,515.6 ns |     175.546 ns |     155.6172 ns |  0.43 |    0.01 |    4.3640 |       - |     - |    66232 B |
| ImmutableTrieDictionary |   100 |     18,726.6 ns |     371.021 ns |     441.6738 ns |  0.52 |    0.02 |    2.6245 |       - |     - |    39712 B |
|             ApexHashMap |   100 |     10,260.8 ns |      34.398 ns |      30.4927 ns |  0.29 |    0.00 |    2.4719 |       - |     - |    37592 B |
|                         |       |                 |                |                 |       |         |           |         |       |            |
|     ImmutableDictionary | 10000 | 10,239,126.8 ns |  93,839.738 ns |  83,186.4831 ns |  1.00 |    0.00 |  437.5000 | 62.5000 |     - |  6677504 B |
|                SasaTrie | 10000 |  3,795,367.3 ns |  23,482.492 ns |  19,608.9447 ns |  0.37 |    0.00 | 1066.4063 | 82.0313 |     - | 16064368 B |
| ImmutableTrieDictionary | 10000 |  4,895,282.4 ns |  23,114.199 ns |  20,490.1351 ns |  0.48 |    0.00 |  507.8125 | 62.5000 |     - |  7729264 B |
|             ApexHashMap | 10000 |  2,391,906.5 ns | 106,203.670 ns | 145,372.7817 ns |  0.24 |    0.02 |  515.6250 | 54.6875 |     - |  7758784 B |

## Lookup

|                  Method | Count |            Mean |         Error |        StdDev | Ratio | RatioSD |   Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------------ |------ |----------------:|--------------:|--------------:|------:|--------:|--------:|------:|------:|----------:|
|     ImmutableDictionary |     5 |        72.72 ns |     0.1703 ns |     0.1593 ns |  1.00 |    0.00 |       - |     - |     - |         - |
|                SasaTrie |     5 |        36.93 ns |     0.2857 ns |     0.2673 ns |  0.51 |    0.00 |       - |     - |     - |         - |
| ImmutableTrieDictionary |     5 |        97.44 ns |     0.4045 ns |     0.3783 ns |  1.34 |    0.00 |  0.0079 |     - |     - |     120 B |
|             ApexHashMap |     5 |        46.94 ns |     0.3155 ns |     0.2951 ns |  0.65 |    0.00 |       - |     - |     - |         - |
|                         |       |                 |               |               |       |         |         |       |       |           |
|     ImmutableDictionary |   100 |     1,804.92 ns |     8.9352 ns |     8.3580 ns |  1.00 |    0.00 |       - |     - |     - |         - |
|                SasaTrie |   100 |     1,434.44 ns |     3.8745 ns |     3.6242 ns |  0.79 |    0.00 |       - |     - |     - |         - |
| ImmutableTrieDictionary |   100 |     1,969.44 ns |    33.3336 ns |    42.1564 ns |  1.09 |    0.03 |  0.1564 |     - |     - |    2400 B |
|             ApexHashMap |   100 |     1,078.85 ns |     5.0928 ns |     4.7638 ns |  0.60 |    0.00 |       - |     - |     - |         - |
|                         |       |                 |               |               |       |         |         |       |       |           |
|     ImmutableDictionary | 10000 | 1,153,800.94 ns | 3,824.2025 ns | 3,577.1614 ns |  1.00 |    0.00 |       - |     - |     - |         - |
|                SasaTrie | 10000 |   199,498.61 ns |   557.8863 ns |   521.8472 ns |  0.17 |    0.00 |       - |     - |     - |         - |
| ImmutableTrieDictionary | 10000 |   366,772.33 ns | 2,927.4180 ns | 2,738.3086 ns |  0.32 |    0.00 | 15.6250 |     - |     - |  240000 B |
|             ApexHashMap | 10000 |   164,495.14 ns |   240.8223 ns |   225.2653 ns |  0.14 |    0.00 |       - |     - |     - |         - |

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
