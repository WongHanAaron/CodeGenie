using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Models.ComponentDefinitions
{
    /// <summary> A class or interface </summary>
    public class ComponentDefinition
    {
        /// <summary> The name of this class </summary>
        public string Name { get; set; }

        /// <summary> The scope of this class </summary>
        public Scope Scope { get; set; }

        /// <summary> Whether this component is an interface or not </summary>
        public bool IsInterface { get; set; }

        /// <summary> The purpose of this component </summary>
        public string Purpose { get; set; }

        /// <summary> The attributes that this component has </summary>
        public List<AttributeDefinition> Attributes { get; set; } = new List<AttributeDefinition>();
        
        /// <summary>  The methods this component has </summary>
        public List<MethodDefinition> MethodDefinition { get; set; } = new List<MethodDefinition>();
    }
}
