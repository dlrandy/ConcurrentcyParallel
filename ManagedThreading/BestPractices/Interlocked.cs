using System;
namespace BestPractices
{
	public class InterlockedEa
	{
		private long _runningTotal;
		public InterlockedEa()
		{
		}
		public void PerformCalculations()
		{
			_runningTotal = 3;
			Parallel.Invoke(() => {
				AddValue().Wait();
			}, () => {
				MultiplyValue().Wait();
			});
		}
		private async Task AddValue() {
			await Task.Delay(100);
			Interlocked.Add(ref _runningTotal, 15);
			Console.WriteLine("running: {0}; ThreadId: {1}",_runningTotal, Thread.CurrentThread.ManagedThreadId);
		}
		private async Task MultiplyValue() {

			await Task.Delay(100);
			var currentTotal = Interlocked.Read(ref _runningTotal);
			Interlocked.Exchange(ref _runningTotal, currentTotal * 10);
            Console.WriteLine("running: {0}; ThreadId: {1}", _runningTotal, Thread.CurrentThread.ManagedThreadId);
        }

	}
}

