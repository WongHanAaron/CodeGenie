using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using System;
using System.Collections.Generic;
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
            return new List<ParsedComponentDefinition>();
        }
    }
}
