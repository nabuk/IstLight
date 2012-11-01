using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IstLight.Services
{
    public class AsyncResultFromSyncJob<T> : IAsyncResult<T>, IDisposable
    {
        private Func<ResultOrError<T>> synchronousJob;
        private readonly ManualResetEvent completedEvent = new ManualResetEvent(false);
        private readonly ConcurrentBag<WeakReference> completionCallbacks = new ConcurrentBag<WeakReference>();

        private void OnCompletion(IAsyncResult ar)
        {
            var jobResult = synchronousJob.EndInvoke(ar);
            this.Result = jobResult.Result;
            this.Error = jobResult.Error;
            this.completedEvent.Set();

            synchronousJob = null;

            FireCallbacks();
        }
        private void FireCallbacks()
        {
            WeakReference wr;
            Action<IAsyncResult<T>> callback;

            while (completionCallbacks.TryTake(out wr))
                if ((callback = wr.Target as Action<IAsyncResult<T>>) != null)
                    Task.Factory.StartNew(() => callback(this));
        }

        public AsyncResultFromSyncJob(Func<ResultOrError<T>> synchronousJob)
        {
            this.synchronousJob = synchronousJob;
            synchronousJob.BeginInvoke(OnCompletion, null);
        }

        #region IAsyncResult<T>
        public T Result { get; private set; }
        public Exception Error { get; private set; }
        public bool IsCompleted { get { return this.completedEvent.WaitOne(0); } }

        public bool Wait(int timeout = Timeout.Infinite)
        {
            return completedEvent.WaitOne(timeout);
        }

        public void AddCallback(Action<IAsyncResult<T>> callback)
        {
            completionCallbacks.Add(new WeakReference(callback));
            
            if(IsCompleted)
                FireCallbacks();
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            completedEvent.Dispose();
        }
        #endregion
    }

    public static class AsyncResultExtensions
    {
        public static AsyncResultFromSyncJob<T> AsAsyncJob<T>(this Func<ResultOrError<T>> synchronousJob)
        {
            return new AsyncResultFromSyncJob<T>(synchronousJob);
        }
    }
}
