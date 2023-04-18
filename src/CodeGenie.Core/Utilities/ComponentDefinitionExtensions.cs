using CodeGenie.Core.Models.ComponentDefinitions.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenie.Core.Utilities
{
    public static class ComponentDefinitionExtensions
    {
        public static bool HasPurpose(this ComponentDefinition component) => !string.IsNullOrWhiteSpace(component.Purpose);
        public static bool HasTags(this ComponentDefinition component) => component.Tags?.Any() ?? false;
        public static bool HasAttributes(this ComponentDefinition component) => component.Attributes?.Any() ?? false;
        public static bool HasNonPurposeOrTagComponentDetails(this ComponentDefinition component)
            => (component.Attributes?.Any() ?? false) ||
               (component.MethodDefinitions?.Any() ?? false) ||
               (component.RelationshipDefinitions?.Any() ?? false);

    }
}
