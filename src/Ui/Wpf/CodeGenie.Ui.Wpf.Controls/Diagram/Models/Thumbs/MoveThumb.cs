using CodeGenie.Ui.Wpf.Controls.Diagram.Models.Diagrams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace CodeGenie.Ui.Wpf.Controls.Diagram.Models.Thumbs
{
    public class MoveThumb : Thumb
    {
        private RotateTransform rotateTransform;
        private IDiagramViewModel Diagram;

        public MoveThumb()
        {
            DragStarted += new DragStartedEventHandler(this.MoveThumb_DragStarted);
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
        }

        private void MoveThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.Diagram = DataContext as IDiagramViewModel;
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (this.Diagram != null)
            {
                Point dragDelta = new Point(e.HorizontalChange, e.VerticalChange);

                if (this.rotateTransform != null)
                {
                    dragDelta = this.rotateTransform.Transform(dragDelta);
                }

                Diagram.Left = Diagram.Left + dragDelta.X;
                Diagram.Top = Diagram.Top + dragDelta.Y;
            }
        }
    }
}
