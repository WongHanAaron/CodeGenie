using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Models.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class AutoIncludeAttribute : Attribute
    {
        public bool Include { get; set; }
        public AutoIncludeAttribute() : this(true) { }
        public AutoIncludeAttribute(bool include)
        {
            Include = include;
        }
    }
}
