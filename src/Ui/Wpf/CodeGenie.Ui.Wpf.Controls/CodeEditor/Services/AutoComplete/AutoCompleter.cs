using CodeGenie.Ui.Wpf.Controls.CodeEditor.Contracts;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using CodeGenie.Ui.Wpf.Controls.Shared.Contracts;
using CodeGenie.Ui.Wpf.Controls.Shared.Services;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete
{
    /// <summary> The class component that can show the auto suggestions </summary>
    public interface IAutoCompleter
    {
        IEnumerable<ICompletionData> ShowSuggestions(TextEnterEventArgs textEnteredArgs);
    }

    public class AutoCompleter : IAutoCompleter
    {
        protected CompletionWindow _completionWindow;
        protected readonly ILogger Logger;
        protected readonly IDateTimeProvider DateTimeProvider;
        protected readonly ITextUpdateListener TextUpdateListener;
        protected readonly ICompletionDataSuggester CompletionSuggester;
        protected readonly ICompletionWindowFactory CompletionWindowFactory;
        protected readonly IDispatcherService DispatcherService;

        public AutoCompleter(ILogger<AutoCompleter> logger, 
                             IDateTimeProvider dateTimeProvider,
                             ITextUpdateListener textUpdateListener,
                             ICompletionDataSuggester completionSuggester,
                             ICompletionWindowFactory completionWindowFactory,
                             IPeriodicEventService periodicEventService,
                             IDispatcherService dispatcherService)
        {
            Logger = logger;
            DateTimeProvider = dateTimeProvider;
            TextUpdateListener = textUpdateListener;
            CompletionSuggester = completionSuggester;
            CompletionWindowFactory = completionWindowFactory;
            DispatcherService = dispatcherService;
            AttachEvents();
        }

        protected void AttachEvents()
        {
            TextUpdateListener.OnTextEntered += TextEntered;
            TextUpdateListener.OnTextEntering += TextEntering;
        }

        protected void TextEntered(object sender, TextEnterEventArgs args)
        {
            if (IsCompletionWindowOpen())
            {
                _completionWindow.Close();
            }

            TryShowSuggestion(sender, args);
        }

        protected void TryShowSuggestion(object sender, TextEnterEventArgs args)
        {
            // If there is a completion window already shown don't show another until it is closed
            if (IsCompletionWindowOpen()) return;

            ShowSuggestions(args);
        }

        public IEnumerable<ICompletionData> ShowSuggestions(TextEnterEventArgs textEnteredArgs)
        {
            var suggestions = CompletionSuggester.GetSuggestions(textEnteredArgs);

            if (!suggestions.Any()) return new List<ICompletionData>();

            DispatcherService.InvokeOnUiThread(() =>
            {
                EnsureCompletionWindowCreated();

                IList<ICompletionData> data = _completionWindow.CompletionList.CompletionData;
                suggestions.ToList().ForEach(s => data.Add(s));

                _completionWindow?.Show();
            });

            return suggestions;
        }

        protected void TextEntering(object sender, TextEnterEventArgs args)
        {
            // If the tab character is entered,
            // load the current suggestion
            var lastChar = args.Text.LastOrDefault();

            if (lastChar == default(char)) return;

            if (lastChar.Equals('\t'))
            {
                _completionWindow.CompletionList.RequestInsertion(args);
                var selected = _completionWindow.CompletionList.SelectedItem;
                _completionWindow.Close();
            }
        }

        protected bool IsCompletionWindowOpen()
            => _completionWindow != null;

        protected void EnsureCompletionWindowCreated()
        {
            if (_completionWindow != null)
            {
                _completionWindow.Close();
                _completionWindow = null;
            }

            _completionWindow = CompletionWindowFactory.Create();
            _completionWindow.Closed += delegate
            {
                _completionWindow = null;
            };
        }
    }
}
