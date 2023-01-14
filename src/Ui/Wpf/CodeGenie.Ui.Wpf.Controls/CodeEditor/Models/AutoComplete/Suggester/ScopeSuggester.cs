using CodeGenie.Core.Models.ComponentDefinitions.State;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.AutoComplete.Suggestions;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.AutoComplete.Suggester
{
    public class ScopeSuggester : SyntaxSuggesterBase
    {
        public ScopeSuggester() : base(SyntaxDescriptor.BeforeStartComponentDefinition,
                                        SyntaxDescriptor.BeforeStartAttributeDefinition) { }

        protected override void CollectOtherSuggestions(SyntaxDescriptor descriptor, TextEnterEventArgs textEnterArgs, List<ICompletionData> toBeReturned)
        {
            toBeReturned.Add(new SimpleTextSuggestion("+ (public)", "Public Scope", textEnterArgs, "+ "));
            toBeReturned.Add(new SimpleTextSuggestion("- (private)", "Private Scope", textEnterArgs,"- "));
            toBeReturned.Add(new SimpleTextSuggestion("# (protected)", "Private Scope", textEnterArgs, "# "));
        }
    }
}
