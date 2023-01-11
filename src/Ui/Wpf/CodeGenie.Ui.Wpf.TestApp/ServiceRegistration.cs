using CodeGenie.Ui.Wpf.Controls.CodeEditor;
using CodeGenie.Ui.Wpf.Controls.ComponentTree;
using CodeGenie.Ui.Wpf.Controls.MessageBoard;
using CodeGenie.Ui.Wpf.Controls.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.TestApp
{
    public static class ServiceRegistration
    {
        public static IServiceProvider GetServiceProvider(IDispatcherService dispatcherService)
        {
            var collection = new ServiceCollection();
            collection.AddSingleton<IDispatcherService>(s => dispatcherService);
            collection.AddControlSharedServices();
            collection.AddCodeEditorServices();
            collection.AddMessageBoardServices();
            collection.AddComponentTreeServices();
            collection.AddTestAppServices();
            return collection.BuildServiceProvider();
        }

        private static void AddTestAppServices(this IServiceCollection collection)
        {
            collection.AddSingleton<MainViewModel>();
        }
    }
}
