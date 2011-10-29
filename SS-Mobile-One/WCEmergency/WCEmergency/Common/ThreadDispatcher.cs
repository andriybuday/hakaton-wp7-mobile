using System;
using System.ComponentModel;
using GalaSoft.MvvmLight.Threading;

namespace WCEmergency.Common
{
    public interface IThreadDispatcher
    {
        void RunOnUiThread(Action action);
        void RunOnBackgroundThread(Action action, Action<Exception> exceptionAction = null);
    }

    public class ThreadDispatcher : IThreadDispatcher
    {
        public void RunOnUiThread(Action action)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(action);
        }

        public void RunOnBackgroundThread(Action action, Action<Exception> exceptionAction = null)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (e, arg) =>
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception exception)
                {
                    if (exceptionAction == null)
                        throw;

                    exceptionAction.Invoke(exception);
                }
            };
            worker.RunWorkerAsync();
        }
    }
}
