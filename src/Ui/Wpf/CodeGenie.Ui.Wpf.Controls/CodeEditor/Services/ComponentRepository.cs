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
    public interface IComponentRepository
    {
        /// <summary> Update the list of components within the repository </summary>
        void UpdateComponents(ParsingResult result);

        /// <summary> The components that were previously valid with no errors </summary>
        public ParsingResult LastValidComponents { get; }

        /// <summary> The components that are currently in the editor </summary>
        public ParsingResult CurrentComponents { get; }
    }

    public class ComponentRepository : IComponentDefinitionProvider, IComponentRepository
    {
        public ParsingResult LastValidComponents => throw new NotImplementedException();

        public ParsingResult CurrentComponents => throw new NotImplementedException();

        public EventHandler<ParsingResult> OnComponentDefinitionsDefined { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ParsingResult GetCurrentlyDefinedComponents()
        {
            throw new NotImplementedException();
        }

        public void UpdateComponents(ParsingResult result)
        {
            throw new NotImplementedException();
        }
    }
}
