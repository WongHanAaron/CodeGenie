using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
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
    public interface ITextUpdateListener
    {
        /// <summary> To be invoked when the text is updated </summary>s
        void TextWasUpdated(object sender, TextUpdateEvent evt);

        /// <summary> Raised when the text is updated </summary>
        public EventHandler<TextUpdateEvent> OnTextUpdated { get; set; }
    }
}
