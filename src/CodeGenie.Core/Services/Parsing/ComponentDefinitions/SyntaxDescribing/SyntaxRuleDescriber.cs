﻿using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CodeGenie.Core.Models.ComponentDefinitions.State;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions.SyntaxDescribing
{
    /// <summary>
    /// A class for containing a mapping between the parser rule type and the logic applied from that node 
    /// to identify the syntax state
    /// </summary>
    public interface ISyntaxRuleDescriber
    {
        Type ParserRuleType { get; }
        SyntaxDescriptor Describe(ParserRuleContext rule, ITerminalNode selectedNode);
    }

    public class SyntaxRuleDescriber<TRule> : ISyntaxRuleDescriber
    {
        public Type ParserRuleType { get; protected set; }

        public Func<TRule, ITerminalNode, SyntaxDescriptor> DescriptionMethod { get; protected set; }

        public SyntaxRuleDescriber(Type parserRuleType, Func<TRule, ITerminalNode, SyntaxDescriptor> descriptionMethod)
        {
            ParserRuleType = parserRuleType;
            DescriptionMethod = descriptionMethod;
        }

        public SyntaxDescriptor Describe(ParserRuleContext rule, ITerminalNode selectedNode)
        {
            if (rule is TRule tRule)
            {
                return DescriptionMethod?.Invoke(tRule, selectedNode) ?? SyntaxDescriptor.Unknown;
            }

            return SyntaxDescriptor.Unknown;
        }
    }
}