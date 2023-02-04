using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
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

        public virtual SyntaxDescription Describe(ParsingResult parsingResults, ParserRuleContext rule, ITerminalNode selectedNode, SyntaxSearchParameters searchParameters)
        {
            var descriptor = SyntaxDescriptor.Unknown;
            
            if (rule is TRule tRule)
            {
                descriptor = Describe(parsingResults, tRule, selectedNode, searchParameters);
            }

            return new SyntaxDescription()
            {
                ParsedResult = parsingResults,
                SyntaxDescriptorAtCaret = descriptor
            };
        }

        public abstract SyntaxDescriptor Describe(ParsingResult parsingResults, TRule rule, ITerminalNode selectedNode, SyntaxSearchParameters searchParameters);
    }
}
