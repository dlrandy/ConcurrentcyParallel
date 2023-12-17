using System;
namespace ManagedThreading
{
	internal class NetworkingWork
	{
		public NetworkingWork()
		{
		}
		public void CheckNetworkStatus(object data)
		{
            for (int i = 0; i < 12; i++)
            {
                bool isNetworkUp = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
                Console.WriteLine($"Thread priority {(string)data}; Is network available? Answer: {isNetworkUp}");
                i++;
            }
        }
		public void CheckNetworkStatusWithCancellationRequested(object data) {
			var cancelToken = (CancellationToken)data;
			while (!cancelToken.IsCancellationRequested)
			{
				bool isNetworkUp = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
				Console.WriteLine($"Is network availiable? Answer:{isNetworkUp}");
			}
			Console.WriteLine("DO something?");
		}
		public void CheckNetworkStatusWithCancellationRegistered(object data) {
			bool finish = false;
			var cancelToken = (CancellationToken)data;
			cancelToken.Register(() => {
				// clear up and end pending work
				finish = true;
			});
			while (!finish)
			{
				bool isNetworkUp = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
                Console.WriteLine($"Is network availiable? Answer:{isNetworkUp}");
            }
            Console.WriteLine("DO something?");
        }
	}
}

