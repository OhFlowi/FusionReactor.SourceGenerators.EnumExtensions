using FusionReactor.SourceGenerators.EnumExtensions.Extensions;
using FusionReactor.SourceGenerators.EnumExtensions.Models;
using System.Text;

using Microsoft.CodeAnalysis;

namespace FusionReactor.SourceGenerators.EnumExtensions.Generators.Strategies.DisplayAttribute;

/// <inheritdoc />
public class PropertyDisplayResultStrategy : IExtensionGeneratorStrategy
{
    /// <summary>
    /// Gets the factory instance of <see cref="PropertyDisplayResultStrategy"/>.
    /// </summary>
    public static IExtensionGeneratorStrategy Factory => new PropertyDisplayResultStrategy();

    /// <inheritdoc />
    public string GetMethod(IncrementalGeneratorInitializationContext context, EnumDefinition enumDeclarationSyntax)
    {
        var stringBuilder = new StringBuilder();

        foreach (var member in enumDeclarationSyntax.Members)
        {
            try
            {
                if (!member.Attributes.Any(x => x.AttributeClass is { Name: "DisplayAttribute" }))
                {
                    stringBuilder.AppendLine($"{{ {enumDeclarationSyntax.Name}.{member.Name}, null }},");

                    continue;
                }

                stringBuilder.Append($"{{ {enumDeclarationSyntax.Name}.{member.Name}, new DisplayResult {{");

                if (member.ExtractStringValue("ShortName", out var shortName))
                {
                    stringBuilder.AppendLine($"ShortName = \"{shortName}\",");
                }

                if (member.ExtractStringValue("Name", out var name))
                {
                    stringBuilder.AppendLine($"Name = \"{name}\",");
                }

                if (member.ExtractStringValue("Description", out var description))
                {
                    stringBuilder.AppendLine($"Description = \"{description}\",");
                }

                if (member.ExtractStringValue("Prompt", out var prompt))
                {
                    stringBuilder.AppendLine($"Prompt = \"{prompt}\",");
                }

                if (member.ExtractStringValue("GroupName", out var groupName))
                {
                    stringBuilder.AppendLine($"GroupName = \"{groupName}\",");
                }

                if (member.ExtractIntValue("Order", out var order))
                {
                    stringBuilder.AppendLine($"Order = {order},");
                }

                stringBuilder.Append("}},");
                stringBuilder.AppendLine();
            }
            catch (NullReferenceException)
            {
                // ignored
            }
        }

        return
            $$"""
              /// <summary>
              /// Returns the <see cref="System.ComponentModel.DataAnnotations.DisplayAttribute"/> of the <see cref="{{enumDeclarationSyntax.Name}}"/> enum.
              /// </summary>
              /// <returns>The display attribute result or the enum value.</returns>
              public static FrozenDictionary<{{enumDeclarationSyntax.Name}}, DisplayResult?> DisplayResults
                => new Dictionary<{{enumDeclarationSyntax.Name}}, DisplayResult?>
                  {
                      {{stringBuilder}}
                  }
                  .ToFrozenDictionary();
              """;
    }
}
