﻿using System;
using System.Threading;
using System.Windows.Threading;

namespace IstLight
{
    /// <summary>
    /// WPF Threading extension methods
    /// </summary>
    public static class WPFControlThreadingExtensions
    {
        #region Public Methods
        /// <summary>
        /// A simple WPF threading extension method, to invoke a delegate
        /// on the correct thread if it is not currently on the correct thread
        /// Which can be used with DispatcherObject types
        /// </summary>
        /// <param name="disp">The Dispatcher object on which to do the Invoke</param>
        /// <param name="dotIt">The delegate to run</param>
        /// <param name="priority">The DispatcherPriority</param>
        public static void InvokeIfRequired(this Dispatcher disp,
            Action dotIt, DispatcherPriority priority = DispatcherPriority.DataBind)
        {
            if (disp.Thread != Thread.CurrentThread)
            {
                disp.Invoke(priority, dotIt);
            }
            else
                dotIt();
        }
        #endregion
    }
}
