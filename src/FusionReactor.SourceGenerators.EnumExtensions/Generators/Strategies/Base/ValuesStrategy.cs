using FusionReactor.SourceGenerators.EnumExtensions.Models;
using System.Text;

using Microsoft.CodeAnalysis;

namespace FusionReactor.SourceGenerators.EnumExtensions.Generators.Strategies.Base;

public class ValuesStrategy : IExtensionGeneratorStrategy
{
    public static IExtensionGeneratorStrategy Factory => new ValuesStrategy();

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
              #if NET8_0_OR_GREATER
              private static readonly FrozenSet<{{enumDeclarationSyntax.Name}}> values = new []
              {
                  {{stringBuilder}}
              }
              .ToFrozenSet();
              #elif NET5_0_OR_GREATER
              private static readonly IReadOnlySet<{{enumDeclarationSyntax.Name}}> values = new HashSet<{{enumDeclarationSyntax.Name}}>()
              {
                  {{stringBuilder}}
              };
              #else
              private static readonly HashSet<{{enumDeclarationSyntax.Name}}> values = new HashSet<{{enumDeclarationSyntax.Name}}>()
              {
                  {{stringBuilder}}
              };
              #endif

              """;
    }
}
