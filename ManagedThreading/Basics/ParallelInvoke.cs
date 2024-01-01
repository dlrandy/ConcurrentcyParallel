using System;
namespace Basics
{
	internal class ParallelInvoke
	{
		internal void DoWorkInParallel() {
			Parallel.Invoke(
				DoComplexWork,
				() => {
					Console.WriteLine($"Hello lambda:Thread Id:{Thread.CurrentThread.ManagedThreadId}");

				},
				new Action(() => {

					Console.WriteLine($"Hello action: Thread id: {Thread.CurrentThread.ManagedThreadId}");
				}),
				delegate () {

					Console.WriteLine($"Hello delegate: thread id: {Thread.CurrentThread.ManagedThreadId}");
				}
				);

		}
		private void DoComplexWork() {
			Console.WriteLine($"Hello complex: thread id: {Thread.CurrentThread.ManagedThreadId}");

		}
		internal void ExecuteParallelForEach(IList<int> numbers) {
			Parallel.ForEach(numbers, number => {
				bool timeContainNumber = DateTime.Now.ToLongTimeString().Contains(number.ToString());
				if (timeContainNumber)
				{
					Console.WriteLine($"curretn time contains {number}.Thread id:{Thread.CurrentThread.ManagedThreadId}");
				}
				else {
                    Console.WriteLine($"curretn time does not contain {number}.Thread id:{Thread.CurrentThread.ManagedThreadId}");

                }

			});

		}
		internal void ExecuteLinqQuery(IList<int> numbers) {
            var evenNumbers = numbers.Where(n => n % 2 == 0);
            OutputNumbers(evenNumbers, "Regular");
        }
		internal void ExecuteParallelLinqQuery(IList<int> numbers) {
            var evenNumbers = numbers.AsParallel().Where(n => IsEven(n));
            OutputNumbers(evenNumbers, "Parallel");
        }
		private bool IsEven(int number)
		{
			Task.Delay(100);
			return number % 2 == 0;
		}
		private void OutputNumbers(IEnumerable<int> numbers, string loopType)
		{
			var numberString = string.Join(",",numbers);
			Console.WriteLine($"{loopType} number string:{numberString}");
		}
	}
}

