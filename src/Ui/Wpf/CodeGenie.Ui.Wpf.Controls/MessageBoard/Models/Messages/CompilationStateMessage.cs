using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.MessageBoard.Models.Messages
{
    /// <summary> The data model for status message of the message board </summary>
    public class CompilationStateMessage
    {
        /// <summary> The level of severity for this state </summary>
        public LogLevel Level { get; set; }

        /// <summary> The message for this state message </summary>
        public string Message { get; set; }
    }
}
