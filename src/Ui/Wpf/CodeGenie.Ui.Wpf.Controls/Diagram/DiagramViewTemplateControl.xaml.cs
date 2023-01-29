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

namespace CodeGenie.Ui.Wpf.Controls.Diagram
{
    /// <summary>
    /// Interaction logic for DiagramViewTemplateControl.xaml
    /// </summary>
    public partial class DiagramViewTemplateControl : UserControl
    {
        public DiagramViewTemplateControl()
        {
            InitializeComponent();
            MouseEnter += DiagramViewTemplateControl_MouseEnter;
            MouseLeave += DiagramViewTemplateControl_MouseLeave;
        }

        private void DiagramViewTemplateControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (ItemDecorator != null)
                ItemDecorator.ShowDecorator = true;
        }

        private void DiagramViewTemplateControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (ItemDecorator != null)
                ItemDecorator.ShowDecorator = false;
        }
    }
}
