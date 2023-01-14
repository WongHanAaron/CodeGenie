using Antlr4.Runtime;
using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using CodeGenie.Core.Services.Parsing.ComponentDefinitions.DefinitionParsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions.Shared
{
    /// <summary> Component to perform the context tree parsing for the ComponentDefinition ruleset </summary>
    public interface IComponentDefinitionContextParser
    {
        /// <summary> Parse the component definition context </summary>
        ParsingResult ParseContext(string script);
    }

    public class ComponentDefinitionContextParser : IComponentDefinitionContextParser
    {
        public ParsingResult ParseContext(string script)
            => GetContext(script);

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
                return new ParsingResult(context, errors);
            }
            else
            {
                return new ParsingResult(context);
            }
        }
    }
}
