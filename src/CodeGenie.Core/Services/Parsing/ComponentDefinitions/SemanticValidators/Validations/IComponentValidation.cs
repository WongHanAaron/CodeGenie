using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions.SemanticValidators.Validations
{
    public interface IComponentValidation
    {
        IEnumerable<ScriptError> Validate(IEnumerable<ParsedComponentDefinition> results);
    }
}
