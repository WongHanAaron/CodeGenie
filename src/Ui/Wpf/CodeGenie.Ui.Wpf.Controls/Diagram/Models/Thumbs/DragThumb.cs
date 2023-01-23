using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace CodeGenie.Ui.Wpf.Controls.Diagram.Models.Thumbs
{
    public class DragThumb : Thumb
    {
        public DragThumb()
        {
            DragDelta += new DragDeltaEventHandler(MoveThumb_DragDelta);
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Control item = DataContext as Control;

            if (item != null)
            {
                double left = Canvas.GetLeft(item);
                double top = Canvas.GetTop(item);

                Canvas.SetLeft(item, left + e.HorizontalChange);
                Canvas.SetTop(item, top + e.VerticalChange);
            }
        }
    }
}
