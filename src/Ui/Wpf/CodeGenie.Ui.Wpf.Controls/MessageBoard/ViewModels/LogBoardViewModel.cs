using CodeGenie.Ui.Wpf.Controls.MessageBoard.Models;
using CodeGenie.Ui.Wpf.Controls.MessageBoard.Models.Messages;
using CodeGenie.Ui.Wpf.Controls.MessageBoard.Services;
using CodeGenie.Ui.Wpf.Controls.Shared;
using CodeGenie.Ui.Wpf.Controls.Shared.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.MessageBoard.ViewModels
{
    public class LogBoardViewModel : ViewModelBase
    {
        protected readonly IDispatcherService DispatcherService;
        protected readonly ISingleMessageUpdateListener UpdateListener;
        public LogBoardViewModel(MessageBoardOptions options,
                                 IDispatcherService dispatcherService,
                                 ISingleMessageUpdateListener updateListener)
        {
            DispatcherService = dispatcherService;
            UpdateListener = updateListener;
            UpdateListener.RegisterForUpdates(options.MessageBoard, typeof(LogMessage), OnMessageReceived);
            Messages = new ObservableCollection<LogMessage>();
        }

        public ObservableCollection<LogMessage> Messages { get => Get<ObservableCollection<LogMessage>>(); set => Set(value); }

        public void OnMessageReceived(IEnumerable<MessageBase> messages)
        {
            DispatcherService.InvokeOnUiThread(() =>
            {
                Messages.Clear();
                messages.Select(m => m as LogMessage).ToList().ForEach(m => Messages.Add(m));
            });
        }
    }
}
