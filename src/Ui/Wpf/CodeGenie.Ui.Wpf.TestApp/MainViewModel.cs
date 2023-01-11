using CodeGenie.Ui.Wpf.Controls.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.TestApp
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get => Get<IServiceProvider>(); set => Set(value); }
        public string Title { get => Get<string>(); set => Set(value); }
    }
}
