using CodeGenie.Ui.Wpf.Controls.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CodeGenie.Ui.Wpf.Controls.ComponentTree
{
    /// <summary>
    /// Interaction logic for ComponentTreeControl.xaml
    /// </summary>
    public partial class ComponentTreeControl : UserControl
    {
        public ComponentTreeControl()
        {
            InitializeComponent();
            ServiceProvider = ComponentTreeServiceExtensions.CreateDefaultServiceProvider(new DispatcherService(this.Dispatcher));
        }

        public void LoadComponents()
        {
            if (ServiceProvider == null) return;
        }

        #region Dependency Properties
        public static readonly DependencyProperty ServiceProviderProperty =
            DependencyProperty.Register(nameof(ServiceProvider),
                typeof(IServiceProvider),
                typeof(ComponentTreeControl), new PropertyMetadata(OnServiceProviderChanged));
        public IServiceProvider ServiceProvider { get => (IServiceProvider)GetValue(ServiceProviderProperty); set => SetValue(ServiceProviderProperty, value); }

        private static void OnServiceProviderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ComponentTreeControl editor)
            {
                editor.ServiceProvider = e.NewValue as IServiceProvider;
                editor.LoadComponents();
            }
        }
        #endregion
    }
}
