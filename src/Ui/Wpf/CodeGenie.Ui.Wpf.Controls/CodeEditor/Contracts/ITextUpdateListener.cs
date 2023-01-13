using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using CodeGenie.Ui.Wpf.Controls.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Contracts
{
    /// <summary>
    /// The component to listen for text updates in the editor
    /// </summary>
    public interface ITextUpdateListener : IInjectsEditor
    {
        /// <summary> Raised when the text is updated </summary>
        public EventHandler<TextUpdateEventArgs> OnTextUpdated { get; set; }

        /// <summary> Raised when some text is entered </summary>
        public EventHandler<TextEnterEventArgs> OnTextEntered { get; set; }

        /// <summary> Raised when some text is being entered </summary>
        public EventHandler<TextEnterEventArgs> OnTextEntering { get; set; }
    }
}
