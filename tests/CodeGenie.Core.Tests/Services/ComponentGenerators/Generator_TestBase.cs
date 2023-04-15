using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using CodeGenie.Core.Services.Generators.ComponentGenerators;
using CodeGenie.Core.Services.Parsing.ComponentDefinitions.DefinitionParsing;
using CodeGenie.Core.Tests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Core.Tests.Services.ComponentGenerators
{
    public abstract class Generator_TestBase<TGenerator> : CodeGenieTestBase where TGenerator : class, IComponentGenerator
    {
        public IComponentGenerator Generator { get; set; }

        public TGenerator TypedGenerator { get; set; }

        public IComponentDefinitionParser Parser { get; set; }

        public IComponentGeneratorFactory GeneratorFactory { get; set; }

        /// <summary> Sets up the generator and parser. To be called with the SetUp </summary>
        public void SetUpGeneratorAndParser()
        {
            ServiceProvider = BuildServiceProvider();

            GeneratorFactory = ServiceProvider?.GetService<IComponentGeneratorFactory>();

            TypedGenerator = GeneratorFactory?.Create<TGenerator>() as TGenerator;

            Generator = TypedGenerator;

            Parser = ServiceProvider?.GetService<IComponentDefinitionParser>();

            Assert.IsNotNull(Generator, $"The {typeof(TGenerator).Name} could not be injected");

            Assert.IsNotNull(Parser, "The parser could not be injected");
        }

        public ParsingResult ParseAndAssertNoErrors(string script)
        {
            var result = Parser.Parse(script);

            Assert.That(result.HasErrors, Is.False, $"Do not expect any errors in script '{script}'");

            return result;
        }
    }
}
