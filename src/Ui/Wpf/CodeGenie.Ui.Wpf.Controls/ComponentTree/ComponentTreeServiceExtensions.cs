using CodeGenie.Ui.Wpf.Controls.ComponentTree.ViewModels;
using CodeGenie.Ui.Wpf.Controls.Shared.Services;
using CodeGenie.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Debug;

namespace CodeGenie.Ui.Wpf.Controls.ComponentTree
{
    public static class ComponentTreeServiceExtensions
    {
        public static IServiceProvider CreateDefaultServiceProvider(IDispatcherService? dispatcherService = null)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddControlSharedServices(dispatcherService);
            serviceCollection.AddComponentTreeServices();
            serviceCollection.AddCodeGenie(o => 
            {
                o.LoggerProviders.Add(new DebugLoggerProvider());
            });
            return serviceCollection.BuildServiceProvider();
        }

        public static void AddComponentTreeServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ComponentTreeViewModel>();
        }
    }
}
