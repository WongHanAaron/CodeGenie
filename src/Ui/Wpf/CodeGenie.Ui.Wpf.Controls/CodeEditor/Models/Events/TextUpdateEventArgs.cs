using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events
{
    /// <summary> The event to be raised when the text editor was updated </summary>
    public class TextUpdateEventArgs : DateTimeEventArgs
    {
        /// <summary> The text after it was updated </summary>
        public string Text { get; set; }
    }
}
