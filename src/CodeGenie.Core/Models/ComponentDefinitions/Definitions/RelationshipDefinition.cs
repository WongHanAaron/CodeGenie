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

        /// <summary> The purpose of this component </summary>
        public string Purpose { get; set; }

        /// <summary> The name of the related component </summary>
        public string RelatedComponentName { get; set; }

        /// <summary> The list of tags </summary>
        public List<string> Tags { get; set; }
    }
}
