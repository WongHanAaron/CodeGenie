﻿using Antlr4.Runtime.Tree;
using CodeGenie.Core.Models.ComponentDefinitions.State;
using CodeGenie.Core.Models.ComponentDefinitions.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CodeGenieParser;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions.SyntaxDescribing.SyntaxRuleDescribers
{
    public class FromRelationshipsContextDescriber : SyntaxRuleDescriberBase<RelationshipsContext>
    {
        public override SyntaxDescriptor Describe(RelationshipsContext rule, ITerminalNode selectedNode, SyntaxSearchParameters searchParameters)
        {
            if (rule.children.FirstOrDefault() == selectedNode) return SyntaxDescriptor.BeforeRelationshipsDivider;
            if (rule.children.ElementAtOrDefault(1) != null &&
                rule.children.ElementAtOrDefault(1) == selectedNode &&
                selectedNode.Symbol.Text.Equals(":")) return SyntaxDescriptor.BeforeRelationshipsDetails;
            return SyntaxDescriptor.Unknown;
        }
    }
}
