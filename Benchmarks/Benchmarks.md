
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

|                  Method | Count |            Mean |          Error |         StdDev |          Median | Ratio | RatioSD |     Gen 0 |    Gen 1 | Gen 2 |  Allocated |
|------------------------ |------ |----------------:|---------------:|---------------:|----------------:|------:|--------:|----------:|---------:|------:|-----------:|
|     ImmutableDictionary |     5 |      1,042.8 ns |      14.262 ns |      12.643 ns |      1,041.2 ns |  1.00 |    0.00 |    0.0572 |        - |     - |      872 B |
|                SasaTrie |     5 |        247.1 ns |       1.700 ns |       1.420 ns |        247.2 ns |  0.24 |    0.00 |    0.0291 |        - |     - |      440 B |
| ImmutableTrieDictionary |     5 |        357.9 ns |       2.513 ns |       2.227 ns |        357.9 ns |  0.34 |    0.01 |    0.0482 |        - |     - |      728 B |
| ImmutableTreeDictionary |     5 |      1,140.5 ns |       5.736 ns |       5.365 ns |      1,140.7 ns |  1.09 |    0.01 |    0.1469 |        - |     - |     2240 B |
|             ApexHashMap |     5 |        172.3 ns |       1.712 ns |       1.430 ns |        172.0 ns |  0.17 |    0.00 |    0.0451 |        - |     - |      680 B |
|                         |       |                 |                |                |                 |       |         |           |          |       |            |
|     ImmutableDictionary |   100 |     49,040.1 ns |     181.651 ns |     169.916 ns |     48,991.4 ns |  1.00 |    0.00 |    2.5635 |        - |     - |    39504 B |
|                SasaTrie |   100 |     12,122.9 ns |      55.062 ns |      51.505 ns |     12,112.4 ns |  0.25 |    0.00 |    2.7313 |        - |     - |    41456 B |
| ImmutableTrieDictionary |   100 |     18,844.1 ns |     204.469 ns |     191.261 ns |     18,770.8 ns |  0.38 |    0.00 |    2.7466 |        - |     - |    41936 B |
| ImmutableTreeDictionary |   100 |     92,928.0 ns |   1,376.229 ns |   1,219.991 ns |     92,405.2 ns |  1.90 |    0.03 |    4.2725 |        - |     - |    65640 B |
|             ApexHashMap |   100 |      8,807.8 ns |      70.037 ns |      62.086 ns |      8,789.4 ns |  0.18 |    0.00 |    2.1210 |        - |     - |    32384 B |
|                         |       |                 |                |                |                 |       |         |           |          |       |            |
|     ImmutableDictionary | 10000 | 11,586,911.2 ns | 119,059.589 ns | 111,368.414 ns | 11,565,068.8 ns |  1.00 |    0.00 |  500.0000 | 125.0000 |     - |  7713376 B |
|                SasaTrie | 10000 |  3,566,065.2 ns |  96,016.165 ns | 128,178.822 ns |  3,509,293.0 ns |  0.31 |    0.01 | 1015.6250 | 121.0938 |     - | 15333552 B |
| ImmutableTrieDictionary | 10000 |  4,497,154.5 ns |  89,039.281 ns | 155,945.119 ns |  4,567,635.9 ns |  0.40 |    0.01 |  531.2500 | 125.0000 |     - |  8084512 B |
| ImmutableTreeDictionary | 10000 | 25,343,482.9 ns | 106,420.615 ns |  88,866.033 ns | 25,323,315.6 ns |  2.19 |    0.02 |  625.0000 |  62.5000 |     - |  9649888 B |
|             ApexHashMap | 10000 |  2,521,063.1 ns |  49,432.848 ns |  79,824.836 ns |  2,502,905.5 ns |  0.22 |    0.01 |  507.8125 |  85.9375 |     - |  7707952 B |

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

