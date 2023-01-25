using CodeGenie.Ui.Wpf.Controls.Diagram.Models.Diagrams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CodeGenie.Ui.Wpf.Controls.Diagram.Services
{
    public class DiagramTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IDiagramViewModel diagramViewModel)
            {
                var controlType = diagramViewModel.ControlType;
                return new DataTemplate()
                {
                    VisualTree = new FrameworkElementFactory(controlType)
                };
            }
            else
            {
                return CreateErrorViewModel(item);
            }
        }

        protected DataTemplate CreateErrorViewModel(object viewModel)
        {
            var returned = new DataTemplate()
            {
                VisualTree = new FrameworkElementFactory(typeof(TextBlock))
            };

            returned.VisualTree.SetValue(TextBlock.TextProperty, $"ERROR: The ViewModel '{viewModel?.GetType().Name ?? null}' cannot be cast as a '{typeof(IDiagramViewModel).Name}'");
            returned.VisualTree.SetValue(TextBlock.TextWrappingProperty, TextWrapping.Wrap);
            returned.VisualTree.SetValue(FrameworkElement.WidthProperty, (double)100.0);

            return returned;
        }
    }
}
