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
    public class DividerSuggester : SyntaxSuggesterBase
    {
        public const string Divider = ":";
        
        public DividerSuggester() : base(SyntaxValidityOption.Invalid,
                                         SyntaxDescriptor.BeforePurposeDefinitionDivider,
                                         SyntaxDescriptor.BeforeComponentDivider,
                                         SyntaxDescriptor.BeforeAttributeDivider,
                                         SyntaxDescriptor.BeforeCardinalityDivider)
        { }

        protected override void CollectOtherSuggestions(SyntaxDescription description, TextEnterEventArgs textEnterArgs, List<ICompletionData> toBeReturned)
        {
            toBeReturned.Add(new SimpleTextSuggestion(Divider, "Divider", textEnterArgs));
        }
    }
}
