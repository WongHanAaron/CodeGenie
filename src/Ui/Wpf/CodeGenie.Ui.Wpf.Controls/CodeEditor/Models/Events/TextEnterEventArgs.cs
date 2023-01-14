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
        /// <summary> Document Line object from the Code Editor </summary>
        public DocumentLine DocumentLine { get; set; }

        /// <summary> The text that raised the event </summary>
        public string Text { get; set; }

        /// <summary> Line number that is currently being edited </summary>
        public int LineNumber { get; set; }

        /// <summary> The column number of the text that was being typed </summary>
        public int ColumnNumber { get; set; }

        /// <summary> Offset within the document </summary>
        public int Offset { get; set; }

        /// <summary> Length of the line </summary>
        public int Length { get; set; }

        /// <summary> The contents of the current line </summary>
        public string LineContent { get; set; }
    }
}
