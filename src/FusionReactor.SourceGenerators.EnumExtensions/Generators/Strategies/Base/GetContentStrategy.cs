using FusionReactor.SourceGenerators.EnumExtensions.Models;
using System.Text;

using Microsoft.CodeAnalysis;

namespace FusionReactor.SourceGenerators.EnumExtensions.Generators.Strategies.Base;

public class GetContentStrategy : IExtensionGeneratorStrategy
{
    public static IExtensionGeneratorStrategy Factory => new GetContentStrategy();

    /// <inheritdoc />
    public string GetMethod(IncrementalGeneratorInitializationContext context, EnumDefinition enumDeclarationSyntax)
    {
        return
            $$"""
              /// <summary>
              /// Gets the content dictionary containing mappings of <see cref="{{enumDeclarationSyntax.Name}}"/> enum values to values.
              /// </summary>
              /// <returns>The read-only content dictionary.</returns>
              public static FrozenDictionary<{{enumDeclarationSyntax.Name}}, {{enumDeclarationSyntax.UnderlyingType}}> GetContent()
              {
                return content;
              }

              /// <summary>
              /// Gets the content dictionary containing mappings of <see cref="{{enumDeclarationSyntax.Name}}"/> enum values to values.
              /// </summary>
              /// <param name="enumValue">The enum value for which to get the content dictionary.</param>
              /// <returns>The read-only content dictionary.</returns>
              public static FrozenDictionary<{{enumDeclarationSyntax.Name}}, {{enumDeclarationSyntax.UnderlyingType}}> GetContent(this {{enumDeclarationSyntax.Name}} enumValue)
              {
                return content;
              }
              """;
    }
}
