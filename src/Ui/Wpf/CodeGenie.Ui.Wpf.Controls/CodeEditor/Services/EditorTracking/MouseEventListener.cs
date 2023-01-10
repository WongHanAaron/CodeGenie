using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.EditorTracking
{
    public interface IMouseEventListener
    {
        
    }

    public class MouseEventListener
    {
        public MouseEventListener()
        {

        }

        protected EventHandler<MouseEventArgs> MouseEnter { get; set; }
        protected EventHandler<MouseEventArgs> MouseLeave { get; set; }
        protected EventHandler<MouseEventArgs> MouseMove { get; set; }

        public void AttachEvents(TextEditor editor)
        {
            editor.MouseMove += Editor_MouseMove;
            editor.MouseLeave += Editor_MouseLeave;
            editor.MouseEnter += Editor_MouseEnter;
        }

        private void Editor_MouseEnter(object sender, MouseEventArgs e) => MouseEnter?.Invoke(sender, e);
        private void Editor_MouseLeave(object sender, MouseEventArgs e) => MouseLeave?.Invoke(sender, e);
        private void Editor_MouseMove(object sender, MouseEventArgs e) => MouseMove?.Invoke(sender, e);

        public void DetachEvents(TextEditor editor)
        {
            editor.MouseMove -= Editor_MouseMove;
            editor.MouseLeave -= Editor_MouseLeave;
            editor.MouseEnter -= Editor_MouseEnter;
        }
    }
}
