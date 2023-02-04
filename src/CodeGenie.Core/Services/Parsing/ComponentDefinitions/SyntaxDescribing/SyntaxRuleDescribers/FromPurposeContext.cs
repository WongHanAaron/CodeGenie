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
    public class FromPurposeContext : SyntaxRuleDescriberBase<PurposeContext>
    {
        public override SyntaxDescriptor Describe(ParsingResult parsingResults, PurposeContext rule, ITerminalNode selectedNode, SyntaxSearchParameters searchParameters)
        {
            if (selectedNode.Symbol.Text.EndsWith("\"") && searchParameters.ColumnNumber >
                selectedNode.Symbol.Column + selectedNode.Symbol.Text.Count() - 1) return SyntaxDescriptor.WithinComponentDetails;
            if (rule.children.FirstOrDefault() == selectedNode) return SyntaxDescriptor.BeforePurposeDefinitionDivider;
            return SyntaxDescriptor.Unknown;
        }
    }
}
