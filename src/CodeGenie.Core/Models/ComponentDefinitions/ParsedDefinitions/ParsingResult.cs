using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions
{
    public class ParsingResult
    {
        public ParsingResult(List<ScriptError> errors) : this(null, null, errors) { }

        public ParsingResult(CodeGenieParser.ComponentDefinitionContext context) : this (null, context, null) { }

        public ParsingResult(List<ParsedComponentDefinition> components, CodeGenieParser.ComponentDefinitionContext context) : this (components, context, null){ }

        public ParsingResult(List<ParsedComponentDefinition> components, CodeGenieParser.ComponentDefinitionContext context, List<ScriptError> errors)
        {
            if (components != null) Components = components;
            if (context != null) Context = context;
            if (errors != null) Errors = errors;
        }

        public CodeGenieParser.ComponentDefinitionContext Context { get; protected set; }
        public List<ParsedComponentDefinition> Components { get; protected set; } = new List<ParsedComponentDefinition>();
        public List<ScriptError> Errors { get; protected set; } = new List<ScriptError>();
        public bool HasErrors => Errors.Any();
    }
}
