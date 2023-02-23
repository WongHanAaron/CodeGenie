using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Models.ComponentDefinitions.Definitions
{
    /// <summary> The base type of the relationship definition </summary>
    public class RelationshipDefinition
    {
        /// <summary> The relationship type name </summary>
        public RelationshipType RelationshipType { get; set; }
        public string RelatedComponentName { get; set; }
    }
}
