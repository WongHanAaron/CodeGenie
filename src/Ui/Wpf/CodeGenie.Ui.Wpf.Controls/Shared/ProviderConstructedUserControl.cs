using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CodeGenie.Ui.Wpf.Controls.Shared
{
    public abstract class ProviderConstructedUserControl<ControlType> : 
                          UserControl where ControlType : 
                          ProviderConstructedUserControl<ControlType>
    {
        public abstract void ReinitializeFromServiceProvider();

        #region Dependency Properties
        public static readonly DependencyProperty ServiceProviderProperty =
            DependencyProperty.Register(nameof(ServiceProvider),
                typeof(IServiceProvider),
                typeof(ControlType), new PropertyMetadata(OnServiceProviderChanged));
        public IServiceProvider ServiceProvider { get => (IServiceProvider)GetValue(ServiceProviderProperty); set => SetValue(ServiceProviderProperty, value); }

        private static void OnServiceProviderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ControlType control)) throw new ArgumentException($"The control is not of type '{typeof(ControlType).Name}' but of '{d.GetType().Name}'");
            if (!(e.NewValue is IServiceProvider serviceProvider)) throw new ArgumentException($"The changed object is not of type '{typeof(IServiceProvider).Name}' but of type '{e.NewValue.GetType().Name}'");
            
            control.ServiceProvider = serviceProvider;
            control.ReinitializeFromServiceProvider();
        }
        #endregion
    }
}
