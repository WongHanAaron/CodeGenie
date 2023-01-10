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

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor
{
    public static class EditorServiceExtensions
    {
        public static ServiceProvider CreateDefaultServiceProvider()
        {
            var collection = new ServiceCollection();
            collection.AddControlSharedServices();
            collection.AddEditorServices(o => 
            {
                o.LoggerProviders.Add(new DebugLoggerProvider());
            });
            return collection.BuildServiceProvider();
        }

        public static void AddEditorServices(this IServiceCollection serviceCollection, Action<ServiceCreationOptions> optionsUpdater = null)
        {
            serviceCollection.AddEditorUiServices();
            serviceCollection.AddCodeGenie(optionsUpdater);
        }

        public static void AddEditorUiServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IAvalonEditConfigurer, AvalonEditConfigurer>();
            serviceCollection.AddSingleton<ICodeEditorSetupService, CodeEditorSetupService>();
            serviceCollection.AddTransient<IHighlightDefinitionAccessor, HighlightingDefinitionAccessor>();
            serviceCollection.AddSingleton<ITextUpdateListener, TextUpdateListener>();
            serviceCollection.AddSingleton<IComponentRepository, ComponentRepository>();
            serviceCollection.AddTransient<IComponentDefinitionProvider>(s => s.GetService<IComponentRepository>());
            serviceCollection.AddSingleton<IComponentCompilationService, ComponentCompilationService>();
        }
    }
}
