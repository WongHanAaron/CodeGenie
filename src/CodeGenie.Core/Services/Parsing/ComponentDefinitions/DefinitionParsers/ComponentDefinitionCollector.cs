using Antlr4.Runtime.Misc;
using CodeGenie.Core.Models.ComponentDefinitions;
using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using System;
using System.Collections.Generic;
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
            var name = context.NAME().Symbol.Text;
            var component = new ParsedComponentDefinition()
            {
                Name = name
            };
            _componentDefinitions.Add(component);
            return component;
        }
    }
}
