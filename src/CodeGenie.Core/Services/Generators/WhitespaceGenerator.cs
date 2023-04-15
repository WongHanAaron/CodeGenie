using CodeGenie.Core.Models.Generation.Contexts;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace CodeGenie.Core.Services.Generators
{
    /// <summary> Generates the white space between text and parts </summary>
    public interface IWhitespaceGenerator
    {
        string GenerateTabs(GenerationContext context);
    }

    public class WhitespaceGenerator : IWhitespaceGenerator
    {
        private const string _tab = "\t";

        public string GenerateTabs(GenerationContext context)
        {
            return new StringBuilder(_tab.Length * context.CurrentNumberOfTabs)
              .Insert(0, _tab, (int)context.CurrentNumberOfTabs)
              .ToString();
        }
    }
}
