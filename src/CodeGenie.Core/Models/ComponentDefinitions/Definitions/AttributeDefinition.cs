using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Models.ComponentDefinitions.Definitions
{
    public class AttributeDefinition
    {
        /// <summary> The scope of accessibility for this attribute </summary>
        public Scope Scope { get; set; }
        /// <summary> The name of this attribute </summary>
        public string Name { get; set; }
        /// <summary> The datatype for this attribute </summary>
        public string Type { get; set; }
    }
}
