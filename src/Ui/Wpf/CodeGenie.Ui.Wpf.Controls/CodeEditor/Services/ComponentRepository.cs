using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using CodeGenie.Ui.Wpf.Controls.CodeEditor.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Services
{
    /// <summary> The component for storing the defined components in this session </summary>
    public interface IComponentRepository : IComponentDefinitionProvider
    {
        /// <summary> Update the list of components within the repository </summary>
        void UpdateComponents(ParsingResult result);

        /// <summary> The components that were previously valid with no errors </summary>
        public ParsingResult LastValidComponents { get; }

        /// <summary> The components that are currently in the editor </summary>
        public ParsingResult CurrentComponents { get; }
    }

    public class ComponentRepository : IComponentRepository
    {
        public ParsingResult LastValidComponents { get; protected set; }

        public ParsingResult CurrentComponents { get; protected set; }

        public EventHandler<ParsingResult> OnComponentDefinitionsDefined { get; set; }

        public ParsingResult GetCurrentlyDefinedComponents() => CurrentComponents;

        public void UpdateComponents(ParsingResult result)
        {
            CurrentComponents = result;
            if (!CurrentComponents.HasErrors) LastValidComponents = result;
            OnComponentDefinitionsDefined?.Invoke(this, CurrentComponents);
        }
    }
}
