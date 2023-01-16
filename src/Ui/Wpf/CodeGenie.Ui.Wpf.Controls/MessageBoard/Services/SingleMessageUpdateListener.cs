using CodeGenie.Ui.Wpf.Controls.MessageBoard.Models.Events;
using CodeGenie.Ui.Wpf.Controls.MessageBoard.Models.Messages;
using CodeGenie.Ui.Wpf.Controls.Shared.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.MessageBoard.Services
{
    public interface ISingleMessageUpdateListener
    {
        void RegisterForUpdates(string channel, Type type, Action<IEnumerable<MessageBase>> onMessageReceived);
        void UnregisterForUpdates();
    }

    public class SingleMessageUpdateListener : ISingleMessageUpdateListener
    {
        protected readonly IPeriodicEventService PeriodicEventService;
        protected readonly IMessageChannelRepository MessageRepository;
        public SingleMessageUpdateListener(IPeriodicEventService periodicEventService,
                                               IMessageChannelRepository messageRepository)
        {
            PeriodicEventService = periodicEventService;
            MessageRepository = messageRepository;
            PeriodicEventService.OnPeriodElapsed += (o, e) => CheckForUpdates();
        }

        string _registeredChannel = "";
        Type? _registeredMessageType = null;
        Action<IEnumerable<MessageBase>> _onMessageReceived;
        public void RegisterForUpdates(string channel, Type type, Action<IEnumerable<MessageBase>> onMessageReceived)
        {
            if (HasRegistered()) 
                UnregisterForUpdates();

            _registeredChannel = channel;
            _registeredMessageType = type;
            _onMessageReceived = onMessageReceived;
        }

        public void UnregisterForUpdates()
        {
            _registeredChannel = "";
            _registeredMessageType = null;
        }

        private DateTime? _lastMessageUpdate = null;
        protected void CheckForUpdates()
        {
            if (!HasRegistered()) return;
            var lastUpdateTime = MessageRepository.GetTimeOfLastMessage(_registeredChannel, _registeredMessageType);
            if (lastUpdateTime <= _lastMessageUpdate) return;
            var messages = MessageRepository.GetMessages(_registeredChannel, _registeredMessageType);

            var newMessages = messages.Where(m => _lastMessageUpdate == null || m.DateTime > _lastMessageUpdate).ToList();

            if (!newMessages.Any()) return;
            _lastMessageUpdate = lastUpdateTime;
            _onMessageReceived?.Invoke(newMessages);
        }

        protected bool HasRegistered()
            => !string.IsNullOrEmpty(_registeredChannel) && _registeredMessageType != null;
    }
}
