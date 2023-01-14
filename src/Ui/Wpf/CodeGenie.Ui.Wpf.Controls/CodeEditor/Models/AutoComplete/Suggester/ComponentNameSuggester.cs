using CodeGenie.Core.Models.ComponentDefinitions.State;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.AutoComplete.Suggestions;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.AutoComplete.Suggester
{
    public class ComponentNameSuggester : SyntaxSuggesterBase
    {
        public ComponentNameSuggester() : base(SyntaxDescriptor.BeforeComponentNameDefinition) { }

        protected override void AddDefaultSuggestions(List<ICompletionData> completionData)
        {
            completionData.Add(new TooltipSuggestion("Component Name", "Enter a component name"));
        }
    }
}
