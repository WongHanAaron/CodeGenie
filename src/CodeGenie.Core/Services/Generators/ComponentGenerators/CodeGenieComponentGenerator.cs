using CodeGenie.Core.Models.ComponentDefinitions.Definitions;
using CodeGenie.Core.Models.Generation;
using CodeGenie.Core.Models.Generation.Contexts;
using CodeGenie.Core.Models.Generation.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Services.Generators.ComponentGenerators
{
    public class CodeGenieComponentGenerator : IComponentGenerator
    {
        protected readonly IWhitespaceGenerator _whitespaceGenerator;
        public CodeGenieComponentGenerator(IWhitespaceGenerator whitespaceGenerator) 
        {
            _whitespaceGenerator = whitespaceGenerator;
        }

        public GeneratedComponentDefinition Generate(ComponentDefinition component)
        {
            var context = new GenerationContext();
            throw new NotImplementedException();
        }

        /// <summary> Generate a component definition </summary>
        public string GenerateComponentDefinition(GenerationContext context, ComponentDefinition component)
        {
            return _whitespaceGenerator.GenerateTabs(context) + GetDefinition(component.Scope, component.Name, GetComponentType(component.IsInterface));
        }

        /// <summary> Generate a generic definition. Usually of format "[Scope] [DefinitionName] : [DefinitionType]"</summary>
        public string GetDefinition(Scope scope, string definitionName, string definitionType)
            => $"{GetScope(scope)} {definitionName} : {definitionType}";

        public string GetScope(Scope scope)
        {
            switch (scope)
            {
                case Scope.Public:
                    return "+";
                case Scope.Protected:
                    return "#";
                case Scope.Private:
                    return "-";
            }

            throw new ArgumentException($"The scope is '{scope}' and is invalid");
        }

        public string GetComponentType(bool isInterface)
            => isInterface ? "interface" : "class";
    }
}
