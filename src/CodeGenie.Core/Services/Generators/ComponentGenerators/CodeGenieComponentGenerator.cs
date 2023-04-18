using CodeGenie.Core.Models.ComponentDefinitions.Definitions;
using CodeGenie.Core.Models.Generation;
using CodeGenie.Core.Models.Generation.Contexts;
using CodeGenie.Core.Models.Generation.Definitions;
using CodeGenie.Core.Utilities;
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
        public void AppendComponentDefinition(GenerationContext context, ComponentDefinition component)
        {            
            context.ContentBuilder.Append(_whitespaceGenerator.GenerateTabs(context));
            
            AppendDefinition(context, component.Scope, component.Name, GetComponentType(component.IsInterface));

            var hasPurpose = component.HasPurpose();
            var hasTags = component.HasTags();
            var hasNonPurposeOrTags = component.HasNonPurposeOrTagComponentDetails();

            if (!ComponentDefinitionGeneratorExtensions
                    .WillNeedDetails(hasPurpose, hasTags, hasNonPurposeOrTags)) return;
            
            if (!ComponentDefinitionGeneratorExtensions
                    .ComponentNeedsNewLineDetails(hasPurpose, hasTags, hasNonPurposeOrTags))
            {
                context.ContentBuilder.Append(" { ");
                if (hasPurpose) AppendPurpose(context, component.Purpose);
                if (hasTags) AppendTags(context, component.Tags);
                context.ContentBuilder.Append(" }");
            }
            else
            {

            }
        }

        /// <summary> Generate a generic definition. Usually of format "[Scope] [DefinitionName] : [DefinitionType]"</summary>
        public void AppendDefinition(GenerationContext context, Scope scope, string definitionName, string definitionType)
            => context.ContentBuilder.Append($"{GetScope(scope)} {definitionName} : {definitionType}");

        public void AppendPurpose(GenerationContext context, string purpose)
            => context.ContentBuilder.Append($"purpose : \"{purpose}\"");

        public void AppendTags(GenerationContext context, IEnumerable<string> tags)
        {
            context.ContentBuilder.Append("tags { ");
            context.ContentBuilder.Append(string.Join(" ", tags));
            context.ContentBuilder.Append(" }");
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
