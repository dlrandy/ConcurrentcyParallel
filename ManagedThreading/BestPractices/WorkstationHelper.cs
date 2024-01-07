using System;
using System.Net.NetworkInformation;
namespace BestPractices
{
	internal class WorkstationHelper
	{
		private static object _workstationLock = new object();
		internal async Task<bool> GetNetworkAvailability()
		{
			await Task.Delay(100);
			lock (_workstationLock)
			{
				WorkstationState.IsNetworkAvailiable = NetworkInterface.GetIsNetworkAvailable();
				WorkstationState.NetworkConnectivityLastUpdated = DateTime.UtcNow;
			}
			return WorkstationState.IsNetworkAvailiable;
		}
		public WorkstationHelper()
		{
		}
		internal async Task<bool> GetNetworkAvailabilityFromSingleton() {

			await Task.Delay(100);
			var state = WorkstationStateSingleton.Instance;
			lock (_workstationLock)
			{
				state.IsNetworkAvailiable = NetworkInterface.GetIsNetworkAvailable();
				state.NetworkConnectivityLastUpdated = DateTime.UtcNow;

			}
			return state.IsNetworkAvailiable;

		}
	}
}

