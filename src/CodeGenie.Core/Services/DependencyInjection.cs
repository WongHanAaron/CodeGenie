using CodeGenie.Core.Models.Configuration;
using CodeGenie.Core.Services.Generators.ComponentGenerators;
using CodeGenie.Core.Services.Parsing.ComponentDefinitions;
using CodeGenie.Core.Services.Parsing.ComponentDefinitions.DefinitionParsing;
using CodeGenie.Core.Services.Parsing.ComponentDefinitions.SemanticValidation;
using CodeGenie.Core.Services.Parsing.ComponentDefinitions.SemanticValidation.Validations;
using CodeGenie.Core.Services.Parsing.ComponentDefinitions.Shared;
using CodeGenie.Core.Services.Parsing.ComponentDefinitions.SyntaxDescribing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenie.Core.Services
{
    public static class DependencyInjection
    {
        /// <summary> Add the dependencies required for CodeGenie </summary>
        public static void AddCodeGenie(this IServiceCollection serviceCollection, Action<ServiceCreationOptions> creationOptions = null)
        {
            var options = new ServiceCreationOptions();
            creationOptions?.Invoke(options);
            serviceCollection.AddLoggerProviders(options);
            serviceCollection.AddDefinitionParserDependencies();
            serviceCollection.AddSemanticValidatorDependencies();
            serviceCollection.AddSyntaxDescribingDependencies();
            serviceCollection.AddGeneratorDependencies();
            serviceCollection.AddTransient<IComponentCompiler, ComponentCompiler>();
        }

        private static void AddLoggerProviders(this IServiceCollection serviceCollection, ServiceCreationOptions options)
        {
            if (options == null) return;
            if (options.LoggerProviders == null) return;
            if (!options.LoggerProviders.Any()) return;

            serviceCollection.AddLogging(builder => 
            {
                builder.ClearProviders();
                builder.SetMinimumLevel(LogLevel.Trace);
                foreach (var provider in options.LoggerProviders)
                {
                    builder.AddProvider(provider);
                }
            });
        }

        private static void AddDefinitionParserDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IComponentDefinitionParser, ComponentDefinitionParser>();
            serviceCollection.AddTransient<IComponentDefinitionContextParser, ComponentDefinitionContextParser>();
        }

        private static void AddSemanticValidatorDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISemanticValidator, ClusteredSemanticValidator>();
            serviceCollection.AddTransient<IComponentValidation, DuplicateComponentValidation>();
            serviceCollection.AddTransient<IComponentValidation, EmptyNameValidation>();
        }
    
        private static void AddSyntaxDescribingDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ISyntaxDescriber, SyntaxDescriber>();
            serviceCollection.AddTransient<ISyntaxDescriberTreeSearcher, SyntaxDescriberTreeSearcher>();
        }

        private static void AddGeneratorDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IComponentGeneratorFactory, ComponentGeneratorFactory>();
            serviceCollection.AddCodeGenieGeneratorDependencies();
            serviceCollection.AddCSharpGeneratorDependencies();
        }

        private static void AddCodeGenieGeneratorDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IComponentGenerator, CodeGenieComponentGenerator>();
        }

        private static void AddCSharpGeneratorDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IComponentGenerator, CSharpComponentGenerator>();
        }
    }
}
