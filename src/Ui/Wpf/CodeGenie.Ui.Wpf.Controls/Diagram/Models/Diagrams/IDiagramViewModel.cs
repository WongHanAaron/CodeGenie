using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CodeGenie.Ui.Wpf.Controls.Diagram.Models.Diagrams
{
    public interface IDiagramViewModel : INotifyPropertyChanged
    {
        /// <summary> The control instance that is displayed </summary>
        Type ControlType { get; }
        public double Top { get; set; }
        public double Left { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public RotateTransform RotateTransform { get; set; }
    }
}
