using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Models.ComponentDefinitions.Syntax
{
    public class SyntaxSearchParameters
    {
        public SyntaxSearchParameters(int lineNumber, int columnNumber)
        {
            LineNumber = lineNumber;
            ColumnNumber = columnNumber;
        }

        public int LineNumber { get; protected set; }
        public int ColumnNumber { get; protected set; }
    }
}
