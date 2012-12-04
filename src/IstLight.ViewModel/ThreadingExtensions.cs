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
