using CodeGenie.Core.Services.Parsing.ComponentDefinitions;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Contracts;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events;
using CodeGenie.Ui.Wpf.Controls.Shared.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.Compiling
{
    /// <summary> The service responsible for listening to text updates and compiling the components </summary>
    public interface IComponentCompilationService
    {
        /// <summary> This method will be invoked by internal listeners when the text is updated </summary>
        void OnTextUpdated(TextUpdateEventArgs evt);

        /// <summary> This method will be called when the text has not been modified and can be compiled </summary>
        void CompileIfTextUnchanged();
    }

    public class ComponentCompilationService : IComponentCompilationService
    {
        protected readonly ILogger<ComponentCompilationService> Logger;
        protected readonly IDateTimeProvider DateTimeProvider;
        protected readonly ITextUpdateListener TextUpdateListener;
        protected readonly IPeriodicEventService PeriodicEventService;
        protected readonly IComponentRepository Repository;
        protected readonly IComponentCompiler Compiler;

        public ComponentCompilationService(ILogger<ComponentCompilationService> logger,
                                           IDateTimeProvider dateTimeProvider,
                                           ITextUpdateListener textUpdateListener,
                                           IPeriodicEventService periodicEventService,
                                           IComponentRepository repository,
                                           IComponentCompiler compiler)
        {
            Logger = logger;
            DateTimeProvider = dateTimeProvider;
            TextUpdateListener = textUpdateListener;
            PeriodicEventService = periodicEventService;
            Repository = repository;
            Compiler = compiler;
            SetupListeners();
        }

        private void SetupListeners()
        {
            PeriodicEventService.Register(this, 4, (o, e) => CompileIfTextUnchanged());
            TextUpdateListener.OnTextUpdated += (o, e) => OnTextUpdated(e);
        }

        private TimeSpan _unchangedTextInterval = TimeSpan.FromSeconds(2);
        private DateTime? _lastCompiledDateTime = null;
        public void CompileIfTextUnchanged()
        {
            var updateEvt = _lastTextUpdateEvent;
            if (updateEvt == null) return;
            
            if (updateEvt.DateTime.Add(_unchangedTextInterval)
                < DateTimeProvider.Now) return;

            if (_lastCompiledDateTime != null && _lastCompiledDateTime.Value.Equals(updateEvt.DateTime))
                return;

            CompileAndUpdate();
        }

        private void CompileAndUpdate()
        {
            var eventToCompile = _lastTextUpdateEvent;

            try
            {
                Logger.LogDebug($"Starting script compilation");
                var parsedComponents = Compiler.Compile(eventToCompile.Text);
                
                if (parsedComponents.HasErrors)
                {
                    Logger.LogError($"There were {parsedComponents.Errors.Count()} errors in compiled script. {parsedComponents.Errors.FirstOrDefault(e => e.Exception != null)?.Exception?.Message}");
                }
                else
                {
                    Logger.LogInformation($"Compiled {parsedComponents.Components.Count()} component definitions");
                }

                Repository.UpdateComponents(parsedComponents);
            }
            catch(Exception ex)
            {
                Logger.LogError($"An error occurred in '{nameof(CompileAndUpdate)}'. {ex}");
            }
            finally
            {
                _lastCompiledDateTime = eventToCompile.DateTime;
            }
        }

        private TextUpdateEventArgs _lastTextUpdateEvent;
        public void OnTextUpdated(TextUpdateEventArgs evt)
        {
            try
            {
                _lastTextUpdateEvent = evt;
            }
            catch(Exception ex)
            {
                Logger.LogError($"An exception occurred in the '{OnTextUpdated}' listener");
            }
        }
    }
}
