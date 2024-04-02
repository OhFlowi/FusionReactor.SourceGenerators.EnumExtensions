using FusionReactor.SourceGenerators.EnumExtensions.Extensions;
using FusionReactor.SourceGenerators.EnumExtensions.Models;
using System.Text;

using Microsoft.CodeAnalysis;

namespace FusionReactor.SourceGenerators.EnumExtensions.Generators.Strategies.DisplayAttribute;

/// <inheritdoc />
public class DisplayOrderStrategy : IExtensionGeneratorStrategy
{
    /// <summary>
    /// Gets the factory instance of <see cref="DisplayOrderStrategy"/>.
    /// </summary>
    public static IExtensionGeneratorStrategy Factory => new DisplayOrderStrategy();

    /// <inheritdoc />
    public string GetMethod(IncrementalGeneratorInitializationContext context, EnumDefinition enumDeclarationSyntax)
    {
        var stringBuilder = new StringBuilder();

        foreach (var member in enumDeclarationSyntax.Members)
        {
            try
            {
                if (!member.ExtractIntValue("Order", out var value))
                {
                    stringBuilder.AppendLine($"{enumDeclarationSyntax.Name}.{member.Name} => null,");

                    continue;
                }

                stringBuilder.AppendLine($"{enumDeclarationSyntax.Name}.{member.Name} => {value},");
            }
            catch (NullReferenceException)
            {
                // ignored
            }
        }

        return
            $$"""
              /// <summary>
              /// Returns the <see cref="System.ComponentModel.DataAnnotations.DisplayAttribute.Order"/> of the <see cref="{{enumDeclarationSyntax.Name}}"/> enum.
              /// </summary>
              /// <param name="enumValue">The enum value.</param>
              /// <returns>The display name or the enum value.</returns>
              public static int? DisplayOrder(this {{enumDeclarationSyntax.Name}} enumValue)
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
