using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace CodeGenie.Ui.Wpf.Controls.Diagram.Models.Adorners.ResizeRotate
{
    public class ResizeRotateAdorner : Adorner
    {
        private VisualCollection visuals;
        private ResizeRotateChrome chrome;

        protected override int VisualChildrenCount
        {
            get
            {
                return visuals.Count;
            }
        }

        public ResizeRotateAdorner(ContentControl designerItem)
            : base(designerItem)
        {
            SnapsToDevicePixels = true;
            chrome = new ResizeRotateChrome();
            chrome.DataContext = designerItem;
            visuals = new VisualCollection(this);
            visuals.Add(chrome);
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            chrome.Arrange(new Rect(arrangeBounds));
            return arrangeBounds;
        }

        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }
    }
}
