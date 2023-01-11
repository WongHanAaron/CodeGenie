using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace CodeGenie.Ui.Wpf.Controls.Shared
{
    public class ServiceProviderContainer : MarkupExtension
    {
        public static IServiceProvider Provider { get; set; }
        public Type ServiceType { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var service = Provider?.GetService(ServiceType);
            return service;
        }
    }
}
