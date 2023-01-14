using CodeGenie.Core.Models.ComponentDefinitions.State;
using CodeGenie.Core.Services.Parsing.ComponentDefinitions.SyntaxDescribing;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Contracts;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.AutoComplete.Suggestions;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.AutoComplete.Suggestions.ComponentDetails;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.AutoComplete.Suggester;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using ICSharpCode.AvalonEdit.CodeCompletion;
using Microsoft.Extensions.Logging;
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
        protected readonly ILogger<CompletionDataSuggester> Logger;
        protected readonly ITextUpdateListener TextUpdateListener;
        protected readonly ISyntaxDescriber SyntaxDescriber;
        public CompletionDataSuggester(ILogger<CompletionDataSuggester> logger,
                                       ITextUpdateListener textUpdateListener, 
                                       ISyntaxDescriber syntaxDescriber)
        {
            Logger = logger;
            TextUpdateListener = textUpdateListener;
            SyntaxDescriber = syntaxDescriber;
        }


        private readonly static List<SyntaxSuggesterBase> _suggestionCollector = new List<SyntaxSuggesterBase>()
        {
            new ScopeSuggester(),
            new DividerSuggester(),
            new ComponentNameSuggester(),
            new ComponentTypeSuggester(),
            new ComponentDetailSuggester()
        };
        public IEnumerable<ICompletionData> GetSuggestions(TextEnterEventArgs eventArgs)
        {
            var fullScript = TextUpdateListener.CurrentText;

            var syntaxDescription = SyntaxDescriber.GetSyntaxDescription(fullScript, eventArgs.LineNumber, eventArgs.ColumnNumber);

            Logger.LogDebug($"CurrentSyntax: {syntaxDescription.ToString()}");

            var returned = new List<ICompletionData>();
            
            foreach (var collector in _suggestionCollector)
            {
                var suggestions = collector.CollectSuggestions(syntaxDescription, eventArgs);
                if (suggestions.Any())
                    returned.AddRange(suggestions);
            }

            return returned;
        }
        
    }
}
