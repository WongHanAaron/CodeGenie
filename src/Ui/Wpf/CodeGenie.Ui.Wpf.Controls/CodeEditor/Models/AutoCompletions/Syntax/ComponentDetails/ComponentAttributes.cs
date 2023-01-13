using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.AutoCompletions.Syntax.ComponentDetails
{
    public class ComponentAttributes : SyntaxCompletionBase
    {
        public ComponentAttributes(TextEnterEventArgs eventArgs) : base(eventArgs) { }

        public override string Text => "{ attributes { } }";

        public override object Description => "Add a component attributes";

        public override void Complete(TextArea textArea, 
                                      ISegment completionSegment, 
                                      EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, "\n\tattributes\n\t{\n\n\t}\n}");
        }
    }
}
