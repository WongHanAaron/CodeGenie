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
    public class CSharpGenerator_Tests : CodeGenieTestBase
    {
        public IComponentGenerator Generator { get; set; }

        public IComponentDefinitionParser Parser { get; set; }

        public IComponentGenerator CreateGenerator()
        {
            return new CSharpGenerator();
        }

        [SetUp]
        public void SetUp()
        {
            Generator = CreateGenerator();

            ServiceProvider = BuildServiceProvider();
            Parser = ServiceProvider?.GetService<IComponentDefinitionParser>();
            Assert.IsNotNull(Parser, "The parser could not be injected");
        }

        //[TestCase("+T:class", 
        //          "public class T {}")]
        public void TestGenerate(string script, string expectedCodeWithoutNewlines)
        {
            var result = Parser.Parse(script);

            Assert.That(result.Errors.Any(), Is.EqualTo(false), $"There was an unexpected error in the script '{script}'");

            Assert.That(result.Components.Any(), "There were no components defined");

            var component = result.Components.FirstOrDefault();

            var generated = Generator.Generate(component);

            Assert.That(generated.GeneratedCode, Is.EqualTo(expectedCodeWithoutNewlines));
        }
    }
}
