﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Models.ComponentDefinitions.Definitions
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
        public List<MethodDefinition> MethodDefinitions { get; set; } = new List<MethodDefinition>();

        /// <summary> The relationships this component has </summary>
        public List<RelationshipDefinition> RelationshipDefinitions { get; set; } = new List<RelationshipDefinition>();

        /// <summary> A list of descriptional tags on this component </summary>
        public List<string> Tags { get; set; }
    }
}
