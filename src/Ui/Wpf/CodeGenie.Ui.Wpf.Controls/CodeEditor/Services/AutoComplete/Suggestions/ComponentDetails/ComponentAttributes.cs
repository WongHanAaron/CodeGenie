using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete.Suggestions;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete.Suggestions.ComponentDetails
{
    public class ComponentAttributes : SuggestionBase
    {
        public const string TextValue = "{ attributes { } }";

        bool _includeExternalBraces;
        public ComponentAttributes(TextEnterEventArgs eventArgs, bool includeExternalBraces) : base(eventArgs)
        {
            _includeExternalBraces = includeExternalBraces;
        }

        public override string Text => TextValue;

        public override object Description => "Add a component attributes";

        public override double Priority => 1;

        public override string GetReplacementText(EventArgs insertionRequestEventArgs)
        {
            var builder = new StringBuilder();
            if (_includeExternalBraces) builder.Append("\n{");
            builder.Append("\n\tattributes\n\t{\n\t\t\n\t}");
            if (_includeExternalBraces) builder.Append("\n}");
            CaretLineNumberPlacement = EventArguments.LineNumber + 3;
            if (_includeExternalBraces) CaretLineNumberPlacement += 1;
            CaretColumnPlacement = 3;
            return builder.ToString();
        }
    }
}
