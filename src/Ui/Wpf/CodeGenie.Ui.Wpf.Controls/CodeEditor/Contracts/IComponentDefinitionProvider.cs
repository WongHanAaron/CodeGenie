using CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenie.Ui.Wpf.Controls.CodeEditor.Contracts
{
    /// <summary> The component that provides component definitions when they are updated </summary>
    public interface IComponentDefinitionProvider
    {
        /// <summary> Event handler for when a new set of component definitions is defined </summary>
        EventHandler<ParsingResult> OnComponentDefinitionsDefined { get; set; }

        /// <summary> Accesses the component definitions that are currently defined </summary>
        ParsingResult GetCurrentlyDefinedComponents();
    }
}
