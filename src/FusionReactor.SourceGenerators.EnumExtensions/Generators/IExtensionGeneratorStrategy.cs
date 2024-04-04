// <copyright file="IExtensionGeneratorStrategy.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

namespace FusionReactor.SourceGenerators.EnumExtensions.Generators;

using FusionReactor.SourceGenerators.EnumExtensions.Models;

using Microsoft.CodeAnalysis;

/// <summary>
/// Represents a strategy for generating method extensions.
/// </summary>
public interface IExtensionGeneratorStrategy
{
    /// <summary>
    /// Generates a method extension based on the provided initialization context and enum declaration syntax.
    /// </summary>
    /// <param name="context">The initialization context for the incremental generator.</param>
    /// <param name="enumDeclarationSyntax">The syntax representing the enum definition.</param>
    /// <returns>A string representing the generated method.</returns>
    string GetMethod(IncrementalGeneratorInitializationContext context, EnumDefinition enumDeclarationSyntax);
}
