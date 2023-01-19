using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CodeGenie.Ui.Wpf.Controls.Shared.Services
{
    public interface IDispatcherService
    {
        void InvokeOnUiThread(Action method);

        TResult InvokeOnUiThread<TResult>(Func<TResult> method);
    }

    public class DispatcherService : IDispatcherService
    {
        Dispatcher Dispatcher;
        public DispatcherService(Dispatcher dispatcher)
        {
            Dispatcher = dispatcher;
        }

        public void InvokeOnUiThread(Action method)
        {
            Dispatcher.Invoke(method);
        }

        public TResult InvokeOnUiThread<TResult>(Func<TResult> method)
        {
            return Dispatcher.Invoke(method);
        }
    }
}
