using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Models.ComponentDefinitions.ParsedDefinitions
{
    public class ParsedRelationshipDetails
    {
        public string Purpose { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}
