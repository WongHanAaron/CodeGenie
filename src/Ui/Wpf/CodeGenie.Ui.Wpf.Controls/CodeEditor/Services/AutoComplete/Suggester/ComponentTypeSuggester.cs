using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenie.Core.Models.ComponentDefinitions.State;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.AutoComplete.Suggestions;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using ICSharpCode.AvalonEdit.CodeCompletion;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete.Suggester
{
    public class ComponentTypeSuggester : SyntaxSuggesterBase
    {
        public ComponentTypeSuggester() : base(SyntaxDescriptor.BeforeComponentTypeDefinition)
        { }

        protected override void CollectOtherSuggestions(SyntaxDescriptor descriptor, TextEnterEventArgs textEnterArgs, List<ICompletionData> toBeReturned)
        {
            toBeReturned.Add(new SimpleTextSuggestion("class", "Add as class component", textEnterArgs));
            toBeReturned.Add(new SimpleTextSuggestion("interface", "Add as interface component", textEnterArgs));
        }
    }
}
