using FusionReactor.SourceGenerators.EnumExtensions.Models;
using System.Text;

using Microsoft.CodeAnalysis;

namespace FusionReactor.SourceGenerators.EnumExtensions.Generators.Strategies.Base;

public class NamesStrategy : IExtensionGeneratorStrategy
{
    public static IExtensionGeneratorStrategy Factory => new NamesStrategy();

    /// <inheritdoc />
    public string GetMethod(IncrementalGeneratorInitializationContext context, EnumDefinition enumDeclarationSyntax)
    {
        var stringBuilder = new StringBuilder();

        foreach (var member in enumDeclarationSyntax.Members)
        {
            try
            {
                stringBuilder.AppendLine($"\"{member.Name}\",");
            }
            catch (NullReferenceException)
            {
                // ignored
            }
        }

        return
            $$"""
               #if NET8_0_OR_GREATER
               private static readonly FrozenSet<string> names = new []
               {
                   {{stringBuilder}}
               }
               .ToFrozenSet();
               #elif NET5_0_OR_GREATER
               private static readonly IReadOnlySet<string> names = new HashSet<string>()
               {
                   {{stringBuilder}}
               };
               #else
               private static readonly HashSet<string> names = new HashSet<string>()
               {
                   {{stringBuilder}}
               };
               #endif

               """;
    }
}
