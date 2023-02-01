using CodeGenie.Core.Tests.Utilities;
using CodeGenie.Ui.Wpf.Controls.CodeEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.Tests.CodeEditor
{
    public class CodeEditorTestBase : MockableTestBase
    {
        protected override void InjectMockedServices(MockableServiceCollection collection)
        {
            collection.AddCodeEditorServices(builder =>
            {
                builder.LoggerProviders.Add(new DebugLoggerProvider());
            });
        }
    }
}
