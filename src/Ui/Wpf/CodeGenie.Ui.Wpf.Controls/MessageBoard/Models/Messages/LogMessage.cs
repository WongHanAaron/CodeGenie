using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.MessageBoard.Models.Messages
{
    /// <summary> An event instance of a logging that was added </summary>
    public class LogMessage : MessageBase
    {
        public LogMessage(string fullSourceName, LogLevel level, string message, Exception exception = null)
        {
            FullSourceName = fullSourceName;
            ShortSourceName = FullSourceName.Split(".").LastOrDefault();
            Level = level;
            Message = message;
            Exception = exception;
        }

        /// <summary> The full source name of the logger category </summary>
        public string FullSourceName { get; set; }

        /// <summary> The last component name of the source </summary>
        public string ShortSourceName { get; set; }

        /// <summary> The logging level for this log </summary>
        public LogLevel Level { get; set; }

        /// <summary> The message for this log </summary>
        public string Message { get; set; }
        
        /// <summary> Any exceptions for this log </summary>
        public Exception Exception { get; set; }
    }
}
