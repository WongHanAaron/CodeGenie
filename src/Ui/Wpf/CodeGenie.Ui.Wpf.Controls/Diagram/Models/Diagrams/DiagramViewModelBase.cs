using CodeGenie.Ui.Wpf.Controls.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CodeGenie.Ui.Wpf.Controls.Diagram.Models.Diagrams
{
    public abstract class DiagramViewModelBase<TControl> : ViewModelBase, IDiagramViewModel where TControl : UIElement
    {
        private Lazy<Type> _controlTypeAccessor = new Lazy<Type>(() => typeof(TControl));
        public Type ControlType => _controlTypeAccessor.Value;

        public double Top { get => Get<double>(); set => Set(value); }
        public double Left { get => Get<double>(); set => Set(value); }
        public double Width { get => Get<double>(); set => Set(value); }
        public double Height { get => Get<double>(); set => Set(value); }
        public RotateTransform RotateTransform { get => Get<RotateTransform>(); set => Set(value); }

        public DiagramViewModelBase(Func<TControl> factory = null)
        {
        }
    }
}
