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
    public class ComponentPurpose : SuggestionBase
    {
        public ComponentPurpose(TextEnterEventArgs eventArgs) : base(eventArgs) { }

        public override string Text => "{ purpose : \"\"}";

        public override object Description => "Add a component purpose";

        public override double Priority => 1;

        public override string GetReplacementText(EventArgs insertionRequestEventArgs)
        {
            var returned = "\n{\n\tpurpose : \"\"\n}";
            
            CaretLineNumberPlacement = EventArguments.LineNumber + 2;
            CaretColumnPlacement = 13;

            return returned;
        }
    }
}
