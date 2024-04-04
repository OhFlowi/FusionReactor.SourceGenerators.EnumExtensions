using FusionReactor.SourceGenerators.EnumExtensions.Models;
using System.Text;

using Microsoft.CodeAnalysis;

namespace FusionReactor.SourceGenerators.EnumExtensions.Generators.Strategies.Base;

public class GetValuesStrategy : IExtensionGeneratorStrategy
{
    public static IExtensionGeneratorStrategy Factory => new GetValuesStrategy();

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
              /// <summary>
              /// Retrieves all available values of the <see cref="{{enumDeclarationSyntax.Name}}"/>.
              /// </summary>
              /// <returns>An enumerable collection of <see cref="{{enumDeclarationSyntax.Name}}"/> values.</returns>
              #if NET8_0_OR_GREATER
              public static FrozenSet<{{enumDeclarationSyntax.Name}}> GetValues()
              #elif NET5_0_OR_GREATER
              public static IReadOnlySet<{{enumDeclarationSyntax.Name}}> GetValues()
              #else
              public static HashSet<{{enumDeclarationSyntax.Name}}> GetValues()
              #endif
              {
                return values;
              }

              /// <summary>
              /// Retrieves all available values of the <see cref="{{enumDeclarationSyntax.Name}}"/>.
              /// </summary>
              /// <param name="enumValue">The enumeration value.</param>
              /// <returns>An enumerable collection of <see cref="{{enumDeclarationSyntax.Name}}"/> values.</returns>
              #if NET8_0_OR_GREATER
              public static FrozenSet<{{enumDeclarationSyntax.Name}}> GetValues(this {{enumDeclarationSyntax.Name}} enumValue)
              #elif NET5_0_OR_GREATER
              public static IReadOnlySet<{{enumDeclarationSyntax.Name}}> GetValues(this {{enumDeclarationSyntax.Name}} enumValue)
              #else
              public static HashSet<{{enumDeclarationSyntax.Name}}> GetValues(this {{enumDeclarationSyntax.Name}} enumValue)
              #endif
              {
                return values;
              }
              """;
    }
}
