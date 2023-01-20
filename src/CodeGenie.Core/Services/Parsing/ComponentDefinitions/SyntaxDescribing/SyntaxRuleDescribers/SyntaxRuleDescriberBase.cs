using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CodeGenie.Core.Models.ComponentDefinitions.State;
using CodeGenie.Core.Models.ComponentDefinitions.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions.SyntaxDescribing.SyntaxRuleDescribers
{
    public abstract class SyntaxRuleDescriberBase<TRule> : ISyntaxRuleDescriber
    {
        public Type ParserRuleType { get; protected set; }
        public SyntaxRuleDescriberBase() : this(typeof(TRule)) { }
        public SyntaxRuleDescriberBase(Type parserRuleType)
        {
            ParserRuleType = parserRuleType;
        }

        public virtual SyntaxDescriptor Describe(ParserRuleContext rule, ITerminalNode selectedNode, SyntaxSearchParameters searchParameters)
        {
            if (rule is TRule tRule)
            {
                return Describe(tRule, selectedNode, searchParameters);
            }

            return SyntaxDescriptor.Unknown;
        }

        public abstract SyntaxDescriptor Describe(TRule rule, ITerminalNode selectedNode, SyntaxSearchParameters searchParameters);
    }
}
