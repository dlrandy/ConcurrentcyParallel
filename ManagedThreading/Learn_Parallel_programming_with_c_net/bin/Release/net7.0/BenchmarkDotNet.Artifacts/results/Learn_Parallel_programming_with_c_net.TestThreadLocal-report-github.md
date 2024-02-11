```

BenchmarkDotNet v0.13.12, macOS Monterey 12.6.4 (21G526) [Darwin 21.6.0]
Intel Core i5-5287U CPU 2.90GHz (Broadwell), 1 CPU, 4 logical and 2 physical cores
.NET SDK 7.0.312
  [Host]     : .NET 7.0.15 (7.0.1523.57226), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.15 (7.0.1523.57226), X64 RyuJIT AVX2


```
| Method                 | Mean     | Error     | StdDev    |
|----------------------- |---------:|----------:|----------:|
| SquareEachValue        | 4.227 ms | 0.0824 ms | 0.1043 ms |
| SquareEachValueChunked | 1.401 ms | 0.0272 ms | 0.0415 ms |
