using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.Shared.Contracts
{
    /// <summary> Service that injects the editor </summary>
    public interface IInjectsEditor
    {
        void InjectEditor(TextEditor editor);
        void TearDownEditor(TextEditor editor);
    }
}
