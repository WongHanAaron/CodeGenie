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

        [TestCase(SyntaxDescriptor.BeforeComponentNameDefinition, 1, 0, "+TestClass:class")]
        [TestCase(SyntaxDescriptor.BeforeComponentDivider, 1, 1, "+TestClass:class")]
        [TestCase(SyntaxDescriptor.BeforeComponentTypeDefinition, 1, 10, "+TestClass:class")]
        [TestCase(SyntaxDescriptor.BeforeComponentDetails, 1, 16, "+TestClass:class ")]
        //[TestCase(SyntaxState.BeforeStartComponentDefinition, 1, 2, "+TestClass:class")]
        public void Get_SyntaxState_At_Line_Column(SyntaxDescriptor expectedState, int lineNumber, int columnNumber, string script)
        {
            var syntaxState = Describer.GetSyntaxState(script, lineNumber, columnNumber);

            Assert.AreEqual(expectedState, syntaxState, $"Expected {expectedState} state but {syntaxState} was received instead for script '{script}'");
        }
    }
}
