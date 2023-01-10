using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Visuals
{
    public class TextMarker : TextSegment
    {
        public TextMarker(int startOffset, int length)
        {
            StartOffset = startOffset;
            Length = length;
        }

        public Color? BackgroundColor { get; set; }
        public Color MarkerColor { get; set; }
        public string ToolTip { get; set; }
    }
}
