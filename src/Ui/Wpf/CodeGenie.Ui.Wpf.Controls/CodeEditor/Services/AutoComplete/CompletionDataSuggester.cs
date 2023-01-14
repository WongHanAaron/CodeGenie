using CodeGenie.Core.Models.ComponentDefinitions.State;
using CodeGenie.Core.Services.Parsing.ComponentDefinitions.SyntaxDescribing;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Contracts;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.AutoCompletions.Syntax;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.AutoCompletions.Syntax.ComponentDetails;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete
{
    /// <summary> Component that suggests completion data for the code editor </summary>
    public interface ICompletionDataSuggester
    {
        /// <summary> Get suggestions based on the text entered args </summary>
        IEnumerable<ICompletionData> GetSuggestions(TextEnterEventArgs eventArgs);
    }

    public class CompletionDataSuggester : ICompletionDataSuggester
    {
        protected readonly ITextUpdateListener TextUpdateListener;
        protected readonly ISyntaxDescriber SyntaxDescriber;
        public CompletionDataSuggester(ITextUpdateListener textUpdateListener, ISyntaxDescriber syntaxDescriber)
        {
            TextUpdateListener = textUpdateListener;
            SyntaxDescriber = syntaxDescriber;
        }

        public IEnumerable<ICompletionData> GetSuggestions(TextEnterEventArgs eventArgs)
        {
            var fullScript = TextUpdateListener.CurrentText;

            var syntaxDescription = SyntaxDescriber.GetSyntaxDescription(fullScript, eventArgs.LineNumber, eventArgs.ColumnNumber);

            if (syntaxDescription == SyntaxDescriptor.BeforeComponentTypeDefinition)
                return AfterComponentDivider(eventArgs);
            else if (syntaxDescription == SyntaxDescriptor.BeforeComponentDetails)
                return AfterComponentDetails(eventArgs);

            return new List<ICompletionData>();
        }

        public IEnumerable<ICompletionData> AfterComponentDivider(TextEnterEventArgs eventArgs)
        {
            return new List<ICompletionData>()
            {
                new ComponentPostDividerCompletion("class", eventArgs),
                new ComponentPostDividerCompletion("interface", eventArgs)
            };
        }

        public IEnumerable<ICompletionData> AfterComponentDetails(TextEnterEventArgs eventArgs)
        {
            return new List<ICompletionData>()
            {
                new ComponentPurpose(eventArgs),
                new ComponentAttributes(eventArgs)
            };
        }
    }
}
