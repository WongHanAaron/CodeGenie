using CodeGenie.Ui.Wpf.Controls.CodeEditor.Contracts;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete.Suggestions;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.EditorTracking;
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
        protected readonly ICaretController CaretController;
        protected readonly IDispatcherService DispatcherService;
        protected readonly IPeriodicEventService PeriodicEventService;

        public AutoCompleter(ILogger<AutoCompleter> logger, 
                             IDateTimeProvider dateTimeProvider,
                             ITextUpdateListener textUpdateListener,
                             ICompletionDataSuggester completionSuggester,
                             ICompletionWindowFactory completionWindowFactory,
                             IPeriodicEventService periodicEventService,
                             ICaretController caretController,
                             IDispatcherService dispatcherService)
        {
            Logger = logger;
            DateTimeProvider = dateTimeProvider;
            TextUpdateListener = textUpdateListener;
            CompletionSuggester = completionSuggester;
            CompletionWindowFactory = completionWindowFactory;
            CaretController = caretController;
            DispatcherService = dispatcherService;
            PeriodicEventService = periodicEventService;
            AttachEvents();
        }

        protected void AttachEvents()
        {
            TextUpdateListener.OnTextEntered += TextEntered;
            PeriodicEventService.Register(this, 1, (o, e) => HandleTextEnteredIfAnyCached());
        }

        TextEnterEventArgs _cachedTextEnterArgs;
        protected void TextEntered(object sender, TextEnterEventArgs args)
        {
            _cachedTextEnterArgs = args;

            if (IsCompletionWindowOpen())
            {
                _completionWindow.Close();
            }
        }

        protected bool _suggestionHandlingInProgress = false;
        protected void HandleTextEnteredIfAnyCached()
        {
            if (_cachedTextEnterArgs == null) return;
            lock (this)
            {
                if (_suggestionHandlingInProgress) return;

                _suggestionHandlingInProgress = true;
            }

            if (IsCompletionWindowOpen())
            {
                DispatcherService.InvokeOnUiThread(() => _completionWindow.Close());
            }

            var args = _cachedTextEnterArgs;
            TryShowSuggestion(args);

            lock (this)
            {
                _suggestionHandlingInProgress = false;
                _cachedTextEnterArgs = null;
            }
        }

        protected void TryShowSuggestion(TextEnterEventArgs args)
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

        protected bool IsCompletionWindowOpen()
            => _completionWindow != null;

        protected ICompletionData _lastSelectedItem;
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
                _lastSelectedItem = _completionWindow.CompletionList.SelectedItem;
                _completionWindow = null;
            };

            _completionWindow.CompletionList.InsertionRequested += delegate
            {
                var selected = _completionWindow?.CompletionList?.SelectedItem ?? _lastSelectedItem;

                if (selected is SuggestionBase auto)
                {
                    if (auto.CaretLineNumberPlacement.HasValue)
                        CaretController.MoveCaretToLineNumber(auto.CaretLineNumberPlacement.Value);
                    if (auto.CaretColumnPlacement.HasValue)
                        CaretController.MoveCaretToColumn(auto.CaretColumnPlacement.Value);

                    TextUpdateListener.TextWasEntered(this, auto.ReplacementText);
                }
            };
        }
    }
}
