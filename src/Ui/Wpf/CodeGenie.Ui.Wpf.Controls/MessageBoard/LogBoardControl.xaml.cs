using CodeGenie.Ui.Wpf.Controls.MessageBoard.ViewModels;
using Microsoft.Extensions.DependencyInjection;
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

namespace CodeGenie.Ui.Wpf.Controls.MessageBoard
{
    /// <summary>
    /// Interaction logic for LogBoardControl.xaml
    /// </summary>
    public partial class LogBoardControl : UserControl
    {
        public LogBoardControl()
        {
            InitializeComponent();
        }

        public void LoadComponents()
        {
            if (ServiceProvider == null) return;
            ViewModel = ServiceProvider.GetService<LogBoardViewModel>();
        }


        #region Dependency Properties
        public static readonly DependencyProperty ServiceProviderProperty =
            DependencyProperty.Register(nameof(ServiceProvider),
                typeof(IServiceProvider),
                typeof(LogBoardControl), new PropertyMetadata(OnServiceProviderChanged));
        public IServiceProvider ServiceProvider { get => (IServiceProvider)GetValue(ServiceProviderProperty); set => SetValue(ServiceProviderProperty, value); }

        private static void OnServiceProviderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LogBoardControl messageBoard)
            {
                messageBoard.ServiceProvider = e.NewValue as IServiceProvider;
                messageBoard.LoadComponents();
            }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel),
                typeof(LogBoardViewModel),
                typeof(LogBoardControl));
        public LogBoardViewModel ViewModel { get => (LogBoardViewModel)GetValue(ViewModelProperty); set => SetValue(ViewModelProperty, value); }

        #endregion
    }
}
