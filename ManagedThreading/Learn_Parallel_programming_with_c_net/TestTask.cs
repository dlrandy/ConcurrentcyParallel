using System;
using System.Threading;

namespace Learn_Parallel_programming_with_c_net
{
	public class TestTask
	{


		public static void TestSemaphoreSlim() {
			var semaphore = new SemaphoreSlim(2, 10);
			for (int i = 0; i < 20; i++)
			{
				Task.Factory.StartNew(() => {
					Console.WriteLine($"Entering task {Task.CurrentId} ");
					semaphore.Wait();
					Console.WriteLine($"Processing task {Task.CurrentId}");
				});
			}
			while (semaphore.CurrentCount <= 2)
			{
				Console.WriteLine($"Semaphore count: {semaphore.CurrentCount}");
				Console.ReadKey();
				semaphore.Release(2);
			}

		}


		public static void TestAutoResetEvent() {

            var evt = new AutoResetEvent(false);
            var makeTea = Task.Factory.StartNew(() => {

                Console.WriteLine("Waiting for water...");
                evt.WaitOne();// false
                Console.WriteLine("Here is your tea");
                var ok = evt.WaitOne(1000);// false
				if (ok)
				{

					Console.WriteLine("enjoy your tea");
				}
				else {
					Console.WriteLine("no tea for you ");
				}
            });
            Task.Factory.StartNew(() => {

                Console.WriteLine("Boiling water");
                evt.Set();// true

            });

            makeTea.Wait();


        }



		public static void TestManualResetEventSlim()
		{
			var evt = new ManualResetEventSlim();
            var makeTea = Task.Factory.StartNew(() => {

                Console.WriteLine("Waiting for water...");
                evt.Wait();
                Console.WriteLine("Here is your tea");
				evt.Wait();
				Console.WriteLine("enjoy your tea");
            });
            Task.Factory.StartNew(() => {

				Console.WriteLine("Boiling water");
				evt.Set();

			});

			makeTea.Wait();
		}



		private static int taskCount = 5;
		static CountdownEvent cte = new CountdownEvent(taskCount);
		private static Random random = new Random();

		public static void TestCountDownEvent() {

			for (int i = 0; i < taskCount; i++)
			{
				Task.Factory.StartNew(() => {

					Console.WriteLine($"Entering task {Task.CurrentId} ");
					Thread.Sleep(random.Next(3000));
					cte.Signal();
					Console.WriteLine($"Exiting task {Task.CurrentId}");

				});
			}

			var finalTask = Task.Factory.StartNew(() => {

				Console.WriteLine($"Waiting for other tasks to complete in {Task.CurrentId}");
				cte.Wait();
				Console.WriteLine("All tasks completed");
			});
			finalTask.Wait();
		}

		static Barrier barrier = new Barrier(2, b => {
			Console.WriteLine($"Phase {b.CurrentPhaseNumber} is finished");
		});
		public static void Water() {
			Console.WriteLine("putting the kettle on (takes a bit longer )");
			Thread.Sleep(2000);
			barrier.SignalAndWait();
			Console.WriteLine("Pouring water into cup.");
			barrier.SignalAndWait();
			Console.WriteLine("putting the kettle away.");
		}
		public static void Cup() {
			Console.WriteLine("Finding the nicest cup of tea (fast)");
			barrier.SignalAndWait();
			Console.WriteLine("Adding tea.");
			barrier.SignalAndWait();
			Console.WriteLine("Adding sugar");
		}
		public static void TestBarrier()
		{
			var water = Task.Factory.StartNew(Water);
			var cup = Task.Factory.StartNew(Cup);
			var tea = Task.Factory.ContinueWhenAll(new[] { water, cup }, (tasks) => {
				Console.WriteLine("enjoy your cup of tea.");
			} );
			tea.Wait();
		} 
		public static void TestContinueWith() {

			var task = Task.Factory.StartNew(() => {

				Console.WriteLine($"Boiiling water ---id:{Task.CurrentId}");

			});
			var task2 = task.ContinueWith(t => {
				Console.WriteLine($"Completed task {t.Id}, pouring water into the cup.");
			});
			task2.Wait();
		}

		public static void TestContinueAnyAll() {
			var task = Task.Factory.StartNew(()=>"Task1 done");
			var task2 = Task.Factory.StartNew(()=>"Task2 done");
			var task3 = Task.Factory.ContinueWhenAll(new[] {task, task2 } , (tasks) => {

				Console.WriteLine("tasks completed:");

				foreach (var t in tasks)
				{
					Console.WriteLine(" - " + t.Result);
				}
				Console.WriteLine("All tasks done");
			});
			task3.Wait();
		}
		public static void TestChildTask()
		{
			var parent = new Task(() => {
				var child = new Task(() => {

					Console.WriteLine("child task starting..");
					Thread.Sleep(3000);
					Console.WriteLine("child task finishing..");
					throw new Exception();
				}, TaskCreationOptions.AttachedToParent);

				var completionHandler = child.ContinueWith(t => {

					Console.WriteLine($"Hooray, task {t.Id} 's state is {t.Status}");
				}, TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.OnlyOnRanToCompletion);

                var failHandler = child.ContinueWith(t => {

                    Console.WriteLine($"Oops, task {t.Id} 's state is {t.Status}");
                }, TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.OnlyOnFaulted);


                child.Start();
			});
			parent.Start();

			try
			{
				parent.Wait();
			}
			catch (AggregateException ae)
			{
				ae.Handle(e => true);
			}
		}
	}
}

