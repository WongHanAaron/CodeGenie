using CodeGenie.Core.Models.ComponentDefinitions.Definitions;
using CodeGenie.Core.Models.Generation;
using CodeGenie.Core.Models.Generation.Contexts;
using CodeGenie.Core.Models.Generation.Definitions;
using CodeGenie.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;

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
            
            if (!NeedMultilineDetails(hasPurpose, hasTags, hasNonPurposeOrTags))
            {
                AppendSinglelinePurposeOrTag(context, hasPurpose, hasTags, component.Purpose, component.Tags);
            }
            else
            {
                AppendMultilineComponentDetails(context, component);
            }
        }

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
            
            if (component.HasMethods())
            {
                AppendLineToTab(context);
                AppendMethods(context, component.MethodDefinitions);
            }

            AppendLineToTab(context, -1);

            context.ContentBuilder.Append("}");
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
            context.ContentBuilder.Append("attributes");

            AppendLineToTab(context);

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

        public void AppendMethods(GenerationContext context, IEnumerable<MethodDefinition> methods)
        {
            context.ContentBuilder.Append("methods");

            AppendLineToTab(context);

            context.ContentBuilder.Append("{");

            context.CurrentNumberOfTabs += 1;

            foreach (var method in methods)
            {
                AppendLineToTab(context);
                AppendMethod(context, method);
            }

            AppendLineToTab(context, -1);

            context.ContentBuilder.Append("}");
        }

        public void AppendMethod(GenerationContext context, MethodDefinition method)
        {
            AppendMethodDefinition(context, method.Scope, method.Name, method.ReturnTypeName, method.Parameters);
        
            
        }

        public void AppendRelationship(GenerationContext context, RelationshipDefinition relationship)
        {

        }

        /// <summary> Generate a generic definition. Usually of format "[Scope] [DefinitionName] : [DefinitionType]"</summary>
        public void AppendDefinition(GenerationContext context, Scope scope, string definitionName, string definitionType)
            => context.ContentBuilder.Append($"{GetScope(scope)} {definitionName} : {definitionType}");

        public void AppendMethodDefinition(GenerationContext context, Scope scope, string definitionName, string returnType, IEnumerable<ParameterDefinition> parameters)
        {
            var parametersString = string.Join(", ", parameters.Select(p => $"{p.Name} : {p.TypeName}"));
            context.ContentBuilder.Append($"{GetScope(scope)} {definitionName} ({parametersString}) : {returnType}");
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
        public static bool NeedMultilineDetails(bool hasPurpose, bool hasTags, bool hasNonPurposeOrTags) => (hasPurpose && hasTags) || hasNonPurposeOrTags;
    }
}
