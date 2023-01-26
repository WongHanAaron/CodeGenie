using CodeGenie.Core.Models.ComponentDefinitions.State;
using CodeGenie.Core.Services.Parsing.ComponentDefinitions.SyntaxDescribing;
using CodeGenie.Core.Tests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Core.Tests.Services.Parsing
{
    [TestFixture]
    public class SyntaxDescribing_Tests : ComponentDefinitionTestBase
    {
        protected ISyntaxDescriber Describer;

        [SetUp]
        public void SetUp()
        {
            ServiceProvider = BuildServiceProvider();
            Describer = ServiceProvider.GetService<ISyntaxDescriber>();

            Assert.IsNotNull(Describer, "The syntax describer is null");
        }
        
        // Component Parsing
        [TestCase(SyntaxDescriptor.BeforeStartComponentDefinition, 1, 0, "")]
        [TestCase(SyntaxDescriptor.BeforeStartComponentDefinition, 3, 0, "\n\n")]
        [TestCase(SyntaxDescriptor.BeforeComponentNameDefinition, 1, 0, "+TestClass:class")]
        [TestCase(SyntaxDescriptor.BeforeComponentNameDefinition, 3, 0, "\n\n+\n")]
        [TestCase(SyntaxDescriptor.BeforeComponentDivider, 1, 1, "+TestClass:class")]
        [TestCase(SyntaxDescriptor.BeforeComponentDivider, 2, 8, "\n+TestClass:class\n{\npurpose:\"\"\n}\n")]
        [TestCase(SyntaxDescriptor.BeforeComponentTypeDefinition, 1, 10, "+TestClass:class")]
        [TestCase(SyntaxDescriptor.BeforeComponentTypeDefinition, 2, 10, "\n+TestClass:class\n{\npurpose:\"\"\n}\n")]
        [TestCase(SyntaxDescriptor.BeforeComponentDetails, 1, 16, "+TestClass:class")]
        [TestCase(SyntaxDescriptor.BeforeComponentDetails, 3, 16, "\n\n+TestClass:class")]
        [TestCase(SyntaxDescriptor.WithinComponentDetails, 1, 9, "+T:class{}")]
        [TestCase(SyntaxDescriptor.WithinComponentDetails, 1, 8, "+T:class{}")]
        [TestCase(SyntaxDescriptor.WithinComponentDetails, 1, 10, "+T:class{relati}")]
        [TestCase(SyntaxDescriptor.WithinComponentDetails, 3, 13, "+TestClass:class\n{\n\tpurpose : \"\" \n}")]
        [TestCase(SyntaxDescriptor.BeforePurposeDefinitionDivider, 1, 15, "+T:class{purpose}")]
        [TestCase(SyntaxDescriptor.BeforeAttributesDivider, 1, 18, "+T:class{attributes}")]
        [TestCase(SyntaxDescriptor.BeforeAttributesDetails, 1, 19, "+T:class{attributes:}")]
        [TestCase(SyntaxDescriptor.BeforeRelationshipsDetails, 1, 21, "+T:class{relationships}")]
        [TestCase(SyntaxDescriptor.WithinRelationshipsDetails, 1, 23, "+T:class{relationships{a}}")]
        [TestCase(SyntaxDescriptor.BeforeRelatedComponentNameDefinition, 1, 30, "+T:class{relationships{composes}}")]
        [TestCase(SyntaxDescriptor.BeforeRelatedComponentNameDefinition, 1, 29, "+T:class{relationships{depends}}")]
        [TestCase(SyntaxDescriptor.BeforeRelatedComponentNameDefinition, 1, 28, "+T:class{relationships{composes}}")]
        [TestCase(SyntaxDescriptor.BeforeRelatedComponentNameDefinition, 1, 32, "+T:class{relationships{aggregates}}")]
        [TestCase(SyntaxDescriptor.BeforeRelatedComponentNameDefinition, 1, 30, "+T:class{relationships{realizes}}")]
        [TestCase(SyntaxDescriptor.BeforeRelatedComponentNameDefinition, 1, 33, "+T:class{relationships{specializes}}")]

        public void Get_SyntaxState_At_Line_Column(SyntaxDescriptor expectedState, int lineNumber, int columnNumber, string script)
        {
            var syntaxState = Describer.GetSyntaxDescription(script, lineNumber, columnNumber);

            Assert.AreEqual(expectedState, syntaxState, $"Expected {expectedState} state but {syntaxState} was received instead for script '{script}'");
        }
        // Debug Tests
        // [TestCase(SyntaxDescriptor.BeforePurposeDefinitionDivider, 3, 12, "+TestClass:class\n{\n\tpurpose:\"\"\n}")] // Caused stack overflow
        // [TestCase(SyntaxDescriptor.Unknown, 4, 0, "+TestClass: class\n{\n\tpurpose : \"\"\n\t\n}")]

    }
}
