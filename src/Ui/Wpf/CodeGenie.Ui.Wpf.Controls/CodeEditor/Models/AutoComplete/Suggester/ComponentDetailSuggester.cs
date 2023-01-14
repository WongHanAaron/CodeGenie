using CodeGenie.Core.Models.ComponentDefinitions.State;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.AutoComplete.Suggestions.ComponentDetails;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.AutoComplete.Suggester
{
    public class ComponentDetailSuggester : SyntaxSuggesterBase
    {
        public ComponentDetailSuggester() : base(SyntaxDescriptor.BeforeComponentDetails,
                                                  SyntaxDescriptor.WithinComponentDetails) { }

        protected override void CollectOtherSuggestions(SyntaxDescriptor descriptor, TextEnterEventArgs textEnterArgs, List<ICompletionData> toBeReturned)
        {
            toBeReturned.Add(new ComponentPurpose(textEnterArgs));
            toBeReturned.Add(new ComponentAttributes(textEnterArgs));
        }
    }
}
