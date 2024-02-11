using System;
using System.Linq;
using System.Collections.Generic;
namespace Learn_Parallel_programming_with_c_net
{
	public class ParallelTest
	{
		public static IEnumerable<int> Range(int start, int end, int step) {
			for (int i = start;  i < end; i+= step)
			{
				yield return i;
			}
		}
		public static void TestInvoke()
		{
			var a = new Action(()=>Console.WriteLine($"First {Task.CurrentId}"));
            var b = new Action(() => Console.WriteLine($"Second {Task.CurrentId}"));
            var c = new Action(() => Console.WriteLine($"Third {Task.CurrentId}"));

			//Parallel.Invoke(a, b, c);


			var cts = new CancellationTokenSource();
			var po = new ParallelOptions();
			po.MaxDegreeOfParallelism = 2;
			po.CancellationToken = cts.Token;
			try
			{
              var result =   Parallel.For(1, 11, po, (int i, ParallelLoopState state) => {

                    Console.WriteLine($"{i * i} \t {Task.CurrentId}");
                    if (i == 10)
                    {
					  //state.Stop();
					  //state.Break();
					  //throw new Exception("PPPPP");
					  cts.Cancel();
				  }
                });

				Console.WriteLine($"was loop completed? { result.IsCompleted}");
				if (result.LowestBreakIteration.HasValue)
				{
					Console.WriteLine($"Lowest break iteration is {result.LowestBreakIteration}");
				}
            }
			catch (AggregateException ex)
			{
				ex.Handle(e =>
				{
					Console.WriteLine(e.Message);
					return true;
				});
			}
			catch (OperationCanceledException ope) {

				Console.WriteLine(ope.Message);
			}


			//string[] words = { "oh","parallel","great"};
			//Parallel.ForEach(words, word =>
			//{
			//	Console.WriteLine($"{word} has length {word.Length} (task {Task.CurrentId})");
			//});
			//Parallel.ForEach(Range(1, 20, 3),Console.WriteLine);
        }
	}
}

