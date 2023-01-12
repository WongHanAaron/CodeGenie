
using CodeGenie.Core.Services.Parsing.ComponentDefinitions.DefinitionParsers;
using CodeGenie.Core.Tests.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace CodeGenie.Core.Tests.Services.Parsing
{
    [TestFixture]
    public class ComponentDefinition_Tests : ComponentDefinitionParserTestBase
    {
        IComponentDefinitionParser Parser;

        [SetUp]
        public void SetUp()
        {
            ServiceProvider = BuildServiceProvider();
            Parser = ServiceProvider.GetService<IComponentDefinitionParser>();
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

        [TestCase("+ Testclass : something", true)]
        [TestCase("TestClass : class",  true)]
        public void Component_Parsing_Error(string script, bool expectError)
        {
            var result = Parser.Parse(script);

            Assert.AreEqual(expectError, result.HasErrors);
        }

        [TestCase("+ TestClass : class", "TestClass", 0, 18, 1, 0)]
        [TestCase("+ TestClass : class\n+ TestClass2 : class", "TestClass2", 20, 39, 2, 0)]
        public void Component_Parse_Correct_Positions(string script, string componentToTest, int startIndex, int endIndex, int lineNumber, int columnPosition)
        {
            var result = Parser.Parse(script);

            Assert.AreEqual(false, result.HasErrors, "Don't expect any parsing errors from this");

            var component = result.Components.FirstOrDefault(c => c.Name.Equals(componentToTest));

            Assert.IsNotNull(component, $"Expect there to be a component by the name of '{componentToTest}'");

            Assert.AreEqual(startIndex, component.ParsedToken.StartIndex);
            Assert.AreEqual(endIndex, component.ParsedToken.EndIndex);
            Assert.AreEqual(lineNumber, component.ParsedToken.LineNumber);
            Assert.AreEqual(columnPosition, component.ParsedToken.ColumnIndex);
        }

        [TestCase("+ TestClass : class { tags { \"test1\" \"test2\"} }", "TestClass", "test1,test2")]
        [TestCase("+ TestClass : class { purpose: \"somepurpose\" attributes { + Attribute1 : string } tags { \"test1\" \"test2\"} }", "TestClass", "test1,test2")]
        public void Component_Parse_Correct_Tags(string script, string componentToTest, string expectedCommaSeparatedTags)
        {
            var result = Parser.Parse(script);
            
            var expectedTags = expectedCommaSeparatedTags.Split(",").ToList();

            var component = result.Components.FirstOrDefault(c => c.Name.Equals(componentToTest));

            Assert.IsNotNull(component, $"Expect there to be a component by the name of '{componentToTest}'");

            Assert.AreEqual(expectedTags, component.Tags);
        }
    }
}
