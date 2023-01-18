using CodeGenie.Ui.Wpf.Controls.MessageBoard.Models;
using CodeGenie.Ui.Wpf.Controls.MessageBoard.Models.Messages;
using CodeGenie.Ui.Wpf.Controls.MessageBoard.Services;
using CodeGenie.Ui.Wpf.Controls.Shared;
using CodeGenie.Ui.Wpf.Controls.Shared.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

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
            MessagesView = CollectionViewSource.GetDefaultView(Messages);
            MessagesView.Filter = Filter;
            Sources = new ObservableCollection<string>();
            MaxLogCount = 100;
            SelectedSource = NoneSourceFilter;
            Sources.Add(NoneSourceFilter);
        }

        public int MaxLogCount { get => Get<int>(); set => Set(value); }
        public const string NoneSourceFilter = "None";
        public string SelectedSource 
        {
            get => Get<string>();
            set
            {
                Set(value);
                MessagesView.Refresh();
            }
        }
        private HashSet<string> _sources = new HashSet<string>();
        public ObservableCollection<string> Sources { get => Get<ObservableCollection<string>>(); set => Set(value); }
        public ObservableCollection<LogMessage> Messages { get => Get<ObservableCollection<LogMessage>>(); set => Set(value); }
        public ICollectionView MessagesView { get => Get<ICollectionView>(); set => Set(value); }
        public void OnMessageReceived(IEnumerable<MessageBase> messages)
        {
            var logs = messages.Select(m => m as LogMessage);
            
            DispatcherService.InvokeOnUiThread(() =>
            {
                lock (this)
                {
                    logs.ToList().ForEach(m =>
                    {
                        Messages.Add(m);
                        if (!_sources.Contains(m.ShortSourceName))
                        {
                            _sources.Add(m.ShortSourceName);
                            Sources.Add(m.ShortSourceName);
                        }
                    });

                    while (Messages.Count() > MaxLogCount)
                    {
                        if (Messages.Count() > 0)
                        {
                            Messages.RemoveAt(Messages.Count() - 1);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            });
        }

        private bool Filter(object obj)
        {
            if (string.IsNullOrWhiteSpace(SelectedSource)) return true;
            if (SelectedSource.Equals(NoneSourceFilter)) return true;
            if (obj is LogMessage log)
            {
                return Regex.IsMatch(log.ShortSourceName, SelectedSource);
            }
            return false;
        }
    }
}
