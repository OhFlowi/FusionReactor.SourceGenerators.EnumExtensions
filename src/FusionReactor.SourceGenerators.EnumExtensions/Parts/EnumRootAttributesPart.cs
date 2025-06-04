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
    public static void WriteField(
        INamedTypeSymbol symbol,
        IndentedTextWriter writer)
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

        WriteFieldNet8(
            data,
            writer);

        writer.WriteLineNoTabs("#elif NET5_0_OR_GREATER");

        WriteFieldNet5(
            data,
            writer);

        writer.WriteLineNoTabs("#else");

        WriteFieldDefault(
            data,
            writer);

        writer.WriteLineNoTabs("#endif");

        writer.Indent--;
    }

    /// <summary>
    /// TODO:.
    /// </summary>
    /// <param name="symbol">TODO: 1.</param>
    /// <param name="writer">TODO: 2.</param>
    public static void WriteMethods(
        INamedTypeSymbol symbol,
        IndentedTextWriter writer)
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

        WriteGetEnumAttributes(
            symbol,
            writer);
        writer.WriteLine();
        WriteGetEnumAttributesExtension(
            symbol,
            writer);

        writer.Indent--;
    }

    /// <summary>
    /// TODO:.
    /// </summary>
    /// <param name="symbol">TODO: 1.</param>
    /// <param name="writer">TODO: 2.</param>
    public static void WriteClass(
        INamedTypeSymbol symbol,
        IndentedTextWriter writer)
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
            .ToArray();

        writer.Indent++;

        writer.WriteLine("/// <summary>");
        writer.WriteLine("/// The existing root attributes.");
        writer.WriteLine("/// </summary>");
        writer.WriteLine("public static class Root");
        writer.WriteLine("{");

        writer.Indent++;

        for (var i = 0; i < data.Length; i++)
        {
            if (i > 0)
            {
                writer.WriteLine();
            }

            var name = data[i].AttributeClass!.Name.TrimEnd("Attribute");

            var matches = data
                .Where(x =>
                    x.AttributeClass!.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) ==
                    data[i].AttributeClass!.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat))
                .ToArray();

            if (matches.Length > 1)
            {
                name = $"{name}_{Array.IndexOf(matches, data[i])}";
            }

            writer.WriteLine("/// <summary>");
            writer.WriteLine(
                "/// Attributes for <see cref=\"{0}\" />",
                data[i].AttributeClass!.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
            writer.WriteLine("/// </summary>");
            writer.WriteLine(
                "{0} static {1} {2}",
                data[i].AttributeClass!.DeclaredAccessibility.ToString().ToLowerInvariant(),
                data[i].AttributeClass!.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                name);

            writer.Indent++;

            writer.WriteLine("=>");

            writer.Indent++;

            if (data[i].ConstructorArguments.Length > 0)
            {
                writer.WriteLine(
                    @"new {0}(",
                    data[i].AttributeClass!.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));

                writer.WriteAttributeConstructorArguments(
                    data[i],
                    ";");

                if (data[i].NamedArguments.Length > 0)
                {
                    writer.WriteAttributeNamedArguments(
                        data[i],
                        ";");
                }
            }
            else
            {
                writer.WriteLine(
                    @"new {0}(){1}",
                    data[i].AttributeClass!.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                    data[i].NamedArguments.Length == 0 ? ';' : string.Empty);

                if (data[i].NamedArguments.Length > 0)
                {
                    writer.WriteAttributeNamedArguments(
                        data[i],
                        ";");
                }
            }

            writer.Indent--;

            writer.Indent--;
        }

        writer.Indent--;

        writer.WriteLine("}");

        writer.Indent--;
    }

    private static void WriteFieldNet8(
        List<AttributeData> data,
        IndentedTextWriter writer)
    {
        writer.WriteLine("private static readonly FrozenSet<Attribute> rootAttributes");

        WriteFieldInternal(
            data,
            writer,
            false);

        writer.WriteLine(".ToFrozenSet();");
    }

    private static void WriteFieldNet5(
        List<AttributeData> data,
        IndentedTextWriter writer)
    {
        writer.WriteLine("private static readonly IReadOnlySet<Attribute> rootAttributes");

        WriteFieldInternal(
            data,
            writer,
            true);
    }

    private static void WriteFieldDefault(
        List<AttributeData> data,
        IndentedTextWriter writer)
    {
        writer.WriteLine("private static readonly HashSet<Attribute> rootAttributes");

        WriteFieldInternal(
            data,
            writer,
            true);
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

    private static void WriteGetEnumAttributesExtension(
        INamedTypeSymbol symbol,
        IndentedTextWriter writer)
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
