using Antlr4.Runtime.Tree;
using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using CodeGenie.Core.Models.ComponentDefinitions.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using static CodeGenieParser;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions.SyntaxDescribing.SyntaxRuleDescribers
{
    public class FromComponentContextDescriber : SyntaxRuleDescriberBase<ComponentContext>
    {
        public override SyntaxDescriptor Describe(ParsingResult parsingResults, ComponentContext rule, ITerminalNode selectedNode, SyntaxSearchParameters searchParameters)
        {
            if (selectedNode.Parent is Access_scopeContext) return SyntaxDescriptor.BeforeComponentNameDefinition;
            if (selectedNode.Parent.GetChild(1) == selectedNode) return SyntaxDescriptor.BeforeComponentDivider;
            if (selectedNode.Parent.GetChild(2) == selectedNode) return SyntaxDescriptor.BeforeComponentTypeDefinition;
            if (selectedNode.Parent is Component_typeContext) return SyntaxDescriptor.BeforeComponentDetails;
            return SyntaxDescriptor.Unknown;
        }
    }
}
