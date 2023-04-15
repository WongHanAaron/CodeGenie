using CodeGenie.Core.Models.ComponentDefinitions.Definitions;
using CodeGenie.Core.Models.Generation;
using CodeGenie.Core.Models.Generation.Contexts;
using CodeGenie.Core.Models.Generation.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenie.Core.Services.Generators.ComponentGenerators
{
    public class CodeGenieComponentGenerator : IComponentGenerator
    {
        protected readonly IWhitespaceGenerator _whitespaceGenerator;
        public CodeGenieComponentGenerator(IWhitespaceGenerator whitespaceGenerator)
        {
            _whitespaceGenerator = whitespaceGenerator;
        }

        public GeneratedComponentDefinition Generate(ComponentDefinition component)
        {
            var context = new GenerationContext();
            throw new NotImplementedException();
        }

        /// <summary> Generate a component definition </summary>
        public string GenerateComponentDefinition(GenerationContext context, ComponentDefinition component)
        {
            var b = new StringBuilder();
            b.Append(_whitespaceGenerator.GenerateTabs(context));
            b.Append(GetDefinition(component.Scope, component.Name, GetComponentType(component.IsInterface)));

            var hasPurpose = HasPurpose(component);
            var hasTags = HasTags(component);
            var hasNonPurposeOrTags = HasNonPurposeOrTagComponentDetails(component);

            if (!WillNeedDetails(hasPurpose, hasTags, hasNonPurposeOrTags)) return b.ToString();
            
            if (!ComponentNeedsNewLineDetails(hasPurpose, hasTags, hasNonPurposeOrTags))
            {
                b.Append(" { ");
                if (hasPurpose) b.Append(GetPurpose(component.Purpose));
                if (hasTags) b.Append(GetTags(component.Tags));
                b.Append(" }");
            }
            else
            {

            }
            return b.ToString();
        }

        public bool WillNeedDetails(bool hasPurpose, bool hasTags, bool hasNonPurposeOrTags)
            => hasPurpose || hasTags || hasNonPurposeOrTags;

        public bool ComponentNeedsNewLineDetails(bool hasPurpose, bool hasTags, bool hasNonPurposeOrTags)
        => (hasPurpose && hasTags) || (hasPurpose && hasNonPurposeOrTags) || (hasTags && hasNonPurposeOrTags);
        

        public bool HasPurpose(ComponentDefinition component) => !string.IsNullOrWhiteSpace(component.Purpose);

        public bool HasTags(ComponentDefinition component) => component.Tags?.Any() ?? false;

        public bool HasNonPurposeOrTagComponentDetails(ComponentDefinition component) 
            => (component.Attributes?.Any() ?? false) || 
               (component.MethodDefinitions?.Any() ?? false) || 
               (component.RelationshipDefinitions?.Any() ?? false);

        /// <summary> Generate a generic definition. Usually of format "[Scope] [DefinitionName] : [DefinitionType]"</summary>
        public string GetDefinition(Scope scope, string definitionName, string definitionType)
            => $"{GetScope(scope)} {definitionName} : {definitionType}";

        public string GetPurpose(string purpose)
            => $"purpose : \"{purpose}\"";

        public string GetTags(IEnumerable<string> tags)
        {
            var joinedTags = string.Join(" ", tags);
            return $"tags {{ {joinedTags} }}";
        }

        public string GetScope(Scope scope)
        {
            switch (scope)
            {
                case Scope.Public:
                    return "+";
                case Scope.Protected:
                    return "#";
                case Scope.Private:
                    return "-";
            }

            throw new ArgumentException($"The scope is '{scope}' and is invalid");
        }

        public string GetComponentType(bool isInterface)
            => isInterface ? "interface" : "class";
    }
}
