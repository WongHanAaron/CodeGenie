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
    public class NameTooltipSuggester : SyntaxSuggesterBase
    {
        public const string EnterName = "Enter a name";

        public NameTooltipSuggester() : base(SyntaxValidityOption.Invalid,
                                               SyntaxDescriptor.BeforeComponentNameDefinition,
                                               SyntaxDescriptor.BeforeAttributeNameDefinition,
                                               SyntaxDescriptor.BeforeMethodNameDefinition) { }

        protected override void CollectOtherSuggestions(SyntaxDescription description, TextEnterEventArgs textEnterArgs, List<ICompletionData> toBeReturned)
        {
            var type = "";
            switch(description.SyntaxDescriptorAtCaret)
            {
                case SyntaxDescriptor.BeforeComponentNameDefinition:
                    type = "component";
                    break;
                case SyntaxDescriptor.BeforeAttributeNameDefinition:
                    type = "attribute";
                    break;
                case SyntaxDescriptor.BeforeMethodNameDefinition:
                    type = "method";
                    break;
            }
            toBeReturned.Add(new TooltipSuggestion(EnterName, $"Enter a name for this {type}"));
        }
    }
}
