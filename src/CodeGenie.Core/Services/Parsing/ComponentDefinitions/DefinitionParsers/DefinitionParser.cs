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
    public class DefinitionParser : IComponentDefinitionParser
    {
        public ParsingResult Parse(string classScript)
        {
            var contextParseResult = GetContext(classScript);

            if (contextParseResult.HasErrors) return contextParseResult;

            var collector = new ComponentDefinitionCollector();

            collector.Visit(contextParseResult.Context);
            
            var components = collector.ComponentDefinitions.ToList();

            return new ParsingResult(components, contextParseResult.Context);
        }

        // Returns the component definition context that can be used to visit the syntax tree
        // Reference: https://tomassetti.me/getting-started-with-antlr-in-csharp/
        public ParsingResult GetContext(string classScript)
        {
            var errors = new List<ScriptError>();

            var lexingErrorCollector = new CodeGenieErrorCollector<int>();

            var parsingErrorCollector = new CodeGenieErrorCollector<IToken>();

            var inputStream = new AntlrInputStream(classScript);

            var speakLexer = new CodeGenieLexer(inputStream);
            
            speakLexer.AddErrorListener(lexingErrorCollector);
            
            var commonTokenStream = new CommonTokenStream(speakLexer);
            
            var speakParser = new CodeGenieParser(commonTokenStream);
            
            speakParser.AddErrorListener(parsingErrorCollector);

            var context = speakParser.componentDefinition();

            if (lexingErrorCollector.Errors.Any())
            {
                errors.AddRange(lexingErrorCollector.Errors);
            }
            
            if (parsingErrorCollector.Errors.Any())
            {
                errors.AddRange(parsingErrorCollector.Errors);
            }

            if (errors.Any())
            {
                return new ParsingResult(errors);
            }
            else
            {
                return new ParsingResult(context);
            }
        }
    }
}
