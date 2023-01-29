using Antlr4.Runtime.Tree;
using CodeGenie.Core.Models.ComponentDefinitions.State;
using CodeGenie.Core.Models.ComponentDefinitions.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CodeGenieParser;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions.SyntaxDescribing.SyntaxRuleDescribers
{
    public class FromComponentDetailsContextDescriber : SyntaxRuleDescriberBase<Component_detailsContext>
    {
        public override SyntaxDescriptor Describe(Component_detailsContext rule, ITerminalNode selectedNode, SyntaxSearchParameters searchParameters)
        {
            if (rule.children.FirstOrDefault() == selectedNode && selectedNode.Symbol.Text.Equals("{")) return SyntaxDescriptor.WithinComponentDetails;
            if (rule.children.LastOrDefault() == selectedNode && selectedNode.Symbol.Text.Equals("}")) return SyntaxDescriptor.WithinComponentDetails;
            if (selectedNode.Symbol.Type.Equals(" ")) return SyntaxDescriptor.WithinComponentDetails;
            if (rule.children.Any(c => c == selectedNode)) return SyntaxDescriptor.WithinComponentDetails;
            return SyntaxDescriptor.WithinComponentDetails;
        }
    }
}
