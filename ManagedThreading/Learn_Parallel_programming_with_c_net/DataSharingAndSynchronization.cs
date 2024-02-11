using System;
namespace Learn_Parallel_programming_with_c_net
{
	public class BankAccount1
	{
        public object padlock = new object();
		public int Balance { get;private set; }
		public void Deposit(int amount) {
			lock (padlock)
			{

			Balance += amount;
			}
		}
		public void Withdraw(int amount) {

			lock (padlock)
			{

			Balance -= amount;
			}
		}
	}
    public class BankAccount2
    {
        private int balance;
        public int Balance
        {
            get
            {
                return balance;
            }
            private set { balance = value; }
        }
        public void Deposit(int amount)
        {
            Interlocked.Add(ref balance, amount);
        }
        public void Withdraw(int amount)
        {

            Interlocked.Add(ref balance, -amount);
        }
    }
    public class BankAccount
    {
        private int balance;
        public int Balance
        {
            get
            {
                return balance;
            }
            private set { balance = value; }
        }
        public void Deposit(int amount)
        {
            balance += amount;
        }
        public void Withdraw(int amount)
        {

            balance -= amount;
        }
        public void Transfer(BankAccount where, int amount) {

            Balance -= amount;
            where.Balance += amount;
        }
    }
    public class DataSharingAndSynchronization
	{

		public static void TestCriticalSection()
		{
			var tasks = new List<Task>();
			var ba = new BankAccount();
			for (int i = 0; i < 10; i++)
			{
				tasks.Add(Task.Factory.StartNew(() => {

					for (int i = 0; i < 1000; i++)
					{
						ba.Deposit(100);
					}
				}));
                tasks.Add(Task.Factory.StartNew(() => {

                    for (int i = 0; i < 1000; i++)
                    {
                        ba.Withdraw(100);
                    }
                }));
            }
			Task.WaitAll(tasks.ToArray());
			Console.WriteLine($"Final balance is {ba.Balance}");
		}

        public static void TestCriticalSection2()
        {
            var tasks = new List<Task>();
            var ba = new BankAccount();
            SpinLock sl = new SpinLock();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() => {

                    for (int i = 0; i < 1000; i++)
                    {
                        var lockTaken = false;
                        try
                        {
                            sl.Enter(ref lockTaken);
                            ba.Deposit(100);
                        }
                        finally
                        {
                            if (lockTaken)
                            {
                                sl.Exit();
                            }
                        }
                    }
                }));
                tasks.Add(Task.Factory.StartNew(() => {

                    for (int i = 0; i < 1000; i++)
                    {
                        var lockTaken = false;
                        try
                        {
                            sl.Enter(ref lockTaken);
                            ba.Withdraw(100);
                        }
                        finally
                        {
                            if (lockTaken)
                            {
                                sl.Exit();
                            }
                        }
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"Final balance is {ba.Balance}");
        }


        static SpinLock sl = new SpinLock(true);
        public static void LockRecursion(int x)
        {
            bool lockTaken = false;
            try
            {
                sl.Enter(ref lockTaken);
            }
            catch (LockRecursionException e) {
                Console.WriteLine($"");
            }
            finally
            {
                if (lockTaken)
                {
                    Console.WriteLine($"Took a lock, x = {x}");
                    LockRecursion(x - 1);
                    sl.Exit();
                }
                else {
                    Console.WriteLine($"Failed to take a lock, x= {x}");
                }
            }
        }

        public static void TestMutex() {

            var tasks = new List<Task>();
            var ba = new BankAccount();
            var ba2 = new BankAccount();
            Mutex mutex = new Mutex();
            Mutex mutex2 = new Mutex();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() => {

                    for (int j = 0; j < 1000; j++)
                    {
                        bool haveLock = mutex.WaitOne();
                        try
                        {

                        ba.Deposit(100);
                        }
                        finally
                        {
                            if (haveLock)
                            {
                                mutex.ReleaseMutex();
                            }
                        }
                    }
                }));
                tasks.Add(Task.Factory.StartNew(() => {

                    for (int j = 0; j < 1000; j++)
                    {
                        bool haveLock = mutex.WaitOne();
                        try
                        {

                            ba.Withdraw(100);
                        }
                        finally
                        {
                            if (haveLock)
                            {
                                mutex.ReleaseMutex();
                            }
                        }
                    }
                }));

            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"Final balance is {ba.Balance}.");
        }

        public static void TestDoubleMutex()
        {

            var tasks = new List<Task>();
            var ba = new BankAccount();
            var ba2 = new BankAccount();
            Mutex mutex = new Mutex();
            Mutex mutex2 = new Mutex();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() => {

                    for (int j = 0; j < 1000; j++)
                    {
                        bool haveLock = mutex.WaitOne();
                        try
                        {

                            ba.Deposit(1);
                        }
                        finally
                        {
                            if (haveLock)
                            {
                                mutex.ReleaseMutex();
                            }
                        }
                    }
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {

                    for (int j = 0; j < 1000; j++)
                    {
                        bool haveLock = mutex2.WaitOne();
                        try
                        {

                            ba2.Deposit(1);
                        }
                        finally
                        {
                            if (haveLock)
                            {
                                mutex2.ReleaseMutex();
                            }
                        }
                    }
                }));
                tasks.Add(Task.Factory.StartNew(() => {

                    for (int j = 0; j < 1000; j++)
                    {
                        bool haveLock = WaitHandle.WaitAll(new[] { mutex, mutex2} );
                        try
                        {

                            ba.Transfer(ba2, 1);
                        }
                        finally
                        {
                            if (haveLock)
                            {
                                mutex.ReleaseMutex();
                                mutex2.ReleaseMutex();
                            }
                        }
                    }
                }));

            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"Final balance ba is {ba.Balance}.");
            Console.WriteLine($"Final balance b2 is {ba2.Balance}.");
        }
        public static void TestGlobalMutex()
        {
            const string appName = "MyApp";
            Mutex mutex;
            try
            {
                mutex = Mutex.OpenExisting(appName);
                Console.WriteLine( $"Sorry, {appName} is Already Running");
            }
            catch (WaitHandleCannotBeOpenedException e)
            {
                Console.WriteLine("we can run the program just fine");
                mutex = new Mutex(false, appName);
            }
            Console.ReadKey();
            mutex.ReleaseMutex();
        }

        static ReaderWriterLockSlim padLock = new ReaderWriterLockSlim();
        static Random random = new Random();
        public static void TestReaderWriterLockSlim() {
            int x = 0;
            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                var y = i;
                tasks.Add(Task.Factory.StartNew(() => {
                    //padLock.EnterReadLock();
                    padLock.EnterUpgradeableReadLock();
                    if (i % 2 == 0)
                    {
                    padLock.EnterWriteLock();
                        x = y;
                        padLock.ExitWriteLock();
                    }
                    Console.WriteLine($"Entered read lock, x = {x}");
                    Thread.Sleep(5000);
                    padLock.ExitUpgradeableReadLock();
                    //padLock.ExitReadLock();
                    Console.WriteLine($"Exited read lock, x = {x}.");
                }));
            }
            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ae)
            {
                ae.Handle(e => {
                    Console.WriteLine(e);
                    return true;
                });
            }

            while (true)
            {
                Console.ReadKey();
                padLock.EnterWriteLock();
                Console.Write("Writing lock acqiured");
                int newValue = random.Next(10);
                x = newValue;
                Console.WriteLine($"Set x = {x}");
                padLock.ExitWriteLock();
            }
        }
    }
}

