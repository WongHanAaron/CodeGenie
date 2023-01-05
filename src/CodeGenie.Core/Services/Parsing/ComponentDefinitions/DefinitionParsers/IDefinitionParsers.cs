using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions.DefinitionParsers
{
    public interface IDefinitionParsers
    {
        List<ParsedComponentDefinition> Parse(string classScript);
    }
}
