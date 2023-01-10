using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using CodeGenie.Core.Services.Parsing.ComponentDefinitions.DefinitionParsers;
using CodeGenie.Core.Services.Parsing.ComponentDefinitions.SemanticValidators;
using CodeGenie.Core.Tests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Core.Tests.Services.Validation
{
    public class ComponentValidation_Tests : ComponentDefinitionParserTestBase
    {
        IComponentDefinitionParser Parser;
        ISemanticValidator Validator;

        [SetUp]
        public void SetUp()
        {
            ServiceProvider = BuildServiceProvider();
            
            Parser = ServiceProvider.GetService<IComponentDefinitionParser>();
            Assert.IsNotNull(Parser, "The parser could not be injected");

            Validator = ServiceProvider.GetService<ISemanticValidator>();
            Assert.IsNotNull(Validator, "The validator could not be injected");
        }

        private ParsingResult ParseWithoutExceptions(string script)
        {
            var parsingResult = Parser.Parse(script);
            Assert.IsFalse(parsingResult.HasErrors, $"Do not expect parsing errors for script '{script}'");
            return parsingResult;
        }

        [TestCase("+ Class1 : class + Class1 : class", true)]
        [TestCase("+ Class1 : class", false)]
        public void DuplicateComponentName_Fails(string script, bool expectError)
        {
            var components = ParseWithoutExceptions(script);
            var result = Validator.Validate(components);

            Assert.AreEqual(expectError, result.HasErrors, $"Expected error {expectError} but was {result.HasErrors} instead");
        }
    }
}
