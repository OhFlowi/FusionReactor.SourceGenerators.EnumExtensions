// <copyright file="NamesStrategy.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

namespace FusionReactor.SourceGenerators.EnumExtensions.Generators.Strategies.Base;

using System.Text;
using FusionReactor.SourceGenerators.EnumExtensions.Models;
using Microsoft.CodeAnalysis;

/// <inheritdoc />
public class NamesStrategy : IExtensionGeneratorStrategy
{
    /// <summary>
    /// Gets the factory instance of <see cref="NamesStrategy"/>.
    /// </summary>
    public static IExtensionGeneratorStrategy Factory => new NamesStrategy();

    /// <inheritdoc />
    public string GetMethod(IncrementalGeneratorInitializationContext context, EnumDefinition enumDeclarationSyntax)
    {
        var stringBuilder = new StringBuilder();

        foreach (var member in enumDeclarationSyntax.Members)
        {
            try
            {
                stringBuilder.AppendLine($"\"{member.Name}\",");
            }
            catch (NullReferenceException)
            {
                // ignored
            }
        }

        return
            $$"""
               #if NET8_0_OR_GREATER
               private static readonly FrozenSet<string> names = new []
               {
                   {{stringBuilder}}
               }
               .ToFrozenSet();
               #elif NET5_0_OR_GREATER
               private static readonly IReadOnlySet<string> names = new HashSet<string>()
               {
                   {{stringBuilder}}
               };
               #else
               private static readonly HashSet<string> names = new HashSet<string>()
               {
                   {{stringBuilder}}
               };
               #endif

               """;
    }
}
