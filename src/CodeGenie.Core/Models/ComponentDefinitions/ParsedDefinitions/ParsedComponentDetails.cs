using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions
{
    public class ParsedComponentDetails
    {
        public string Purpose { get; set; }
        public List<ParsedAttributeDefinition> Attributes { get; set; }
        public List<MethodDefinition> Methods { get; set; }
    }
}
