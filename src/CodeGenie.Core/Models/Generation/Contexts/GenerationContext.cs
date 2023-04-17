using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Models.Generation.Contexts
{
    /// <summary> The context data for the current generation process </summary>
    public class GenerationContext
    {
        /// <summary> The current number of tabs that the previous line was writing from </summary>
        public int CurrentNumberOfTabs { get; set; } = 0;

        /// <summary> The content builder for this generation </summary>
        public StringBuilder ContentBuilder { get; set; } = new StringBuilder();
    }
}
