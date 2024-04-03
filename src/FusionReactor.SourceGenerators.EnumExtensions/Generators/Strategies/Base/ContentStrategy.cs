using FusionReactor.SourceGenerators.EnumExtensions.Models;
using System.Text;

using Microsoft.CodeAnalysis;

namespace FusionReactor.SourceGenerators.EnumExtensions.Generators.Strategies.Base;

public class ContentStrategy : IExtensionGeneratorStrategy
{
    public static IExtensionGeneratorStrategy Factory => new ContentStrategy();

    /// <inheritdoc />
    public string GetMethod(IncrementalGeneratorInitializationContext context, EnumDefinition enumDeclarationSyntax)
    {
        var stringBuilder = new StringBuilder();

        foreach (var member in enumDeclarationSyntax.Members)
        {
            stringBuilder.AppendLine($"{{ {enumDeclarationSyntax.Name}.{member.Name}, {member.Value} }},");
        }

        return
            $$"""
              private static readonly FrozenDictionary<{{enumDeclarationSyntax.Name}}, {{enumDeclarationSyntax.UnderlyingType}}> content
                = new Dictionary<{{enumDeclarationSyntax.Name}}, {{enumDeclarationSyntax.UnderlyingType}}>
                  {
                      {{stringBuilder}}
                  }
                  .ToFrozenDictionary();

              """;
    }
}
