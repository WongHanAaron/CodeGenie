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
    public class ComponentPurpose : SyntaxCompletionBase
    {
        public ComponentPurpose(TextEnterEventArgs eventArgs) : base(eventArgs) { }

        public override string Text => "{ purpose : \"\"}";

        public override object Description => "Add a component purpose";

        public override void Complete(TextArea textArea, 
                                      ISegment completionSegment, 
                                      EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, "\n\tpurpose : \"\"\n}");
        }
    }
}
