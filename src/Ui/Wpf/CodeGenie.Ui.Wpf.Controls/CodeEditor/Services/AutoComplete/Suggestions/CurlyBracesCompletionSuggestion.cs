using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete.Suggestions
{
    public class CurlyBracesCompletionSuggestion : SuggestionBase
    {
        public const string TextValue = "}";

        public CurlyBracesCompletionSuggestion(TextEnterEventArgs args) : base(args)
        {
        }

        public override string Text => TextValue;

        public override object Description => "Complete the curly braces";

        public override double Priority => int.MaxValue;

        public override string GetReplacementText(EventArgs insertionRequestEventArgs)
        {
            var b = new StringBuilder();
            var existingTabLevel = EventArguments.LineContent.Count(c => c.Equals('\t'));
            b.Append("\n");
            b.Append(string.Join("", new string[existingTabLevel + 1].Select(s => "\t")));
            b.Append("\n");
            b.Append(string.Join("", new string[existingTabLevel].Select(s => "\t")));
            b.Append("}");
            CaretLineNumberPlacement = EventArguments.LineNumber + 1;
            CaretColumnPlacement += existingTabLevel + 1;
            return b.ToString();
        }
    }
}
