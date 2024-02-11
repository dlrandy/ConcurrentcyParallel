using System;
namespace Learn_Parallel_programming_with_c_net
{
	public class ParallelRange
	{
		public static void RangeParallel()
		{
			const int count = 50;
			var items = Enumerable.Range(1, count).ToArray();
			var results = new int[count];
			items.AsParallel().ForAll(x =>
			{
				int newValue = x * x * x;
				Console.WriteLine($"{newValue} ({Task.CurrentId})");
				results[x - 1] = newValue;
			});
			Console.WriteLine();
			//foreach (var item in results)
			//{
			//    Console.WriteLine($"{item}\t");
			//}
			var cubes = items.AsParallel().AsOrdered().Select(x => x * x * x);
			foreach (var item in cubes)
			{
				Console.WriteLine($"{item}\t");
			}
		}

		public static void TestParallelEnumerable()
		{
			var cts = new CancellationTokenSource();
			var items = ParallelEnumerable.Range(1, 20);
			var results = items.WithCancellation(cts.Token).Select(i =>
			{
				double result = Math.Log10(i);
				//if (result > 1)
				//{
				//	throw new InvalidOperationException();
				//}
				if (result > 1)
				{
					cts.Cancel();
				}
				Console.WriteLine($"i = {i} . tid = {Task.CurrentId}");
				return result;
			});
			try
			{
				foreach (var item in results)
				{

					Console.WriteLine($"{item}\t");
				}
			}
			catch (AggregateException ae)
			{
				ae.Handle(e =>
				{

					Console.WriteLine($"{e.GetType().Name}: {e.Message}");
					return true;
				});
			}
			catch (OperationCanceledException ex)
			{

				Console.WriteLine($"Cancel {ex.Message}");
			}
		}
		public static void TestParallelEnumerableMerge()
		{
			var numbers = Enumerable.Range(1, 20).ToArray();
			var results = numbers.AsParallel()
				.WithMergeOptions(ParallelMergeOptions.NotBuffered)
				.Select(x => {

                double result = Math.Log10(x);
               
                Console.WriteLine($"Produced {result}");
                return result;
            });
            foreach (var item in results)
            {

                Console.WriteLine($"consumed {item}\t");
            }
        }
        public static void TestParallelEnumerableAggregation()
        {
			//var sum = Enumerable.Range(1, 1000).Sum();
			//var sum = Enumerable.Range(1, 1000).Aggregate(0, (i, acc) => i + acc);
			var sum = ParallelEnumerable.Range(1, 1000).Aggregate(0,
				(part, i)=> part + i,
				(total, sub)=> total + sub,
				f =>f
				);
                Console.WriteLine($"sum: {sum}\t");
             
        }
    }
}

