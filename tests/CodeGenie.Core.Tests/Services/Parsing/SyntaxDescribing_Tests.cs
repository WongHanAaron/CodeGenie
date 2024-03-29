﻿using CodeGenie.Core.Models.ComponentDefinitions.Syntax;
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
    public class SyntaxDescribing_Tests : CodeGenieTestBase
    {
        protected ISyntaxDescriber Describer;

        [SetUp]
        public void SetUp()
        {
            ServiceProvider = BuildServiceProvider();
            Describer = ServiceProvider?.GetService<ISyntaxDescriber>();

            Assert.IsNotNull(Describer, "The syntax describer is null");
        }
        
        // Component Parsing
        [TestCase(true, false, SyntaxDescriptor.BeforeStartComponentDefinition, 1, 0, "")]
        [TestCase(true, false, SyntaxDescriptor.BeforeStartComponentDefinition, 3, 0, "\n\n")]
        [TestCase(false, false, SyntaxDescriptor.BeforeComponentNameDefinition, 1, 0, "+TestClass:class")]
        [TestCase(true, true, SyntaxDescriptor.BeforeComponentNameDefinition, 3, 0, "\n\n+\n")]
        [TestCase(false, false, SyntaxDescriptor.BeforeComponentDivider, 1, 1, "+TestClass:class")]
        [TestCase(false, false, SyntaxDescriptor.BeforeComponentDivider, 2, 8, "\n+TestClass:class\n{\npurpose:\"\"\n}\n")]
        [TestCase(false, false, SyntaxDescriptor.BeforeComponentTypeDefinition, 1, 10, "+TestClass:class")]
        [TestCase(false, false, SyntaxDescriptor.BeforeComponentTypeDefinition, 2, 10, "\n+TestClass:class\n{\npurpose:\"\"\n}\n")]
        [TestCase(false, false, SyntaxDescriptor.BeforeComponentDetails, 1, 16, "+TestClass:class")]
        [TestCase(false, false, SyntaxDescriptor.BeforeComponentDetails, 3, 16, "\n\n+TestClass:class")]
        [TestCase(true, true, SyntaxDescriptor.WithinComponentDetails, 1, 9, "+T:class{}")]
        [TestCase(true, true, SyntaxDescriptor.WithinComponentDetails, 1, 8, "+T:class{}")]
        [TestCase(true, true, SyntaxDescriptor.WithinComponentDetails, 1, 10, "+T:class{relati}")]
        [TestCase(false, false, SyntaxDescriptor.WithinComponentDetails, 3, 13, "+TestClass:class\n{\n\tpurpose : \"\" \n}")]
        [TestCase(true, true, SyntaxDescriptor.BeforePurposeDefinitionDivider, 1, 15, "+T:class{purpose}")]
        [TestCase(true, true, SyntaxDescriptor.BeforeRelationshipsDetails, 1, 21, "+T:class{relationships}")]
        [TestCase(true, true, SyntaxDescriptor.WithinRelationshipsDetails, 1, 23, "+T:class{relationships{a}}")]
        [TestCase(true, true, SyntaxDescriptor.BeforeRelatedComponentNameDefinition, 1, 30, "+T:class{relationships{composes}}")]
        [TestCase(true, true, SyntaxDescriptor.BeforeRelatedComponentNameDefinition, 1, 29, "+T:class{relationships{depends}}")]
        [TestCase(true, true, SyntaxDescriptor.BeforeRelatedComponentNameDefinition, 1, 28, "+T:class{relationships{composes}}")]
        [TestCase(true, true, SyntaxDescriptor.BeforeRelatedComponentNameDefinition, 1, 32, "+T:class{relationships{aggregates}}")]
        [TestCase(true, true, SyntaxDescriptor.BeforeRelatedComponentNameDefinition, 1, 30, "+T:class{relationships{realizes}}")]
        [TestCase(true, true, SyntaxDescriptor.BeforeRelatedComponentNameDefinition, 1, 33, "+T:class{relationships{specializes}}")]
        [TestCase(true, true, SyntaxDescriptor.BeforeMethodsDetails, 1, 15, "+T:class{methods{}}")]
        [TestCase(true, true, SyntaxDescriptor.BeforeMethodsDetails, 1, 15, "+T:class{methods}")]
        [TestCase(true, true, SyntaxDescriptor.WithinComponentDetails, 3, 0, "+T:class\n{\n\t\n}")]
        [TestCase(false,false, SyntaxDescriptor.BeforeStartComponentDefinition, 5, 0, "+Test:class\n{\n\tpurpose : \"\"\n}\n ")]
        [TestCase(false, false, SyntaxDescriptor.WithinComponentDetails, 4, 1, "+Test:class\n{\n\tpurpose : \"\"\n\t\n}")]
        [TestCase(true, true, SyntaxDescriptor.BeforeAttributesDetails, 1, 18, "+T:class{attributes}")]
        [TestCase(true, true, SyntaxDescriptor.BeforeStartAttributeDefinition, 5, 2, "+ T : class \n{\n\tattributes\n\t{\n\t\t\n\t}\n}")]
        [TestCase(true, true, SyntaxDescriptor.BeforeAttributeNameDefinition, 5, 3, "+ T : class \n{\n\tattributes\n\t{\n\t\t+\n\t}\n}")]
        [TestCase(true, true, SyntaxDescriptor.BeforeAttributeDivider, 5, 3, "+ T : class \n{\n\tattributes\n\t{\n\t\t+A\n\t}\n}")]
        [TestCase(true, true, SyntaxDescriptor.BeforeAttributeTypeDefinition, 5, 4, "+ T : class \n{\n\tattributes\n\t{\n\t\t+A:\n\t}\n}")]
        [TestCase(true, true, SyntaxDescriptor.BeforeComponentNameDefinition, 2, 0, "+T:class\n+")]
        [TestCase(true, true, SyntaxDescriptor.BeforeMethodsDetails, 3, 7, "+ T : class \n{\n\tmethods\n}")]
        [TestCase(true, true, SyntaxDescriptor.BeforeStartMethodDefinition, 5, 1, "+ T : class \n{\n\tmethods\n\t{\n\t\t\n\t}\n}")]
        [TestCase(true, true, SyntaxDescriptor.BeforeMethodNameDefinition, 5, 2, "+ T : class \n{\n\tmethods\n\t{\n\t\t+\n\t}\n}")]
        [TestCase(true, true, SyntaxDescriptor.BeforeMethodOpenParenthesis, 5, 3, "+ T : class \n{\n\tmethods\n\t{\n\t\t+M\n\t}\n}")]
        [TestCase(true, true, SyntaxDescriptor.BeforeParameterNameDefinition, 5, 4, "+ T : class \n{\n\tmethods\n\t{\n\t\t+M(\n\t}\n}")]
        [TestCase(true, true, SyntaxDescriptor.BeforeParameterDivider, 5, 5, "+ T : class \n{\n\tmethods\n\t{\n\t\t+M(P\n\t}\n}")]
        [TestCase(true, true, SyntaxDescriptor.BeforeParameterTypeDefinition, 5, 6, "+ T : class \n{\n\tmethods\n\t{\n\t\t+M(P:\n\t}\n}")]
        [TestCase(true, false, SyntaxDescriptor.AfterParameterTypeDefinition, 5, 7, "+ T : class \n{\n\tmethods\n\t{\n\t\t+M(P:T\n\t}\n}")]
        [TestCase(true, true, SyntaxDescriptor.BeforeParameterNameDefinition, 5, 8, "+ T : class \n{\n\tmethods\n\t{\n\t\t+M(P:T,\n\t}\n}")]
        [TestCase(true, true, SyntaxDescriptor.WithinComponentDetails, 4, 0, "+T:class{\n\n\n\n}")]
        public void Get_SyntaxState_At_Line_Column(bool expectErrors, bool expectErrorsOnRule, SyntaxDescriptor expectedState, int lineNumber, int columnNumber, string script)
        {
            var description = Describer.GetSyntaxDescription(script, lineNumber, columnNumber);

            Assert.AreEqual(expectedState, description.SyntaxDescriptorAtCaret, $"Expected {expectedState} state but {description.SyntaxDescriptorAtCaret} was received instead for script '{script}'");

            var errorDescription = expectErrors ? "has" : "does not have";
            Assert.That(description.HasSyntaxError, Is.EqualTo(expectErrors), $"Expected that script {errorDescription} errors. For script '{script}'");
        
            errorDescription = expectErrorsOnRule ? "has" : "does not have";
            Assert.That(description.HasErrorOnSyntaxRule, Is.EqualTo(expectErrorsOnRule), $"Expected that script {errorDescription} errors on rule. For script '{script}'");
        }
        // Debug Tests
        // [TestCase(SyntaxDescriptor.BeforePurposeDefinitionDivider, 3, 12, "+TestClass:class\n{\n\tpurpose:\"\"\n}")] // Caused stack overflow
        // [TestCase(SyntaxDescriptor.Unknown, 4, 0, "+TestClass: class\n{\n\tpurpose : \"\"\n\t\n}")]

    }
}
