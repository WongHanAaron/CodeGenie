using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Models.ComponentDefinitions.Definitions
{
    public enum RelationshipType
    {
        Dependency,
        Aggregation,
        Composition,
        Realization,
        Specialization
    }
}
