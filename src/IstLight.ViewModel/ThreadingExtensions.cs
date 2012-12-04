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
using System.Windows.Threading;

namespace IstLight
{
    public static class ThreadingExtensions
    {
        public static void InvokeIfRequired(this Dispatcher dispatcher, Action action, DispatcherPriority priority = DispatcherPriority.DataBind)
        {
            if (dispatcher.Thread != Thread.CurrentThread)
                dispatcher.Invoke(priority, action);
            else
                action();
        }
    }
}
