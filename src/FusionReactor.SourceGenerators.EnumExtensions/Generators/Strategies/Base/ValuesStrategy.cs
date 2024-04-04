// <copyright file="ValuesStrategy.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

namespace FusionReactor.SourceGenerators.EnumExtensions.Generators.Strategies.Base;

using System.Text;
using FusionReactor.SourceGenerators.EnumExtensions.Models;
using Microsoft.CodeAnalysis;

/// <inheritdoc />
public class ValuesStrategy : IExtensionGeneratorStrategy
{
    /// <summary>
    /// Gets the factory instance of <see cref="ValuesStrategy"/>.
    /// </summary>
    public static IExtensionGeneratorStrategy Factory => new ValuesStrategy();

    /// <inheritdoc />
    public string GetMethod(IncrementalGeneratorInitializationContext context, EnumDefinition enumDeclarationSyntax)
    {
        var stringBuilder = new StringBuilder();

        foreach (var member in enumDeclarationSyntax.Members)
        {
            try
            {
                stringBuilder.AppendLine($"{enumDeclarationSyntax.Name}.{member.Name},");
            }
            catch (NullReferenceException)
            {
                // ignored
            }
        }

        return
            $$"""
              #if NET8_0_OR_GREATER
              private static readonly FrozenSet<{{enumDeclarationSyntax.Name}}> values = new []
              {
                  {{stringBuilder}}
              }
              .ToFrozenSet();
              #elif NET5_0_OR_GREATER
              private static readonly IReadOnlySet<{{enumDeclarationSyntax.Name}}> values = new HashSet<{{enumDeclarationSyntax.Name}}>()
              {
                  {{stringBuilder}}
              };
              #else
              private static readonly HashSet<{{enumDeclarationSyntax.Name}}> values = new HashSet<{{enumDeclarationSyntax.Name}}>()
              {
                  {{stringBuilder}}
              };
              #endif

              """;
    }
}
