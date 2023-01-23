using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.Diagram.Models.Diagrams
{
    public interface IDiagramViewModel : INotifyPropertyChanged
    {
        /// <summary> The control instance that is displayed </summary>
        Type ControlType { get; }
    }
}
