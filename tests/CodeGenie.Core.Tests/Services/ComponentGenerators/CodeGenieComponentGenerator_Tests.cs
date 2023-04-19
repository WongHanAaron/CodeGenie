﻿using CodeGenie.Core.Models.Generation.Contexts;
using CodeGenie.Core.Services.Generators.ComponentGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Core.Tests.Services.ComponentGenerators
{
    [TestFixture]
    public class CodeGenieComponentGenerator_Tests : 
                    Generator_TestBase<CodeGenieComponentGenerator>
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
        [TestCase("T", "+T:class{attributes{+A:string}}", "+ T : class\r\n{\r\n\tattributes\r\n\t{\r\n\t\t+ A : string\r\n\t}\r\n}")]
        [TestCase("T", "+T:class{attributes{+A:string +B:type}}", "+ T : class\r\n{\r\n\tattributes\r\n\t{\r\n\t\t+ A : string\r\n\t\t+ B : type\r\n\t}\r\n}")]
        public void ComponentDefinition_Creates_Accurately(string targetComponent, string inputScript, string expectedOutputScript)
        {
            var context = new GenerationContext();

            var result = ParseAndAssertNoErrors(inputScript);

            var component = GetComponentAndAssertNotNull(result, targetComponent);

            TypedGenerator.AppendComponentDefinition(context, component);

            var generated = context.ContentBuilder.ToString();

            Assert.That(generated, Is.EqualTo(expectedOutputScript));
        }

        [TestCase("T", "A", "+T:class{attributes{+A:string}}", "+ A : string")]
        [TestCase("T", "B", "+T:class{attributes{+A:string +B:int}}", "+ B : int")]
        public void AttributeDefinition_Creates_Accurately(string targetComponent, string targetAttribute, string inputScript, string expectedAttributeScript)
        {
            var context = new GenerationContext();

            var result = ParseAndAssertNoErrors(inputScript);

            var attribute = GetAttributeAndAssertNotNull(result, targetComponent, targetAttribute);

            TypedGenerator.AppendAttribute(context, attribute);

            var generated = context.ContentBuilder.ToString();

            Assert.That(generated, Is.EqualTo(expectedAttributeScript));
        }
    }
}
