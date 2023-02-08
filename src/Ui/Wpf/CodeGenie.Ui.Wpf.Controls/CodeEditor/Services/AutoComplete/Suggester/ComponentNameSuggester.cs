using CodeGenie.Core.Models.ComponentDefinitions.Syntax;
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
    public class ComponentNameSuggester : SyntaxSuggesterBase
    {
        public const string EnterComponentName = "Enter a component name";

        public ComponentNameSuggester() : base(SyntaxValidityOption.Invalid,
                                               SyntaxDescriptor.BeforeComponentNameDefinition) { }

        protected override void CollectOtherSuggestions(SyntaxDescription description, TextEnterEventArgs textEnterArgs, List<ICompletionData> toBeReturned)
        {
            toBeReturned.Add(new TooltipSuggestion(EnterComponentName, "Enter a name for this component"));
        }
    }
}
