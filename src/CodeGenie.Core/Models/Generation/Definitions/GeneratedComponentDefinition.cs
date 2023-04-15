using CodeGenie.Core.Models.ComponentDefinitions.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Models.Generation.Definitions
{
    /// <summary> The representation of the generated code for the component </summary>
    public class GeneratedComponentDefinition
    {
        /// <summary> The generated code </summary>
        public string GeneratedCode { get; set; }

        /// <summary> The component the generated code is from </summary>
        public ComponentDefinition Component { get; set; }
    }
}
