using CodeGenie.Ui.Wpf.Controls.Shared.Contracts;
using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.EditorTracking
{
    /// <summary> An event listener for events on the TextView </summary>
    public interface ITextViewEventListener : IInjectsEditor
    {
        EventHandler<MouseEventArgs> MouseHover { get; set; }
        EventHandler<MouseEventArgs> MouseHoverStopped { get; set; }
        EventHandler<EventArgs> VisualLinesChanged { get; set; }
    }

    public class TextViewEventListener : ITextViewEventListener
    {
        public EventHandler<MouseEventArgs> MouseHover { get; set; }
        public EventHandler<MouseEventArgs> MouseHoverStopped { get; set; }
        public EventHandler<EventArgs> VisualLinesChanged { get; set; }

        public void InjectEditor(TextEditor editor)
        {
            var textView = editor.TextArea.TextView;
            textView.MouseHover += TextView_MouseHover;
            textView.MouseHoverStopped += TextView_MouseHoverStopped;
            textView.VisualLinesChanged += TextView_VisualLinesChanged;
        }

        private void TextView_VisualLinesChanged(object? sender, EventArgs e) => VisualLinesChanged?.Invoke(sender, e);
        private void TextView_MouseHoverStopped(object sender, MouseEventArgs e) => MouseHoverStopped?.Invoke(sender, e);
        private void TextView_MouseHover(object sender, MouseEventArgs e) => MouseHover?.Invoke(sender, e);

        public void TearDownEditor(TextEditor editor)
        {
            var textView = editor.TextArea.TextView;
            textView.MouseHover -= TextView_MouseHover;
            textView.MouseHoverStopped -= TextView_MouseHoverStopped;
            textView.VisualLinesChanged -= TextView_VisualLinesChanged;
        }
    }
}
