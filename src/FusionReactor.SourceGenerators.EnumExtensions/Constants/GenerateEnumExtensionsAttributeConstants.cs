// <copyright file="GenerateEnumExtensionsAttributeConstants.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

namespace FusionReactor.SourceGenerators.EnumExtensions.Constants;

/// <summary>
/// Constants related to the "GenerateEnumExtensionsAttributeAttribute".
/// </summary>
public static class GenerateEnumExtensionsAttributeConstants
{
    /// <summary>
    /// The name of the EnumsFastAttribute.
    /// </summary>
    public const string Name = "GenerateEnumExtensions";

    /// <summary>
    /// The content of the EnumsFastAttribute.
    /// </summary>
    public const string Content =
        """
        // <auto-generated />

        using System;
        using System.CodeDom.Compiler;

        namespace FusionReactor.SourceGenerators.EnumExtensions;

        /// <summary>
        /// Attribute to mark an enum for FusionReactor.SourceGenerators.EnumExtensions extension generations.
        /// </summary>
        /// <remarks>
        /// This attribute is used to mark an enum for FusionReactor.SourceGenerators.EnumExtensions extension generations.
        /// </remarks>
        [GeneratedCode("FusionReactor.SourceGenerators.EnumExtensions", null)]
        [AttributeUsage(AttributeTargets.Enum)]
        public class GenerateEnumExtensionsAttribute : Attribute
        {
        }

        """;
}
