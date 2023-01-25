using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Models.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class AutoExcludeAttribute : Attribute
    {
        public bool Exclude { get; set; }
        public AutoExcludeAttribute() : this(true) { }
        public AutoExcludeAttribute(bool exclude)
        {
            Exclude = exclude;
        }
    }
}
