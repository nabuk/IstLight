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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace IstLight.Services
{
    public class OneTimeCallbackContainer<T>
    {
        private readonly List<Action<T>> callbacks = new List<Action<T>>();
        private readonly object locker = new object();

        public void AddCallback(Action<T> callback)
        {
            lock (locker)
                callbacks.Add(callback);
        }

        public void RemoveCallback(Action<T> callback)
        {
            lock (locker)
                callbacks.RemoveAll(x => x == callback);
        }

        public void FireCallbacks(T arg)
        {
            lock (locker)
            {
                callbacks.ForEach(x => FireCallback(x, arg));
                callbacks.Clear();
            }
        }

        protected virtual void FireCallback(Action<T> callback, T arg)
        {
            callback(arg);
        }
    }
}
