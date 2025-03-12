// <copyright file="TryParsePart.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

namespace FusionReactor.SourceGenerators.EnumExtensions.Parts;

using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis;

/// <summary>
/// TODO:.
/// </summary>
public static class TryParsePart
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

        WriteTryParseFastOverload(symbol, writer);
        writer.WriteLine();
        WriteTryParseFast(symbol, writer);
        writer.WriteLine();
        WriteTryParseFastExtensionOverload(symbol, writer);
        writer.WriteLine();
        WriteTryParseFastExtension(symbol, writer);

        writer.Indent--;
    }

    private static void WriteTryParseFastOverload(
        INamedTypeSymbol symbol,
        IndentedTextWriter writer)
    {
        writer.WriteLine("/// <summary>");
        writer.WriteLine("/// Tries to parse the specified string representation of an enumeration value to its corresponding");
        writer.WriteLine(
            @"/// <see cref=""{0}""/> enumeration value.",
            symbol.Name);
        writer.WriteLine("/// </summary>");
        writer.WriteLine(@"/// <param name=""value"">The string representation of the enumeration value.</param>");
        writer.WriteLine(@"/// <param name=""result"">");
        writer.WriteLine(
            @"/// When this method returns, contains the <see cref=""{0}""/> value equivalent",
            symbol.Name);
        writer.WriteLine(
            @"/// to the string representation, if the parse succeeded, or default({0}) if the parse failed.</param>",
            symbol.Name);
        writer.WriteLine("/// <returns><c>true</c> if the parsing was successful; otherwise, <c>false</c>.</returns>");

        writer.WriteLine(
            "{1} static bool TryParseFast(string value, out {0}? result)",
            symbol.Name,
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.Indent++;
        writer.WriteLine("=> TryParseFast(value, false, out result);");
        writer.Indent--;
    }

    private static void WriteTryParseFast(
        INamedTypeSymbol symbol,
        IndentedTextWriter writer)
    {
        writer.WriteLine("/// <summary>");
        writer.WriteLine("/// Tries to parse the specified string representation of an enumeration value to its corresponding");
        writer.WriteLine(
            @"/// <see cref=""{0}""/> enumeration value.",
            symbol.Name);
        writer.WriteLine("/// </summary>");
        writer.WriteLine(@"/// <param name=""value"">The value to parse.</param>");
        writer.WriteLine(@"/// <param name=""ignoreCase"">");
        writer.WriteLine("/// A boolean indicating whether to ignore case during the parsing. Default is <c>false</c>.");
        writer.WriteLine("/// </param>");
        writer.WriteLine(@"/// <param name=""result"">");
        writer.WriteLine(
            @"/// When this method returns, contains the <see cref=""{0}""/> value equivalent",
            symbol.Name);
        writer.WriteLine("/// to the string representation, if the parse succeeded, or <c>null</c> if the parse failed.");
        writer.WriteLine("/// </param>");
        writer.WriteLine("/// <returns><c>true</c> if the parsing was successful; otherwise, <c>false</c>.</returns>");

        writer.WriteLine(
            "{1} static bool TryParseFast(string value, bool ignoreCase, out {0}? result)",
            symbol.Name,
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());

        WriteTryParseFastBody(symbol, writer);
    }

    private static void WriteTryParseFastExtensionOverload(
        INamedTypeSymbol symbol,
        IndentedTextWriter writer)
    {
        writer.WriteLine("/// <summary>");
        writer.WriteLine("/// Tries to parse the specified string representation of an enumeration value to its corresponding");
        writer.WriteLine(
            @"/// <see cref=""{0}""/> enumeration value.",
            symbol.Name);
        writer.WriteLine("/// </summary>");
        writer.WriteLine(@"/// <param name=""enumValue"">The enumeration value.</param>");
        writer.WriteLine(@"/// <param name=""value"">The value to parse.</param>");
        writer.WriteLine(@"/// <param name=""result"">");
        writer.WriteLine(
            @"/// When this method returns, contains the <see cref=""{0}""/> value equivalent",
            symbol.Name);
        writer.WriteLine("/// to the string representation, if the parse succeeded, or <c>null</c> if the parse failed.");
        writer.WriteLine("/// </param>");
        writer.WriteLine("/// <returns><c>true</c> if the parsing was successful; otherwise, <c>false</c>.</returns>");

        writer.WriteLine(
            "{1} static bool TryParseFast(this {0} enumValue, string value, out {0}? result)",
            symbol.Name,
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.Indent++;
        writer.WriteLine("=> enumValue.TryParseFast(value, false, out result);");
        writer.Indent--;
    }

    private static void WriteTryParseFastExtension(INamedTypeSymbol symbol, IndentedTextWriter writer)
    {
        writer.WriteLine("/// <summary>");
        writer.WriteLine("/// Tries to parse the specified string representation of an enumeration value to its corresponding");
        writer.WriteLine(
            @"/// <see cref=""{0}""/> enumeration value.",
            symbol.Name);
        writer.WriteLine("/// </summary>");
        writer.WriteLine(@"/// <param name=""enumValue"">The enumeration value.</param>");
        writer.WriteLine(@"/// <param name=""value"">The value to parse.</param>");
        writer.WriteLine(@"/// <param name=""ignoreCase"">");
        writer.WriteLine("/// A boolean indicating whether to ignore case during the parsing. Default is <c>false</c>.");
        writer.WriteLine("/// </param>");
        writer.WriteLine(@"/// <param name=""result"">");
        writer.WriteLine(
            @"/// When this method returns, contains the <see cref=""{0}""/> value equivalent",
            symbol.Name);
        writer.WriteLine("/// to the string representation, if the parse succeeded, or <c>null</c> if the parse failed.");
        writer.WriteLine("/// </param>");
        writer.WriteLine("/// <returns><c>true</c> if the parsing was successful; otherwise, <c>false</c>.</returns>");

        writer.WriteLine(
            "{1} static bool TryParseFast(this {0} enumValue, string value, bool ignoreCase, out {0}? result)",
            symbol.Name,
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());

        WriteTryParseFastBody(symbol, writer);
    }

    private static void WriteTryParseFastBody(
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
        writer.WriteLine("result = value.ToLowerInvariant() switch");
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

        writer.WriteLine("_ => null,");

        writer.Indent--;
        writer.WriteLine("};");
        writer.Indent--;
        writer.WriteLine("}");
        writer.WriteLine("else");
        writer.WriteLine("{");
        writer.Indent++;
        writer.WriteLine("result = value switch");
        writer.WriteLine("{");
        writer.Indent++;

        foreach (var line in data)
        {
            writer.WriteLine(
                @"""{1}"" => {0}.{1},",
                symbol.Name,
                line);
        }

        writer.WriteLine("_ => null,");

        writer.Indent--;
        writer.WriteLine("};");
        writer.Indent--;
        writer.WriteLine("}");
        writer.WriteLine();
        writer.WriteLine("return result is not null;");
        writer.Indent--;
        writer.WriteLine("}");
    }
}