|                  Method | Count |             Mean |          Error |         StdDev | Ratio | RatioSD |   Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------------ |------ |-----------------:|---------------:|---------------:|------:|--------:|--------:|------:|------:|----------:|
|     ImmutableDictionary |     5 |         72.97 ns |      0.1879 ns |      0.1569 ns |  1.00 |    0.00 |       - |     - |     - |         - |
|                SasaTrie |     5 |         37.08 ns |      0.1512 ns |      0.1414 ns |  0.51 |    0.00 |       - |     - |     - |         - |
| ImmutableTrieDictionary |     5 |        105.57 ns |      0.4420 ns |      0.4135 ns |  1.45 |    0.01 |  0.0080 |     - |     - |     120 B |
|        TunnelVisionLabs |     5 |        469.01 ns |      3.7565 ns |      3.1368 ns |  6.43 |    0.05 |  0.0448 |     - |     - |     680 B |
|             ApexHashMap |     5 |         40.45 ns |      0.3183 ns |      0.2977 ns |  0.56 |    0.00 |       - |     - |     - |         - |
|                         |       |                  |                |                |       |         |         |       |       |           |
|     ImmutableDictionary |   100 |      1,900.06 ns |     15.1814 ns |     14.2007 ns |  1.00 |    0.00 |       - |     - |     - |         - |
|                SasaTrie |   100 |      1,366.41 ns |     12.3674 ns |     10.9634 ns |  0.72 |    0.01 |       - |     - |     - |         - |
| ImmutableTrieDictionary |   100 |      2,079.30 ns |      7.2245 ns |      6.0328 ns |  1.10 |    0.01 |  0.1564 |     - |     - |    2400 B |
|        TunnelVisionLabs |   100 |     42,189.60 ns |    135.5263 ns |    120.1405 ns | 22.23 |    0.13 |  0.9155 |     - |     - |   13600 B |
|             ApexHashMap |   100 |      1,073.79 ns |      3.7380 ns |      3.4965 ns |  0.57 |    0.00 |       - |     - |     - |         - |
|                         |       |                  |                |                |       |         |         |       |       |           |
|     ImmutableDictionary | 10000 |  1,000,426.08 ns |  6,664.6173 ns |  6,234.0871 ns |  1.00 |    0.00 |       - |     - |     - |         - |
|                SasaTrie | 10000 |    247,388.96 ns |    751.0222 ns |    702.5066 ns |  0.25 |    0.00 |       - |     - |     - |         - |
| ImmutableTrieDictionary | 10000 |    424,860.45 ns |  1,695.7666 ns |  1,586.2211 ns |  0.42 |    0.00 | 16.1133 |     - |     - |  240000 B |
|        TunnelVisionLabs | 10000 | 11,665,202.40 ns | 66,965.7947 ns | 62,639.8459 ns | 11.66 |    0.10 | 78.1250 |     - |     - | 1360000 B |
|             ApexHashMap | 10000 |    216,513.15 ns |    389.2727 ns |    345.0801 ns |  0.22 |    0.00 |       - |     - |     - |         - |

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
|     ImmutableDictionary |     5 |      1,515.6 ns |      14.769 ns |      13.815 ns |  1.00 |    0.00 |    0.0591 |        - |     - |      904 B |
|                SasaTrie |     5 |        491.7 ns |       3.025 ns |       2.829 ns |  0.32 |    0.00 |    0.0448 |        - |     - |      680 B |
| ImmutableTrieDictionary |     5 |        482.7 ns |       3.638 ns |       3.225 ns |  0.32 |    0.00 |    0.0515 |        - |     - |      768 B |
| ImmutableTreeDictionary |     5 |      1,951.7 ns |      41.290 ns |      50.708 ns |  1.29 |    0.04 |    0.1678 |        - |     - |     2560 B |
|             ApexHashMap |     5 |        325.9 ns |       1.607 ns |       1.342 ns |  0.21 |    0.00 |    0.0525 |        - |     - |      800 B |
|                         |       |                 |                |                |       |         |           |          |       |            |
|     ImmutableDictionary |   100 |     75,017.6 ns |     193.911 ns |     171.897 ns |  1.00 |    0.00 |    3.0518 |        - |     - |    45728 B |
|                SasaTrie |   100 |     18,256.5 ns |     102.467 ns |      95.848 ns |  0.24 |    0.00 |    3.3264 |        - |     - |    50272 B |
| ImmutableTrieDictionary |   100 |     22,221.8 ns |      75.775 ns |      67.172 ns |  0.30 |    0.00 |    2.8076 |        - |     - |    42712 B |
| ImmutableTreeDictionary |   100 |    129,326.5 ns |     555.452 ns |     519.570 ns |  1.72 |    0.01 |    4.6387 |        - |     - |    72920 B |
|             ApexHashMap |   100 |     14,117.2 ns |      46.855 ns |      43.828 ns |  0.19 |    0.00 |    2.4872 |        - |     - |    37832 B |
|                         |       |                 |                |                |       |         |           |          |       |            |
|     ImmutableDictionary | 10000 | 17,113,511.2 ns | 193,253.581 ns | 150,879.685 ns |  1.00 |    0.00 |  593.7500 | 187.5000 |     - |  8770944 B |
|                SasaTrie | 10000 |  5,058,896.8 ns |  28,628.325 ns |  26,778.953 ns |  0.30 |    0.00 | 1039.0625 | 148.4375 |     - | 15833224 B |
| ImmutableTrieDictionary | 10000 |  4,890,294.1 ns |  36,869.513 ns |  34,487.766 ns |  0.29 |    0.00 |  539.0625 | 132.8125 |     - |  8170752 B |
| ImmutableTreeDictionary | 10000 | 32,855,975.4 ns |  95,626.408 ns |  84,770.320 ns |  1.92 |    0.02 |  687.5000 | 125.0000 |     - | 10366704 B |
|             ApexHashMap | 10000 |  3,405,661.8 ns |  66,408.458 ns |  90,900.647 ns |  0.20 |    0.01 |  542.9688 |  97.6563 |     - |  8233088 B |

