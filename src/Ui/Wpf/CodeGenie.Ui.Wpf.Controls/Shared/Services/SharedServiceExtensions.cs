using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.Shared.Services
{
    public static class SharedServiceExtensions
    {
        public static void AddControlSharedServices(this IServiceCollection serviceCollection, IDispatcherService? dispatcherService = null)
        {
            serviceCollection.AddTransient<IDateTimeProvider, DateTimeProvider>();
            serviceCollection.AddSingleton<IPeriodicEventService, PeriodicEventService>();
            if (dispatcherService != null)
                serviceCollection.AddSingleton<IDispatcherService>(s => dispatcherService);
        }
    }
}
