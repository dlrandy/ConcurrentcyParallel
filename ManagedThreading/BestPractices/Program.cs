// See https://aka.ms/new-console-template for more information
using BestPractices;

Console.WriteLine("Hello, World!");

//var interlockered = new InterlockedEa();
//interlockered.PerformCalculations();

//List<string> strings = new List<string> {
//"a","b","c","d"
//};
//var threadLimits = new ThreadingLimit();
//threadLimits.ProcessParallelForEachWithLimits(strings);
//var bolo = threadLimits.ProcessPlinqWithLimits(strings);
//Console.WriteLine(bolo);

var helper = new WorkstationHelper();
await helper.GetNetworkAvailability();

Parallel.For(1, 30, async (x) =>
{
    await helper.GetNetworkAvailability();
});
await helper.GetNetworkAvailabilityFromSingleton();
Console.WriteLine($"Network availability last updated {WorkstationState.NetworkConnectivityLastUpdated} for computer {WorkstationState.Name} at IP {WorkstationState.IpAddress}");
