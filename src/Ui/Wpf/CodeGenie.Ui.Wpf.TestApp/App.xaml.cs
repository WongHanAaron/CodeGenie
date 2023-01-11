using CodeGenie.Ui.Wpf.Controls.Shared;
using CodeGenie.Ui.Wpf.Controls.Shared.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CodeGenie.Ui.Wpf.TestApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ServiceProviderContainer.Provider = 
                ServiceRegistration.GetServiceProvider(new DispatcherService(App.Current.Dispatcher));
        }
    }
}
