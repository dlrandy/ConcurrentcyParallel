using System;
using System.Collections.Concurrent;

namespace Learn_Parallel_programming_with_c_net
{
	public class TestConcurentCollection
	{
		private static ConcurrentDictionary<string, string> capitals = new ConcurrentDictionary<string, string>();

		public static void AddPairs()
		{
			bool success = capitals.TryAdd("France", "Pairs");
			string who = Task.CurrentId.HasValue ? $"Task: {Task.CurrentId}" : "Main Thread";
			Console.WriteLine($"{who} {(success ? "added" : "did not add")} the element.");
		}
		public static void TestAddPairs()
		{
			Task.Factory.StartNew(AddPairs).Wait();
			AddPairs();

			capitals["Russia"] = "LeningGrad";
			capitals["Russia"] = "Moscow";

			capitals.AddOrUpdate("Russia", "Russia", (k, old) => old + "-- Russia");
			Console.WriteLine("The capital of Russia is " + capitals["Russia"]);

			capitals["Sweden"] = "Uppsala";
			var capOfSweden = capitals.GetOrAdd("Sweden", "Stockholm");
			Console.WriteLine($"The capital of Swedeb is {capOfSweden}");

			const string toRemove = "Russia";
			string removed;
			var didRemove = capitals.TryRemove(toRemove, out removed);
			if (didRemove)
			{
				Console.WriteLine($"We just removed {removed}");
			}
			else
			{
				Console.WriteLine($"Failed to remove the capital of {toRemove}");
			}

			foreach (var item in capitals)
			{
				Console.WriteLine($"- {item.Value} is the capital of {item.Key}");
			}

		}

		public static void TestConcurrentQueue()
		{
			var q = new ConcurrentQueue<int>();
			q.Enqueue(1);
			q.Enqueue(2);
			int result;
			if (q.TryDequeue(out result))
			{
				Console.WriteLine($"Removed element {result}");
			}

			if (q.TryPeek(out result))
			{
				Console.WriteLine($"Front element is {result}");
			}
		}

		public static void TestConcurrentStack()
		{
			var q = new ConcurrentStack<int>();
			q.Push(1);
			q.Push(2);
			q.Push(3);
			q.Push(4);
			int result;
			if (q.TryPeek(out result))
			{
				Console.WriteLine($"{result} is on top");
			}

			if (q.TryPop(out result))
			{
				Console.WriteLine($"Popped element is {result}");
			}

			var items = new int[5];
			if (q.TryPopRange(items, 0, 5) > 0)
			{
				var text = string.Join(", ", items.Select(i => i.ToString()));
				Console.WriteLine($"Popped these items: {text}");
			}
		}

		public static void TestConcurrentBag()
		{
			var bag = new ConcurrentBag<int>();
			var tasks = new List<Task>();
			for (int i = 0; i < 10; i++)
			{
				var j = i;
				tasks.Add(Task.Factory.StartNew(() =>{

					bag.Add(j);
					Console.WriteLine($"{Task.CurrentId} has added {j}");
					int result;
					if (bag.TryPeek(out result))
					{
						Console.WriteLine($"{Task.CurrentId} has peeked the value {result}");
					}
				}));
			}
			Task.WaitAll(tasks.ToArray());
			int last;
			if (bag.TryTake(out last))
			{
				Console.WriteLine($"I got {last}");
			}
		}


		static BlockingCollection<int> messages = new BlockingCollection<int>(
			new ConcurrentBag<int>(), 10
			);
		static CancellationTokenSource cts = new CancellationTokenSource();

		static Random random = new Random();

        public static void TestBlockingCollection()
        {
			Task.Factory.StartNew(TestBlockingCollectionInner, cts.Token);
            Console.ReadKey();
            cts.Cancel();
        }

        public static void TestBlockingCollectionInner() {
			var producer = Task.Factory.StartNew(RunProducer);
			var consumer = Task.Factory.StartNew(RunConsumer);
			try
			{
				Task.WaitAll(new[] { producer, consumer}, cts.Token );
				
			}
			catch (AggregateException ae)
			{
				ae.Handle((e)=>true);
			}
		}

        private static void RunProducer()
        {
			while (true)
			{
				cts.Token.ThrowIfCancellationRequested();
				int i = random.Next(100);
				messages.Add(i);
				Console.WriteLine($"+ {i} \t");
				Thread.Sleep(random.Next(1000));
			}
        }

        private static void RunConsumer()
        {
			foreach (var item in messages.GetConsumingEnumerable())
			{
				cts.Token.ThrowIfCancellationRequested();
				Console.WriteLine($"- {item} \t");
				Thread.Sleep(random.Next(1000));
			}
        }
    }
}
