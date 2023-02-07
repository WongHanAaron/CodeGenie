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
    public class DetailsBracketSuggester : SyntaxSuggesterBase
    {
        public DetailsBracketSuggester() : base(SyntaxDescriptor.BeforeComponentDetails,
                                                SyntaxDescriptor.BeforeRelationshipsDetails,
                                                SyntaxDescriptor.BeforeAttributesDetails,
                                                SyntaxDescriptor.BeforeAttributeDetails,
                                                SyntaxDescriptor.BeforeMethodsDetails)
        { }

        protected override void CollectOtherSuggestions(SyntaxDescription description, TextEnterEventArgs textEnterArgs, List<ICompletionData> toBeReturned)
        {
            if (!description.HasSyntaxErrorOnSelectedRule) return;

            toBeReturned.Add(new NewBracketSuggestion(textEnterArgs, true));
        }
    }
}
