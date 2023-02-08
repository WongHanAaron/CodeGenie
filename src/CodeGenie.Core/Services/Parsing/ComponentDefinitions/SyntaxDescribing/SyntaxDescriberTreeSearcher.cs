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

            if (!terminalNodes.Any()) return null;

            var closestNode = FindNodeAtLineAndColumn(terminalNodes.ToArray(), lineNumber, columnNumber);

            return closestNode;
        }

        protected ITerminalNode FindNodeAtLineAndColumn(ITerminalNode[] terminalNodes, int lineNumber, int columnNumber)
        {
            var first = terminalNodes.FirstOrDefault();
            var last = terminalNodes.LastOrDefault();

            if (GetLineNumber(first) > lineNumber) return first;
            if (GetLineNumber(last) < lineNumber) return last;

            return FindNodeAtLineAndColumn(terminalNodes, 0, terminalNodes.Count() - 1, lineNumber, columnNumber);
        }

        protected ITerminalNode FindNodeAtLineAndColumn(ITerminalNode[] terminalNodes, int startIndex, int endIndex, int lineNumber, int columnNumber)
        {
            if (endIndex < 0) return terminalNodes.FirstOrDefault();
            
            var midIndex = (int)((endIndex - startIndex) / 2.0) + startIndex;

            var span = terminalNodes.Skip(startIndex).Take((endIndex - startIndex));
            if (span.Count() <= 2 && span.Any())
            {
                var indexFromSpan = FindNodeClosestToDesiredLine(span, lineNumber);
                return SearchForClosestTokenInSameLine(terminalNodes, startIndex + indexFromSpan, lineNumber, columnNumber);
            }


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

        /// <summary>
        /// Search for both the token that encapsulates the column number or the token
        /// that is closest to that column number. If none encapsulates it, go with
        /// the token that is closest
        /// </summary>
        protected ITerminalNode SearchForClosestTokenInSameLine(ITerminalNode[] terminalNodes, int indexOfNodeOnLine, int lineNumberToKeepOn, int searchedColumnNumber)
        {
            var minIndex = indexOfNodeOnLine;
            var minNode = terminalNodes[indexOfNodeOnLine];
            var minNodeColumnDistance = GetDistanceFromDesiredColumn(minNode, searchedColumnNumber);
            ITerminalNode capturingToken = null;
            if (ColumnNumberIsOnToken(minNode, searchedColumnNumber)) 
                capturingToken = minNode;

            if (capturingToken != null) return capturingToken;

            // Start at index and look backwards continually looking for a node that might have the smallest distance to the desired columnNumber
            for (int i = indexOfNodeOnLine - 1; i >= 0; i--)
            {
                var node = terminalNodes[i];

                var line = GetLineNumber(node);
                if (line != lineNumberToKeepOn) break;

                if (ColumnNumberIsOnToken(node, searchedColumnNumber))
                    capturingToken = node;

                if (capturingToken != null) break;

                var distance = GetDistanceFromDesiredColumn(node, searchedColumnNumber);
                var start = GetColumnNumberStart(node);
                var end = GetColumnNumberEnd(node);
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

                if (ColumnNumberIsOnToken(node, searchedColumnNumber))
                    capturingToken = node;

                if (capturingToken != null) break;


                var distance = GetDistanceFromDesiredColumn(node, searchedColumnNumber);
                if (distance < minNodeColumnDistance)
                {
                    minNode = node;
                    minNodeColumnDistance = distance;
                    minIndex = i;
                }
            }

            return capturingToken != null ? capturingToken : minNode;
        }

        protected bool ColumnNumberIsOnToken(ITerminalNode node, int columnNumber)
        {
            var start = GetColumnNumberStart(node);
            var end = GetColumnNumberEnd(node);
            return start <= columnNumber && end >= columnNumber;
        }

        protected int GetDistanceFromDesiredColumn(ITerminalNode node, int columnNumber)
        {
            return (int) Math.Min(Math.Abs(GetColumnNumberStart(node) - columnNumber), Math.Abs(GetColumnNumberEnd(node) - columnNumber));
        }

        protected int FindNodeClosestToDesiredLine(IEnumerable<ITerminalNode> nodes, int desiredLineNumber)
        {
            var minIndex = 0;
            var minDistance = GetDistanceFromDesiredLineNumber(nodes.ElementAtOrDefault(minIndex), desiredLineNumber);

            for (int i = 0; i < nodes.Count(); i++)
            {
                var distance = GetDistanceFromDesiredLineNumber(nodes.ElementAtOrDefault(i), desiredLineNumber);
                if (distance < minDistance)
                {
                    minIndex = i;
                }
            }
            return minIndex;
        }

        protected int GetDistanceFromDesiredLineNumber(ITerminalNode node, int lineNumber)
            => (int)Math.Abs(GetLineNumber(node) - lineNumber);

        protected int GetLineNumber(ITerminalNode node) => node.Symbol.Line;
        protected int GetColumnNumberStart(ITerminalNode node) => node.Symbol.Column;
        protected int GetColumnNumberEnd(ITerminalNode node) => node.Symbol.Column + node.Symbol.Text.Count() - 1;
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
