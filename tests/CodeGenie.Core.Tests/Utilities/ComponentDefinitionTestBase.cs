using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenie.Core.Services;

namespace CodeGenie.Core.Tests.Utilities
{
    /// <summary> The base test class for component definition parsers </summary>
    public class ComponentDefinitionTestBase : MockableTestBase
    {
        protected override void InjectMockedServices(MockableServiceCollection collection)
        {
            MockableServiceCollection.AddCodeGenie(builder =>
            {
                builder.LoggerProviders.Add(new DebugLoggerProvider());
            });
        }
    }
}
