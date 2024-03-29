﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Models.ComponentDefinitions.Definitions
{
    /// <summary> A definition for the method </summary>
    public class MethodDefinition
    {
        /// <summary> The scope of this method </summary>
        public Scope Scope { get; set; }

        /// <summary> The name of this method </summary>
        public string Name { get; set; }

        /// <summary> The name of the return type </summary>
        public string ReturnTypeName { get; set; }

        /// <summary> The Parameters to this method </summary>
        public List<ParameterDefinition> Parameters { get; set; } = new List<ParameterDefinition>();
    }
}
