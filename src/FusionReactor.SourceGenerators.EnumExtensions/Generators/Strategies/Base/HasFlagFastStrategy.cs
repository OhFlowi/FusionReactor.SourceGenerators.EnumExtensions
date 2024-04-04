// <copyright file="HasFlagFastStrategy.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

namespace FusionReactor.SourceGenerators.EnumExtensions.Generators.Strategies.Base;

using System.Text;
using FusionReactor.SourceGenerators.EnumExtensions.Models;
using Microsoft.CodeAnalysis;

/// <inheritdoc />
public class HasFlagFastStrategy : IExtensionGeneratorStrategy
{
    /// <summary>
    /// Gets the factory instance of <see cref="HasFlagFastStrategy"/>.
    /// </summary>
    public static IExtensionGeneratorStrategy Factory => new HasFlagFastStrategy();

    /// <inheritdoc />
    public string GetMethod(IncrementalGeneratorInitializationContext context, EnumDefinition enumDeclarationSyntax)
    {
        if (!enumDeclarationSyntax.HasFlags)
        {
            return
                $$"""
                  /// <summary>
                  /// Always <see langword="false"/>, since the <see cref="{{enumDeclarationSyntax.Name}}" /> has not
                  /// the <see cref="FlagsAttribute" />.
                  /// </summary>
                  /// <param name="enumValue">The enum value to check.</param>
                  /// <param name="flag">The flag to check for.</param>
                  /// <returns><see langword="false"/>.</returns>
                  public static bool HasFlagFast(this {{enumDeclarationSyntax.Name}} enumValue, {{enumDeclarationSyntax.Name}} flag)
                  {
                    return false;
                  }
                  """;
        }

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
              /// Determines whether the {{enumDeclarationSyntax.Name}} value has the specified flag set.
              /// </summary>
              /// <param name="enumValue">The enum value to check.</param>
              /// <param name="flag">The flag to check for.</param>
              /// <returns>
              /// <see langword="true"/> if the enum value has the specified flag set; otherwise, <see langword="false"/>.
              /// </returns>
              public static bool HasFlagFast(this {{enumDeclarationSyntax.Name}} enumValue, {{enumDeclarationSyntax.Name}} flag)
              {
                return (enumValue & flag) == flag;
              }
              """;
    }
}
