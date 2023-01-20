using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CodeGenie.Core.Models.ComponentDefinitions.State;
using CodeGenie.Core.Models.ComponentDefinitions.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions.SyntaxDescribing.SyntaxRuleDescribers
{
    public class GenericSyntaxRuleDescriber<TRule> : SyntaxRuleDescriberBase<TRule>
    {
        public Func<TRule, ITerminalNode, SyntaxSearchParameters, SyntaxDescriptor> DescriptionMethod { get; protected set; }

        public GenericSyntaxRuleDescriber(Type parserRuleType, Func<TRule, ITerminalNode, SyntaxSearchParameters, SyntaxDescriptor> descriptionMethod) : base(parserRuleType)
        {
            DescriptionMethod = descriptionMethod;
        }

        public override SyntaxDescriptor Describe(TRule rule, ITerminalNode selectedNode, SyntaxSearchParameters searchParameters)
        {
            return DescriptionMethod?.Invoke(rule, selectedNode, searchParameters) ?? SyntaxDescriptor.Error;
        }
    }
}
