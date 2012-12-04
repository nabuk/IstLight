// Copyright 2012 Jakub Niemyjski
//
// This file is part of IstLight.
//
// IstLight is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// IstLight is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with IstLight.  If not, see <http://www.gnu.org/licenses/>.

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
