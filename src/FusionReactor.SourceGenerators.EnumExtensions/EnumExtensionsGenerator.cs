// <copyright file="EnumExtensionsGenerator.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

namespace FusionReactor.SourceGenerators.EnumExtensions;

using System.CodeDom.Compiler;
using System.Text;
using FusionReactor.SourceGenerators.EnumExtensions.Constants;
using FusionReactor.SourceGenerators.EnumExtensions.Parts;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

/// <inheritdoc />
[Generator]
public class EnumExtensionsGenerator : IIncrementalGenerator
{
    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static x =>
            x.AddSource(
                AttributeConstants.Name + ".g.cs",
                SourceText.From(
                    AttributeConstants.Content,
                    Encoding.UTF8)));

        var pipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: AttributeConstants.FullName,
            predicate: static (
                syntaxNode,
                _) => syntaxNode is EnumDeclarationSyntax,
            transform: static (
                    context,
                    cancellationToken)
                => (INamedTypeSymbol?)context
                    .SemanticModel
                    .GetDeclaredSymbol(
                        context.TargetNode,
                        cancellationToken));

        context.RegisterSourceOutput(
            pipeline,
            (
                productionContext,
                symbol) =>
            {
                if (symbol is null)
                {
                    return;
                }

                productionContext.AddSource(
                    $"{symbol.Name}.Extensions.g.cs",
                    SourceText.From(
                        BuildEnumExtensions(symbol),
                        Encoding.UTF8));
            });
    }

    private static string BuildEnumExtensions(INamedTypeSymbol symbol)
    {
        using var stringWriter = new StringWriter();
        using var textWriter = new IndentedTextWriter(stringWriter);

        textWriter.Indent = 0;

        EnumExtensionPart.WriteHeader(symbol, textWriter);

        ContentPart.WriteField(symbol, textWriter);
        textWriter.WriteLine();
        NamesPart.WriteField(symbol, textWriter);
        textWriter.WriteLine();
        ValuesPart.WriteField(symbol, textWriter);
        textWriter.WriteLine();
        EnumRootAttributesPart.WriteField(symbol, textWriter);
        textWriter.WriteLine();
        EnumMemberAttributesPart.WriteField(symbol, textWriter);
        textWriter.WriteLine();
        ContentPart.WriteMethods(symbol, textWriter);
        textWriter.WriteLine();
        NamesPart.WriteMethods(symbol, textWriter);
        textWriter.WriteLine();
        ValuesPart.WriteMethods(symbol, textWriter);
        textWriter.WriteLine();
        ParsePart.WriteMethods(symbol, textWriter);
        textWriter.WriteLine();
        TryParsePart.WriteMethods(symbol, textWriter);
        textWriter.WriteLine();
        EnumRootAttributesPart.WriteMethods(symbol, textWriter);
        textWriter.WriteLine();
        EnumMemberAttributesPart.WriteMethods(symbol, textWriter);

        EnumExtensionPart.WriteFooter(textWriter);

        return stringWriter.ToString();
    }
}
