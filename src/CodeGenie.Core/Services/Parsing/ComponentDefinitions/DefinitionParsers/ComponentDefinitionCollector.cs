using Antlr4.Runtime.Misc;
using CodeGenie.Core.Models.ComponentDefinitions;
using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions.DefinitionParsers
{
    /// <summary>
    /// A class for parsing a ComponentDefinition context and collecting the list of parsed component definitions
    /// within that context.
    /// </summary>
    public class ComponentDefinitionCollector : CodeGenieBaseVisitor<object>
    {
        public IReadOnlyList<ParsedComponentDefinition> ComponentDefinitions => _componentDefinitions;
        protected List<ParsedComponentDefinition> _componentDefinitions = new List<ParsedComponentDefinition>();

        public override object VisitComponent([NotNull] CodeGenieParser.ComponentContext context)
        {
            var component = new ParsedComponentDefinition();
            component.Name = context.NAME().Symbol.Text;
            component.IsInterface = IsInterface(context.component_type().GetText());

            var start = context.Start;
            var stop = context.Stop;

            var componentDetailsContext = context.component_details();
            var details = VisitComponent_details(componentDetailsContext) as ParsedComponentDetails;
            if (details != null)
            {
                component.Purpose = details.Purpose;
                component.Attributes = details.Attributes.Select(a => a as AttributeDefinition).ToList();
                component.Tags = details.Tags;
                stop = componentDetailsContext.Stop;
            };

            component.ParsedToken = ParsedToken.Create<ParsedToken>(start, stop);
            _componentDefinitions.Add(component);
            return component;
        }

        protected bool IsInterface(string componentType)
        {
            return componentType.Equals("interface", StringComparison.InvariantCultureIgnoreCase);
        }

        public override object VisitComponent_details([NotNull] CodeGenieParser.Component_detailsContext context)
        {
            if (context == null) return null;

            var componentDetails = new ParsedComponentDetails();

            // Parse Attributes
            var collectedParsedAttributes = new List<ParsedAttributeDefinition>();
            foreach (var attributes in context.attributes() ?? new CodeGenieParser.AttributesContext[0])
            {
                var parsedAttributes = VisitAttributes(attributes) as IEnumerable<ParsedAttributeDefinition>;
                if (parsedAttributes != null) collectedParsedAttributes.AddRange(parsedAttributes);
            }
            componentDetails.Attributes = collectedParsedAttributes;

            // Parse Tags
            var collectedTags = new List<string>();
            foreach (var tags in context.tags() ?? new CodeGenieParser.TagsContext[0])
            {
                var t = VisitTags(tags) as List<string>;
                if (t != null && t.Any())
                {
                    collectedTags.AddRange(t);
                }
            }
            componentDetails.Tags = collectedTags; 

            return componentDetails;
        }

        public override object VisitAttributes([NotNull] CodeGenieParser.AttributesContext context)
        {
            if (context == null) return null;

            var attributes = new List<ParsedAttributeDefinition>();
            foreach (var attribute in context.attribute())
            {
                attributes.Add(LoadAttributeDefinition(attribute));
            }
            return attributes;
        }

        public override object VisitTags([NotNull] CodeGenieParser.TagsContext context)
        {
            var tags = new List<string>();
            foreach (var tag in context?.tag() ?? new CodeGenieParser.TagContext[0])
            {
                var t = VisitTag(tag);
                if (t is string stringTag && !string.IsNullOrWhiteSpace(stringTag))
                {
                    tags.Add(stringTag);
                }
            }
            return tags;
        }

        public override object VisitTag([NotNull] CodeGenieParser.TagContext context)
        {
            var value = context.STRING().GetText().Trim('\"');
            return value;
        }

        protected ParsedAttributeDefinition LoadAttributeDefinition([NotNull] CodeGenieParser.AttributeContext context)
        {
            var parsedAttribute = new ParsedAttributeDefinition();
            parsedAttribute.Name = context.NAME().GetText();
            parsedAttribute.Type = context.type().GetText();
            return parsedAttribute;
        }


    }
}
