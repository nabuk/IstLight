using System;
using System.Threading;

namespace IstLight.Services
{
    public class AsyncResultFromSyncJob<T> : IAsyncResult<T>
    {
        private Func<ValueOrError<T>> synchronousJob;
        private readonly AsyncOneTimeCallbackContainer<IAsyncResult<T>> callbackContainer = new AsyncOneTimeCallbackContainer<IAsyncResult<T>>();
        private readonly ManualResetEvent completionEvent;

        private void OnCompletion(IAsyncResult ar)
        {
            var jobResult = synchronousJob.EndInvoke(ar);
            this.Result = jobResult.Value;
            this.Error = jobResult.Error;
            this.IsCompleted = true;
            completionEvent.Set();

            synchronousJob = null;

            callbackContainer.FireCallbacks(this);

            completionEvent.Close();
        }

        public AsyncResultFromSyncJob(Func<ValueOrError<T>> synchronousJob)
        {
            if (synchronousJob == null) throw new ArgumentNullException("synchronousJob");

            this.synchronousJob = synchronousJob;
            this.completionEvent = new ManualResetEvent(false);
            synchronousJob.BeginInvoke(OnCompletion, null);
        }

        #region IAsyncResult<T>
        public T Result { get; private set; }
        public Exception Error { get; private set; }
        public bool IsCompleted { get; private set; }

        public bool Wait(int timeout = Timeout.Infinite)
        {
            if (!IsCompleted)
                return completionEvent.WaitOne(timeout);
            return true;
        }

        public void AddCallback(Action<IAsyncResult<T>> callback)
        {
            if (callback == null) throw new ArgumentNullException("callback");

            callbackContainer.AddCallback(callback);

            if (IsCompleted)
                callbackContainer.FireCallbacks(this);
        }
        #endregion
    }

    //Not used, not tested
    //public static class AsyncResultExtensions
    //{
    //    public static AsyncResultFromSyncJob<T> AsAsyncJob<T>(this Func<ValueOrError<T>> synchronousJob)
    //    {
    //        return new AsyncResultFromSyncJob<T>(synchronousJob);
    //    }
    //}
}
