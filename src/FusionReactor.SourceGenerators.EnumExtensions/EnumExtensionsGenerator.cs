using FusionReactor.SourceGenerators.EnumExtensions.Constants;
using FusionReactor.SourceGenerators.EnumExtensions.Extensions;
using FusionReactor.SourceGenerators.EnumExtensions.Generators;
using FusionReactor.SourceGenerators.EnumExtensions.Models;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace FusionReactor.SourceGenerators.EnumExtensions;

/// <inheritdoc />
[Generator]
public class EnumExtensionsGenerator : IIncrementalGenerator
{
    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.AddGenerateEnumExtensionsAttribute();
        context.AddDisplayResult();

        var enums = context
            .SyntaxProvider
            .ForAttributeWithMetadataName(
                "FusionReactor.SourceGenerators.EnumExtensions." + GenerateEnumExtensionsAttributeConstants.Name + "Attribute",
                predicate: (node, _) => node is EnumDeclarationSyntax,
                transform: static (ctx, _)
                    => GetEnumDefinition(ctx.SemanticModel, ctx.TargetNode))
            .Where(static x => x is not null);

        context.RegisterSourceOutput(
            enums,
            (productionContext, node) =>
            {
                if (node is null)
                {
                    return;
                }

                productionContext.AddSource(
                    node.Value.Name + "Extensions.Base.g.cs",
                    SourceText.From(BaseExtensionGenerator.Generate(context, node.Value), Encoding.UTF8));

                productionContext.AddSource(
                    node.Value.Name + "Extensions.DisplayAttribute.g.cs",
                    SourceText.From(DisplayAttributeExtensionGenerator.Generate(context, node.Value), Encoding.UTF8));
            });
    }

    private static EnumDefinition? GetEnumDefinition(SemanticModel semanticModel, SyntaxNode enumDeclarationSyntax)
    {
        if (semanticModel.GetDeclaredSymbol(enumDeclarationSyntax) is not INamedTypeSymbol enumSymbol)
        {
            return null;
        }

        var enumMembers = enumSymbol.GetMembers();

        var members = new List<EnumMember>(enumMembers.Length);

        members.AddRange(
            enumMembers
                .Where(member => member is IFieldSymbol { ConstantValue: not null })
                .Cast<IFieldSymbol>()
                .Select(enumMember => new EnumMember
                {
                    Name = enumMember.Name,
                    Value = enumMember.ConstantValue?.ToString() ?? string.Empty,
                    Attributes = enumMember.GetAttributes(),
                }));

        return new EnumDefinition
        {
            Namespace = enumSymbol.ContainingNamespace.FullNamespace(),
            Access = enumSymbol.DeclaredAccessibility.ToString().ToLowerInvariant(),
            Name = enumSymbol.Name,
            UnderlyingType = enumSymbol.EnumUnderlyingType?.Name ?? "int",
            HasFlags = enumSymbol.GetAttributes().Any(x => x.AttributeClass is { Name: nameof(FlagsAttribute) }),
            Members = members,
        };
    }
}
