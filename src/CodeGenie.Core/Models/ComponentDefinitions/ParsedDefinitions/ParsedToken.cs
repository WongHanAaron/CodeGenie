using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions
{
    /// <summary> The container class for containing the parsed token and location of that token </summary>
    public class ParsedToken
    {
        public IToken Token { get; set; }
        public int LineNumber { get; set; }
        public int ColumnIndex { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }

        public static TToken Create<TToken>(IToken token) where TToken : ParsedToken, new()
        {
            return new TToken()
            {
                Token = token,
                LineNumber = token.Line,
                ColumnIndex = token.Column,
                StartIndex = token.StartIndex,
                EndIndex = token.StopIndex
            };
        }

        public static TToken Create<TToken>(IToken start, IToken end) where TToken : ParsedToken, new()
        {
            return new TToken()
            {
                Token = start,
                LineNumber = start.Line,
                ColumnIndex = start.Column,
                StartIndex = start.StartIndex,
                EndIndex = end.StopIndex
            };
        }
        public static TToken Create<TToken>(ParsedToken token) where TToken : ParsedToken, new()
        {
            return new TToken()
            {
                Token = token.Token,
                LineNumber = token.LineNumber,
                ColumnIndex = token.ColumnIndex,
                StartIndex = token.StartIndex,
                EndIndex = token.EndIndex
            };
        }
    }
}
