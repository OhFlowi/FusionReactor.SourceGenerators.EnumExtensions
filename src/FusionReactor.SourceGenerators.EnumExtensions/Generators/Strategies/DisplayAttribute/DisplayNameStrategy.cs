using FusionReactor.SourceGenerators.EnumExtensions.Extensions;
using FusionReactor.SourceGenerators.EnumExtensions.Models;
using System.Text;

using Microsoft.CodeAnalysis;

namespace FusionReactor.SourceGenerators.EnumExtensions.Generators.Strategies.DisplayAttribute;

/// <inheritdoc />
public class DisplayNameStrategy : IExtensionGeneratorStrategy
{
    /// <summary>
    /// Gets the factory instance of <see cref="DisplayNameStrategy"/>.
    /// </summary>
    public static IExtensionGeneratorStrategy Factory => new DisplayNameStrategy();

    /// <inheritdoc />
    public string GetMethod(IncrementalGeneratorInitializationContext context, EnumDefinition enumDeclarationSyntax)
    {
        var stringBuilder = new StringBuilder();

        foreach (var member in enumDeclarationSyntax.Members)
        {
            try
            {
                if (!member.ExtractStringValue("Name", out var value))
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
              /// Returns the <see cref="System.ComponentModel.DataAnnotations.DisplayAttribute.Name"/> of the <see cref="{{enumDeclarationSyntax.Name}}"/> enum.
              /// </summary>
              /// <param name="enumValue">The enum value.</param>
              /// <returns>The name or the enum value.</returns>
              public static string? DisplayName(this {{enumDeclarationSyntax.Name}} enumValue)
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
