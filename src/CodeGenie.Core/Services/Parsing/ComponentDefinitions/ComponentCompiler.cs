using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using CodeGenie.Core.Services.Parsing.ComponentDefinitions.DefinitionParsers;
using CodeGenie.Core.Services.Parsing.ComponentDefinitions.SemanticValidators;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions
{
    /// <summary>
    /// The software component responsible for compiling and validating the 
    /// components
    /// </summary>
    public interface IComponentCompiler
    {
        ParsingResult Compile(string script);
    }

    public class ComponentCompiler : IComponentCompiler
    {
        protected readonly IComponentDefinitionParser Parser;
        protected readonly ISemanticValidator Validator;

        public ComponentCompiler(IComponentDefinitionParser parser, 
                                 ISemanticValidator validator)
        {
            Parser = parser;
            Validator = validator;
        }

        public ParsingResult Compile(string script)
        {
            var parsingResults = Parser.Parse(script);
            var validationResult = Validator.Validate(parsingResults);
            return validationResult;
        }
    }
}
