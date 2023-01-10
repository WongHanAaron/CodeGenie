using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions
{
    /// <summary> The container class for containing the parsed token and location of that token </summary>
    public class ParsedToken
    {
        public IToken Token { get; set; }
        public int LineNumber { get; set; }
        public int CharacterPositionInLine { get; set; }
    }
}
