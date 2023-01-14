using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using CodeGenie.Core.Models.Exceptions.ComponentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions.SemanticValidation.Validations
{
    public class DuplicateComponentValidation : IComponentValidation
    {
        public IEnumerable<ScriptError> Validate(IEnumerable<ParsedComponentDefinition> components)
        {
            var errors = new List<ScriptError>();

            var duplicateComponentNames = components.Where(c => !string.IsNullOrEmpty(c.Name))
                                                    .GroupBy(c => c.Name)
                                                    .Where(g => g.Count() > 1);
            
            foreach (var duplicate in duplicateComponentNames)
            {
                var duplicatedComponents = duplicate.Skip(1); // Skip the first instance that is not a duplicate
                errors.AddRange(CreateScriptErrorFromDuplicates(duplicatedComponents));
            }

            return errors;
        }

        public IEnumerable<ScriptError> CreateScriptErrorFromDuplicates(IEnumerable<ParsedComponentDefinition> duplicatedComponents)
        {
            foreach (var c in duplicatedComponents)
            {
                var returned = ParsedToken.Create<ScriptError>(c.ParsedToken.Token);
                returned.Exception = new DuplicateNameException(c.Name);
                yield return returned;
            }
        }
    }
}
