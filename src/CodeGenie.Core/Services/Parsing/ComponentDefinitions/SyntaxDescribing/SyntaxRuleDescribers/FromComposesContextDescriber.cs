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
    public class FromComposesContextDescriber : SyntaxRuleDescriberBase<ComposesContext>
    {
        public override SyntaxDescriptor Describe(ComposesContext rule, ITerminalNode selectedNode, SyntaxSearchParameters searchParameters)
        {
            if (rule.children.FirstOrDefault() == selectedNode) return SyntaxDescriptor.BeforeComposesTargetDefinition;
            return SyntaxDescriptor.Unknown;
        }
    }
}
