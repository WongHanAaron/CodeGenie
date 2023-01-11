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
    /// Interaction logic for MessageBoardControl.xaml
    /// </summary>
    public partial class MessageBoardControl : UserControl
    {
        public MessageBoardControl()
        {
            InitializeComponent();
            ServiceProvider = MessageBoardServiceExtensions.CreateDefaultServiceProvider();
        }

        public void LoadComponents()
        {
            if (ServiceProvider == null) return;
        }


        #region Dependency Properties
        public static readonly DependencyProperty ServiceProviderProperty =
            DependencyProperty.Register(nameof(ServiceProvider),
                typeof(IServiceProvider),
                typeof(MessageBoardControl), new PropertyMetadata(OnServiceProviderChanged));
        public IServiceProvider ServiceProvider { get => (IServiceProvider)GetValue(ServiceProviderProperty); set => SetValue(ServiceProviderProperty, value); }

        private static void OnServiceProviderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MessageBoardControl messageBoard)
            {
                messageBoard.ServiceProvider = e.NewValue as IServiceProvider;
                messageBoard.LoadComponents();
            }
        }
        #endregion
    }
}
