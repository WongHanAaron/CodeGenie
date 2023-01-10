using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor
{
    public static class EditorServiceExtensions
    {
        public static ServiceProvider CreateDefaultServiceProvider()
        {
            var collection = new ServiceCollection();
            collection.AddEditorServices();
            return collection.BuildServiceProvider();
        }

        public static void AddEditorServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAvalonEditConfigurer, AvalonEditConfigurer>();
            serviceCollection.AddTransient<IHighlightSchemaAccessor, HighlightingDefinitionAccessor>();
        }
    }
}
