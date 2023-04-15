using CodeGenie.Core.Models.ComponentDefinitions.Definitions;
using CodeGenie.Core.Models.Generation;
using CodeGenie.Core.Models.Generation.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Services.Generators.ComponentGenerators
{
    /// <summary> The component generator interface for creating the code for a component from a ComponentDefinition </summary>
    public interface IComponentGenerator
    {
        /// <summary> The method to generate the code for a component </summary>
        GeneratedComponentDefinition Generate(ComponentDefinition component);
    }
}
