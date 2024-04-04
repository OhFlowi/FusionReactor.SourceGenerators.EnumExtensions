// <copyright file="TryParseStrategy.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

namespace FusionReactor.SourceGenerators.EnumExtensions.Generators.Strategies.Base;

using System.Text;
using FusionReactor.SourceGenerators.EnumExtensions.Models;
using Microsoft.CodeAnalysis;

/// <inheritdoc />
public class TryParseStrategy : IExtensionGeneratorStrategy
{
    /// <summary>
    /// Gets the factory instance of <see cref="TryParseStrategy"/>.
    /// </summary>
    public static IExtensionGeneratorStrategy Factory => new TryParseStrategy();

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
              /// Tries to parse the specified string representation of an enumeration value to its corresponding
              /// <see cref="{{enumDeclarationSyntax.Name}}"/> enumeration value.
              /// </summary>
              /// <param name="value">The string representation of the enumeration value.</param>
              /// <param name="result">
              /// When this method returns, contains the <see cref="{{enumDeclarationSyntax.Name}}"/> value equivalent
              /// to the string representation, if the parse succeeded, or default({{enumDeclarationSyntax.Name}}) if the parse failed.</param>
              /// <returns><c>true</c> if the parsing was successful; otherwise, <c>false</c>.</returns>
              public static bool TryParse(string value, out {{enumDeclarationSyntax.Name}}? result)
              {
                return TryParse(value, false, out result);
              }

              /// <summary>
              /// Tries to parse the specified string representation of an enumeration value to its corresponding
              /// <see cref="{{enumDeclarationSyntax.Name}}"/> enumeration value.
              /// </summary>
              /// <param name="value">The string representation of the enumeration value.</param>
              /// <param name="ignoreCase">A boolean indicating whether case should be ignored when parsing.</param>
              /// <param name="result">
              /// When this method returns, contains the <see cref="{{enumDeclarationSyntax.Name}}"/> value equivalent
              /// to the string representation, if the parse succeeded, or default({{enumDeclarationSyntax.Name}}) if the parse failed.</param>
              /// <returns><c>true</c> if the parsing was successful; otherwise, <c>false</c>.</returns>
              public static bool TryParse(string value, bool ignoreCase, out {{enumDeclarationSyntax.Name}}? result)
              {
                if(ignoreCase)
                  {
                      result = value.ToLowerInvariant() switch
                      {
                          {{ignoreCaseStringBuilder}}
                          _ => null,
                      };
                  }
                  else
                  {
                      result = value switch
                      {
                          {{stringBuilder}}
                          _ => null,
                      };
                  }

                  return result != null;
              }

              /// <summary>
              /// Tries to parse the specified string representation of an enumeration value to its corresponding
              /// <see cref="{{enumDeclarationSyntax.Name}}"/> enumeration value.
              /// </summary>
              /// <param name="enumValue">The enumeration value to parse.</param>
              /// <param name="value">The string representation of the enumeration value.</param>
              /// <param name="result">
              /// When this method returns, contains the <see cref="{{enumDeclarationSyntax.Name}}"/> value equivalent
              /// to the string representation, if the parse succeeded, or default({{enumDeclarationSyntax.Name}}) if the parse failed.</param>
              /// <returns><c>true</c> if the parsing was successful; otherwise, <c>false</c>.</returns>
              public static bool TryParse(this {{enumDeclarationSyntax.Name}} enumValue, string value, out {{enumDeclarationSyntax.Name}}? result)
              {
                return TryParse(value, false, out result);
              }

              /// <summary>
              /// Tries to parse the specified string representation of an enumeration value to its corresponding
              /// <see cref="{{enumDeclarationSyntax.Name}}"/> enumeration value.
              /// </summary>
              /// <param name="enumValue">The enumeration value to parse.</param>
              /// <param name="value">The string representation of the enumeration value.</param>
              /// <param name="ignoreCase">A boolean indicating whether case should be ignored when parsing.</param>
              /// <param name="result">
              /// When this method returns, contains the <see cref="{{enumDeclarationSyntax.Name}}"/> value equivalent
              /// to the string representation, if the parse succeeded, or default({{enumDeclarationSyntax.Name}}) if the parse failed.</param>
              /// <returns><c>true</c> if the parsing was successful; otherwise, <c>false</c>.</returns>
              public static bool TryParse(this {{enumDeclarationSyntax.Name}} enumValue, string value, bool ignoreCase, out {{enumDeclarationSyntax.Name}}? result)
              {
                if(ignoreCase)
                  {
                      result = value.ToLowerInvariant() switch
                      {
                          {{ignoreCaseStringBuilder}}
                          _ => null,
                      };
                  }
                  else
                  {
                      result = value switch
                      {
                          {{stringBuilder}}
                          _ => null,
                      };
                  }

                  return result != null;
              }

              """;
    }
}
