using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Basics
{
	public class ThreadingTimerSample
	{
		private System.Threading.Timer? _timer;

		public void StartTimer() {
			if (_timer == null)
			{
				InitializeTimer();
			}
		}
		public async Task DisposeTimerAsync() {
			if (_timer != null)
			{
				await _timer.DisposeAsync();
			}
		}
		private void InitializeTimer() {
			var updater = new MessageUpdater();
			_timer = new System.Threading.Timer(
				callback: new TimerCallback(TimerFired),
				state: updater,
				dueTime: 500,
				period: 1000
				);
		}
		private void TimerFired(object? state)
		{
			int messageCount = CheckForNewMesageCount();

			if (messageCount > 0 && state is MessageUpdater updater)
			{
				updater.Update(messageCount);
			}
		}
		private int CheckForNewMesageCount() {

			return new Random().Next(100);
		}
	}
	internal class MessageUpdater
	{
		internal void Update(int messageCount)
		{
			Console.WriteLine($"You have {messageCount} new Messages!");
		}

	}
}

