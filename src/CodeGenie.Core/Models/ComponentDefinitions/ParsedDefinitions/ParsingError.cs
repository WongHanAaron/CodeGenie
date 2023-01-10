using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions
{
    /// <summary> The erroneously parsed token or location </summary>
    public class ScriptError : ParsedToken
    {
        public ScriptError() { }
        public Exception Exception { get; set; }
    }
}
