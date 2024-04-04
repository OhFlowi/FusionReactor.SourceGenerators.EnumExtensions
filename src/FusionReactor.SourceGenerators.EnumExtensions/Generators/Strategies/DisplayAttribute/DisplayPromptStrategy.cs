// <copyright file="DisplayPromptStrategy.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

namespace FusionReactor.SourceGenerators.EnumExtensions.Generators.Strategies.DisplayAttribute;

using System.Text;
using FusionReactor.SourceGenerators.EnumExtensions.Extensions;
using FusionReactor.SourceGenerators.EnumExtensions.Models;
using Microsoft.CodeAnalysis;

/// <inheritdoc />
public class DisplayPromptStrategy : IExtensionGeneratorStrategy
{
    /// <summary>
    /// Gets the factory instance of <see cref="DisplayPromptStrategy"/>.
    /// </summary>
    public static IExtensionGeneratorStrategy Factory => new DisplayPromptStrategy();

    /// <inheritdoc />
    public string GetMethod(IncrementalGeneratorInitializationContext context, EnumDefinition enumDeclarationSyntax)
    {
        var stringBuilder = new StringBuilder();

        foreach (var member in enumDeclarationSyntax.Members)
        {
            try
            {
                if (!member.ExtractStringValue("Prompt", out var value))
                {
                    stringBuilder.AppendLine($"{enumDeclarationSyntax.Name}.{member.Name} => null,");

                    continue;
                }

                stringBuilder.AppendLine($"{enumDeclarationSyntax.Name}.{member.Name} => \"{value}\",");
            }
            catch (NullReferenceException)
            {
                // ignored
            }
        }

        return
            $$"""
              /// <summary>
              /// Returns the <see cref="System.ComponentModel.DataAnnotations.DisplayAttribute.Prompt"/> of the <see cref="{{enumDeclarationSyntax.Name}}"/> enum.
              /// </summary>
              /// <param name="enumValue">The enum value.</param>
              /// <returns>The display name or the enum value.</returns>
              public static string? DisplayPrompt(this {{enumDeclarationSyntax.Name}} enumValue)
              {
                return enumValue switch
                {
                    {{stringBuilder}}
                    _ => null
                };
              }
              """;
    }
}
