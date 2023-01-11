using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Contracts;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.Setup;
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

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor
{
    /// <summary>
    /// Interaction logic for EditorControl.xaml
    /// </summary>
    public partial class EditorControl : UserControl
    {
        public EditorControl()
        {
            InitializeComponent();
            ServiceProvider = EditorServiceExtensions.CreateDefaultServiceProvider();
        }

        public void LoadComponents()
        {
            if (ServiceProvider == null) return;
            var setupService = ServiceProvider.GetService<ICodeEditorSetupService>();
            setupService.Setup(TextEditor);
        }

        #region Dependency Properties
        public static readonly DependencyProperty ServiceProviderProperty =
            DependencyProperty.Register(nameof(ServiceProvider),
                typeof(IServiceProvider),
                typeof(EditorControl), new PropertyMetadata(OnServiceProviderChanged));
        public IServiceProvider ServiceProvider { get => (IServiceProvider)GetValue(ServiceProviderProperty); set => SetValue(ServiceProviderProperty, value); }

        private static void OnServiceProviderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EditorControl editor)
            {
                editor.ServiceProvider = e.NewValue as IServiceProvider;
                editor.LoadComponents();
            }
        }
        #endregion
    }
}
