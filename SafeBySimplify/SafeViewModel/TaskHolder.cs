using System;
using System.Threading;
using System.Threading.Tasks;

namespace SafeViewModel
{
    public class TaskHolder
    {
        public TaskHolder(Action<CancellationTokenSource> taskDelegate)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _runningTask = Task.Run(() =>
            {
                try
                {
                    taskDelegate.Invoke(_cancellationTokenSource);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }

        public Task _runningTask;

        private CancellationTokenSource _cancellationTokenSource;

        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }

        public void WaitOnHoldingTask()
        {
            _runningTask.Wait();
        }

    }
}