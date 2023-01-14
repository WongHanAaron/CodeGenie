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
        /// <summary> Tells the TextUpdateListener that the text was updated </summary>
        void TextUpdated(object sender);

        /// <summary> Tells the TextUpdateListener that the entering </summary>
        void TextEnteringOccurred(object sender, string textEntering);

        /// <summary> Tells the TextUpdateListener that the text was entered </summary>
        void TextWasEntered(object sender, string textEntered);

        /// <summary> Raised when the text is updated </summary>
        EventHandler<TextUpdateEventArgs> OnTextUpdated { get; set; }

        /// <summary> Raised when some text is entered </summary>
        EventHandler<TextEnterEventArgs> OnTextEntered { get; set; }

        /// <summary> Raised when some text is being entered </summary>
        EventHandler<TextEnterEventArgs> OnTextEntering { get; set; }
        
        /// <summary> The current text in the text editor </summary>
        string CurrentText { get; }
    }
}
