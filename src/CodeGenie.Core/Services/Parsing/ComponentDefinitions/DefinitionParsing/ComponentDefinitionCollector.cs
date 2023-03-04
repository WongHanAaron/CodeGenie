using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using CodeGenie.Core.Models.ComponentDefinitions.Definitions;
using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CodeGenieParser;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions.DefinitionParsing
{
    /// <summary>
    /// A class for parsing a ComponentDefinition context and collecting the list of parsed component definitions
    /// within that context.
    /// </summary>
    public class ComponentDefinitionCollector : CodeGenieBaseVisitor<object>
    {
        public IReadOnlyList<ParsedComponentDefinition> ComponentDefinitions => _componentDefinitions;
        protected List<ParsedComponentDefinition> _componentDefinitions = new List<ParsedComponentDefinition>();
        public IReadOnlyList<ScriptError> Errors => _errors;
        protected List<ScriptError> _errors = new List<ScriptError>();

        public override object Visit(IParseTree tree)
        {
            _componentDefinitions.Clear();
            _errors.Clear();
            return base.Visit(tree);
        }

        public override object VisitComponent([NotNull] CodeGenieParser.ComponentContext context)
        {
            var component = new ParsedComponentDefinition();
            component.Name = context.NAME().Symbol.Text;
            component.IsInterface = IsInterface(context.component_type().GetText());
            component.Scope = LoadScope(context.access_scope());

            var start = context.Start;
            var stop = context.Stop;

            var componentDetailsContext = context.component_details();
            var details = VisitComponent_details(componentDetailsContext) as ParsedComponentDetails;
            if (details != null)
            {
                component.Purpose = details.Purpose;
                component.Attributes = details.Attributes.Select(a => a as AttributeDefinition).ToList();
                component.MethodDefinitions = details.Methods.Select(m => m as MethodDefinition).ToList();
                component.RelationshipDefinitions = details.Relationships.Select(r => r as RelationshipDefinition).ToList();
                component.Tags = details.Tags;
                stop = componentDetailsContext.Stop;
            };

            component.ParsedToken = ParsedToken.Create<ParsedToken>(start, stop);
            _componentDefinitions.Add(component);
            return component;
        }   

        protected Scope LoadScope(Access_scopeContext accessScope)
        {
            var scope = accessScope.children.FirstOrDefault();
            if (scope == null) return Scope.Unknown;
            if (scope.GetText().Equals("+") || scope.GetText().Equals("public")) return Scope.Public;
            if (scope.GetText().Equals("-") || scope.GetText().Equals("private")) return Scope.Private;
            if (scope.GetText().Equals("#") || scope.GetText().Equals("protected")) return Scope.Protected;
            return Scope.Unknown;
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

            // Parse Purpose
            var purposes = context.purpose();
            if (purposes.Count() > 1)
            {
                purposes.Skip(1).ToList().ForEach(c => 
                {
                    _errors.Add(ParsedToken.Create<ScriptError>(c.Start, c.Stop));
                });
            }
            componentDetails.Purpose = VisitPurpose(purposes.FirstOrDefault()) as string;

            // Parse Methods
            var collectedParsedMethods = new List<ParsedMethodDefinition>();
            foreach (var methods in context.methods() ?? new CodeGenieParser.MethodsContext[0])
            {
                var parsedMethods = VisitMethods(methods) as IEnumerable<ParsedMethodDefinition>;
                if (parsedMethods != null) collectedParsedMethods.AddRange(parsedMethods);
            }
            componentDetails.Methods = collectedParsedMethods;

            // Parse Relationships
            var collectedParsedRelationships = new List<ParsedRelationshipDefinition>();
            foreach (var relationshipsContext in context.relationships() ?? new RelationshipsContext[0])
            {
                var parsedRelationships = VisitRelationships(relationshipsContext) as IEnumerable<ParsedRelationshipDefinition>;
                if (parsedRelationships != null) collectedParsedRelationships.AddRange(parsedRelationships);
            }
            componentDetails.Relationships = collectedParsedRelationships;

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

        public override object VisitMethods([NotNull] MethodsContext context)
        {
            if (context == null) return null;

            var methods = new List<ParsedMethodDefinition>();
            foreach (var method in context.method())
            {
                methods.Add(LoadMethodDefinition(method));
            }
            return methods;
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

        public override object VisitPurpose([NotNull] CodeGenieParser.PurposeContext context)
        {
            var value = context?.value()?.STRING()?.GetText()?.Trim('\"');
            return value;
        }

        protected ParsedAttributeDefinition LoadAttributeDefinition([NotNull] CodeGenieParser.AttributeContext context)
        {
            var parsedAttribute = new ParsedAttributeDefinition();
            parsedAttribute.Name = context.NAME().GetText();
            parsedAttribute.Type = context.type().GetText();
            parsedAttribute.Scope = LoadScope(context.access_scope());
            return parsedAttribute;
        }

        protected ParsedMethodDefinition LoadMethodDefinition([NotNull] CodeGenieParser.MethodContext context)
        {
            var parsedMethod = new ParsedMethodDefinition();
            parsedMethod.Name = context.NAME().GetText();
            parsedMethod.ReturnTypeName = context.type().GetText();
            parsedMethod.Scope = LoadScope(context.access_scope());

            // Collect attributes
            var collectedParsedAttributes = new List<ParsedParameterDefinition>();
            foreach (var parameter in context.parameter() ?? new CodeGenieParser.ParameterContext[0])
            {
                var parsedParameters = VisitParameter(parameter) as ParsedParameterDefinition;
                if (parsedParameters != null) collectedParsedAttributes.Add(parsedParameters);
            }
            parsedMethod.Parameters = collectedParsedAttributes.Select(p => p as ParameterDefinition).ToList();

            return parsedMethod;
        }

        public override object VisitParameter([NotNull] ParameterContext context)
        {
            var parsedParameter = new ParsedParameterDefinition();
            parsedParameter.Name = context.NAME().GetText();
            parsedParameter.TypeName = context.type().GetText();
            return parsedParameter;
        }

        public override object VisitRelationships([NotNull] RelationshipsContext context)
        {
            var returned = new List<ParsedRelationshipDefinition>();

            foreach (var child in context.relationship() ?? new RelationshipContext[0])
            {
                var result = VisitRelationship(child) as ParsedRelationshipDefinition;
                if (result != null)
                {
                    returned.Add(result);
                }
            }

            return returned;
        }

        public override object VisitRelationship([NotNull] RelationshipContext context)
        {
            ParsedRelationshipDefinition parsed = null;

            if (TryVisitAndParse<DependencyContext>(() => context.dependency(), VisitDependency, out parsed))
            {
                return parsed;
            }

            if (TryVisitAndParse<AggregatesContext>(() => context.aggregates(), VisitAggregates, out parsed))
            {
                return parsed;
            }

            if (TryVisitAndParse<ComposesContext>(() => context.composes(), VisitComposes, out parsed))
            {
                return parsed;
            }

            if (TryVisitAndParse<SpecializesContext>(() => context.specializes(), VisitSpecializes, out parsed))
            {
                return parsed;
            }

            if (TryVisitAndParse<RealizesContext>(() => context.realizes(), VisitRealizes, out parsed))
            {
                return parsed;
            }

            return parsed;
        }

        public bool TryVisitAndParse<TRelationship>(
            Func<TRelationship> relationshipExtractor,
            Func<TRelationship, object> visitorMethod,
            out ParsedRelationshipDefinition parsed)
                where TRelationship : ParserRuleContext
        {
            parsed = null;

            var ruleContext = relationshipExtractor?.Invoke();

            if (ruleContext == null) return false;

            var visitedResult = visitorMethod?.Invoke(ruleContext);

            parsed = visitedResult as ParsedRelationshipDefinition;

            return parsed != null;
        }

        public override object VisitDependency([NotNull] DependencyContext context)
        {
            var returned = ParseRelationshipDefinition<DependencyContext, ParsedRelationshipDefinition>(context);
            return returned;
        }

        public override object VisitAggregates([NotNull] AggregatesContext context)
        {
            var returned = ParseRelationshipDefinition<AggregatesContext, ParsedRelationshipDefinition>(context);
            return returned;
        }

        public override object VisitComposes([NotNull] ComposesContext context)
        {
            var returned = ParseRelationshipDefinition<ComposesContext,ParsedRelationshipDefinition>(context);
            return returned;
        }

        public override object VisitSpecializes([NotNull] SpecializesContext context)
        {
            var returned = ParseRelationshipDefinition<SpecializesContext, ParsedRelationshipDefinition>(context);
            return returned;
        }

        public override object VisitRealizes([NotNull] RealizesContext context)
        {
            var returned = ParseRelationshipDefinition<RealizesContext, ParsedRelationshipDefinition>(context);
            return returned;
        }

        public TReturn ParseRelationshipDefinition<TRelationship, TReturn>(ParserRuleContext ruleContext) where TReturn : ParsedRelationshipDefinition
        {
            var created = Activator.CreateInstance(typeof(TReturn)) as TReturn;

            var relationshipType = GetRelationshipType(ruleContext);

            if (!relationshipType.HasValue) return null;

            created.RelationshipType = relationshipType.Value;

            var type = typeof(TRelationship);

            var typeMethod = type.GetMethod(nameof(DependencyContext.type));

            if (typeMethod == null) return null;

            var typeContext = typeMethod.Invoke(ruleContext, new object[0]) as TypeContext;

            if (typeContext == null) return null;

            var componentType = typeContext.GetText();

            created.RelatedComponentName = componentType;

            return created;
        }

        public ParsedRelationshipDetails GetRelationshipDetails(ParserRuleContext parserRuleContext)
        {
            return null;
        }

        public RelationshipType? GetRelationshipType(ParserRuleContext parserRuleContext)
        {
            switch(parserRuleContext.GetType().Name)
            {
                case nameof(DependencyContext):
                    return RelationshipType.Dependency;
                case nameof(ComposesContext):
                    return RelationshipType.Composition;
                case nameof(AggregatesContext):
                    return RelationshipType.Aggregation;
                case nameof(RealizesContext):
                    return RelationshipType.Realization;
                case nameof(SpecializesContext):
                    return RelationshipType.Specialization;
                default:
                    return null;
            }
        }
    }
}
