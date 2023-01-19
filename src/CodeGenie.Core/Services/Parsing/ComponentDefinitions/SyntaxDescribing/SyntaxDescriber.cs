using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CodeGenie.Core.Models.ComponentDefinitions.State;
using CodeGenie.Core.Models.ComponentDefinitions.Syntax;
using CodeGenie.Core.Services.Parsing.ComponentDefinitions.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CodeGenieParser;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions.SyntaxDescribing
{
    /// <summary> Passes in the script and describes the syntax in the passed in location in the text </summary>
    public interface ISyntaxDescriber
    {
        /// <summary> Get the syntax state at that line and column number </summary>
        SyntaxDescriptor GetSyntaxDescription(string script, int lineNumber, int columnNumber);
    }

    public class SyntaxDescriber : ISyntaxDescriber
    {
        protected readonly ILogger<SyntaxDescriber> Logger;
        protected readonly IComponentDefinitionContextParser ContextParser;
        protected readonly ISyntaxDescriberTreeSearcher SyntaxTreeSearcher;

        public SyntaxDescriber(ILogger<SyntaxDescriber> logger, 
                               IComponentDefinitionContextParser contextParser,
                               ISyntaxDescriberTreeSearcher syntaxTreeTraverser)
        {
            Logger = logger;
            ContextParser = contextParser;
            SyntaxTreeSearcher = syntaxTreeTraverser;
            SetupDescribers();
        }

        public SyntaxDescriptor GetSyntaxDescription(string script, int lineNumber, int columnNumber)
        {
            var result = ContextParser.ParseContext(script);

            if (result.Context == null) return SyntaxDescriptor.Unknown;

            var closestNode = SyntaxTreeSearcher.GetClosestNode(result.Context, lineNumber, columnNumber);

            if (closestNode == null) return SyntaxDescriptor.BeforeStartComponentDefinition;

            return GetSyntaxStateFromNode(closestNode, new SyntaxSearchParameters(lineNumber, columnNumber));
        }

        protected SyntaxDescriptor GetSyntaxStateFromNode(ITerminalNode node, SyntaxSearchParameters searchParameters)
        {
            var rule = GetClosestMatchingRule(node);

            var ruleType = rule.GetType();

            var describer = GetRuleDescriber(ruleType);

            if (describer == null) return SyntaxDescriptor.Unknown;

            return describer.Describe(rule, node, searchParameters);
        }

        protected SyntaxDescriptor FromComponentContext(ComponentContext rule, ITerminalNode node, SyntaxSearchParameters searchParameters)
        {
            if (node.Parent is Access_scopeContext) return SyntaxDescriptor.BeforeComponentNameDefinition;
            if (node.Parent.GetChild(1) == node) return SyntaxDescriptor.BeforeComponentDivider;
            if (node.Parent.GetChild(2) == node) return SyntaxDescriptor.BeforeComponentTypeDefinition;
            if (node.Parent is Component_typeContext) return SyntaxDescriptor.BeforeComponentDetails;
            return SyntaxDescriptor.Unknown;
        }

        protected SyntaxDescriptor FromComponentDetailsContext(Component_detailsContext rule, ITerminalNode node, SyntaxSearchParameters searchParameters)
        {
            if (rule.children.FirstOrDefault() == node && node.Symbol.Text.Equals("{")) return SyntaxDescriptor.WithinComponentDetails;
            if (rule.children.LastOrDefault() == node && node.Symbol.Text.Equals("}")) return SyntaxDescriptor.WithinComponentDetails;
            if (node.Symbol.Type.Equals(" ")) return SyntaxDescriptor.WithinComponentDetails;
            return SyntaxDescriptor.Unknown;
        }

        protected SyntaxDescriptor FromPurposeContext(PurposeContext rule, ITerminalNode node, SyntaxSearchParameters searchParameters)
        {
            if (node.Symbol.Text.EndsWith("\"") && searchParameters.ColumnNumber > 
                node.Symbol.Column + node.Symbol.Text.Count()) return SyntaxDescriptor.WithinComponentDetails;
            return SyntaxDescriptor.Unknown;
        }

        protected void SetupDescribers()
        {
            AddDescriber<ComponentContext>(FromComponentContext);
            AddDescriber<Component_detailsContext>(FromComponentDetailsContext);
            AddDescriber<PurposeContext>(FromPurposeContext);
        }

        protected ConcurrentDictionary<Type, ISyntaxRuleDescriber> _describer = new ConcurrentDictionary<Type, ISyntaxRuleDescriber>();

        protected void AddDescriber<TRule>(Func<TRule, ITerminalNode, SyntaxSearchParameters, SyntaxDescriptor> method) where TRule : ParserRuleContext
            => _describer[typeof(TRule)] = new SyntaxRuleDescriber<TRule>(typeof(TRule), method);
        
        protected ISyntaxRuleDescriber GetRuleDescriber(Type ruleType)
            => _describer.FirstOrDefault(kvp => kvp.Key.IsAssignableFrom(ruleType)).Value;

        protected ParserRuleContext GetClosestMatchingRule(IParseTree node)
        {
            var nodeType = node.GetType();
            if (_describer.Any(kvp => kvp.Key.IsAssignableFrom(nodeType)) && node is ParserRuleContext rule)
            {
                return rule;
            }
            else
            {
                return GetClosestMatchingRule(node.Parent);
            }
        }
    }
}
