using CodeGenie.Ui.Wpf.Controls.MessageBoard.Contracts;
using CodeGenie.Ui.Wpf.Controls.MessageBoard.Models;
using CodeGenie.Ui.Wpf.Controls.MessageBoard.Services;
using CodeGenie.Ui.Wpf.Controls.MessageBoard.ViewModels;
using CodeGenie.Ui.Wpf.Controls.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.MessageBoard
{
    public static class MessageBoardServiceExtensions
    {
        public static IServiceProvider CreateDefaultServiceProvider(IDispatcherService dispatcherService = null)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddControlSharedServices(dispatcherService);
            serviceCollection.AddMessageBoardServices();
            return serviceCollection.BuildServiceProvider();
        }

        public static void AddMessageBoardServices(this IServiceCollection serviceCollection, Action<MessageBoardOptions> optionsUpdater = null)
        {
            var options = new MessageBoardOptions();
            optionsUpdater?.Invoke(options);
            serviceCollection.AddMessageBoardDependencies(options);
        }

        private static void AddMessageBoardDependencies(this IServiceCollection serviceCollection, MessageBoardOptions options)
        {
            var loggerProvider = new MessageBoardProvider(options.MessageBoard);
            
            serviceCollection.AddLogging(b => 
            {
                b.ClearProviders();
                b.AddProvider(loggerProvider);
                b.SetMinimumLevel(LogLevel.Trace);
            });

            serviceCollection.AddSingleton<MessageBoardOptions>(options);
            serviceCollection.AddSingleton<IMessageBoardProvider, MessageBoardProvider>(s => loggerProvider);
            serviceCollection.AddSingleton<IMessageChannelRepository, MessageChannelRepository>();
            serviceCollection.AddTransient<ISingleMessageUpdateListener, SingleMessageUpdateListener>();
            serviceCollection.AddSingleton<LogBoardViewModel>();
            serviceCollection.AddSingleton<StatusBoardViewModel>();
        }
    }
}
