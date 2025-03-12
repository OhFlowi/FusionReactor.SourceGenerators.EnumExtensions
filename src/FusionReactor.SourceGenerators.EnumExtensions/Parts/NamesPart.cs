// <copyright file="NamesPart.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

namespace FusionReactor.SourceGenerators.EnumExtensions.Parts;

using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis;

/// <summary>
/// TODO:.
/// </summary>
public static class NamesPart
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

        WriteGetName(symbol, writer);
        writer.WriteLine();
        WriteGetNames(symbol, writer);
        writer.WriteLine();
        WriteGetNamesExtension(symbol, writer);

        writer.Indent--;
    }

    private static void WriteFieldNet8(
        List<string> data,
        IndentedTextWriter writer)
    {
        writer.WriteLine("private static readonly FrozenSet<string> names");

        WriteFieldInternal(data, writer, false);

        writer.WriteLine(".ToFrozenSet();");
    }

    private static void WriteFieldNet5(
        List<string> data,
        IndentedTextWriter writer)
    {
        writer.WriteLine("private static readonly IReadOnlySet<string> names");

        WriteFieldInternal(data, writer, true);
    }

    private static void WriteFieldDefault(
        List<string> data,
        IndentedTextWriter writer)
    {
        writer.WriteLine("private static readonly HashSet<string> names");

        WriteFieldInternal(data, writer, true);
    }

    private static void WriteFieldInternal(
        List<string> data,
        IndentedTextWriter writer,
        bool endWithSemicolon)
    {
        writer.Indent++;

        writer.WriteLine("= new HashSet<string>()");

        writer.WriteLine("{");
        writer.Indent++;

        foreach (var line in data)
        {
            writer.WriteLine(
                @"""{0}"",",
                line);
        }

        writer.Indent--;

        writer.WriteLine(
            endWithSemicolon
                ? "};"
                : "}");

        writer.Indent--;
    }

    private static void WriteGetName(
        INamedTypeSymbol symbol,
        IndentedTextWriter writer)
    {
        var data = symbol
            .GetMembers()
            .Where(member => member is IFieldSymbol { ConstantValue: not null })
            .Cast<IFieldSymbol>()
            .Select(x => x.Name)
            .ToList();

        writer.WriteLine("/// <summary>");
        writer.WriteLine(
            @"/// Retrieves the name of the constant in the <see cref=""{0}""/>.",
            symbol.Name);
        writer.WriteLine("/// </summary>");
        writer.WriteLine(@"/// <param name=""enumValue"">The enum value to convert.</param>");
        writer.WriteLine("/// <returns>");
        writer.WriteLine(
            @"/// A string containing the name of the <see cref=""{0}""/>;",
            symbol.Name);
        writer.WriteLine(@"/// or <see langword=""null""/> if no such constant is found.");
        writer.WriteLine("/// </returns>");
        writer.WriteLine(
            "{1} static string? GetName(this {0} enumValue)",
            symbol.Name,
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());

        writer.Indent++;

        writer.WriteLine("=> enumValue switch");
        writer.WriteLine("{");

        writer.Indent++;

        foreach (var line in data)
        {
            writer.WriteLine(
                @"{0}.{1} => nameof({0}.{1}),",
                symbol.Name,
                line);
        }

        writer.WriteLine("_ => null,");

        writer.Indent--;

        writer.WriteLine("};");
        writer.Indent--;
    }

    private static void WriteGetNames(
        INamedTypeSymbol symbol,
        IndentedTextWriter writer)
    {
        writer.WriteLine("/// <summary>");
        writer.WriteLine(
            @"/// Retrieves all available names of the <see cref=""{0}""/>.",
            symbol.Name);
        writer.WriteLine("/// </summary>");
        writer.WriteLine(
            @"/// <returns>An enumerable collection of <see cref=""{0}""/> names.</returns>",
            symbol.Name);
        writer.WriteLineNoTabs("#if NET8_0_OR_GREATER");
        writer.WriteLine(
            "{0} static FrozenSet<string> GetNames()",
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.WriteLineNoTabs("#elif NET5_0_OR_GREATER");
        writer.WriteLine(
            "{0} static IReadOnlySet<string> GetNames()",
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.WriteLineNoTabs("#else");
        writer.WriteLine(
            "{0} static HashSet<string> GetNames()",
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.WriteLineNoTabs("#endif");
        writer.Indent++;
        writer.WriteLine("=> names;");
        writer.Indent--;
    }

    private static void WriteGetNamesExtension(INamedTypeSymbol symbol, IndentedTextWriter writer)
    {
        writer.WriteLine("/// <summary>");
        writer.WriteLine(
            @"/// Retrieves all available names of the <see cref=""{0}""/>.",
            symbol.Name);
        writer.WriteLine("/// </summary>");
        writer.WriteLine(@"/// <param name=""enumValue"">The enumeration value.</param>");
        writer.WriteLine(
            @"/// <returns>An enumerable collection of <see cref=""{0}""/> names.</returns>",
            symbol.Name);
        writer.WriteLineNoTabs("#if NET8_0_OR_GREATER");
        writer.WriteLine(
            "{1} static FrozenSet<string> GetNames(this {0} enumValue)",
            symbol.Name,
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.WriteLineNoTabs("#elif NET5_0_OR_GREATER");
        writer.WriteLine(
            "{1} static IReadOnlySet<string> GetNames(this {0} enumValue)",
            symbol.Name,
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.WriteLineNoTabs("#else");
        writer.WriteLine(
            "{1} static HashSet<string> GetNames(this {0} enumValue)",
            symbol.Name,
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.WriteLineNoTabs("#endif");
        writer.Indent++;
        writer.WriteLine("=> names;");
        writer.Indent--;
    }
}
