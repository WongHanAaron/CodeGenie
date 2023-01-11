using CodeGenie.Ui.Wpf.Controls.MessageBoard.Models.Messages;
using CodeGenie.Ui.Wpf.Controls.Shared.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CodeGenie.Ui.Wpf.Controls.MessageBoard.Services.MessageChannelRepository;

namespace CodeGenie.Ui.Wpf.Controls.MessageBoard.Services
{
    /// <summary>
    /// The message channel repository for the set of messages stored on the message board.
    /// All messages are stored statically by channel. This allows for any classes at any level to add to
    /// the message channel repository if need be
    /// </summary>
    public interface IMessageChannelRepository
    {
        /// <summary> Add a new message to the channel at that specific message type </summary>
        void AddMessage<TMessage>(string messageChannel, TMessage message) where TMessage : MessageBase;

        /// <summary> Clear the messages in this channel at that specified message type </summary>
        void Clear<TMessage>(string messageChannel) where TMessage : MessageBase;

        /// <summary> Clear all messages from this message channel </summary>
        void Clear(string messageChannel);

        /// <summary> Gets the messages at this channel </summary>
        IEnumerable<TMessage> GetMessages<TMessage>(string messageChannel) where TMessage : MessageBase;
        
        /// <summary> Gets the messages at this channel </summary>
        IEnumerable<MessageBase> GetMessages(string messageChannel, Type type);

        /// <summary> Get the time of the last created message </summary>
        DateTime? GetTimeOfLastMessage<TMessage>(string messageChannel) where TMessage : MessageBase;

        /// <summary> Get the time of the last created message </summary>
        DateTime? GetTimeOfLastMessage(string messageChannel, Type type);
    }

    public class MessageChannelRepository : IMessageChannelRepository
    {
        private static readonly ConcurrentDictionary<string, MessageChannel> _channels = 
            new ConcurrentDictionary<string, MessageChannel>();

        protected readonly IDateTimeProvider DateTimeProvider;

        public MessageChannelRepository(IDateTimeProvider dateTimeProvider)
        {
            DateTimeProvider = dateTimeProvider;
        }

        public void AddMessage<TMessage>(string messageChannel, TMessage message) where TMessage : MessageBase
        {
            var channel = GetChannel(messageChannel);
            channel.AddMessage(message);
            if (message is LogMessage logMessage)
            {
                channel.DequeueUntil<LogMessage>(10); // TODO change the limit
            }
        }

        public void Clear<TMessage>(string messageChannel) where TMessage : MessageBase
        {
            var channel = GetChannel(messageChannel);
            channel.Clear<TMessage>();
        }

        public void Clear(string messageChannel)
        {
            var channel = GetChannel(messageChannel);
            channel.Clear();
        }

        public IEnumerable<TMessage> GetMessages<TMessage>(string messageChannel) where TMessage : MessageBase
            => GetMessages(messageChannel, typeof(TMessage)).Select(m => m as TMessage);

        public IEnumerable<MessageBase> GetMessages(string messageChannel, Type type)
        {
            var channel = GetChannel(messageChannel);
            return channel.GetMessages(type);
        }

        protected MessageChannel GetChannel(string channel)
        {
            EnsureCreated(channel);
            return _channels[channel];
        }

        protected void EnsureCreated(string channel)
        {
            if (!_channels.ContainsKey(channel))
            {
                _channels[channel] = new MessageChannel(DateTimeProvider);
            }
        }

        public DateTime? GetTimeOfLastMessage<TMessage>(string messageChannel) where TMessage : MessageBase
            => GetTimeOfLastMessage(messageChannel, typeof(TMessage));

        public DateTime? GetTimeOfLastMessage(string messageChannel, Type type)
        {
            if (!_channels.ContainsKey(messageChannel)) return null;
            return _channels[messageChannel].GetTimeOfLastMessage(type);
        }

        /// <summary> A channel of messages by message type </summary>
        public class MessageChannel
        {
            private readonly ConcurrentDictionary<Type, ConcurrentQueueWithDateTime<MessageBase>> _messages = 
                new ConcurrentDictionary<Type, ConcurrentQueueWithDateTime<MessageBase>>();

            IDateTimeProvider DateTimeProvider;
            public MessageChannel(IDateTimeProvider dateTimeProvider)
            {
                DateTimeProvider = dateTimeProvider;
            }

            /// <summary> Add a message to the specified channel </summary>
            public void AddMessage<TMessage>(TMessage message) where TMessage : MessageBase
            {
                lock (this)
                {
                    var key = EnsureCreated<TMessage>();
                    _messages[key].DateTime = DateTimeProvider.Now;
                    _messages[key].Enqueue(message);
                }
            }
            
            /// <summary> Clear messages from the specified channel </summary>
            public void Clear<TMessage>() where TMessage : MessageBase
            {
                lock (this)
                {
                    var key = EnsureCreated<TMessage>();
                    _messages[key].DateTime = DateTimeProvider.Now;
                    _messages[key].Clear();
                }
            }

            public IEnumerable<TMessage> GetMessages<TMessage>() where TMessage : MessageBase
                => GetMessages(typeof(TMessage)).Select(m => m as TMessage);

            public IEnumerable<MessageBase> GetMessages(Type type)
            {
                var key = EnsureCreated(type);
                return _messages[key].ToList();
            }

            protected Type EnsureCreated<TMessage>() where TMessage : MessageBase
            {
                var key = typeof(TMessage);
                return EnsureCreated(key);
            }

            protected Type EnsureCreated(Type key)
            {
                if (!_messages.ContainsKey(key))
                {
                    _messages[key] = new ConcurrentQueueWithDateTime<MessageBase>(DateTimeProvider);
                }
                return key;
            }

            /// <summary> Clear everything from the message channel </summary>
            public void Clear()
            {
                _messages.Clear();
            }

            public void DequeueUntil<TMessage>(int count)
            {
                lock (this)
                {
                    var key = typeof(TMessage);
                    if (!_messages.ContainsKey(key))
                    {
                        _messages[key] = new ConcurrentQueueWithDateTime<MessageBase>();
                    }
                    
                    while (_messages[key].Count() > count)
                    {
                        _messages[key].TryDequeue(out var removedMessage);
                    }
                }
            }

            /// <summary> Get the date time of the last message that was added to the channel </summary>s
            public DateTime? GetTimeOfLastMessage<TMessage>() where TMessage : MessageBase
                => GetTimeOfLastMessage(typeof(TMessage));

            /// <summary> Get the date time of the last message that was added to the channel </summary>s
            public DateTime? GetTimeOfLastMessage(Type type)
            {
                var key = type;
                if (_messages.ContainsKey(key)) return null;
                return _messages[key].DateTime;
            }

            public class ConcurrentQueueWithDateTime<T> : ConcurrentQueue<T>
            {
                public ConcurrentQueueWithDateTime() : this(new DateTimeProvider()) { }

                public ConcurrentQueueWithDateTime(IDateTimeProvider dateTimeProvider)
                {
                    DateTime = dateTimeProvider.Now;
                }

                public DateTime DateTime { get; set; }
            }
        }
    }
}
