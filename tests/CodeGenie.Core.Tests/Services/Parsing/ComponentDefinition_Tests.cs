
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
            var components = Parser.Parse(script).Components;
            Assert.AreEqual(expectedCount, components.Count(), "The returned count of components do not match the expected count");
        }

        [TestCase("+ TestClass : class", "TestClass", "", false)]
        [TestCase("+ TestClass2 : interface", "TestClass2", "", true)]
        [TestCase("+ TestClass3 : interface { purpose : \"Something\"}", "TestClass3", "Something", true)]
        public void Component_Parse_Matches_Name(string script, string expectedName, string purpose, bool expectIsInterface)
        {
            var components = Parser.Parse(script).Components;
            var first = components.FirstOrDefault();
            Assert.AreEqual(expectedName, first.Name);
            Assert.AreEqual(expectIsInterface, first.IsInterface);
        }

        [TestCase("+ TestClass : class { attributes { + Attribute : string } }", 1)]
        public void Component_Parse_Attribute_Count_Matches(string script, int expectedAttributeCount)
        {
            var components = Parser.Parse(script).Components;
            Assert.AreEqual(expectedAttributeCount, components.FirstOrDefault().Attributes.Count());
        }

        [TestCase("+ TestClass : class { attributes { + Attribute : string} }", "Attribute", "string")]
        public void Component_Parse_Attribute_Name_And_Type_Matches(string script, string expectedName, string expectedType)
        {
            var components = Parser.Parse(script).Components;
            var first = components.FirstOrDefault();
            var attribute = first.Attributes.FirstOrDefault();
            Assert.AreEqual(expectedName, attribute.Name);
            Assert.AreEqual(expectedType, attribute.Type);
        }

        [TestCase("+ Testclass : somehing", true)]
        [TestCase("TestClass : class",  true)]
        public void Component_Parsing_Error(string script, bool expectError)
        {
            var result = Parser.Parse(script);

            Assert.AreEqual(expectError, result.HasErrors);
        }
    }
}
