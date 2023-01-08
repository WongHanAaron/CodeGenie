using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions
{
    public class ParsingResult
    {
        public ParsingResult(CodeGenieParser.ComponentDefinitionContext context) : this (null, context) { }

        public ParsingResult(List<ParsedComponentDefinition> components, CodeGenieParser.ComponentDefinitionContext context)
        {
            Components = components;
            Context = context;
        }

        public ParsingResult(List<ParsingError> errors)
        {
            Errors = errors;
        }

        public CodeGenieParser.ComponentDefinitionContext Context { get; protected set; }
        public List<ParsedComponentDefinition> Components { get; protected set; }
        public List<ParsingError> Errors { get; protected set; } = new List<ParsingError>();
        public bool HasErrors => Errors.Any();
    }
}
