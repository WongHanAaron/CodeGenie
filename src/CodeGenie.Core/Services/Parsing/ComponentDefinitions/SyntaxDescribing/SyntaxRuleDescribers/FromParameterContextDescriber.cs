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
    public class FromParameterContextDescriber : SyntaxRuleDescriberBase<ParameterContext>
    {
        public override SyntaxDescriptor Describe(ParsingResult parsingResults, ParameterContext rule, ITerminalNode selectedNode, SyntaxSearchParameters searchParameters)
        {
            if (rule.GetChild(0) == selectedNode) return SyntaxDescriptor.BeforeParameterDivider;
            if (rule.GetChild(1) == selectedNode) return SyntaxDescriptor.BeforeParameterTypeDefinition;
            if (selectedNode.Parent.GetChild(0) == selectedNode) return SyntaxDescriptor.AfterParameterTypeDefinition;
            return SyntaxDescriptor.BeforeParameterNameDefinition;
        }
    }
}
