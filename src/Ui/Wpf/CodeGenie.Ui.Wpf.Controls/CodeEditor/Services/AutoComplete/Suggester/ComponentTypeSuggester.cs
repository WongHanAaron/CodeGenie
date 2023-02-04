using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenie.Core.Models.ComponentDefinitions.Syntax;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete.Suggestions;
using ICSharpCode.AvalonEdit.CodeCompletion;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete.Suggester
{
    public class ComponentTypeSuggester : SyntaxSuggesterBase
    {
        public const string Class = "class";
        public const string Interface = "interface";

        public ComponentTypeSuggester() : base(SyntaxDescriptor.BeforeComponentTypeDefinition)
        { }

        protected override void CollectOtherSuggestions(SyntaxDescription description, TextEnterEventArgs textEnterArgs, List<ICompletionData> toBeReturned)
        {
            toBeReturned.Add(new SimpleTextSuggestion(Class, "Add as class component", textEnterArgs));
            toBeReturned.Add(new SimpleTextSuggestion(Interface, "Add as interface component", textEnterArgs));
        }
    }
}
