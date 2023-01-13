using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Windows.Media;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.AutoCompletions.Syntax
{
    public abstract class SyntaxCompletionBase : ICompletionData
    {
        public SyntaxCompletionBase(TextEnterEventArgs eventArgs)
        {
            EventArgs = eventArgs;
        }

        public TextEnterEventArgs EventArgs { get; protected set; }

        public virtual ImageSource Image => null;

        public abstract string Text { get; }

        public object Content => Text;

        public abstract object Description { get; }

        public virtual double Priority => 1;

        public abstract void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs);
    }
}
