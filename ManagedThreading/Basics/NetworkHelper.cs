using System;
namespace Basics
{
	internal class NetworkHelper
	{
		public NetworkHelper()
		{
		}
		internal async Task CheckNetworkStatusAsync() {

			Task t = NetworkCheckInternalAsync();
			for (int i = 0; i < 8; i++)
			{
				Console.WriteLine("Top level method working...");
				await Task.Delay(500);
			}
			await t;

		}
		private async Task NetworkCheckInternalAsync() {

			for (int i = 0; i < 10; i++)
			{
				bool isNetworkUp = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
				Console.WriteLine($"Iis network availiable? Answer: {isNetworkUp}");
				await Task.Delay(100);
			}
		}

		internal void BackgroundPing() {

			ThreadPool.QueueUserWorkItem((o) => {
				for (int i = 0; i < 20; i++)
				{
					bool isNetworkUp = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
					Console.WriteLine($"Is network availiable?Answer:{isNetworkUp}");
					Thread.Sleep(100);
				}

			});

			for (int i = 0; i < 10; i++)
			{
				Console.WriteLine("inner Main thread working...");
				Task.Delay(500);
			}

			Console.WriteLine("Done");
			Console.ReadKey();
		}
	}
}

