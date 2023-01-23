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
            if (item == null) return null;
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
                throw new ArgumentException($"The content {item.GetType()} of the diagram view is not of type '{nameof(IDiagramViewModel)}'");
            }
        }
    }
}
