﻿using CodeGenie.Ui.Wpf.Controls.CodeEditor.Contracts;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.Compiling;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.Setup;
using CodeGenie.Ui.Wpf.Controls.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using CodeGenie.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenie.Core.Models.Configuration;
using Microsoft.Extensions.Logging.Debug;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.EditorTracking;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.TextMarkers;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor
{
    public static class EditorServiceExtensions
    {
        public static ServiceProvider CreateDefaultServiceProvider(IDispatcherService? dispatcherService = null)
        {
            var collection = new ServiceCollection();
            AddDefaultServices(collection, dispatcherService);
            return collection.BuildServiceProvider();
        }

        public static void AddDefaultServices(IServiceCollection collection, IDispatcherService? dispatcherService = null)
        {
            collection.AddControlSharedServices(dispatcherService);
            collection.AddCodeEditorServices(o =>
            {
                o.LoggerProviders.Add(new DebugLoggerProvider());
            });
        }

        public static void AddCodeEditorServices(this IServiceCollection serviceCollection, Action<ServiceCreationOptions>? optionsUpdater = null)
        {
            serviceCollection.AddEditorUiServices();
            serviceCollection.AddCodeGenie(optionsUpdater);
        }

        private static void AddEditorUiServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IAvalonEditConfigurer, AvalonEditConfigurer>();
            serviceCollection.AddSingleton<ICodeEditorSetupService, CodeEditorSetupService>();
            serviceCollection.AddTransient<IHighlightDefinitionAccessor, HighlightingDefinitionAccessor>();
            serviceCollection.AddSingleton<ITextUpdateListener, TextUpdateListener>();
            serviceCollection.AddSingleton<ITextViewEventListener, TextViewEventListener>();
            serviceCollection.AddSingleton<IComponentRepository, ComponentRepository>();
            serviceCollection.AddTransient<IComponentDefinitionProvider>(s => s.GetService<IComponentRepository>());
            serviceCollection.AddSingleton<IComponentCompilationService, ComponentCompilationService>();
            serviceCollection.AddSingleton<IComponentDefinitionMarker, ComponentDefinitionErrorMarker>();
            serviceCollection.AddSingleton<ITextMarkerService, TextMarkerService>();
            serviceCollection.AddSingleton<IComponentDefinitionMarker, ComponentDefinitionErrorMarker>();
            serviceCollection.AddSingleton<IAutoCompleter, AutoCompleter>();
            serviceCollection.AddSingleton<ICompletionDataSuggester, CompletionDataSuggester>();
            serviceCollection.AddSingleton<ICompletionWindowFactory, CompletionWindowFactory>();
            serviceCollection.AddSingleton<ICaretController, CaretController>();
        }
    }
}
