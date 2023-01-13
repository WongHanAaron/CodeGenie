using CodeGenie.Ui.Wpf.Controls.Shared.Contracts;
using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.EditorTracking
{
    public interface ICaretController : IInjectsEditor
    {
        void MoveCaretToLineNumber(int lineNumber);
        void MoveCaretToColumn(int column);
    }

    public class CaretController : ICaretController
    {
        protected TextEditor _editor;
        public void InjectEditor(TextEditor editor) => _editor = editor;
        public void TearDownEditor(TextEditor editor) => _editor = null;

        public void MoveCaretToLineNumber(int lineNumber)
        {
            if (_editor == null) return;
            _editor.TextArea.Caret.Line = lineNumber;
        }

        public void MoveCaretToColumn(int column)
        {
            if (_editor == null) return;
            _editor.TextArea.Caret.Column = column;
        }
    }
}
