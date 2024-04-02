using FusionReactor.SourceGenerators.EnumExtensions.Models;
using System.Text;

using Microsoft.CodeAnalysis;

namespace FusionReactor.SourceGenerators.EnumExtensions.Generators.Strategies.Base;

public class GetNamesStrategy : IExtensionGeneratorStrategy
{
    public static IExtensionGeneratorStrategy Factory => new GetNamesStrategy();

    /// <inheritdoc />
    public string GetMethod(IncrementalGeneratorInitializationContext context, EnumDefinition enumDeclarationSyntax)
    {
        return
            $$"""
               /// <summary>
               /// Retrieves all available names of the <see cref="{{enumDeclarationSyntax.Name}}"/>.
               /// </summary>
               /// <returns>An enumerable collection of <see cref="{{enumDeclarationSyntax.Name}}"/> names.</returns>
               public static IEnumerable<string> GetNames()
               {
                 return names;
               }

               /// <summary>
               /// Retrieves all available names of the <see cref="{{enumDeclarationSyntax.Name}}"/>.
               /// </summary>
               /// <param name="enumValue">The enumeration value.</param>
               /// <returns>An enumerable collection of <see cref="{{enumDeclarationSyntax.Name}}"/> names.</returns>
               public static IEnumerable<string> GetNames(this {{enumDeclarationSyntax.Name}} enumValue)
               {
                 return names;
               }
               """;
    }
}
