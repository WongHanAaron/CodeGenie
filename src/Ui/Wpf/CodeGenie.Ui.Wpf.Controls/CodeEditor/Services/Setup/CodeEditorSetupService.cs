using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.Compiling;
using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.Setup
{
    /// <summary> The component to setup the code editor and its dependencies </summary>
    public interface ICodeEditorSetupService
    {
        void Setup(TextEditor textEditor);
        void TearDown(TextEditor textEditor);
    }

    public class CodeEditorSetupService : ICodeEditorSetupService
    {
        protected readonly IAvalonEditConfigurer EditorConfigurer;
        protected readonly IComponentCompilationService CompilationService;
        public CodeEditorSetupService(IAvalonEditConfigurer configurer,
                                      IComponentCompilationService compilationService)
        {
            EditorConfigurer = configurer;
            CompilationService = compilationService;
        }

        public void Setup(TextEditor textEditor)
        {
            EditorConfigurer.Configure(textEditor);
        }

        public void TearDown(TextEditor textEditor)
        {
            EditorConfigurer.TearDown(textEditor);
        }
    }
}
