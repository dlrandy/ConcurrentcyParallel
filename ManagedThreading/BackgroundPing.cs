using System;
namespace ManagedThreading
{
	public class BackgroundPing
	{
		public BackgroundPing()
		{
		}
        #region thread with cancellationTokenRequested
        public static void RunWithCacellationTokenRequested() {
            var networkingWork = new NetworkingWork();
            var pingThread = new Thread(networkingWork.CheckNetworkStatusWithCancellationRequested);
            var ctSource = new CancellationTokenSource();
            pingThread.Start(ctSource.Token);
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Main thread working...");
                Thread.Sleep(100);
            }
            ctSource.Cancel();
            pingThread.Join();
            ctSource.Dispose();
            Console.WriteLine("Done");
            Console.ReadKey();
        }
        #endregion
        #region thread with cancellationTokenRegister
        public static void RunWithCacellationTokenRegistered()
        {
            var networkingWork = new NetworkingWork();
            var pingThread = new Thread(networkingWork.CheckNetworkStatusWithCancellationRegistered);
            var ctSource = new CancellationTokenSource();
            pingThread.Start(ctSource.Token);
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Main thread working...");
                Thread.Sleep(100);
            }
            ctSource.Cancel();
            pingThread.Join();
            ctSource.Dispose();
            Console.WriteLine("Done");
            Console.ReadKey();
        }
        #endregion
        #region thread with priority
        public static void RunWithPriority() {
            var networkingWork = new NetworkingWork();
            var bgThread1 = new Thread(networkingWork.CheckNetworkStatus);
            var bgThread2 = new Thread(networkingWork.CheckNetworkStatus);
            var bgThread3 = new Thread(networkingWork.CheckNetworkStatus);
            var bgThread4 = new Thread(networkingWork.CheckNetworkStatus);
            var bgThread5 = new Thread(networkingWork.CheckNetworkStatus);

            bgThread1.Priority = ThreadPriority.Lowest;
            bgThread2.Priority = ThreadPriority.BelowNormal;
            bgThread3.Priority = ThreadPriority.Normal;
            bgThread4.Priority = ThreadPriority.AboveNormal;
            bgThread5.Priority = ThreadPriority.Highest;

            bgThread1.Start("Lowest");
            bgThread2.Start("BelowNormal");
            bgThread3.Start("Normal");
            bgThread4.Start("AboveNormal");
            bgThread5.Start("Highest");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Main thread working...");
            }
            Console.WriteLine("Done");
            Console.ReadKey();
        }
        #endregion
        #region thread with pause
        public static void RunWIthPause() {
            var bgThread = new Thread((object? data) =>{
                if (data is null)
                {
                    return;
                }
                int counter = 0;
                var isParsed = int.TryParse(data.ToString(), out int maxCount);
                if (!isParsed)
                {
                    return;
                }
                while (counter < maxCount)
                {
                    bool isNetworkUp = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
                    Console.WriteLine($"Is network availiable? Answer: {isNetworkUp}");
                    Thread.Sleep(10);
                    counter++;
                }
            });

            bgThread.Start(12);
            for (int i = 0; i < 12; i++)
            {
                Console.WriteLine("Main Thread working...");
                Thread.Sleep(100);
            }
            Console.WriteLine("Done");
            Console.ReadKey();
        }
        #endregion
        #region thread with params
        public static void RunWithParams() {
            var bgThread = new Thread((object? data) => {
                if (data is null)
                {
                    return;
                }
                int counter = 0;
                var isParsed = int.TryParse(data.ToString(), out int maxCount);
                if (!isParsed)
                {
                    return;
                }
                while (counter < maxCount)
                {
                    bool isNetworkUp = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
                    Console.WriteLine($"Is network availiable? Answer: {isNetworkUp}");
                    Thread.Sleep(100);
                    counter++;
                }
            });
            bgThread.IsBackground = true;
            bgThread.Start(12);
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Main Thread working...");
                Task.Delay(500);
            }
            Console.WriteLine("Done");
            Console.ReadKey();

        }
        #endregion
        #region thread without params
        public static async Task RunTask()
		{
			Console.WriteLine("get hands dirty on thread!");
			var bgThread = new Thread(() => {
				while (true)
				{
					bool isNetworkUp = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
					Console.WriteLine($"Is network availiable? Answer: {isNetworkUp}");
					Thread.Sleep(500);
				}
			});

			bgThread.IsBackground = true;
			bgThread.Start();

			for (int i = 0; i < 10; i++)
			{
				Console.WriteLine("Main thread working...");
				await Task.Delay(500);
			}
            Console.WriteLine("Done");
            Console.ReadKey();
        }

        public static void Run()
        {
            Console.WriteLine("get hands dirty on thread!");
            var bgThread = new Thread(() => {
                while (true)
                {
                    bool isNetworkUp = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
                    Console.WriteLine($"Is network availiable? Answer: {isNetworkUp}");
                    Thread.Sleep(500);
                }
            });

            bgThread.IsBackground = true;
            bgThread.Start();

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Main thread working...");
                Task.Delay(500);
            }
            Console.WriteLine("Done");
            Console.ReadKey();
        }

        public static async void RunAsync()
        {
            Console.WriteLine("get hands dirty on thread!");
            var bgThread = new Thread(() => {
                while (true)
                {
                    bool isNetworkUp = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
                    Console.WriteLine($"Is network availiable? Answer: {isNetworkUp}");
                    Thread.Sleep(100);
                }
            });

            //bgThread.IsBackground = true;
            bgThread.Start();

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Main thread working...");
                await Task.Delay(500);
            }
            Console.WriteLine("Done");
            Console.ReadKey();
        }
        #endregion
    }
}

