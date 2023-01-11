using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Models.Exceptions.ComponentValidation
{
    public class DuplicateNameException : Exception
    {
        public string DuplicatedComponentName { get; protected set; }
        public DuplicateNameException(string duplicatedComponentName)
        {
            DuplicatedComponentName = duplicatedComponentName;
        }
    }
}
