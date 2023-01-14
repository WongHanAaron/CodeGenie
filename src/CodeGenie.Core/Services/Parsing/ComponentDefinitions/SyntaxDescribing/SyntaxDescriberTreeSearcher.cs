using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions.SyntaxDescribing
{
    /// <summary> Component responsible for traversing the syntax tree </summary>
    public interface ISyntaxDescriberTreeSearcher
    {
        ITerminalNode GetClosestNode(CodeGenieParser.ComponentDefinitionContext contextDefinitionContext, int lineNumber, int columnNumber);
    }

    public class SyntaxDescriberTreeSearcher : ISyntaxDescriberTreeSearcher
    {
        public ITerminalNode GetClosestNode(CodeGenieParser.ComponentDefinitionContext contextDefinitionContext, int lineNumber, int columnNumber)
        {
            var terminalNodes = new List<ITerminalNode>();

            TerminalNodeCollection(contextDefinitionContext, terminalNodes);

            var closestNode = FindNodeAtLineAndColumn(terminalNodes.ToArray(), lineNumber, columnNumber);

            return closestNode;
        }

        protected ITerminalNode FindNodeAtLineAndColumn(ITerminalNode[] terminalNodes, int lineNumber, int columnNumber)
            => FindNodeAtLineAndColumn(terminalNodes, 0, terminalNodes.Count() - 1, lineNumber, columnNumber);

        protected ITerminalNode FindNodeAtLineAndColumn(ITerminalNode[] terminalNodes, int startIndex, int endIndex, int lineNumber, int columnNumber)
        {
            var midIndex = (int)Math.Floor((endIndex - startIndex) / 2.0);

            var midLineNumber = GetLineNumber(terminalNodes[midIndex]);
            if (lineNumber == midLineNumber)
            {
                return SearchForClosestTokenInSameLine(terminalNodes, midIndex, lineNumber, columnNumber);
            }
            else if (lineNumber < midLineNumber)
            {
                return FindNodeAtLineAndColumn(terminalNodes, startIndex, midIndex, lineNumber, columnNumber);
            }
            else
            {
                return FindNodeAtLineAndColumn(terminalNodes, midIndex, endIndex, lineNumber, columnNumber);
            }
        }

        protected ITerminalNode SearchForClosestTokenInSameLine(ITerminalNode[] terminalNodes, int indexOfNodeOnLine, int lineNumberToKeepOn, int searchedColumnNumber)
        {
            var minIndex = indexOfNodeOnLine;
            var minNode = terminalNodes[indexOfNodeOnLine];
            var minNodeColumnDistance = GetDistanceFromDesiredColumn(minNode, searchedColumnNumber);

            // Start at index and look backwards continually looking for a node that might have the smallest distance to the desired columnNumber
            for (int i = indexOfNodeOnLine - 1; i >= 0; i--)
            {
                var node = terminalNodes[i];
                var line = GetLineNumber(node);
                if (line != lineNumberToKeepOn) break;

                var distance = GetDistanceFromDesiredColumn(node, searchedColumnNumber);
                if (distance < minNodeColumnDistance)
                {
                    minNode = node;
                    minNodeColumnDistance = distance;
                    minIndex = i;
                }
            }

            for (int i = indexOfNodeOnLine + 1; i < terminalNodes.Count(); i++)
            {
                var node = terminalNodes[i];
                var line = GetLineNumber(node);
                if (line != lineNumberToKeepOn) break;

                var distance = GetDistanceFromDesiredColumn(node, searchedColumnNumber);
                if (distance < minNodeColumnDistance)
                {
                    minNode = node;
                    minNodeColumnDistance = distance;
                    minIndex = i;
                }
            }

            return minNode;
        }

        protected int GetDistanceFromDesiredColumn(ITerminalNode node, int columnNumber)
        {
            return (int) Math.Abs(GetColumnNumber(node) - columnNumber);
        }

        protected int GetLineNumber(ITerminalNode node) => node.Symbol.Line;
        protected int GetColumnNumber(ITerminalNode node) => node.Symbol.Column;
        protected void TerminalNodeCollection(IParseTree root, List<ITerminalNode> collectedNodes)
        {
            // Collect if a leaf
            if (root.ChildCount == 0 && root is ITerminalNode terminal)
            {
                collectedNodes.Add(terminal);
            }

            // Traverse from left to right
            for (int i = 0; i < root.ChildCount; i++)
            {
                var child = root.GetChild(i);
                TerminalNodeCollection(child, collectedNodes);
            }
        }

    }
}
