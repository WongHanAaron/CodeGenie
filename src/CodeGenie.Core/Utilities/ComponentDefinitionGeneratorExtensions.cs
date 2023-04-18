using CodeGenie.Core.Models.ComponentDefinitions.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenie.Core.Utilities
{
    public static class ComponentDefinitionGeneratorExtensions
    {
        public static bool WillNeedDetails(bool hasPurpose, bool hasTags, bool hasNonPurposeOrTags) => hasPurpose || hasTags || hasNonPurposeOrTags;
        public static bool ComponentNeedsNewLineDetails(bool hasPurpose, bool hasTags, bool hasNonPurposeOrTags) => (hasPurpose && hasTags) || (hasPurpose && hasNonPurposeOrTags) || (hasTags && hasNonPurposeOrTags);
        public static bool HasPurpose(this ComponentDefinition component) => !string.IsNullOrWhiteSpace(component.Purpose);
        public static bool HasTags(this ComponentDefinition component) => component.Tags?.Any() ?? false;
        public static bool HasNonPurposeOrTagComponentDetails(this ComponentDefinition component)
            => (component.Attributes?.Any() ?? false) ||
               (component.MethodDefinitions?.Any() ?? false) ||
               (component.RelationshipDefinitions?.Any() ?? false);

    }
}
