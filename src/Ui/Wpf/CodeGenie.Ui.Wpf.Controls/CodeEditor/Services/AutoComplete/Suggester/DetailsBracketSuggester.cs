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
    public class DetailsBracketSuggester : SyntaxSuggesterBase
    {
        public DetailsBracketSuggester() : base(SyntaxDescriptor.BeforeComponentDetails,
                                                SyntaxDescriptor.BeforeRelationshipsDetails,
                                                SyntaxDescriptor.BeforeAttributesDetails,
                                                SyntaxDescriptor.BeforeAttributeDetails,
                                                SyntaxDescriptor.BeforeMethodsDetails)
        { }

        protected override void CollectOtherSuggestions(SyntaxDescriptor descriptor, TextEnterEventArgs textEnterArgs, List<ICompletionData> toBeReturned)
        {
            toBeReturned.Add(new NewBracketSuggestion(textEnterArgs, true));
        }
    }
}
