using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions.SemanticValidation.Validations
{
    public interface IComponentValidation
    {
        IEnumerable<ScriptError> Validate(IEnumerable<ParsedComponentDefinition> results);
    }
}
