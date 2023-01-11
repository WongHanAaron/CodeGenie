using CodeGenie.Ui.Wpf.Controls.MessageBoard.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.MessageBoard.Models.Events
{
    /// <summary> The event arguments for when a message board update has occurred </summary>
    public class MessageBoardUpdatedEventArgs
    {
        /// <summary> The channel that the message was from </summary>
        public string Channel { get; set; }

        /// <summary> The message type that the messages are </summary>
        public Type MessageType { get; set; }

        /// <summary> The current set of messages </summary>
        public IEnumerable<MessageBase> Messages { get; set; }
    }
}
