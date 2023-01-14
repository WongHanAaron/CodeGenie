﻿using CodeGenie.Core.Models.ComponentDefinitions.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions
{
    public class ParsedComponentDefinition : ComponentDefinition
    {
        /// <summary> The parsed token for this component </summary>
        public ParsedToken ParsedToken { get; set; }
    }
}
