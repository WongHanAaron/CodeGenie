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

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.AutoCompletions
{
    public abstract class AutoCompletionBase : ICompletionData
    {
        public AutoCompletionBase(TextEnterEventArgs args)
        {
            EventArguments = args;
        }

        public TextEnterEventArgs EventArguments { get; protected set; }

        public ImageSource Image => null;

        public abstract string Text { get; }

        public virtual object Content => Text;

        public abstract object Description { get; }

        public abstract double Priority { get; }

        public abstract void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs);
    }
}
