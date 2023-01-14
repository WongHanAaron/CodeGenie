using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions.DefinitionParsing
{
    public interface IComponentDefinitionParser
    {
        ParsingResult Parse(string classScript);
    }
}
