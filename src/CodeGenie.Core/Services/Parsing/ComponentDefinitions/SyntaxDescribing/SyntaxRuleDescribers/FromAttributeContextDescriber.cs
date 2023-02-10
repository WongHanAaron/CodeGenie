using Antlr4.Runtime.Tree;
using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using CodeGenie.Core.Models.ComponentDefinitions.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CodeGenieParser;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions.SyntaxDescribing.SyntaxRuleDescribers
{
    public class FromAttributeContextDescriber : SyntaxRuleDescriberBase<AttributeContext>
    {
        public override SyntaxDescriptor Describe(ParsingResult parsingResults, AttributeContext rule, ITerminalNode selectedNode, SyntaxSearchParameters searchParameters)
        {
            if (rule.children.FirstOrDefault() == selectedNode) return SyntaxDescriptor.BeforeAttributesDetails;
            if (selectedNode.Parent.GetChild(0) == selectedNode) return SyntaxDescriptor.BeforeAttributeNameDefinition;
            if (selectedNode.Parent.GetChild(1) == selectedNode) return SyntaxDescriptor.BeforeAttributeDivider;
            if (selectedNode.Parent.GetChild(2) == selectedNode) return SyntaxDescriptor.BeforeAttributeTypeDefinition;
            return SyntaxDescriptor.BeforeStartAttributeDefinition;
        }
    }
}
