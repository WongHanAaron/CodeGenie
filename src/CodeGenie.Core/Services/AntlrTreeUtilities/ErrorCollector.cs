using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Services.AntlrTreeUtilities
{
    /// <summary> Component that collects if there are any errors in this rule's descendents </summary>
    public interface IErrorCollector
    {
        /// <summary> Checks if the rule has any descendent rule contexts with errors </summary>
        bool HasAnyErrorsInDescendents(ParserRuleContext ruleContext);
    }


    public class ErrorCollector : IErrorCollector
    {
        public bool HasAnyErrorsInDescendents(ParserRuleContext ruleContext)
            => HasAnyErrorsInDescendentsProc(ruleContext);

        public static bool HasAnyErrorsInDescendentsProc(ParserRuleContext ruleContext)
        {
            if (HasOrIsError(ruleContext)) return true;

            foreach (var child in ruleContext.children ?? new IParseTree[0])
            {
                if (HasOrIsError(child)) return true;
            }

            foreach (var child in ruleContext.children ?? new IParseTree[0])
            {
                if (child is ParserRuleContext parserRuleContext)
                {
                    if (HasAnyErrorsInDescendentsProc(parserRuleContext))
                        return true;
                }
            }

            return false;
        }

        public static bool HasOrIsError(ISyntaxTree treeNode)
        {
            var childType = treeNode.GetType();
            if (treeNode is ParserRuleContext parserRuleContext)
            {
                if (parserRuleContext.exception != null)
                    return true;
            }
            else if (treeNode is ErrorNodeImpl errorNode)
            {
                return true;
            }
            return false;
        }
    }
}
