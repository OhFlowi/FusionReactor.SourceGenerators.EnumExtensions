// <copyright file="ValuesPart.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

namespace FusionReactor.SourceGenerators.EnumExtensions.Parts;

using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis;

/// <summary>
/// TODO:.
/// </summary>
public static class ValuesPart
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
            .Select(x => x.Name)
            .ToList();

        writer.Indent++;

        writer.WriteLineNoTabs("#if NET8_0_OR_GREATER");

        WriteFieldNet8(symbol, data, writer);

        writer.WriteLineNoTabs("#elif NET5_0_OR_GREATER");

        WriteFieldNet5(symbol, data, writer);

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

        WriteGetValues(symbol, writer);
        writer.WriteLine();
        WriteGetValuesExtension(symbol, writer);

        writer.Indent--;
    }

    private static void WriteFieldNet8(
        INamedTypeSymbol symbol,
        List<string> data,
        IndentedTextWriter writer)
    {
        writer.WriteLine(
            "private static readonly FrozenSet<{0}> values",
            symbol.Name);

        WriteFieldInternal(symbol, data, writer, false);

        writer.WriteLine(".ToFrozenSet();");
    }

    private static void WriteFieldNet5(
        INamedTypeSymbol symbol,
        List<string> data,
        IndentedTextWriter writer)
    {
        writer.WriteLine(
            "private static readonly IReadOnlySet<{0}> names",
            symbol.Name);

        WriteFieldInternal(symbol, data, writer, true);
    }

    private static void WriteFieldDefault(
        INamedTypeSymbol symbol,
        List<string> data,
        IndentedTextWriter writer)
    {
        writer.WriteLine(
            "private static readonly HashSet<{0}> names",
            symbol.Name);

        WriteFieldInternal(symbol, data, writer, true);
    }

    private static void WriteFieldInternal(
        INamedTypeSymbol symbol,
        List<string> data,
        IndentedTextWriter writer,
        bool endWithSemicolon)
    {
        writer.Indent++;

        writer.WriteLine(
            "= new HashSet<{0}>()",
            symbol.Name);

        writer.WriteLine("{");
        writer.Indent++;

        foreach (var line in data)
        {
            writer.WriteLine(
                "{0}.{1},",
                symbol.Name,
                line);
        }

        writer.Indent--;

        writer.WriteLine(
            endWithSemicolon
                ? "};"
                : "}");

        writer.Indent--;
    }

    private static void WriteGetValues(
        INamedTypeSymbol symbol,
        IndentedTextWriter writer)
    {
        writer.WriteLine("/// <summary>");
        writer.WriteLine(
            @"/// Retrieves all available values of the <see cref=""{0}""/>.",
            symbol.Name);
        writer.WriteLine("/// </summary>");
        writer.WriteLine(
            @"/// <returns>An enumerable collection of <see cref=""{0}""/> values.</returns>",
            symbol.Name);
        writer.WriteLineNoTabs("#if NET8_0_OR_GREATER");
        writer.WriteLine(
            "{1} static FrozenSet<{0}> GetValues()",
            symbol.Name,
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.WriteLineNoTabs("#elif NET5_0_OR_GREATER");
        writer.WriteLine(
            "{1} static IReadOnlySet<{0}> GetValues()",
            symbol.Name,
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.WriteLineNoTabs("#else");
        writer.WriteLine(
            "{1} static HashSet<{0}> GetValues()",
            symbol.Name,
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.WriteLineNoTabs("#endif");
        writer.Indent++;
        writer.WriteLine("=> values;");
        writer.Indent--;
    }

    private static void WriteGetValuesExtension(INamedTypeSymbol symbol, IndentedTextWriter writer)
    {
        writer.WriteLine("/// <summary>");
        writer.WriteLine(
            @"/// Retrieves all available values of the <see cref=""{0}""/>.",
            symbol.Name);
        writer.WriteLine("/// </summary>");
        writer.WriteLine(@"/// <param name=""enumValue"">The enumeration value.</param>");
        writer.WriteLine(
            @"/// <returns>An enumerable collection of <see cref=""{0}""/> values.</returns>",
            symbol.Name);
        writer.WriteLineNoTabs("#if NET8_0_OR_GREATER");
        writer.WriteLine(
            "{1} static FrozenSet<{0}> GetValues(this {0} enumValue)",
            symbol.Name,
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.WriteLineNoTabs("#elif NET5_0_OR_GREATER");
        writer.WriteLine(
            "{1} static IReadOnlySet<{0}> GetValues(this {0} enumValue)",
            symbol.Name,
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.WriteLineNoTabs("#else");
        writer.WriteLine(
            "{1} static HashSet<{0}> GetValues(this {0} enumValue)",
            symbol.Name,
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.WriteLineNoTabs("#endif");
        writer.Indent++;
        writer.WriteLine("=> values;");
        writer.Indent--;
    }
}
