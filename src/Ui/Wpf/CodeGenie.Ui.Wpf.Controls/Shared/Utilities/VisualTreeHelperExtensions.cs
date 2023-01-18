using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CodeGenie.Ui.Wpf.Controls.Shared.Utilities
{
    /// <summary>
    /// Reference: https://stackoverflow.com/questions/41132649/get-datagrids-scrollviewer
    /// </summary>
    public static class VisualTreeHelperExtensions
    {
        public static TElement GetFirstChildElement<TElement>(this UIElement element) where TElement : UIElement
        {
            if (element == null) return null;

            TElement retour = null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element) && retour == null; i++)
            {
                if (VisualTreeHelper.GetChild(element, i) is TElement)
                {
                    retour = (TElement)(VisualTreeHelper.GetChild(element, i));
                }
                else
                {
                    retour = GetFirstChildElement<TElement>(VisualTreeHelper.GetChild(element, i) as UIElement);
                }
            }
            return retour;
        }
    }
}
