using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions
{
    public class ParsingError
    {
        public IToken Token { get; set; }
        public int LineNumber { get; set; }
        public int CharacterPositionInLine { get; set; }
        public RecognitionException Exception { get; set; }
    }
}