## Remove

## Lookup

|                  Method | Count |            Mean |           Error |          StdDev | Ratio | RatioSD |   Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------------ |------ |----------------:|----------------:|----------------:|------:|--------:|--------:|------:|------:|----------:|
|     ImmutableDictionary |     5 |        158.6 ns |       1.5934 ns |       1.4905 ns |  1.00 |    0.00 |       - |     - |     - |         - |
|                SasaTrie |     5 |        138.5 ns |       0.5544 ns |       0.5186 ns |  0.87 |    0.01 |       - |     - |     - |         - |
| ImmutableTrieDictionary |     5 |        254.5 ns |       0.8523 ns |       0.7555 ns |  1.60 |    0.02 |  0.0076 |     - |     - |     120 B |
|        TunnelVisionLabs |     5 |        865.0 ns |      15.3938 ns |      13.6462 ns |  5.45 |    0.12 |  0.0448 |     - |     - |     680 B |
|             ApexHashMap |     5 |        182.2 ns |       0.2918 ns |       0.2730 ns |  1.15 |    0.01 |       - |     - |     - |         - |
|                         |       |                 |                 |                 |       |         |         |       |       |           |
|     ImmutableDictionary |   100 |      3,843.1 ns |      34.0775 ns |      31.8761 ns |  1.00 |    0.00 |       - |     - |     - |         - |
|                SasaTrie |   100 |      5,279.3 ns |      19.4096 ns |      18.1557 ns |  1.37 |    0.01 |       - |     - |     - |         - |
| ImmutableTrieDictionary |   100 |      5,028.1 ns |      95.5187 ns |     106.1688 ns |  1.31 |    0.04 |  0.1602 |     - |     - |    2400 B |
|        TunnelVisionLabs |   100 |     55,991.6 ns |     228.8409 ns |     214.0579 ns | 14.57 |    0.14 |  0.9155 |     - |     - |   13600 B |
|             ApexHashMap |   100 |      4,495.9 ns |      13.2370 ns |      11.0535 ns |  1.17 |    0.01 |       - |     - |     - |         - |
|                         |       |                 |                 |                 |       |         |         |       |       |           |
|     ImmutableDictionary | 10000 |  1,366,415.8 ns |   6,050.4799 ns |   5,363.5928 ns |  1.00 |    0.00 |       - |     - |     - |         - |
|                SasaTrie | 10000 |    903,188.3 ns |   2,291.9011 ns |   2,031.7106 ns |  0.66 |    0.00 |       - |     - |     - |         - |
| ImmutableTrieDictionary | 10000 |    841,601.1 ns |  17,926.7918 ns |  18,409.4954 ns |  0.62 |    0.01 | 15.6250 |     - |     - |  240000 B |
|        TunnelVisionLabs | 10000 | 14,014,118.2 ns | 112,721.6183 ns | 105,439.8717 ns | 10.26 |    0.09 | 78.1250 |     - |     - | 1360000 B |
|             ApexHashMap | 10000 |    705,095.5 ns |   2,298.1463 ns |   1,794.2414 ns |  0.52 |    0.00 |       - |     - |     - |         - |
