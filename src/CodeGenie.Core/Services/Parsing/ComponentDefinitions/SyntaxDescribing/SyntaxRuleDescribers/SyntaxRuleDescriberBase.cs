using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using CodeGenie.Core.Models.ComponentDefinitions.Syntax;
using CodeGenie.Core.Services.AntlrTreeUtilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions.SyntaxDescribing.SyntaxRuleDescribers
{
    public abstract class SyntaxRuleDescriberBase<TRule> : ISyntaxRuleDescriber where TRule : ParserRuleContext
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

            var selectedRuleHasError = false;

            if (rule is TRule tRule)
            {
                selectedRuleHasError = ErrorCollector.HasAnyErrorsInDescendentsProc(rule);
                descriptor = Describe(parsingResults, tRule, selectedNode, searchParameters);
            }

            return SyntaxDescription.Create(parsingResults, descriptor, selectedRuleHasError);
        }

        public abstract SyntaxDescriptor Describe(ParsingResult parsingResults, TRule rule, ITerminalNode selectedNode, SyntaxSearchParameters searchParameters);
    }
}
