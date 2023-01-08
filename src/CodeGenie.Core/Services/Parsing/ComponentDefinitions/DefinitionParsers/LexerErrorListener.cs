using Antlr4.Runtime;
using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions.DefinitionParsers
{
    public class CodeGenieErrorCollector<T> : IAntlrErrorListener<T>
    {
        public List<ParsingError> Errors { get; set; } = new List<ParsingError>();
        public void SyntaxError(TextWriter output, IRecognizer recognizer, T offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            IToken errorSymbol = null;
            if (offendingSymbol is IToken offendingToken)
            {
                errorSymbol = offendingToken;
            }

            Errors.Add(new ParsingError()
            {
                Token = errorSymbol,
                Exception = e,
                LineNumber = line,
                CharacterPositionInLine = charPositionInLine
            });
        }
    }
}
