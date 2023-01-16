using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.MessageBoard.Models.Messages
{
    public abstract class MessageBase
    {
        /// <summary> DateTime when the message occurred </summary>
        public DateTime DateTime { get; set; }
    }
}
