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
    public class ScopeSuggester : SyntaxSuggesterBase
    {
        public const string PublicScope = "+ (public)";
        public const string PrivateScope = "- (private)";
        public const string ProtectedScope = "# (protected)";

        public ScopeSuggester() : base(SyntaxDescriptor.BeforeStartComponentDefinition,
                                       SyntaxDescriptor.BeforeStartAttributeDefinition,
                                       SyntaxDescriptor.BeforeComponentDetails)
        { }

        protected override void CollectOtherSuggestions(SyntaxDescriptor descriptor, TextEnterEventArgs textEnterArgs, List<ICompletionData> toBeReturned)
        {
            toBeReturned.Add(new SimpleTextSuggestion(PublicScope, "Public Scope", textEnterArgs, "+ "));
            toBeReturned.Add(new SimpleTextSuggestion(PrivateScope, "Private Scope", textEnterArgs, "- "));
            toBeReturned.Add(new SimpleTextSuggestion(ProtectedScope, "Private Scope", textEnterArgs, "# "));
        }
    }
}
