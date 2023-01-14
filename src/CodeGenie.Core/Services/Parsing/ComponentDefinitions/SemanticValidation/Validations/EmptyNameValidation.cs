using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions.SemanticValidation.Validations
{
    /// <summary> Perform validation that no component, attribute, method or relationship name should be empty </summary>
    public class EmptyNameValidation : IComponentValidation
    {
        public IEnumerable<ScriptError> Validate(IEnumerable<ParsedComponentDefinition> components)
        {
            var emptyComponentResults = components.Where(c => string.IsNullOrEmpty(c.Name))
                                                  .Select(c =>
                                                  {
                                                      var error = ParsedToken.Create<ScriptError>(c.ParsedToken);
                                                      error.Exception = new ArgumentNullException("The component name is empty");
                                                      return error;
                                                  });
            return emptyComponentResults;    
        }
    }
}
