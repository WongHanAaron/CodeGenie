using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Core.Tests.Utilities
{
    /// <summary> Service collection that allows you to replace specific service registrations with the desired implementation or factory for testing purposes </summary>
    public class MockableServiceCollection : ServiceCollection
    {
        private ConcurrentDictionary<Type, ServiceDescriptor> _overriddenServices = new ConcurrentDictionary<Type, ServiceDescriptor>();

        public void OverrideService<TInterface>(object implementation)
        {
            _overriddenServices[typeof(TInterface)] = new ServiceDescriptor(typeof(TInterface), implementation);
        }

        public void OverrideService<TInterface, TImplementation>() where TImplementation : TInterface
        {
            // Just store it as a singleton for now
            _overriddenServices[typeof(TInterface)] = new ServiceDescriptor(typeof(TInterface), typeof(TImplementation), ServiceLifetime.Singleton);
        }

        public void OverrideService<TInterface>(Func<IServiceProvider, object> factoryMethod)
        {
            _overriddenServices[typeof(TInterface)] = new ServiceDescriptor(typeof(TInterface), factoryMethod, ServiceLifetime.Singleton);
        }

        public IServiceProvider BuildMockedServiceProvider()
        {
            var usedServiceCollection = new ServiceCollection();

            for (int i = 0; i < this.Count; i++)
            {
                var definedDescription = this[i];
                var tInterface = definedDescription.ServiceType;
                
                if (_overriddenServices.ContainsKey(tInterface))
                {
                    var overriddenDescription = _overriddenServices[tInterface];
                    usedServiceCollection.Add(OverrideImplementation(definedDescription, overriddenDescription));
                }
                else
                {
                    usedServiceCollection.Add(definedDescription);
                }
            }

            return usedServiceCollection.BuildServiceProvider();
        }

        public ServiceDescriptor OverrideImplementation(ServiceDescriptor defined, ServiceDescriptor overridden)
        {
            if (overridden.ImplementationFactory != null)
            {
                return new ServiceDescriptor(defined.ServiceType, overridden.ImplementationFactory, defined.Lifetime);
            }
            else if (overridden.ImplementationInstance != null)
            {
                return new ServiceDescriptor(defined.ServiceType, overridden.ImplementationInstance);
            }
            else if (overridden.ImplementationType != null)
            {
                return new ServiceDescriptor(defined.ServiceType, overridden.ImplementationType, defined.Lifetime);
            }
            else
            {
                throw new ArgumentException($"No factory, instance or implementation defined for service of type '{defined.ServiceType.Name}'");
            }
        }
    }
}
