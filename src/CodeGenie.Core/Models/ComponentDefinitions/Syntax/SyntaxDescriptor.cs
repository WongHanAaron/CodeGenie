﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenie.Core.Models.ComponentDefinitions.Syntax
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
        BeforeAttributesDetails,
        BeforeStartAttributeDefinition,
        BeforeAttributeNameDefinition,
        BeforeAttributeDivider,
        BeforeAttributeTypeDefinition,
        BeforeAttributeDetails,
        // Method Definition Tree
        BeforeMethodsDetails,
        BeforeStartMethodDefinition,
        BeforeMethodNameDefinition,
        BeforeMethodOpenParenthesis,
        BeforeParameterNameDefinition,
        BeforeParameterDivider,
        BeforeParameterTypeDefinition,
        AfterParameterTypeDefinition,
        BeforeMethodReturnTypeDefinition,
        // Relationship Definition Tree
        BeforeStartRelationshipsDefinition,
        BeforeRelationshipsDetails,
        WithinRelationshipsDetails,
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
