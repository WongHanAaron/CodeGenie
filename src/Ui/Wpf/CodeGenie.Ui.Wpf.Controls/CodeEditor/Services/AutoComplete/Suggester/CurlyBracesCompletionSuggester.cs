using CodeGenie.Core.Models.ComponentDefinitions.State;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete.Suggestions;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete.Suggester
{
    public class CurlyBracesCompletionSuggester : SyntaxSuggesterBase
    {
        public override IEnumerable<ICompletionData> CollectSuggestions(SyntaxDescriptor descriptor, TextEnterEventArgs textEnterArgs)
        {
            if (textEnterArgs.LineContent.EndsWith("{"))
            {
                return new List<ICompletionData>() { new CurlyBracesCompletionSuggestion(textEnterArgs) };
            }
            else
            {
                return new List<ICompletionData>();
            }
        }
    }
}
