﻿using CodeGenie.Core.Models.ComponentDefinitions.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions
{
    public class ParsedComponentDetails
    {
        public string Purpose { get; set; }
        public List<ParsedAttributeDefinition> Attributes { get; set; }
        public List<ParsedMethodDefinition> Methods { get; set; }
        public List<ParsedRelationshipDefinition> Relationships { get; set; }
        public List<string> Tags { get; set; }
    }
}
