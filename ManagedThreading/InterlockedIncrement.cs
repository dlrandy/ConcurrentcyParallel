using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ManagedThreading
{
	public class InterlockedIncrement
	{
		private static readonly object _lockObj = new object();
		private static int _isSafe = 1;
		private static bool _isSafeBool = true;

		public static void Main(string[] args)
		{
            //var src = Enumerable.Range(1, 100_000);
            //Func<int, bool> predicate = n => n % 2 == 0;
            //         Measure(NOSynchronize, src, predicate, "WARM_UP");
            //         Measure(NOSynchronize, src, predicate, nameof(NOSynchronize));
            //         Measure(WithLock, src, predicate, nameof(WithLock));
            //         Measure(InterLockedIncrement, src, predicate, nameof(InterlockedIncrement));

            //Measure(() => CheckWithLock(100), "WARM UP");
            //Measure(() => CheckWithLock(500_000), nameof(CheckWithLock));
            //Measure(() => CheckWithInterlocked(500_000), nameof(CheckWithInterlocked));


            var src = Enumerable.Range(1, 100_000);
            Measure(NoSynchronize, src, "WARM_UP");
            Measure(NoSynchronize, src, nameof(NoSynchronize));
            Measure(WithLock, src, nameof(WithLock));
            Measure(WithLockLocalVar, src, nameof(WithLockLocalVar));
            Measure(InterlockedAdd, src, nameof(InterlockedAdd));
            Measure(InterlockedAddLocalVar, src, nameof(InterlockedAddLocalVar));

        }
		public static void CheckWithLock(int load) {
			Parallel.For(0, load, (i, loop) => {
				lock (_lockObj)
				{
					if (!_isSafeBool)
					{
						return;
					}
					else {
						_isSafeBool = false;
					}
				}
				DummyDoWork();
				lock (_lockObj)
				{
					_isSafeBool = true;
				}

			});
		}
		public static void CheckWithInterlocked(int load)
		{
			Parallel.For(0, load, (i, loop) => {
				if (Interlocked.Exchange(ref _isSafe, 0) == 1)
				{
					DummyDoWork();
					Interlocked.Exchange(ref _isSafe, 1);
				}
			});
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		static void DummyDoWork() {
			var a = 1;
		}
        public static void Measure(Func<IEnumerable<int>, long> func, IEnumerable<int> src, string caseName)
        {
            var sw = new Stopwatch();
            sw.Start();
            var sum = func(src);
            sw.Stop();
            var ts = sw.ElapsedMilliseconds;
            Console.WriteLine($"Result for {caseName}: {sum}.Runtime: {ts}ms");
        }
        public static void Measure(Func<IEnumerable<int>, Func<int, bool>, long> func,
			IEnumerable<int> src, Func<int, bool> predicate, string caseName)
		{
			var sw = new Stopwatch();
			sw.Start();
			var sum = func(src, predicate);
			sw.Stop();
			var ts = sw.ElapsedMilliseconds;
			Console.WriteLine($"Result for {caseName}: {sum}.Runtime:{ts} ms");
		}
		public static void Measure(Action act, string caseName) {
            var sw = new Stopwatch();
            sw.Start();
            act();
            sw.Stop();
            var ts = sw.ElapsedMilliseconds;
            Console.WriteLine($"Result for {caseName}: {ts}ms");
        }
		public static long InterLockedIncrement(IEnumerable<int> src, Func<int, bool> predicate) {

			long sum = 0;
			Parallel.ForEach(src, n => {
				if (predicate(n))
				{
					Interlocked.Increment(ref sum);
				}
			});
			return sum;
		}
		public static long WithLock(IEnumerable<int> src, Func<int, bool> predicate)
		{
			long sum = 0;
			Parallel.ForEach(src, n => {
				if (predicate(n))
				{
					lock (_lockObj)
					{
						sum++;
					}
				}

			});

			return sum;
		}

		public static long NOSynchronize(IEnumerable<int> src, Func<int, bool> predicate)
		{
			long sum = 0;
			Parallel.ForEach(src, n =>
			{
				if (predicate(n))
				{
					sum++;
				}
			});
			return sum;


		}
		public static long InterlockedAdd(IEnumerable<int> src) {
			long sum = 0;

			Parallel.ForEach(src, n => Interlocked.Add(ref sum, n));

			return sum;
		}
		public static long InterlockedAddLocalVar(IEnumerable<int> src) {
			long sum = 0;
			Parallel.ForEach<int, long>(
				src,
				()=> 0,
				(n,_, localSum) => localSum += n,
				threadSum => Interlocked.Add(ref sum, threadSum)
				);
			return sum;

		}
		public static long WithLock(IEnumerable<int> src)
		{
			long sum = 0;

			Parallel.ForEach(src, n => {

				lock (_lockObj)
				{
					sum += n;
				}
			});

			return sum;
		}

		public static long WithLockLocalVar(IEnumerable<int> src)
		{
			long sum = 0;
			Parallel.ForEach<int, long>(
				src,
				()=> 0,
				(n, _, localSum) => localSum += n,
				threadSum => {
					lock (_lockObj)
					{
						sum += threadSum;
					}
				}
				);
			return sum;
		}

		public static long NoSynchronize(IEnumerable<int> src)
		{
			long sum = 0;
			Parallel.ForEach(src, n => sum += n);
			return sum;
		}
	}
}

