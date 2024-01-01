// See https://aka.ms/new-console-template for more information
using Basics;
Console.WriteLine("Hello, World!");
//var a = new TimerSample();
//var b = new ThreadingTimerSample();

//a.StartTimer();

//Thread.Sleep(5000);
//a.StopTimer();
//a.Dispose();

//b.StartTimer();

//Thread.Sleep(5000);
//await b.DisposeTimerAsync();


//var a = new ParallelInvoke();

////a.DoWorkInParallel();
//var numbers = new List<int> { 1, 3, 5, 7, 9, 0 };
////a.ExecuteParallelForEach(numbers);



//var linqNumbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };


//a.ExecuteLinqQuery(linqNumbers);
//a.ExecuteParallelLinqQuery(linqNumbers);



var networkHelper = new NetworkHelper();
//await networkHelper.CheckNetworkStatusAsync();


networkHelper.BackgroundPing();
Console.WriteLine("Main method complete.");
Console.ReadKey();















