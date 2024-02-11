using System;
using System.Collections.Concurrent;

namespace Learn_Parallel_programming_with_c_net
{
	public class TestThreadLocal
	{
		public static void ParallelLocalTest() {
			int sum = 0;
			//Parallel.For(1, 1001, x => {
			//	Interlocked.Add(ref sum, x);
			//});

			Parallel.For(1, 1001, () => 0,

				(x, state, localSum) => {
					localSum += x;
					return localSum;
				},
				localSum => {
					Interlocked.Add(ref sum, localSum);
				}
				);
			Console.WriteLine($"Sum: {sum}");
		}

		[BenchmarkDotNet.Attributes.Benchmark]
	public void SquareEachValue()
	{
		const int count = 100000;
		var values = Enumerable.Range(0, count);
		var results = new int[count];
		Parallel.ForEach(values, x =>
		{
			results[x] = (int)Math.Pow(x, 2);
		});
	}

        [BenchmarkDotNet.Attributes.Benchmark]
        public void SquareEachValueChunked()
        {
            const int count = 100000;
            var values = Enumerable.Range(0, count);
            var results = new int[count];
			var part = Partitioner.Create(0, count, 10000);
            Parallel.ForEach(part, range =>
            {
				for (int i = range.Item1; i < range.Item2; i++)
				{
                    results[i] = (int)Math.Pow(i, 2);
                }
            });
        }
    }
}

