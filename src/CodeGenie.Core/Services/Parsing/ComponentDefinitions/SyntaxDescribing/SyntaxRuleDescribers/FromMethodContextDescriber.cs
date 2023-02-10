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
    public class FromMethodContextDescriber : SyntaxRuleDescriberBase<MethodContext>
    {
        public override SyntaxDescriptor Describe(ParsingResult parsingResults, MethodContext rule, ITerminalNode selectedNode, SyntaxSearchParameters searchParameters)
        {
            if (rule.children.FirstOrDefault() == selectedNode) return SyntaxDescriptor.BeforeMethodsDetails;
            if (selectedNode.Parent.GetChild(0) == selectedNode) return SyntaxDescriptor.BeforeMethodNameDefinition;
            if (selectedNode.Parent.GetChild(1) == selectedNode) return SyntaxDescriptor.BeforeMethodOpenParenthesis;
            if (selectedNode.Parent.GetChild(2) == selectedNode) return SyntaxDescriptor.BeforeParameterNameDefinition;
            if (selectedNode.Symbol.Text.Equals(",")) return SyntaxDescriptor.BeforeParameterNameDefinition;
            return SyntaxDescriptor.BeforeStartMethodDefinition;
        }
    }
}
