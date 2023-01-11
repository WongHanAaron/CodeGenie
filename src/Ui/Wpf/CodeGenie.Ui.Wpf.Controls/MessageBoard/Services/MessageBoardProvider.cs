using CodeGenie.Ui.Wpf.Controls.MessageBoard.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.MessageBoard.Services
{
    public class MessageBoardProvider : IMessageBoardProvider
    {
        private readonly LogLevel _minimumLogLevel;
        private readonly string _channel;
        public MessageBoardProvider(string channel, LogLevel minimumLogLevel = LogLevel.Debug)
        {
            _channel = channel;
            _minimumLogLevel = minimumLogLevel;
        }

        public ILogger CreateLogger(string categoryName) => new MessageBoardLogger(_channel, categoryName, _minimumLogLevel);
        public void Dispose() { }
    }
}
