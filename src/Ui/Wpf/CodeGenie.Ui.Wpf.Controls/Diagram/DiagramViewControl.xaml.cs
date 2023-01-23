using CodeGenie.Ui.Wpf.Controls.Diagram.Models.Diagrams;
using CodeGenie.Ui.Wpf.Controls.Diagram.Models.Diagrams.Samples;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace CodeGenie.Ui.Wpf.Controls.Diagram
{
    /// <summary>
    /// Interaction logic for DiagramViewControl.xaml
    /// </summary>
    public partial class DiagramViewControl : UserControl
    {
        public DiagramViewControl()
        {
            InitializeComponent();
            Diagrams = new ObservableCollection<IDiagramViewModel>()
            {
                new EllipseViewModel()
            };
        }

        public void LoadComponents()
        {
            if (ServiceProvider == null) return;
        }

        #region Dependency Properties
        public static readonly DependencyProperty ServiceProviderProperty =
            DependencyProperty.Register(nameof(ServiceProvider),
                typeof(IServiceProvider),
                typeof(DiagramViewControl), new PropertyMetadata(OnServiceProviderChanged));
        public IServiceProvider ServiceProvider { get => (IServiceProvider)GetValue(ServiceProviderProperty); set => SetValue(ServiceProviderProperty, value); }

        private static void OnServiceProviderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DiagramViewControl control)
            {
                control.ServiceProvider = e.NewValue as IServiceProvider;
                control.LoadComponents();
            }
        }

        public static readonly DependencyProperty DiagramsProperty =
            DependencyProperty.Register(nameof(Diagrams),
                typeof(ObservableCollection<IDiagramViewModel>),
                typeof(DiagramViewControl));
        public ObservableCollection<IDiagramViewModel> Diagrams { get => (ObservableCollection<IDiagramViewModel>)GetValue(DiagramsProperty); set => SetValue(DiagramsProperty, value); }

        #endregion
    }
}
