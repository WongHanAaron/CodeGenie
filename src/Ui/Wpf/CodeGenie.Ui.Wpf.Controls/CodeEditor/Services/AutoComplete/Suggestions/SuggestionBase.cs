using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete.Suggestions
{
    /// <summary> The base auto completion object </summary>
    public abstract class SuggestionBase : ICompletionData
    {
        public SuggestionBase(TextEnterEventArgs args)
        {
            EventArguments = args;
        }

        /// <summary> The arguments that occurs when the autocompletion event occurs </summary>
        public TextEnterEventArgs EventArguments { get; protected set; }

        /// <summary> The icon on the autocompletion </summary>
        public ImageSource Image => null;

        /// <summary> The text shown on the autocompletion window </summary>
        public abstract string Text { get; }

        public virtual object Content => Text;

        /// <summary> The tooltip description on the autocompletion option </summary>
        public abstract object Description { get; }

        /// <summary> The priority in sorting on the autocompletion options </summary>
        public abstract double Priority { get; }

        /// <summary> The line number to update the caret number to. No update is made if the value is null </summary>
        public virtual int? CaretLineNumberPlacement { get; protected set; } = null;

        /// <summary> The column number to update the caret number to. No update is made if the value is null </summary>
        public virtual int? CaretColumnPlacement { get; protected set; } = null;

        /// <summary> The replacement text used in that spot </summary>
        public string ReplacementText { get; protected set; }

        public virtual void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            ReplacementText = GetReplacementText(insertionRequestEventArgs);
            textArea.Document.Replace(completionSegment, ReplacementText);
        }

        /// <summary> Get the text to replace at the completion segment </summary>
        public abstract string GetReplacementText(EventArgs insertionRequestEventArgs);
    }
}
