// <copyright file="GetNamesStrategy.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

namespace FusionReactor.SourceGenerators.EnumExtensions.Generators.Strategies.Base;

using FusionReactor.SourceGenerators.EnumExtensions.Models;
using Microsoft.CodeAnalysis;

/// <inheritdoc />
public class GetNamesStrategy : IExtensionGeneratorStrategy
{
    /// <summary>
    /// Gets the factory instance of <see cref="GetNamesStrategy"/>.
    /// </summary>
    public static IExtensionGeneratorStrategy Factory => new GetNamesStrategy();

    /// <inheritdoc />
    public string GetMethod(IncrementalGeneratorInitializationContext context, EnumDefinition enumDeclarationSyntax) => $$"""
               /// <summary>
               /// Retrieves all available names of the <see cref="{{enumDeclarationSyntax.Name}}"/>.
               /// </summary>
               /// <returns>An enumerable collection of <see cref="{{enumDeclarationSyntax.Name}}"/> names.</returns>
               #if NET8_0_OR_GREATER
               public static FrozenSet<string> GetNames()
               #elif NET5_0_OR_GREATER
               public static IReadOnlySet<string> GetNames()
               #else
               public static HashSet<string> GetNames()
               #endif
               {
                 return names;
               }

               /// <summary>
               /// Retrieves all available names of the <see cref="{{enumDeclarationSyntax.Name}}"/>.
               /// </summary>
               /// <param name="enumValue">The enumeration value.</param>
               /// <returns>An enumerable collection of <see cref="{{enumDeclarationSyntax.Name}}"/> names.</returns>
               #if NET8_0_OR_GREATER
               public static FrozenSet<string> GetNames(this {{enumDeclarationSyntax.Name}} enumValue)
               #elif NET5_0_OR_GREATER
               public static IReadOnlySet<string> GetNames(this {{enumDeclarationSyntax.Name}} enumValue)
               #else
               public static HashSet<string> GetNames(this {{enumDeclarationSyntax.Name}} enumValue)
               #endif
               {
                 return names;
               }
               """;
}
