using Antlr4.Runtime.Misc;
using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Contracts;
using CodeGenie.Ui.Wpf.Controls.MessageBoard.Models;
using CodeGenie.Ui.Wpf.Controls.MessageBoard.Models.Messages;
using CodeGenie.Ui.Wpf.Controls.MessageBoard.Services;
using CodeGenie.Ui.Wpf.Controls.Shared;
using CodeGenie.Ui.Wpf.Controls.Shared.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.MessageBoard.ViewModels
{
    public class StatusBoardViewModel : ViewModelBase
    {
        protected readonly MessageBoardOptions Options;
        protected readonly IDispatcherService DispatcherService;
        protected readonly IComponentDefinitionProvider ComponentDefinitionProvider;

        public StatusBoardViewModel(MessageBoardOptions options,
                                    IDispatcherService dispatcherService,
                                    IComponentDefinitionProvider componentDefinitionProvider)
        {
            Options = options;
            DispatcherService = dispatcherService;
            ComponentDefinitionProvider = componentDefinitionProvider;
            ComponentDefinitionProvider.OnComponentDefinitionsDefined += ReceivedNewParsingResults;
            Messages = new ObservableCollection<CompilationStateMessage>();
            Messages.Add(CreateState(LogLevel.Information, "Welcome to CodeGenie!"));
        }

        public ObservableCollection<CompilationStateMessage> Messages { get => Get<ObservableCollection<CompilationStateMessage>>(); set => Set(value); }

        protected void ReceivedNewParsingResults(object sender, ParsingResult result)
        {
            DispatcherService.InvokeOnUiThread(() => 
            {
                Messages.Clear();
                if (result.HasErrors)
                {
                    foreach (var error in result.Errors)
                    {
                        Messages.Add(CreateErrorState(error));
                    }
                }
                else
                {
                    Messages.Add(CreateState(LogLevel.Information, 
                        $"Successfully compiled {result.Components.Count()} components"));
                }
            });
        }

        protected CompilationStateMessage CreateErrorState(ScriptError scriptError)
        {
            var exceptionName = scriptError.Exception?.GetType().Name ?? "General error";
            var line = "";
            
            if (scriptError.Token != null)
            {
                var stream = scriptError.Token.InputStream;
                var min = Math.Min(scriptError.StartIndex, scriptError.EndIndex);
                var max = Math.Max(scriptError.StartIndex, scriptError.EndIndex);
                
                if (min >= 0 && max >= 0) 
                    line = $" '{stream.GetText(Interval.Of(min, max))}'";
            }

            var errorDescription = $"line {scriptError.LineNumber}, column {scriptError.ColumnIndex} ({scriptError.StartIndex}-{scriptError.EndIndex}){line}";
            return CreateState(LogLevel.Error, $"{exceptionName} occurred at {errorDescription}");
        }

        protected CompilationStateMessage CreateState(LogLevel logLevel, string message)
        {
            return new CompilationStateMessage()
            {
                Level = logLevel,
                Message = message
            };
        }
    }
}
