// <copyright file="ParsePart.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

namespace FusionReactor.SourceGenerators.EnumExtensions.Parts;

using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis;

/// <summary>
/// TODO:.
/// </summary>
public static class ParsePart
{
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

        WriteParseFast(symbol, writer);
        writer.WriteLine();
        WriteParseFastExtension(symbol, writer);

        writer.Indent--;
    }

    private static void WriteParseFast(
        INamedTypeSymbol symbol,
        IndentedTextWriter writer)
    {
        writer.WriteLine("/// <summary>");
        writer.WriteLine("/// Parses the specified string representation of the enumeration value to its corresponding");
        writer.WriteLine(
            @"/// <see cref=""{0}""/> value.",
            symbol.Name);
        writer.WriteLine("/// </summary>");
        writer.WriteLine(@"/// <param name=""value"">A string containing the name or value to convert.</param>");
        writer.WriteLine(@"/// <param name=""ignoreCase"">");
        writer.WriteLine("/// A boolean indicating whether to ignore case during the parsing. Default is <c>false</c>.");
        writer.WriteLine("/// </param>");
        writer.WriteLine("/// <returns>");
        writer.WriteLine(
            @"/// The <see cref=""{0}""/> value equivalent to the specified string representation.",
            symbol.Name);
        writer.WriteLine("/// </returns>");

        writer.WriteLine(
            "{1} static {0} ParseFast(string value, bool ignoreCase = false)",
            symbol.Name,
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());

        WriteParseFastBody(symbol, writer);
    }

    private static void WriteParseFastExtension(INamedTypeSymbol symbol, IndentedTextWriter writer)
    {
        writer.WriteLine("/// <summary>");
        writer.WriteLine("/// Parses the specified string representation of the enumeration value to its corresponding");
        writer.WriteLine(
            @"/// <see cref=""{0}""/> value.",
            symbol.Name);
        writer.WriteLine("/// </summary>");
        writer.WriteLine(@"/// <param name=""enumValue"">The enumeration value.</param>");
        writer.WriteLine(@"/// <param name=""value"">A string containing the name or value to convert.</param>");
        writer.WriteLine(@"/// <param name=""ignoreCase"">");
        writer.WriteLine("/// A boolean indicating whether to ignore case during the parsing. Default is <c>false</c>.");
        writer.WriteLine("/// </param>");
        writer.WriteLine("/// <returns>");
        writer.WriteLine(
            @"/// The <see cref=""{0}""/> value equivalent to the specified string representation.",
            symbol.Name);
        writer.WriteLine("/// </returns>");

        writer.WriteLine(
            "{1} static {0} ParseFast(this {0} enumValue, string value, bool ignoreCase = false)",
            symbol.Name,
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());

        WriteParseFastBody(symbol, writer);
    }

    private static void WriteParseFastBody(
        INamedTypeSymbol symbol,
        IndentedTextWriter writer)
    {
        var data = symbol
            .GetMembers()
            .Where(member => member is IFieldSymbol { ConstantValue: not null })
            .Cast<IFieldSymbol>()
            .Select(x => x.Name)
            .ToList();

        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine("if (ignoreCase)");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine("return value.ToLowerInvariant() switch");
        writer.WriteLine("{");
        writer.Indent++;

        foreach (var line in data)
        {
            writer.WriteLine(
                @"""{2}"" => {0}.{1},",
                symbol.Name,
                line,
                line.ToLowerInvariant());
        }

        writer.WriteLine("_ => throw new ArgumentException(),");

        writer.Indent--;
        writer.WriteLine("};");
        writer.Indent--;
        writer.WriteLine("}");
        writer.WriteLine("else");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine("return value switch");
        writer.WriteLine("{");
        writer.Indent++;

        foreach (var line in data)
        {
            writer.WriteLine(
                @"""{1}"" => {0}.{1},",
                symbol.Name,
                line);
        }

        writer.WriteLine("_ => throw new ArgumentException(),");

        writer.Indent--;
        writer.WriteLine("};");
        writer.Indent--;
        writer.WriteLine("}");
        writer.Indent--;
        writer.WriteLine("}");
    }
}
