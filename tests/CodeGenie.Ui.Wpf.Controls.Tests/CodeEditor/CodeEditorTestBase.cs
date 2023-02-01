using CodeGenie.Core.Tests.Utilities;
using CodeGenie.Ui.Wpf.Controls.CodeEditor;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Services.Setup;
using CodeGenie.Ui.Wpf.Controls.Shared.Services;
using ICSharpCode.AvalonEdit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CodeGenie.Ui.Wpf.Controls.Tests.CodeEditor
{
    public class CodeEditorTestBase : MockableTestBase
    {
        protected IDispatcherService DispatcherService = new DispatcherService(Dispatcher.CurrentDispatcher);

        public override IServiceProvider BuildServiceProvider(Action<MockableServiceCollection>? mockServicesMethod = null)
        {
            var provider = base.BuildServiceProvider(mockServicesMethod);
            var setupService = provider.GetService<ICodeEditorSetupService>();
            if (setupService == null) Assert.Fail($"The '{nameof(ICodeEditorSetupService)}' is null at this point");

            
            // setupService?.Setup(Editor);

            return provider;
        }

        protected override void InjectMockedServices(MockableServiceCollection collection)
        {
            EditorServiceExtensions.AddDefaultServices(collection, DispatcherService);
        }
    }
}
