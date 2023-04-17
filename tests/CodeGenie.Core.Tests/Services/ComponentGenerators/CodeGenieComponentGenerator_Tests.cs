using CodeGenie.Core.Models.Generation.Contexts;
using CodeGenie.Core.Services.Generators.ComponentGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Core.Tests.Services.ComponentGenerators
{
    [TestFixture]
    public class CodeGenieComponentGenerator_Tests : Generator_TestBase<CodeGenieComponentGenerator>
    {
        [SetUp]
        public void SetUp()
        {
            SetUpGeneratorAndParser();
        }

        [TestCase("T", "+T:class", "+ T : class")]
        [TestCase("TestClass", "+TestClass:class", "+ TestClass : class")]
        [TestCase("ITest", "+ITest:interface", "+ ITest : interface")]
        [TestCase("T", "+T:class{purpose:\"test\"}", "+ T : class { purpose : \"test\" }")]
        [TestCase("T", "+T:class{purpose:\"test2\"}", "+ T : class { purpose : \"test2\" }")]
        public void ComponentDefinition_Creates_Accurately(string targetComponent, string inputScript, string expectedOutputScript)
        {
            var context = new GenerationContext();

            var result = ParseAndAssertNoErrors(inputScript);

            var component = result.Components.FirstOrDefault(c => c.Name == targetComponent);

            Assert.That(component, Is.Not.Null);

            TypedGenerator.AppendComponentDefinition(context, component);

            var generated = context.ContentBuilder.ToString();

            Assert.That(generated, Is.EqualTo(expectedOutputScript));
        }
    }
}
