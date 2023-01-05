using CodeGenie.Core.Models.Configuration;
using CodeGenie.Core.Services.Parsing.ComponentDefinitions.DefinitionParsers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Services
{
    public static class ServiceExtensions
    {
        /// <summary> Add the dependencies required for CodeGenie </summary>
        public static void AddCodeGenie(this IServiceCollection serviceCollection, Action<ServiceCreationOptions> creationOptions = null)
        {
            var options = new ServiceCreationOptions();
            creationOptions?.Invoke(options);
            AddDefinitionParserDependencies(serviceCollection);
        }

        private static void AddDefinitionParserDependencies(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IDefinitionParser, DefinitionParser>();
        }
    }
}
