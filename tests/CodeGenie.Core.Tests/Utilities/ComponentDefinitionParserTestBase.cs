using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenie.Core.Services;

namespace CodeGenie.Core.Tests.Utilities
{
    /// <summary> The base test class for component definition parsers </summary>
    public class ComponentDefinitionParserTestBase
    {
        protected MockableServiceCollection MockableServiceCollection = new MockableServiceCollection();
        protected IServiceProvider ServiceProvider { get; set; }

        public IServiceProvider BuildServiceProvider(Action<MockableServiceCollection> mockServicesMethod = null)
        {
            MockableServiceCollection.AddCodeGenie(builder => 
            {
                builder.LoggerProviders.Add(new DebugLoggerProvider());
            });
            mockServicesMethod?.Invoke(MockableServiceCollection);
            return MockableServiceCollection.BuildMockedServiceProvider();
        }
    }
}
