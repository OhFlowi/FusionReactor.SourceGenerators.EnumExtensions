// <copyright file="ParseFastStrategy.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

namespace FusionReactor.SourceGenerators.EnumExtensions.Generators.Strategies.Base;

using System.Text;
using FusionReactor.SourceGenerators.EnumExtensions.Models;
using Microsoft.CodeAnalysis;

/// <inheritdoc />
public class ParseFastStrategy : IExtensionGeneratorStrategy
{
    /// <summary>
    /// Gets the factory instance of <see cref="ParseFastStrategy"/>.
    /// </summary>
    public static IExtensionGeneratorStrategy Factory => new ParseFastStrategy();

    /// <inheritdoc />
    public string GetMethod(IncrementalGeneratorInitializationContext context, EnumDefinition enumDeclarationSyntax)
    {
        var ignoreCaseStringBuilder = new StringBuilder();
        var stringBuilder = new StringBuilder();

        foreach (var member in enumDeclarationSyntax.Members)
        {
            try
            {
                var name = enumDeclarationSyntax.Name + "." + member.Name;

                ignoreCaseStringBuilder.AppendLine($"\"{member.Name.ToLowerInvariant()}\" => {enumDeclarationSyntax.Name}.{member.Name},");
                stringBuilder.AppendLine($"\"{member.Name}\" => {enumDeclarationSyntax.Name}.{member.Name},");
            }
            catch (NullReferenceException)
            {
                // ignored
            }
        }

        return
            $$"""
              /// <summary>
              /// Parses the specified string representation of the enumeration value to its corresponding
              /// <see cref="{{enumDeclarationSyntax.Name}}"/> value.
              /// </summary>
              /// <param name="value">A string containing the name or value to convert.</param>
              /// <param name="ignoreCase">
              /// A boolean indicating whether to ignore case during the parsing. Default is <c>false</c>.
              /// </param>
              /// <returns>
              /// The <see cref="{{enumDeclarationSyntax.Name}}"/> value equivalent to the specified string representation.
              /// </returns>
              public static {{enumDeclarationSyntax.Name}} ParseFast(string value, bool ignoreCase = false)
              {
                if(ignoreCase)
                {
                    return value.ToLowerInvariant() switch
                    {
                        {{ignoreCaseStringBuilder}}
                        _ => throw new ArgumentException(),
                    };
                }
                else
                {
                    return value switch
                    {
                        {{stringBuilder}}
                        _ => throw new ArgumentException(),
                    };
                }
              }

              /// <summary>
              /// Parses the specified string representation of the enumeration value to its corresponding
              /// <see cref="{{enumDeclarationSyntax.Name}}"/> value.
              /// </summary>
              /// <param name="enumValue">The current <see cref="{{enumDeclarationSyntax.Name}}"/> value.</param>
              /// <param name="value">A string containing the name or value to convert.</param>
              /// <param name="ignoreCase">
              /// A boolean indicating whether to ignore case during the parsing. Default is <c>false</c>.
              /// </param>
              /// <returns>
              /// The <see cref="{{enumDeclarationSyntax.Name}}"/> value equivalent to the specified string representation.
              /// </returns>
              public static {{enumDeclarationSyntax.Name}} ParseFast(this {{enumDeclarationSyntax.Name}} enumValue, string value, bool ignoreCase = false)
              {
                  if(ignoreCase)
                  {
                      return value.ToLowerInvariant() switch
                      {
                          {{ignoreCaseStringBuilder}}
                          _ => throw new ArgumentException(),
                      };
                  }
                  else
                  {
                      return value switch
                      {
                          {{stringBuilder}}
                          _ => throw new ArgumentException(),
                      };
                  }
              }
              """;
    }
}
