using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using CodeGenie.Core.Services.Parsing.ComponentDefinitions.SemanticValidators.Validations;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions.SemanticValidators
{
    /// <summary>
    /// The component for performing all required semantic validation
    /// </summary>
    public interface ISemanticValidator
    {
        ParsingResult Validate(ParsingResult priorResult);
    }

    public class ClusteredSemanticValidator : ISemanticValidator
    {
        protected readonly ILogger<ClusteredSemanticValidator> Logger;
        protected readonly IEnumerable<IComponentValidation> Validators;
        public ClusteredSemanticValidator(ILogger<ClusteredSemanticValidator> logger, IEnumerable<IComponentValidation> validators)
        {
            Logger = logger;
            Validators = validators;
        }

        public ParsingResult Validate(ParsingResult priorParsingResult)
        {
            var errors = new List<ScriptError>();
            if (priorParsingResult.HasErrors)
            {
                errors.AddRange(priorParsingResult.Errors);
            }

            foreach (var validator in Validators)
            {
                var parsingErrors = validator.Validate(priorParsingResult.Components);
                if (parsingErrors.Any())
                {
                    errors.AddRange(parsingErrors);
                }
            }

            if (errors.Any())
            {
                return new ParsingResult(errors);
            }
            else
            {
                return new ParsingResult(priorParsingResult.Components, priorParsingResult.Context);
            }
        }
    }
}
