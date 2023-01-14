using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Models.ComponentDefinitions.State
{
    /// <summary> A description of where a location is in a syntax tree </summary>
    public enum SyntaxDescriptor
    {
        Unknown = 0,
        Error,
        // Component Definition Tree
        BeforeStartComponentDefinition,
        BeforeComponentNameDefinition,
        BeforeComponentDivider,
        BeforeComponentTypeDefinition,
        BeforeComponentDetails,
        WithinComponentDetails,
        // Attribute Definition Tree
        BeforeStartAttributeDefinition,
        BeforeAttributeNameDefinition,
        BeforeAttributeDivider,
        BeforeAttributeTypeDefinition,
        BeforeAttributeDetails,
        // Method Definition Tree
        BeforeStartMethodDefinition,
        BeforeMethodNameDefinition,
        BeforeMethodParenthesis,
        BeforeParameterNameDefinition,
        BeforeParameterDivider,
        BeforeParameterTypeDefinition,
        BeforeNextParameterDivider,
        BeforeMethodReturnTypeDefinition,
        // Relationship Definition Tree
        BeforeStartRelationshipDefinition,
        BeforeRelatedComponentNameDefinition,
        BeforeDependencyDetails,
        BeforeComposesDetails,
        BeforeAggregatesDetails,
        BeforeRealizesDetails,
        BeforeSpecializesDetails,
        // Shared Definition Trees 
        // - Purpose Definition
        BeforePurposeDefinitionDivider,
        // - Tags Definition
        BeforeTagsOpenBrace,
        BeforeTagsDivider,
        // - Cardinality Definition
        BeforeCardinalityDefinitionDivider,
        BeforeCardinalityDivider
    }
}
