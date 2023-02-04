using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using CodeGenie.Core.Models.ComponentDefinitions.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions.SyntaxDescribing.SyntaxRuleDescribers
{

    /// <summary>
    /// A class for containing a mapping between the parser rule type and the logic applied from that node 
    /// to identify the syntax state
    /// </summary>
    public interface ISyntaxRuleDescriber
    {
        Type ParserRuleType { get; }
        SyntaxDescription Describe(ParsingResult parsingResults, ParserRuleContext rule, ITerminalNode selectedNode, SyntaxSearchParameters searchParameters);
    }
}
