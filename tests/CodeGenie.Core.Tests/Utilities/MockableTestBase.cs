using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Core.Tests.Utilities
{
    public abstract class MockableTestBase
    {
        protected MockableServiceCollection MockableServiceCollection = new MockableServiceCollection();
        protected IServiceProvider? ServiceProvider { get; set; }
        public virtual IServiceProvider BuildServiceProvider(Action<MockableServiceCollection>? mockServicesMethod = null)
        {
            MockableServiceCollection = new MockableServiceCollection();
            InjectMockedServices(MockableServiceCollection);
            mockServicesMethod?.Invoke(MockableServiceCollection);
            return MockableServiceCollection.BuildMockedServiceProvider();
        }

        protected abstract void InjectMockedServices(MockableServiceCollection collection);
    }
}
