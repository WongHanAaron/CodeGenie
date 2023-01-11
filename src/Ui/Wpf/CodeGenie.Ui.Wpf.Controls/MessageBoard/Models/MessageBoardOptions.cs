using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.MessageBoard.Models
{
    public class MessageBoardOptions
    {
        public int MaxLogMessageCount { get; set; } = 50;
        public string MessageBoard { get; set; } = "default";
    }
}
