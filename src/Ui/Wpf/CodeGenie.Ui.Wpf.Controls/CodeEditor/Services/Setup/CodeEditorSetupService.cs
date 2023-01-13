using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.AutoComplete;
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
        protected readonly IServiceProvider ServiceProvider;
        protected readonly IComponentCompilationService CompilationService;
        protected readonly List<Type> ServicesToStart = new List<Type>()
        {
            typeof(IComponentCompilationService),
            typeof(IAutoCompleter)
        };

        public CodeEditorSetupService(IAvalonEditConfigurer configurer,
                                      IServiceProvider serviceProvider)
        {
            EditorConfigurer = configurer;
            ServiceProvider = serviceProvider;
        }

        public void Setup(TextEditor textEditor)
        {
            EditorConfigurer.Configure(textEditor);
            StartServices();
        }

        public void TearDown(TextEditor textEditor)
        {
            EditorConfigurer.TearDown(textEditor);
        }

        protected void StartServices()
        {
            ServicesToStart.ForEach(s => ServiceProvider.GetService(s));
        }
    }
}
