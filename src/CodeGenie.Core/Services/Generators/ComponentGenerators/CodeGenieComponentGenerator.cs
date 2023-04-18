using CodeGenie.Core.Models.ComponentDefinitions.Definitions;
using CodeGenie.Core.Models.Generation;
using CodeGenie.Core.Models.Generation.Contexts;
using CodeGenie.Core.Models.Generation.Definitions;
using CodeGenie.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            AppendTabs(context);
            
            AppendDefinition(context, component.Scope, component.Name, GetComponentType(component.IsInterface));

            var hasPurpose = component.HasPurpose();
            var hasTags = component.HasTags();
            var hasNonPurposeOrTags = component.HasNonPurposeOrTagComponentDetails();

            if (!WillNeedDetails(hasPurpose, hasTags, hasNonPurposeOrTags)) return;
            
            if (!NeedSinglelineDetails(hasPurpose, hasTags, hasNonPurposeOrTags))
            {
                AppendSinglelinePurposeOrTag(context, hasPurpose, hasTags, component.Purpose, component.Tags);
            }
            else
            {
                AppendMultilineComponentDetails(context, component);
            }
        }

        /// <summary> Generate a generic definition. Usually of format "[Scope] [DefinitionName] : [DefinitionType]"</summary>
        public void AppendDefinition(GenerationContext context, Scope scope, string definitionName, string definitionType)
            => context.ContentBuilder.Append($"{GetScope(scope)} {definitionName} : {definitionType}");

        public void AppendSinglelinePurposeOrTag(GenerationContext context, bool hasPurpose, bool hasTags, string purpose, IEnumerable<string> tags)
        {
            context.ContentBuilder.Append(" { ");
            if (hasPurpose) AppendPurpose(context, purpose);
            if (hasTags) AppendTags(context, tags);
            context.ContentBuilder.Append(" }");
        }

        public void AppendMultilineComponentDetails(GenerationContext context, ComponentDefinition component)
        {
            context.ContentBuilder.AppendLine();

            AppendTabs(context);

            context.ContentBuilder.Append("{");

            context.CurrentNumberOfTabs += 1;

            if (component.HasPurpose())
            {
                AppendLineToTab(context);
                AppendPurpose(context, component.Purpose);
            }

            if (component.HasTags())
            {
                AppendLineToTab(context);
                AppendTags(context, component.Tags);
            }

            if (component.HasAttributes())
            {
                AppendLineToTab(context);
                AppendAttributes(context, component.Attributes);
            }

            context.CurrentNumberOfTabs -= 1;
        }

        public void AppendPurpose(GenerationContext context, string purpose)
            => context.ContentBuilder.Append($"purpose : \"{purpose}\"");

        public void AppendTags(GenerationContext context, IEnumerable<string> tags)
        {
            context.ContentBuilder.Append("tags { ");
            context.ContentBuilder.Append(string.Join(" ", tags));
            context.ContentBuilder.Append(" }");
        }

        public void AppendAttributes(GenerationContext context, IEnumerable<AttributeDefinition> attributes)
        {
            context.ContentBuilder.AppendLine("attributes");
            context.ContentBuilder.Append("{");
            
            context.CurrentNumberOfTabs += 1;

            foreach (var attribute in attributes)
            {
                AppendLineToTab(context);
                AppendAttribute(context, attribute);
            }

            AppendLineToTab(context, -1);

            context.ContentBuilder.Append("}");
        }

        public void AppendAttribute(GenerationContext context, AttributeDefinition attribute)
        {
            AppendDefinition(context, attribute.Scope, attribute.Name, attribute.Type);
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

        public void AppendLineToTab(GenerationContext context, int? increment = null)
        {
            if (increment.HasValue) context.CurrentNumberOfTabs += increment.Value;
            context.ContentBuilder.AppendLine();
            AppendTabs(context);
        }

        public void AppendTabs(GenerationContext context)
        {
            context.ContentBuilder.Append(_whitespaceGenerator.GenerateTabs(context));
        }

        public static bool WillNeedDetails(bool hasPurpose, bool hasTags, bool hasNonPurposeOrTags) => hasPurpose || hasTags || hasNonPurposeOrTags;
        public static bool NeedSinglelineDetails(bool hasPurpose, bool hasTags, bool hasNonPurposeOrTags) => (hasPurpose && hasTags) || (hasPurpose && hasNonPurposeOrTags) || (hasTags && hasNonPurposeOrTags);
    }
}
