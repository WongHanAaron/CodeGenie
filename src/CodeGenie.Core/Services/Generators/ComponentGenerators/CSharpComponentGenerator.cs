using CodeGenie.Core.Models.ComponentDefinitions.Definitions;
using CodeGenie.Core.Models.Generation;
using CodeGenie.Core.Models.Generation.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Services.Generators.ComponentGenerators
{
    public class CSharpComponentGenerator : IComponentGenerator
    {
        public GeneratedComponentDefinition Generate(ComponentDefinition component)
        {
            return new GeneratedComponentDefinition()
            {
                Component = component
            };
        }
    }
}
