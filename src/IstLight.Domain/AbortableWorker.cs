using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace IstLight.Domain
{
    public class AbortableWorker : IDisposable
    {
        private readonly Func<bool> externalWork;
        private readonly AutoResetEvent resumeEvent = new AutoResetEvent(false);
        private readonly ManualResetEvent workDoneEvent = new ManualResetEvent(false);

        private void Worker()
        {
            try
            {
                while (true)
                {
                    resumeEvent.WaitOne(Timeout.Infinite);

                    if (stopRequested)
                        return;

                    if (!(LastRunResult = externalWork()))
                        return;

                    workDoneEvent.Set();
                }
            }
            catch (ThreadAbortException) { }
            finally { workDoneEvent.Set(); }
        }
        private bool stopRequested = false;

        public AbortableWorker(Func<bool> externalWork)
        {
            if (externalWork == null) throw new ArgumentNullException("externalWork");
            this.externalWork = externalWork;

            ThreadInstance = new Thread(Worker) { Priority = ThreadPriority.Normal };
        }

        public Thread ThreadInstance { get; private set; }

        public void Start()
        {
            ThreadInstance.Start();
        }

        /// <summary>
        /// Allows inner thread to do externalWork call.
        /// </summary>
        public void ResumeWork()
        {
            workDoneEvent.Reset();
            resumeEvent.Set();
        }

        /// <summary>
        /// Waits for DoWork to finish.
        /// </summary>
        /// <param name="millisecondsTimeout">Timeout.</param>
        /// <returns>True if work is not still running.</returns>
        public bool WaitForWorkDone(int millisecondsTimeout = Timeout.Infinite)
        {
            return workDoneEvent.WaitOne(millisecondsTimeout);
        }

        /// <summary>
        /// Waits specified time for worker method to end. If time elapsed, aborts thread.
        /// </summary>
        /// <param name="millisecondsTimeout">Timeout before thread abort.</param>
        public void Abort(int millisecondsTimeout = 0)
        {
            if (Aborted)
                return;

            bool wasWorking = !workDoneEvent.WaitOne(0);
            stopRequested = true;
            resumeEvent.Set();

            if (!ThreadInstance.Join(wasWorking ? millisecondsTimeout : Timeout.Infinite))
                ThreadInstance.Abort();
        }

        public bool LastRunResult { get; private set; }

        public bool IsWaiting
        {
            get
            {
                return ThreadInstance.ThreadState == ThreadState.WaitSleepJoin;
            }
        }

        public bool Aborted
        {
            get
            {
                var state = ThreadInstance.ThreadState;
                return (state & (ThreadState.Aborted | ThreadState.AbortRequested | ThreadState.Stopped | ThreadState.StopRequested)) > 0;
            }
        }

        #region IDisposable
        public void Dispose()
        {
            if (ThreadInstance.ThreadState != ThreadState.Unstarted)
            {
                Abort();
                ThreadInstance.Join();
            }
            resumeEvent.Dispose();
            workDoneEvent.Dispose();
        }
        #endregion //IDisposable
    }
}
