using CodeGenie.Core.Services.Parsing.ComponentDefinitions.SyntaxDescribing;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Contracts;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using ICSharpCode.AvalonEdit.CodeCompletion;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.Compiling;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete.Suggester;

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
        protected readonly IComponentRepository ComponentRepository;

        public CompletionDataSuggester(ILogger<CompletionDataSuggester> logger,
                                       ITextUpdateListener textUpdateListener, 
                                       ISyntaxDescriber syntaxDescriber,
                                       IComponentRepository componentRepository)
        {
            Logger = logger;
            TextUpdateListener = textUpdateListener;
            SyntaxDescriber = syntaxDescriber;
            ComponentRepository = componentRepository;
            SetupSuggestionCollectors();
        }

        protected void SetupSuggestionCollectors()
        {
            _suggestionCollector.Add(new ScopeSuggester());
            _suggestionCollector.Add(new DividerSuggester());
            _suggestionCollector.Add(new NameTooltipSuggester());
            _suggestionCollector.Add(new ComponentTypeSuggester());
            _suggestionCollector.Add(new ComponentDetailSuggester());
            _suggestionCollector.Add(new TypeSuggester(ComponentRepository));
            _suggestionCollector.Add(new ExistingComponentNameSuggester(ComponentRepository));
            _suggestionCollector.Add(new DetailsBracketSuggester());
            _suggestionCollector.Add(new CurlyBracesCompletionSuggester());
        }


        private readonly List<SyntaxSuggesterBase> _suggestionCollector = new List<SyntaxSuggesterBase>();
        public IEnumerable<ICompletionData> GetSuggestions(TextEnterEventArgs eventArgs)
        {
            var fullScript = TextUpdateListener.CurrentText;

            var syntaxDescription = SyntaxDescriber.GetSyntaxDescription(fullScript, eventArgs.LineNumber, eventArgs.ColumnNumber);

            Logger.LogDebug($"CurrentSyntax: {syntaxDescription.SyntaxDescriptorAtCaret.ToString()}");

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
