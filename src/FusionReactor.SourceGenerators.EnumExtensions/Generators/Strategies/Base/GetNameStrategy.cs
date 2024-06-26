// <copyright file="GetNameStrategy.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

namespace FusionReactor.SourceGenerators.EnumExtensions.Generators.Strategies.Base;

using System.Text;
using FusionReactor.SourceGenerators.EnumExtensions.Models;
using Microsoft.CodeAnalysis;

/// <inheritdoc />
public class GetNameStrategy : IExtensionGeneratorStrategy
{
    /// <summary>
    /// Gets the factory instance of <see cref="GetNameStrategy"/>.
    /// </summary>
    public static IExtensionGeneratorStrategy Factory => new GetNameStrategy();

    /// <inheritdoc />
    public string GetMethod(IncrementalGeneratorInitializationContext context, EnumDefinition enumDeclarationSyntax)
    {
        var stringBuilder = new StringBuilder();

        foreach (var member in enumDeclarationSyntax.Members)
        {
            try
            {
                stringBuilder.AppendLine($"{enumDeclarationSyntax.Name}.{member.Name} => nameof({enumDeclarationSyntax.Name}.{member.Name}),");
            }
            catch (NullReferenceException)
            {
                // ignored
            }
        }

        return
            $$"""
              /// <summary>
              /// Retrieves the name of the constant in the <see cref="{{enumDeclarationSyntax.Name}}"/>.
              /// </summary>
              /// <param name="enumValue">The enum value to convert.</param>
              /// <returns>
              /// A string containing the name of the <see cref="{{enumDeclarationSyntax.Name}}"/>;
              /// or <see langword="null" /> if no such constant is found.
              /// </returns>
              public static string? GetName(this {{enumDeclarationSyntax.Name}} enumValue)
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
