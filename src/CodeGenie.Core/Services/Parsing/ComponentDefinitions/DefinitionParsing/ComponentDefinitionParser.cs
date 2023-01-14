using Antlr4.Runtime;
using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using CodeGenie.Core.Services.Parsing.ComponentDefinitions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions.DefinitionParsing
{
    /// <summary>
    /// The implementation to parse a string script into a list of parsed out component definitions
    /// </summary>
    public class ComponentDefinitionParser : IComponentDefinitionParser
    {
        protected readonly IComponentDefinitionContextParser ComponentDefinitionContextParser;

        public ComponentDefinitionParser(IComponentDefinitionContextParser componentDefinitionContextParser)
        {
            ComponentDefinitionContextParser = componentDefinitionContextParser;
        }

        public ParsingResult Parse(string classScript)
        {
            var contextParseResult = ComponentDefinitionContextParser.ParseContext(classScript);

            if (contextParseResult.HasErrors) return contextParseResult;

            var collector = new ComponentDefinitionCollector();

            collector.Visit(contextParseResult.Context);
            
            var components = collector.ComponentDefinitions.Where(c => c!= null).ToList();

            var errors = collector.Errors.ToList();

            return new ParsingResult(components, contextParseResult.Context, errors);
        }
    }
}
