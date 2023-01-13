using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.AutoCompletions.Syntax
{
    public abstract class SyntaxCompletionBase : ICompletionData
    {
        public virtual ImageSource Image => null;

        public abstract string Text { get; }

        public object Content => Text;

        public abstract object Description { get; }

        public virtual double Priority => 1;

        public abstract void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs);
    }
}
