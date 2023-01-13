using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.AutoCompletions.Syntax
{
    public class ClassPartCompletion : SyntaxCompletionBase
    {
        public override string Text => "Class";

        public override object Description => "Add class as component type";

        public override void Complete(TextArea textArea, 
                                      ISegment completionSegment, 
                                      EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, this.Text);
        }
    }
}
