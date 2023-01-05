
using CodeGenie.Core.Services.Parsing.ComponentDefinitions.DefinitionParsers;
using CodeGenie.Core.Tests.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace CodeGenie.Core.Tests.Services.Parsing
{
    [TestFixture]
    public class ComponentDefinition_Tests : ComponentDefinitionParserTestBase
    {
        IDefinitionParser Parser;

        [SetUp]
        public void SetUp()
        {
            ServiceProvider = BuildServiceProvider();
            Parser = ServiceProvider.GetService<IDefinitionParser>();
            Assert.IsNotNull(Parser, "The parser could not be injected");
        }

        [TestCase("+ TestClass : class", 1)]
        [TestCase("+ TestClass : class - TestClass2 : class + TestClass3 : interface", 3)]
        public void Component_Parse_Matches_Count(string script, int expectedCount)
        {
            var components = Parser.Parse(script);
            Assert.AreEqual(expectedCount, components.Count(), "The returned count of components do not match the expected count");
        }
    }
}
