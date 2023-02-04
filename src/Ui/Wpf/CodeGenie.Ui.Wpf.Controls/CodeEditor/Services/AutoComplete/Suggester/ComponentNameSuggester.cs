using CodeGenie.Core.Models.ComponentDefinitions.State;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete.Suggestions;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete.Suggester
{
    public class ComponentNameSuggester : SyntaxSuggesterBase
    {
        public const string EnterComponentName = "Enter a component name";

        public ComponentNameSuggester() : base(SyntaxDescriptor.BeforeComponentNameDefinition) { }

        protected override void AddDefaultSuggestions(List<ICompletionData> completionData)
        {
            completionData.Add(new TooltipSuggestion(EnterComponentName, "Enter a name for this component"));
        }
    }
}
