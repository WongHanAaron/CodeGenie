using CodeGenie.Ui.Wpf.Controls.MessageBoard.Models.Messages;
using CodeGenie.Ui.Wpf.Controls.Shared.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.MessageBoard.Services
{
    public class MessageBoardLogger : ILogger
    {
        private LogLevel _minimumLogLevel;
        private string _channel;
        private string _categoryName;
        private readonly IMessageChannelRepository _channelRepository;
        private IDateTimeProvider _dateTimeProvider;
        public MessageBoardLogger(string channel, string categoryName, LogLevel minimumLogLevel = LogLevel.Debug, IDateTimeProvider dateTimeProvider = null)
        {
            _dateTimeProvider = dateTimeProvider ?? new DateTimeProvider();
            _minimumLogLevel = minimumLogLevel;
            _channel = channel;
            _categoryName = categoryName;
            _channelRepository = new MessageChannelRepository(_dateTimeProvider);
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return default;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _minimumLogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var message = formatter?.Invoke(state, exception);
            
            _channelRepository.AddMessage(_channel, new LogMessage(_categoryName, logLevel, message));
        }
    }
}
