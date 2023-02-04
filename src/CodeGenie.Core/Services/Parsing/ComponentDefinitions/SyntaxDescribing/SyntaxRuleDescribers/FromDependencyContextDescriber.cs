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
    public class FromDependencyContextDescriber : SyntaxRuleDescriberBase<DependencyContext>
    {
        public override SyntaxDescriptor Describe(ParsingResult parsingResults, DependencyContext rule, ITerminalNode selectedNode, SyntaxSearchParameters searchParameters)
        {
            if (rule.children.FirstOrDefault() == selectedNode) return SyntaxDescriptor.BeforeRelatedComponentNameDefinition;
            return SyntaxDescriptor.Unknown;
        }
    }
}
