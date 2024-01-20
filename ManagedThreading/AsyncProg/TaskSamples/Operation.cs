using System;
namespace AsyncProg.TaskSamples
{
	public class Operation
	{
		public void ProcessOrders(List<Order> orders, int customerId)
		{
			Task<List<Order>> processOrdersTask = Task.Run(()=>PerpareOrders(orders));
			Task labelTask = Task.Factory.StartNew(()=> CreateLabels(orders), TaskCreationOptions.LongRunning);
            Task sendTask = processOrdersTask.ContinueWith(task => SendOrders(task.Result));
            Task.WaitAll(new[] { labelTask, sendTask} );
            SendConfirmation(customerId);
		}
        public void ProcessData(object data, bool uiRequired) {
            Task processTask = new (() => DoDataProcessing(data));
            if (uiRequired)
            {
                processTask.RunSynchronously();
            }
            else {
                processTask.Start();
            }
        }

        private object DoDataProcessing(object data)
        {
            throw new NotImplementedException();
        }

        private void SendConfirmation(int customerId)
        {
            throw new NotImplementedException();
        }

        private void SendOrders(List<Order> result)
        {
            throw new NotImplementedException();
        }

        private void CreateLabels(List<Order> orders)
        {
            throw new NotImplementedException();
        }

        private Task<List<Order>>? PerpareOrders(List<Order> orders)
        {
            throw new NotImplementedException();
        }
    }
}

