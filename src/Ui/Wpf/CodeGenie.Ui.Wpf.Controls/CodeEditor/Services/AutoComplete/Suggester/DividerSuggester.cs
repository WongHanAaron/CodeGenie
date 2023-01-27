using CodeGenie.Core.Models.ComponentDefinitions.State;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.AutoComplete.Suggestions;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete.Suggester
{
    public class DividerSuggester : SyntaxSuggesterBase
    {
        public DividerSuggester() : base(SyntaxDescriptor.BeforeComponentDivider,
                                         SyntaxDescriptor.BeforeAttributesDivider,
                                         SyntaxDescriptor.BeforeAttributeDivider,
                                         SyntaxDescriptor.BeforeCardinalityDivider)
        { }

        protected override void CollectOtherSuggestions(SyntaxDescriptor descriptor, TextEnterEventArgs textEnterArgs, List<ICompletionData> toBeReturned)
        {
            toBeReturned.Add(new SimpleTextSuggestion(":", "Divider", textEnterArgs));
        }
    }
}
