using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Models.Events
{
    public class TextEnterEventArgs : DateTimeEventArgs
    {
        public DocumentLine DocumentLine { get; set; }
        public int LineNumber { get; set; }

        /// <summary> Offset within the document </summary>
        public int Offset { get; set; }

        /// <summary> Length of the line </summary>
        public int Length { get; set; }

        /// <summary> The contents of the current line </summary>
        public string LineContent { get; set; }
    }
}
