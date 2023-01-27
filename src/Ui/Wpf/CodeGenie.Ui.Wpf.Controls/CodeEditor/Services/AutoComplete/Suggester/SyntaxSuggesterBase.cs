using CodeGenie.Core.Models.ComponentDefinitions.State;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete.Suggester
{
    public abstract class SyntaxSuggesterBase
    {
        public SyntaxSuggesterBase(params SyntaxDescriptor[] syntaxDescriptors)
        {
            foreach (var d in syntaxDescriptors)
            {
                AcceptedSyntaxDescriptor.Add(d);
            }
            AddDefaultSuggestions(Suggestions);
        }

        public List<SyntaxDescriptor> AcceptedSyntaxDescriptor { get; set; } = new List<SyntaxDescriptor>();

        public List<ICompletionData> Suggestions { get; protected set; } = new List<ICompletionData>();

        public virtual IEnumerable<ICompletionData> CollectSuggestions(SyntaxDescriptor descriptor, TextEnterEventArgs textEnterArgs)
        {
            var returned = new List<ICompletionData>();
            if (!AcceptedSyntaxDescriptor.Any(d => d == descriptor)) return returned;

            returned = Suggestions.ToList();

            CollectOtherSuggestions(descriptor, textEnterArgs, returned);

            return returned;
        }

        protected virtual void AddDefaultSuggestions(List<ICompletionData> completionData) { }

        protected virtual void CollectOtherSuggestions(SyntaxDescriptor descriptor, TextEnterEventArgs textEnterArgs, List<ICompletionData> toBeReturned) { }
    }
}
