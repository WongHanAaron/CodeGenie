
using CodeGenie.Core.Models.ComponentDefinitions.Definitions;
using CodeGenie.Core.Services.Parsing.ComponentDefinitions.DefinitionParsing;
using CodeGenie.Core.Tests.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace CodeGenie.Core.Tests.Services.Parsing
{
    [TestFixture]
    public class ComponentDefinition_Tests : CodeGenieTestBase
    {
        IComponentDefinitionParser? Parser;

        [SetUp]
        public void SetUp()
        {
            ServiceProvider = BuildServiceProvider();
            Parser = ServiceProvider?.GetService<IComponentDefinitionParser>();
            Assert.IsNotNull(Parser, "The parser could not be injected");
        }

        [TestCase("+ TestClass : class", 1)]
        [TestCase("+ TestClass : class - TestClass2 : class + TestClass3 : interface", 3)]
        public void Component_Parse_Matches_Count(string script, int expectedCount)
        {
            var components = Parser?.Parse(script).Components;
            Assert.That(components?.Count(), Is.EqualTo(expectedCount), "The returned count of components do not match the expected count");
        }

        [TestCase("+ TestClass : class", "TestClass", "", false)]
        [TestCase("+ TestClass2 : interface", "TestClass2", "", true)]
        [TestCase("+ TestClass3 : interface { purpose : \"Something\"}", "TestClass3", "Something", true)]
        public void Component_Parse_Matches_Name(string script, string expectedName, string purpose, bool expectIsInterface)
        {
            var components = Parser?.Parse(script).Components;
            var first = components?.FirstOrDefault();
            Assert.That(first?.Name, Is.EqualTo(expectedName));
            Assert.That(first?.IsInterface, Is.EqualTo(expectIsInterface));
        }

        [TestCase(1, "+ TestClass : class { attributes { + Attribute : string } }")]
        [TestCase(2, "+ TestClass : class { attributes { + Attribute : string - Attribute2 : string } }")]
        [TestCase(3, "+ TestClass : class { attributes { + Attribute : string - Attribute2 : string # Attribute3: otherType } }")]
        public void Component_Parse_Attribute_Count_Matches(int expectedAttributeCount, string script)
        {
            var components = Parser?.Parse(script).Components;
            Assert.That(components?.FirstOrDefault()?.Attributes.Count(), Is.EqualTo(expectedAttributeCount));
        }

        [TestCase("Attribute", "string", Scope.Public, "+ TestClass : class { attributes { + Attribute : string} }")]
        [TestCase("Attribute", "string", Scope.Protected, "+ TestClass : class { attributes { # Attribute : string} }")]
        [TestCase("Attribute", "string", Scope.Private, "+ TestClass : class { attributes { - Attribute : string} }")]
        public void Component_Parse_Attribute_Name_Type_And_Scope_Matches(string expectedName, string expectedType, Scope expectedScope,  string script)
        {
            var components = Parser?.Parse(script).Components;
            var first = components?.FirstOrDefault();
            var attribute = first?.Attributes.FirstOrDefault();
            Assert.That(attribute?.Name, Is.EqualTo(expectedName));
            Assert.That(attribute?.Type, Is.EqualTo(expectedType));
            Assert.That(attribute?.Scope, Is.EqualTo(expectedScope));
        }

        [TestCase("+ Testclass : something", true)]
        [TestCase("TestClass : class",  true)]
        [TestCase("+ : class", true)]
        [TestCase("+T:class+T:class", false)]
        public void Component_Parsing_Error(string script, bool expectError)
        {
            var result = Parser.Parse(script);

            Assert.That(result.HasErrors, Is.EqualTo(expectError));
        }

        [TestCase("+ TestClass : class", "TestClass", 0, 18, 1, 0)]
        [TestCase("+ TestClass : class\n+ TestClass2 : class", "TestClass2", 20, 39, 2, 0)]
        public void Component_Parse_Correct_Positions(string script, string componentToTest, int startIndex, int endIndex, int lineNumber, int columnPosition)
        {
            var result = Parser?.Parse(script);

            Assert.That(result?.HasErrors, Is.EqualTo(false), "Don't expect any parsing errors from this");

            var component = result?.Components.FirstOrDefault(c => c.Name.Equals(componentToTest));

            Assert.IsNotNull(component, $"Expect there to be a component by the name of '{componentToTest}'");

            Assert.That(component.ParsedToken.StartIndex, Is.EqualTo(startIndex));
            Assert.That(component.ParsedToken.EndIndex, Is.EqualTo(endIndex));
            Assert.That(component.ParsedToken.LineNumber, Is.EqualTo(lineNumber));
            Assert.That(component.ParsedToken.ColumnIndex, Is.EqualTo(columnPosition));
        }

        [TestCase("+ TestClass : class { tags { \"test1\" \"test2\"} }", "TestClass", "test1,test2")]
        [TestCase("+ TestClass : class { purpose: \"somepurpose\" attributes { + Attribute1 : string } tags { \"test1\" \"test2\"} }", "TestClass", "test1,test2")]
        [TestCase("+ TestClass : class { purpose: \"somepurpose\" attributes { + Attribute1 : string } tags { \"test1\" \"test2\"} } + TestClass2 : class { tags { \"test3\" } }", "TestClass2", "test3")]
        [TestCase("+ TestClass : class { purpose: \"purpose\" tags { \"tag1\" } } + TestClass2 : class { tags { \"test3\" } }", "TestClass", "tag1")]
        [TestCase("+ TestClass : class { purpose: \"purpose\" tags { \"tag2\" } } + TestClass2 : class { tags { \"test3\" } }", "TestClass", "tag2")]
        [TestCase("+ TestClass : class \n\n\n\n+ TestClass2 : class { tags { \"tag3\" } }", "TestClass2", "tag3")]
        public void Component_Parse_Correct_Tags(string script, string componentToTest, string expectedCommaSeparatedTags)
        {
            var result = Parser?.Parse(script);
            
            var expectedTags = expectedCommaSeparatedTags.Split(",").ToList();

            var component = result?.Components.FirstOrDefault(c => c.Name.Equals(componentToTest));

            Assert.IsNotNull(component, $"Expect there to be a component by the name of '{componentToTest}'");

            Assert.That(component.Tags, Is.EqualTo(expectedTags));
        }

        [TestCase("+TestClass:class{purpose:\"some purpose\"}", "TestClass", "some purpose", false)]
        [TestCase("+TestClass:class{purpose:\"some purpose\"}+TestClass2:class{purpose:\"some purpose as well\"}", "TestClass2", "some purpose as well", false)]
        [TestCase("+TestClass:class{purpose:\"some purpose\"purpose:\"some other purpose\"}", "TestClass", "some purpose", true)]
        public void Component_Parse_Correct_Purpose(string script, string componentToTest, string expectedPurpose, bool expectError)
        {
            var result = Parser.Parse(script);

            var component = result.Components.FirstOrDefault(c => c.Name.Equals(componentToTest));

            Assert.That(result.Errors.Any(), Is.EqualTo(expectError));

            Assert.IsNotNull(component, $"Expect there to be a component by the name of '{componentToTest}'");

            Assert.That(component.Purpose, Is.EqualTo(expectedPurpose));
        }

        [TestCase("+TestClass:class", Scope.Public)]
        [TestCase("-TestClass:class", Scope.Private)]
        [TestCase("#TestClass:class", Scope.Protected)]
        [TestCase("public TestClass:class", Scope.Public)]
        [TestCase("private TestClass:class", Scope.Private)]
        [TestCase("protected TestClass:class", Scope.Protected)]
        public void Component_Parse_Correct_Scope(string script, Scope expectedScope)
        {
            var result = Parser.Parse(script);

            var component = result.Components.FirstOrDefault();

            Assert.That(result.Errors.Any(), Is.EqualTo(false));

            Assert.That(component, Is.Not.Null, $"Expect there to be a component to test against");

            Assert.That(component.Scope, Is.EqualTo(expectedScope));
        }

        [TestCase("+T:class{methods{+M(P1:string,P2:integer):void}}", "M", Scope.Public, "void", "P1,P2", "string,integer")]
        [TestCase("+T:class{methods{+M(P1:string,P2:integer):void+M2(P3:something):string}}", "M2", Scope.Public, "string", "P3", "something")]
        [TestCase("+T:class{methods{+O(P1:string,P2:integer,P3:other):other}}", "O", Scope.Public, "other", "P1,P2,P3", "string,integer,other")]
        [TestCase("+T:class{methods{+M(\n\n\nP1:string,\n\n\nP2:integer)\n:\nvoid}}", "M", Scope.Public, "void", "P1,P2", "string,integer")]
        public void ParseCorrectMethod(string script, string methodToTest, Scope expectedScope, string expectedReturnTypeName, string commaSeparatedParameterNameList, string commaSeparatedParameterTypeList)
        {
            var result = Parser.Parse(script);

            var component = result.Components.FirstOrDefault();

            Assert.That(result.Errors.Any(), Is.EqualTo(false));

            Assert.That(component, Is.Not.Null, $"Expect there to be a component to test against");

            var matchingMethod = component.MethodDefinitions.FirstOrDefault(m => m.Name.Equals(methodToTest));

            Assert.That(matchingMethod, Is.Not.Null, $"Expected there to be a method by the name '{methodToTest}'");

            Assert.That(matchingMethod.Scope, Is.EqualTo(expectedScope), $"Expected the scope to be {expectedScope.ToString()}");

            Assert.That(matchingMethod.ReturnTypeName, Is.EqualTo(expectedReturnTypeName));

            Assert.That(matchingMethod.Parameters.Select(p => p.Name).ToList(), Is.EqualTo(commaSeparatedParameterNameList.Split(",").ToList()));
            Assert.That(matchingMethod.Parameters.Select(p => p.TypeName).ToList(), Is.EqualTo(commaSeparatedParameterTypeList.Split(",").ToList()));
        }

        [TestCase("+T:class{relationships{depends T}}", $"{nameof(RelationshipType.Dependency)}", "T")]
        [TestCase("+T:class{relationships{aggregates T}}", $"{nameof(RelationshipType.Aggregation)}", "T")]
        [TestCase("+T:class{relationships{composes T}}", $"{nameof(RelationshipType.Composition)}", "T")]
        [TestCase("+T:class{relationships{specializes T}}", $"{nameof(RelationshipType.Specialization)}", "T")]
        [TestCase("+T:class{relationships{realizes T}}", $"{nameof(RelationshipType.Realization)}", "T")]
        [TestCase("+T:class{relationships{realizes T depends T composes T}}", 
                    $"{nameof(RelationshipType.Realization)},{nameof(RelationshipType.Dependency)},{nameof(RelationshipType.Composition)}", "T,T,T")]
        [TestCase("+T:class{relationships{depends T depends T2 depends T3}}", 
                    $"{nameof(RelationshipType.Dependency)},{nameof(RelationshipType.Dependency)},{nameof(RelationshipType.Dependency)}", "T,T2,T3")]
        public void ParseCorrectRelationshipType(string script, string csvOfRelationshipTypes, string csvOfRelatedComponents)
        {
            var result = Parser.Parse(script);

            var component = result.Components.FirstOrDefault();

            Assert.That(result.Errors.Any(), Is.EqualTo(false));

            Assert.That(component, Is.Not.Null, $"Expect there to be a component to test against");

            var expectedRelationshipTypes = csvOfRelationshipTypes.Split(",");
            var expectedRelatedComponents = csvOfRelatedComponents.Split(",");

            for (int i = 0; i < expectedRelationshipTypes.Count(); i++)
            {
                var expectedRelationshipType = expectedRelationshipTypes[i];
                var expectedRelatedComponent = expectedRelatedComponents[i];

                var parsedRelationship = component.RelationshipDefinitions.ElementAtOrDefault(i);

                Assert.That(parsedRelationship, Is.Not.Null, $"Expected relationship at element '{i}' to be '{expectedRelationshipType}' and '{expectedRelatedComponent}' but was null");

                Assert.That(parsedRelationship.RelationshipType.ToString(), Is.EqualTo(expectedRelationshipType), $"Expected relationship at element '{i}' to be '{expectedRelationshipType}' but was '{parsedRelationship.RelationshipType.ToString()}'");
                Assert.That(parsedRelationship.RelatedComponentName, Is.EqualTo(expectedRelatedComponent), $"Expected relationship at element '{i}' to be related to '{expectedRelationshipType}' but was '{parsedRelationship.RelationshipType.ToString()}'");
            }
        }

        [TestCase("+T:class{relationships{depends T {purpose:\"something\" tags{\"tag1\"}}}}", $"{nameof(RelationshipType.Dependency)}", "something", "tag1", "1", "1")]
        [TestCase("+T:class{relationships{depends T {purpose:\"something1\" tags{\"tag1\" \"tag2\"}}}}", $"{nameof(RelationshipType.Dependency)}", "something1", "tag1,tag2", "1", "1")]
        [TestCase("+T:class{relationships{depends T {purpose:\"something2\" tags{\"tag3\"}}}}", $"{nameof(RelationshipType.Dependency)}", "something2", "tag3", "1", "1")]
        [TestCase("+T:class{relationships{depends T}}", $"{nameof(RelationshipType.Dependency)}", "", "", "1", "1")]
        public void ParseCorrectRelationshipDetails(string script, string targetedRelationshipType, string expectedPurpose, string csvOfTags, string sourceCardinality, string destinationCardinality)
        {
            var result = Parser.Parse(script);

            var component = result.Components.FirstOrDefault();

            Assert.That(result.Errors.Any(), Is.EqualTo(false));

            Assert.That(component, Is.Not.Null, $"Expect there to be a component to test against");

            var targetedRelationship = component.RelationshipDefinitions.FirstOrDefault(r => r.RelationshipType.ToString().Equals(targetedRelationshipType));

            Assert.That(targetedRelationship, Is.Not.Null, $"There does not exist a relationship of type '{targetedRelationshipType}'");

            Assert.That(targetedRelationship.Purpose, Is.EqualTo(expectedPurpose));

            var expectedTags = csvOfTags.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();

            Assert.That(targetedRelationship.Tags, Is.EqualTo(expectedTags));
        }
    }
}
