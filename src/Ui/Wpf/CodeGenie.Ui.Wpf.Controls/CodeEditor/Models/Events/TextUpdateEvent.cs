using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events
{
    /// <summary> The event to be raised when the text editor was updated </summary>
    public class TextUpdateEvent : EventArgs
    {
        /// <summary> DateTime when the text was updated </summary>
        public DateTime DateTime { get; set; }
        
        /// <summary> The text after it was updated </summary>
        public string Text { get; set; }
    }
}
