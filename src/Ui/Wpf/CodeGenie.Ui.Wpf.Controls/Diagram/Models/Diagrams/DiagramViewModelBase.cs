using CodeGenie.Ui.Wpf.Controls.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CodeGenie.Ui.Wpf.Controls.Diagram.Models.Diagrams
{
    public abstract class DiagramViewModelBase<TControl> : ViewModelBase, IDiagramViewModel where TControl : UIElement
    {
        private Lazy<Type> _controlTypeAccessor = new Lazy<Type>(() => typeof(TControl));
        public Type ControlType => _controlTypeAccessor.Value;

        public DiagramViewModelBase(Func<TControl> factory = null)
        {
        }
    }
}
