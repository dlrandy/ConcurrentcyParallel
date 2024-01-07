using System.Net;
using System.Net.NetworkInformation;
namespace BestPractices
{
	internal class WorkstationState
	{
		internal static string Name { get; set; }
		internal static string IpAddress { get; set; }
		internal static bool IsNetworkAvailiable { get; set; }

		[ThreadStatic]
		internal static DateTime? NetworkConnectivityLastUpdated;
		static WorkstationState()
		{
			Name = Dns.GetHostName();
			IpAddress = GetLocalIpAddress(Name);
			IsNetworkAvailiable = NetworkInterface.GetIsNetworkAvailable();
			NetworkConnectivityLastUpdated = DateTime.UtcNow;
			Thread.Sleep(2000);
		}
	private static string GetLocalIpAddress(string hostName) {
		var hostEntry = Dns.GetHostEntry(hostName);
		foreach (var address in hostEntry.AddressList.Where(a=>a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
		{
			return address.ToString();
		}
		return string.Empty;
	}
	}
}

