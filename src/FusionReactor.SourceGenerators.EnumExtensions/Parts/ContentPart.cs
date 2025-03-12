// <copyright file="ContentPart.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

namespace FusionReactor.SourceGenerators.EnumExtensions.Parts;

using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis;

/// <summary>
/// TODO:.
/// </summary>
public static class ContentPart
{
    /// <summary>
    /// TODO:.
    /// </summary>
    /// <param name="symbol">TODO: 1.</param>
    /// <param name="writer">TODO: 2.</param>
    public static void WriteField(INamedTypeSymbol symbol, IndentedTextWriter writer)
    {
        if (writer == null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        if (symbol == null)
        {
            throw new ArgumentNullException(nameof(symbol));
        }

        var data = symbol
            .GetMembers()
            .Where(member => member is IFieldSymbol { ConstantValue: not null })
            .Cast<IFieldSymbol>()
            .ToDictionary(x => x.Name, x => x.ConstantValue?.ToString() ?? string.Empty);

        writer.Indent++;

        writer.WriteLineNoTabs("#if NET8_0_OR_GREATER");

        WriteFieldNet8(symbol, data, writer);

        writer.WriteLineNoTabs("#else");

        WriteFieldDefault(symbol, data, writer);

        writer.WriteLineNoTabs("#endif");

        writer.Indent--;
    }

    /// <summary>
    /// TODO:.
    /// </summary>
    /// <param name="symbol">TODO: 1.</param>
    /// <param name="writer">TODO: 2.</param>
    public static void WriteMethods(INamedTypeSymbol symbol, IndentedTextWriter writer)
    {
        if (writer == null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        if (symbol == null)
        {
            throw new ArgumentNullException(nameof(symbol));
        }

        writer.Indent++;

        WriteGetContent(symbol, writer);
        writer.WriteLine();
        WriteGetContentExtension(symbol, writer);

        writer.Indent--;
    }

    private static void WriteFieldNet8(
        INamedTypeSymbol symbol,
        Dictionary<string, string> data,
        IndentedTextWriter writer)
    {
        writer.WriteLine(
            "private static readonly FrozenDictionary<{0}, {1}> content",
            symbol.Name,
            symbol.EnumUnderlyingType?.Name ?? "int");

        WriteFieldInternal(symbol, data, writer, false);

        writer.WriteLine(".ToFrozenDictionary();");
    }

    private static void WriteFieldDefault(
        INamedTypeSymbol symbol,
        Dictionary<string, string> data,
        IndentedTextWriter writer)
    {
        writer.WriteLine(
            "private static readonly Dictionary<{0}, {1}> contentDictionary",
            symbol.Name,
            symbol.EnumUnderlyingType?.Name ?? "int");

        WriteFieldInternal(symbol, data, writer, true);

        writer.WriteLine();
        writer.WriteLine(
            "private static readonly IReadOnlyDictionary<{0}, {1}> content",
            symbol.Name,
            symbol.EnumUnderlyingType?.Name ?? "int");
        writer.Indent++;
        writer.WriteLine(
            "= new ReadOnlyDictionary<{0}, {1}>(contentDictionary);",
            symbol.Name,
            symbol.EnumUnderlyingType?.Name ?? "int");
        writer.Indent--;
    }

    private static void WriteFieldInternal(
        INamedTypeSymbol symbol,
        Dictionary<string, string> data,
        IndentedTextWriter writer,
        bool endWithSemicolon)
    {
        writer.Indent++;

        writer.WriteLine(
            "= new Dictionary<{0}, {1}>",
            symbol.Name,
            symbol.EnumUnderlyingType?.Name ?? "int");

        writer.WriteLine("{");
        writer.Indent++;

        foreach (var line in data)
        {
            writer.WriteLine(
                "{{ {0}.{1}, {2} }},",
                symbol.Name,
                line.Key,
                line.Value);
        }

        writer.Indent--;

        writer.WriteLine(
            endWithSemicolon
                ? "};"
                : "}");

        writer.Indent--;
    }

    private static void WriteGetContent(
        INamedTypeSymbol symbol,
        IndentedTextWriter writer)
    {
        writer.WriteLine("/// <summary>");
        writer.WriteLine(
            @"/// Gets the content dictionary containing mappings of <see cref=""{0}""/> enum values to values.",
            symbol.Name);
        writer.WriteLine("/// </summary>");
        writer.WriteLine("/// <returns>The read-only content dictionary.</returns>");
        writer.WriteLineNoTabs("#if NET8_0_OR_GREATER");
        writer.WriteLine(
            "{2} static FrozenDictionary<{0}, {1}> GetContent()",
            symbol.Name,
            symbol.EnumUnderlyingType?.Name ?? "int",
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.WriteLineNoTabs("#else");
        writer.WriteLine(
            "{2} static IReadOnlyDictionary<{0}, {1}> GetContent()",
            symbol.Name,
            symbol.EnumUnderlyingType?.Name ?? "int",
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.WriteLineNoTabs("#endif");
        writer.Indent++;
        writer.WriteLine("=> content;");
        writer.Indent--;
    }

    private static void WriteGetContentExtension(INamedTypeSymbol symbol, IndentedTextWriter writer)
    {
        writer.WriteLine("/// <summary>");
        writer.WriteLine(
            @"/// Gets the content dictionary containing mappings of <see cref=""{0}""/> enum values to values.",
            symbol.Name);
        writer.WriteLine("/// </summary>");
        writer.WriteLine(@"/// <param name=""enumValue"">The enum value for which to get the content dictionary.</param>");
        writer.WriteLine("/// <returns>The read-only content dictionary.</returns>");
        writer.WriteLineNoTabs("#if NET8_0_OR_GREATER");
        writer.WriteLine(
            "{2} static FrozenDictionary<{0}, {1}> GetContent(this {0} enumValue)",
            symbol.Name,
            symbol.EnumUnderlyingType?.Name ?? "int",
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.WriteLineNoTabs("#else");
        writer.WriteLine(
            "{2} static IReadOnlyDictionary<{0}, {1}> GetContent(this {0} enumValue)",
            symbol.Name,
            symbol.EnumUnderlyingType?.Name ?? "int",
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.WriteLineNoTabs("#endif");
        writer.Indent++;
        writer.WriteLine("=> content;");
        writer.Indent--;
    }
}
