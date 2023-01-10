using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Services.Parsing.ComponentDefinitions
{
    /// <summary>
    /// The software component responsible for compiling and validating the 
    /// components
    /// </summary>
    public interface IComponentCompiler
    {
        ParsingResult Compile(string script);
    }

    public class ComponentCompiler : IComponentCompiler
    {


        public ParsingResult Compile(string script)
        {
            return null;
        }
    }
}
