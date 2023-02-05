using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Models.ComponentDefinitions.Syntax
{
    /// <summary> The data model for the syntax description of the script currently </summary>
    public class SyntaxDescription
    {
        /// <summary> The parsing results from this instance of the script </summary>
        public ParsingResult ParsedResult { get; set; }

        /// <summary> The syntax descriptor at the current cursor/caret </summary>
        public SyntaxDescriptor SyntaxDescriptorAtCaret { get; set; }

        public bool HasSyntaxErrorOnSelectedRule { get; set; } = false;

        /// <summary> If the current instance of the script has any syntax errors </summary>
        public bool HasSyntaxError => ParsedResult.HasErrors;

        public static SyntaxDescription CreateUnknown(ParsingResult result) => Create(result, SyntaxDescriptor.Unknown, false);
        public static SyntaxDescription CreateError(ParsingResult result, bool ruleHasSyntaxError) => Create(result, SyntaxDescriptor.Error, ruleHasSyntaxError);
        public static SyntaxDescription Create(ParsingResult result, SyntaxDescriptor description, bool ruleHasSyntaxError)
        {
            return new SyntaxDescription()
            {
                ParsedResult = result, 
                SyntaxDescriptorAtCaret = description,
                HasSyntaxErrorOnSelectedRule = ruleHasSyntaxError
            };
        }
    }
}
