using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CodeGenie.Core.Models.Attributes;
using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using CodeGenie.Core.Models.ComponentDefinitions.Syntax;
using CodeGenie.Core.Services.Parsing.ComponentDefinitions.Shared;
using CodeGenie.Core.Services.Parsing.ComponentDefinitions.SyntaxDescribing.SyntaxRuleDescribers;
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
        SyntaxDescription GetSyntaxDescription(string script, int lineNumber, int columnNumber);
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
            SetupAutoIncludedDescribers();
        }

        public SyntaxDescription GetSyntaxDescription(string script, int lineNumber, int columnNumber)
        {
            Logger.LogDebug($"{nameof(GetSyntaxDescription)} for line {lineNumber} and column {columnNumber}");

            var result = ContextParser.ParseContext(script);

            if (result.Context == null) return SyntaxDescription.CreateUnknown(result);

            var closestNode = SyntaxTreeSearcher.GetClosestNode(result.Context, lineNumber, columnNumber);

            if (closestNode == null)
            {
                Logger.LogDebug($"{nameof(SyntaxDescriberTreeSearcher.GetClosestNode)} returned null");
                return SyntaxDescription.Create(result, SyntaxDescriptor.BeforeStartComponentDefinition, false);
            }

            Logger.LogDebug($"{nameof(SyntaxDescriberTreeSearcher.GetClosestNode)} returned {closestNode.GetType()} on {closestNode?.Symbol?.Line}, {closestNode?.Symbol?.Column}");
            return GetSyntaxStateFromNode(result, closestNode, new SyntaxSearchParameters(lineNumber, columnNumber));
        }

        protected SyntaxDescription GetSyntaxStateFromNode(ParsingResult result, ITerminalNode node, SyntaxSearchParameters searchParameters)
        {
            var rule = GetClosestMatchingRule(node);

            var ruleType = rule.GetType();

            Logger.LogDebug($"{nameof(GetClosestMatchingRule)} returned {ruleType}");

            if (!IsSearchWithinRuleBounds(rule, searchParameters))
            {
                // If it is not in the bounds, it might be a new line. Need to handle
                Logger.LogDebug($"Rule does not bound the search target");

                return SyntaxDescription.Create(result, SyntaxDescriptor.BeforeStartComponentDefinition, false);
            }

            var describer = GetRuleDescriber(ruleType);

            if (describer == null) return SyntaxDescription.CreateUnknown(result);

            return describer.Describe(result, rule, node, searchParameters);
        }

        protected bool IsSearchWithinRuleBounds(ParserRuleContext rule, SyntaxSearchParameters parameters)
        {
            var minLine = Math.Min(rule.Start.Line, rule.Stop.Line);
            var maxLine = Math.Max(rule.Start.Line, rule.Stop.Line);
            
            // Check if the search was for a character outside the line bounds
            if (parameters.LineNumber > maxLine || parameters.LineNumber < minLine) return false;

            if (parameters.LineNumber == minLine)
                return IsSearchWithinLineColumnBounds(rule.Start, false, parameters);
            
            if (parameters.LineNumber == maxLine)
                return IsSearchWithinLineColumnBounds(rule.Stop, true, parameters);

            return true;
        }

        protected bool IsSearchWithinLineColumnBounds(IToken token, bool expectTokenGreaterThanColumn, SyntaxSearchParameters parameters)
        {
            if (expectTokenGreaterThanColumn)
            {
                return token.Column >= parameters.ColumnNumber;
            }
            else
            {
                return token.Column <= parameters.ColumnNumber;
            }
        }

        protected void SetupAutoIncludedDescribers()
        {
            var syntaxRuleDescribers = this.GetType().Assembly.GetTypes().Where(t => typeof(ISyntaxRuleDescriber).IsAssignableFrom(t));
            foreach (var ruleDescriberType in syntaxRuleDescribers)
            {
                if (ruleDescriberType.IsAbstract) continue;
                if (ruleDescriberType.IsInterface) continue;
                var autoExcludeAttribute = 
                    ruleDescriberType.GetCustomAttributes(true).
                    FirstOrDefault(a => a is AutoExcludeAttribute) as AutoExcludeAttribute;
                if (autoExcludeAttribute != null &&
                    autoExcludeAttribute.Exclude) continue;

                var instance = Activator.CreateInstance(ruleDescriberType) as ISyntaxRuleDescriber;
                if (instance != null)
                    AddDescriber(instance);
            }
        }

        protected ConcurrentDictionary<Type, ISyntaxRuleDescriber> _describer = new ConcurrentDictionary<Type, ISyntaxRuleDescriber>();

        protected void AddDescriber(ISyntaxRuleDescriber describer)
            => _describer[describer.ParserRuleType] = describer;

        protected void AddGenericDescriber<TRule>(Func<TRule, ITerminalNode, SyntaxSearchParameters, SyntaxDescriptor> method) where TRule : ParserRuleContext
            => _describer[typeof(TRule)] = new GenericSyntaxRuleDescriber<TRule>(typeof(TRule), method);
        
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
