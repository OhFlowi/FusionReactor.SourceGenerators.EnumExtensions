// <copyright file="GetContentStrategy.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

namespace FusionReactor.SourceGenerators.EnumExtensions.Generators.Strategies.Base;

using FusionReactor.SourceGenerators.EnumExtensions.Models;
using Microsoft.CodeAnalysis;

/// <inheritdoc />
public class GetContentStrategy : IExtensionGeneratorStrategy
{
    /// <summary>
    /// Gets the factory instance of <see cref="GetContentStrategy"/>.
    /// </summary>
    public static IExtensionGeneratorStrategy Factory => new GetContentStrategy();

    /// <inheritdoc />
    public string GetMethod(IncrementalGeneratorInitializationContext context, EnumDefinition enumDeclarationSyntax)
        => $$"""
              /// <summary>
              /// Gets the content dictionary containing mappings of <see cref="{{enumDeclarationSyntax.Name}}"/> enum values to values.
              /// </summary>
              /// <returns>The read-only content dictionary.</returns>
              #if NET8_0_OR_GREATER
              public static FrozenDictionary<{{enumDeclarationSyntax.Name}}, {{enumDeclarationSyntax.UnderlyingType}}> GetContent()
              #else
              public static IReadOnlyDictionary<{{enumDeclarationSyntax.Name}}, {{enumDeclarationSyntax.UnderlyingType}}> GetContent()
              #endif
              {
                return content;
              }

              /// <summary>
              /// Gets the content dictionary containing mappings of <see cref="{{enumDeclarationSyntax.Name}}"/> enum values to values.
              /// </summary>
              /// <param name="enumValue">The enum value for which to get the content dictionary.</param>
              /// <returns>The read-only content dictionary.</returns>
              #if NET8_0_OR_GREATER
              public static FrozenDictionary<{{enumDeclarationSyntax.Name}}, {{enumDeclarationSyntax.UnderlyingType}}> GetContent(this {{enumDeclarationSyntax.Name}} enumValue)
              #else
              public static IReadOnlyDictionary<{{enumDeclarationSyntax.Name}}, {{enumDeclarationSyntax.UnderlyingType}}> GetContent(this {{enumDeclarationSyntax.Name}} enumValue)
              #endif
              {
                return content;
              }
              """;
}
