// <copyright file="EnumRootAttributesPart.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

namespace FusionReactor.SourceGenerators.EnumExtensions.Parts;

using System.CodeDom.Compiler;
using FusionReactor.SourceGenerators.EnumExtensions.Extensions;
using Microsoft.CodeAnalysis;

/// <summary>
/// TODO:.
/// </summary>
public static class EnumRootAttributesPart
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
            .GetAttributes()
            .ToList();

        writer.Indent++;

        writer.WriteLineNoTabs("#if NET8_0_OR_GREATER");

        WriteFieldNet8(data, writer);

        writer.WriteLineNoTabs("#elif NET5_0_OR_GREATER");

        WriteFieldNet5(data, writer);

        writer.WriteLineNoTabs("#else");

        WriteFieldDefault(data, writer);

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

        WriteGetEnumAttributes(symbol, writer);
        writer.WriteLine();
        WriteGetEnumAttributesExtension(symbol, writer);

        writer.Indent--;
    }

    private static void WriteFieldNet8(
        List<AttributeData> data,
        IndentedTextWriter writer)
    {
        writer.WriteLine("private static readonly FrozenSet<Attribute> rootAttributes");

        WriteFieldInternal(data, writer, false);

        writer.WriteLine(".ToFrozenSet();");
    }

    private static void WriteFieldNet5(
        List<AttributeData> data,
        IndentedTextWriter writer)
    {
        writer.WriteLine("private static readonly IReadOnlySet<Attribute> rootAttributes");

        WriteFieldInternal(data, writer, true);
    }

    private static void WriteFieldDefault(
        List<AttributeData> data,
        IndentedTextWriter writer)
    {
        writer.WriteLine("private static readonly HashSet<Attribute> rootAttributes");

        WriteFieldInternal(data, writer, true);
    }

    private static void WriteFieldInternal(
        List<AttributeData> data,
        IndentedTextWriter writer,
        bool endWithSemicolon)
    {
        writer.Indent++;

        writer.WriteLine("= new HashSet<Attribute>()");

        writer.WriteLine("{");
        writer.Indent++;

        foreach (var line in data)
        {
            writer.WriteAttribute(line);
        }

        writer.Indent--;

        writer.WriteLine(
            endWithSemicolon
                ? "};"
                : "}");

        writer.Indent--;
    }

    private static void WriteGetEnumAttributes(
        INamedTypeSymbol symbol,
        IndentedTextWriter writer)
    {
        writer.WriteLine("/// <summary>");
        writer.WriteLine(
            @"/// Retrieves all available attributes of the <see cref=""{0}""/>.",
            symbol.Name);
        writer.WriteLine("/// </summary>");
        writer.WriteLine(
            @"/// <returns>An enumerable collection of <see cref=""{0}""/> attributes.</returns>",
            symbol.Name);
        writer.WriteLineNoTabs("#if NET8_0_OR_GREATER");
        writer.WriteLine(
            "{0} static FrozenSet<Attribute> GetRootAttributes()",
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.WriteLineNoTabs("#elif NET5_0_OR_GREATER");
        writer.WriteLine(
            "{0} static IReadOnlySet<Attribute> GetRootAttributes()",
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.WriteLineNoTabs("#else");
        writer.WriteLine(
            "{0} static HashSet<Attribute> GetRootAttributes()",
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.WriteLineNoTabs("#endif");
        writer.Indent++;
        writer.WriteLine("=> rootAttributes;");
        writer.Indent--;
    }

    private static void WriteGetEnumAttributesExtension(INamedTypeSymbol symbol, IndentedTextWriter writer)
    {
        writer.WriteLine("/// <summary>");
        writer.WriteLine(
            @"/// Retrieves all available attributes of the <see cref=""{0}""/>.",
            symbol.Name);
        writer.WriteLine("/// </summary>");
        writer.WriteLine(@"/// <param name=""enumValue"">The enumeration value.</param>");
        writer.WriteLine(
            @"/// <returns>An enumerable collection of <see cref=""{0}""/> attributes.</returns>",
            symbol.Name);
        writer.WriteLineNoTabs("#if NET8_0_OR_GREATER");
        writer.WriteLine(
            "{1} static FrozenSet<Attribute> GetRootAttributes(this {0} enumValue)",
            symbol.Name,
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.WriteLineNoTabs("#elif NET5_0_OR_GREATER");
        writer.WriteLine(
            "{1} static IReadOnlySet<Attribute> GetRootAttributes(this {0} enumValue)",
            symbol.Name,
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.WriteLineNoTabs("#else");
        writer.WriteLine(
            "{1} static HashSet<Attribute> GetRootAttributes(this {0} enumValue)",
            symbol.Name,
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.WriteLineNoTabs("#endif");
        writer.Indent++;
        writer.WriteLine("=> rootAttributes;");
        writer.Indent--;
    }
}
