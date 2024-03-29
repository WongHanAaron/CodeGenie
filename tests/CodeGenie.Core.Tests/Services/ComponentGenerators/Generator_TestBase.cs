﻿using CodeGenie.Core.Models.ComponentDefinitions.Definitions;
using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using CodeGenie.Core.Services.Generators.ComponentGenerators;
using CodeGenie.Core.Services.Parsing.ComponentDefinitions.DefinitionParsing;
using CodeGenie.Core.Tests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Core.Tests.Services.ComponentGenerators
{
    public abstract class Generator_TestBase<TGenerator> : CodeGenieTestBase where TGenerator : class, IComponentGenerator
    {
        public IComponentGenerator Generator { get; set; }

        public TGenerator TypedGenerator { get; set; }

        public IComponentDefinitionParser Parser { get; set; }

        public IComponentGeneratorFactory GeneratorFactory { get; set; }

        /// <summary> Sets up the generator and parser. To be called with the SetUp </summary>
        public void SetUpGeneratorAndParser()
        {
            ServiceProvider = BuildServiceProvider();

            GeneratorFactory = ServiceProvider?.GetService<IComponentGeneratorFactory>();

            TypedGenerator = GeneratorFactory?.Create<TGenerator>() as TGenerator;

            Generator = TypedGenerator;

            Parser = ServiceProvider?.GetService<IComponentDefinitionParser>();

            Assert.IsNotNull(Generator, $"The {typeof(TGenerator).Name} could not be injected");

            Assert.IsNotNull(Parser, "The parser could not be injected");
        }

        public ParsingResult ParseAndAssertNoErrors(string script)
        {
            var result = Parser.Parse(script);

            Assert.That(result.HasErrors, Is.False, $"Do not expect any errors in script '{script}'");

            return result;
        }

        public ComponentDefinition GetComponentAndAssertNotNull(ParsingResult parsedResult, string targetComponentName)
        {
            var component = parsedResult.Components.FirstOrDefault(c => c.Name == targetComponentName);

            Assert.That(component, Is.Not.Null, $"Expect there to be a component named '{targetComponentName}' in result");

            return component;
        }

        public AttributeDefinition GetAttributeAndAssertNotNull(ParsingResult parsedResult, string targetComponentName, string targetAttributeName)
        {
            var component = GetComponentAndAssertNotNull(parsedResult, targetComponentName);

            var attribute = component.Attributes.FirstOrDefault(a => a.Name == targetAttributeName);

            Assert.That(attribute, Is.Not.Null, $"Expect there to be an attribute named '{targetAttributeName}' in result");

            return attribute;
        }

        public MethodDefinition GetMethodAndAssertNotNull(ParsingResult parsedResult, string targetComponentName, string targetMethodName)
        {
            var component = GetComponentAndAssertNotNull(parsedResult, targetComponentName);

            var method = component.MethodDefinitions.FirstOrDefault(a => a.Name == targetMethodName);

            Assert.That(method, Is.Not.Null, $"Expect there to be an method named '{targetMethodName}' in result");

            return method;
        }

        public RelationshipDefinition GetRelationshipAndAssertNotNull(ParsingResult parsedResult, string targetComponentName, string targetMethodName)
        {
            var component = GetComponentAndAssertNotNull(parsedResult, targetComponentName);

            var relationship = component.RelationshipDefinitions.FirstOrDefault(a => a.RelatedComponentName == targetMethodName);

            Assert.That(relationship, Is.Not.Null, $"Expect there to be a relation related to component named '{targetMethodName}' in result");

            return relationship;
        }
    }
}
