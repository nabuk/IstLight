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

namespace IstLight.Services
{
    public class ResultAsAsyncResult<T> : IAsyncResult<T>
    {
        public ResultAsAsyncResult(ValueOrError<T> result)
        {
            this.Result = result.Value;
            this.Error = result.Error;
        }
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
            get { return true; }
        }

        public bool Wait(int timeout)
        {
            return true;
        }

        public void AddCallback(Action<IAsyncResult<T>> callback)
        {
            callback(this);
        }
    }
}
