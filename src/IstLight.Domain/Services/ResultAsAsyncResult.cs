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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace IstLight.Services
{
    public class ResultAsAsyncResult<T> : IAsyncResult<T>
    {
        private readonly OneTimeCallbackContainer<IAsyncResult<T>> callbackContainer = new OneTimeCallbackContainer<IAsyncResult<T>>();
        private readonly ManualResetEvent completionEvent;

        public ResultAsAsyncResult()
        {
            this.completionEvent = new ManualResetEvent(false);
        }

        public void SetResult(ValueOrError<T> result)
        {
            this.Result = result.Value;
            this.Error = result.Error;
            this.IsCompleted = true;

            completionEvent.Set();
            callbackContainer.FireCallbacks(this);
            completionEvent.Close();
        }

        #region IAsyncResult
        public T Result
        {
            get; private set;
        }

        public Exception Error
        {
            get; private set;
        }

        public bool IsCompleted
        {
            get;
            private set;
        }

        public bool Wait(int timeout)
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
        #endregion // IAsyncResult
    }
}
