using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.AutoComplete.Suggestions;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.AutoComplete.Suggestions.ComponentDetails
{
    public class ComponentAttributes : SuggestionBase
    {
        public ComponentAttributes(TextEnterEventArgs eventArgs) : base(eventArgs) { }

        public override string Text => "{ attributes { } }";

        public override object Description => "Add a component attributes";

        public override double Priority => 1;

        public override string GetReplacementText(EventArgs insertionRequestEventArgs)
        {
            CaretLineNumberPlacement = EventArguments.LineNumber + 4;
            CaretColumnPlacement = 3;
            return "\n{\n\tattributes\n\t{\n\t\t\n\t}\n}";
        }
    }
}
