using Antlr4.Runtime;
using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions.DefinitionParsing
{
    public class CodeGenieErrorCollector<T> : IAntlrErrorListener<T>
    {
        public List<ScriptError> Errors { get; set; } = new List<ScriptError>();
        public void SyntaxError(TextWriter output, IRecognizer recognizer, T offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            ScriptError error = null;
            if (offendingSymbol is IToken offendingToken)
            {
                error = ParsedToken.Create<ScriptError>(offendingToken);
            }
            else
            {
                error = new ScriptError();
            }

            error.Exception = e;

            Errors.Add(error);
        }
    }
}
