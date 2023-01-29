using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete.Suggestions
{
    public class NewBracketSuggestion : SuggestionBase
    {
        protected bool _createNewLine;
        public NewBracketSuggestion(TextEnterEventArgs args, bool createNewLine) : base(args)
        {
            _createNewLine = createNewLine;
        }

        public override string Text => "{ }";

        public override object Description => "Add details bracket";

        public override double Priority => 5;

        public override string GetReplacementText(EventArgs insertionRequestEventArgs)
        {
            var b = new StringBuilder();
            CaretColumnPlacement = EventArguments.ColumnNumber;
            if (_createNewLine)
            {
                var existingTabLevel = EventArguments.LineContent.Count(c => c.Equals('\t'));
                b.Append("\n");
                b.Append(string.Join("", new string[existingTabLevel].Select(s => "\t")));
                b.Append("{");
                b.Append("\n");
                b.Append(string.Join("", new string[existingTabLevel + 1].Select(s => "\t")));
                b.Append("\n");
                b.Append(string.Join("", new string[existingTabLevel].Select(s => "\t")));
                b.Append("}");
                CaretLineNumberPlacement = EventArguments.LineNumber + 2;
                CaretColumnPlacement += existingTabLevel + 1;            }
            else
            {
                CaretColumnPlacement += 2;
                b.Append("{ }");
            }
            return b.ToString();
        }
    }
}
