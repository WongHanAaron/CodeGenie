using Antlr4.Runtime;
using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions.DefinitionParsers
{
    /// <summary>
    /// The implementation to parse a string script into a list of parsed out component definitions
    /// </summary>
    public class DefinitionParser : IDefinitionParser
    {
        public List<ParsedComponentDefinition> Parse(string classScript)
        {
            var definitionContext = GetContext(classScript);
            var collector = new ComponentDefinitionCollector();
            collector.Visit(definitionContext);
            return collector.ComponentDefinitions.ToList();
        }

        // Returns the component definition context that can be used to visit the syntax tree
        // Reference: https://tomassetti.me/getting-started-with-antlr-in-csharp/
        public CodeGenieParser.ComponentDefinitionContext GetContext(string classScript)
        {
            var inputStream = new AntlrInputStream(classScript);
            var speakLexer = new CodeGenieLexer(inputStream);
            var commonTokenStream = new CommonTokenStream(speakLexer);
            var speakParser = new CodeGenieParser(commonTokenStream);
            var context = speakParser.componentDefinition();
            return context;
        }
    }
}
