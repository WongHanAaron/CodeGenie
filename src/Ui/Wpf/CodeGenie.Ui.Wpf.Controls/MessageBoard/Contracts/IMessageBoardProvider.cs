using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.MessageBoard.Contracts
{
    /// <summary> Implements an interface to inject a logger provider that will load messages to the message board </summary>
    public interface IMessageBoardProvider : ILoggerProvider
    {
    }
}
