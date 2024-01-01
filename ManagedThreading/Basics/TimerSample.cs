using System;
using System.Diagnostics;

namespace Basics
{
	public class TimerSample:IDisposable
	{
        private System.Timers.Timer? _timer;
		public TimerSample()
		{
		}

        public void StartTimer()
        {
            if (_timer == null)
            {
                InitializeTimer();
            }
            if (_timer != null && !_timer.Enabled)
            {
                _timer.Enabled = true;
            }
        }
        public void StopTimer()
        {
            if (_timer?.Enabled == true)
            {
                _timer.Enabled = false;
            }
        }
        private void InitializeTimer()
        {
            _timer = new System.Timers.Timer
            {
                Interval = 1000
            };

            _timer.Elapsed += _timerElapsed;
        }
        private void _timerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
        { 
            int messageCount = CheckForNewMessageCount();
            if (messageCount > 0)
            {
                AlertUser(messageCount);
            }
        }
        private void AlertUser(int messageCount)
        {
            Console.WriteLine($"You have {messageCount} new mesasges!");
        }
        private int CheckForNewMessageCount() {
            return new Random().Next(100);
        }
        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Elapsed -= _timerElapsed;
                _timer.Dispose();
            }
        }
    }
}

