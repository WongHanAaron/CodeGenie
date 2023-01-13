using CodeGenie.Ui.Wpf.Controls.Shared.Contracts;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete
{
    /// <summary> The component to create completion windows for the code editor </summary>
    public interface ICompletionWindowFactory : IInjectsEditor
    {
        /// <summary> Create a completion window for the code editor </summary>
        CompletionWindow Create();
    }

    public class CompletionWindowFactory : ICompletionWindowFactory
    {
        public TextEditor _editor;
        public CompletionWindow Create()
        {
            if (_editor == null) return null;
            return new CompletionWindow(_editor.TextArea);
        }

        public void InjectEditor(TextEditor editor)
        {
            _editor = editor;
        }

        public void TearDownEditor(TextEditor editor)
        {
            _editor = null;
        }
    }
}
