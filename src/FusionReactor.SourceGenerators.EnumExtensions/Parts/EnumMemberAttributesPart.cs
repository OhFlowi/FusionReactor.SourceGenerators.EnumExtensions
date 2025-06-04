// <copyright file="EnumMemberAttributesPart.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

namespace FusionReactor.SourceGenerators.EnumExtensions.Parts;

using System.CodeDom.Compiler;
using FusionReactor.SourceGenerators.EnumExtensions.Extensions;
using Microsoft.CodeAnalysis;

/// <summary>
/// TODO:.
/// </summary>
public static class EnumMemberAttributesPart
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
            .GetMembers()
            .Where(member => member is IFieldSymbol { ConstantValue: not null })
            .Cast<IFieldSymbol>()
            .ToList();

        writer.Indent++;

        writer.WriteLineNoTabs("#if NET8_0_OR_GREATER");

        WriteFieldNet8(
            symbol,
            data,
            writer);

        writer.WriteLineNoTabs("#else");

        WriteFieldDefault(
            symbol,
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

        var members = symbol
            .GetMembers()
            .Where(member => member is IFieldSymbol { ConstantValue: not null })
            .Cast<IFieldSymbol>()
            .ToArray();

        writer.Indent++;

        writer.WriteLine("/// <summary>");
        writer.WriteLine("/// The existing member attributes.");
        writer.WriteLine("/// </summary>");
        writer.WriteLine("public static class Member");
        writer.WriteLine("{");

        writer.Indent++;

        for (var index = 0; index < members.Length; index++)
        {
            if (index > 0)
            {
                writer.WriteLine();
            }

            writer.WriteLine("/// <summary>");
            writer.WriteLine("/// The existing member attributes.");
            writer.WriteLine("/// </summary>");
            writer.WriteLine(
                "public static class {0}",
                members[index].Name);
            writer.WriteLine("{");

            writer.Indent++;

            var data = members[index]
                .GetAttributes()
                .ToArray();

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
        }

        writer.Indent--;

        writer.WriteLine("}");

        writer.Indent--;
    }

    private static void WriteFieldNet8(
        INamedTypeSymbol symbol,
        List<IFieldSymbol> data,
        IndentedTextWriter writer)
    {
        writer.WriteLine(
            "private static readonly FrozenDictionary<{0}, Attribute[]> memberAttributes",
            symbol.Name);

        WriteFieldInternal(
            symbol,
            data,
            writer,
            false);

        writer.WriteLine(".ToFrozenDictionary();");
    }

    private static void WriteFieldDefault(
        INamedTypeSymbol symbol,
        List<IFieldSymbol> data,
        IndentedTextWriter writer)
    {
        writer.WriteLine(
            "private static readonly Dictionary<{0}, Attribute[]> memberAttributesDictionary",
            symbol.Name);

        WriteFieldInternal(
            symbol,
            data,
            writer,
            true);

        writer.WriteLine();
        writer.WriteLine(
            "private static readonly IReadOnlyDictionary<{0}, Attribute[]> memberAttributes",
            symbol.Name);
        writer.Indent++;
        writer.WriteLine(
            "= new ReadOnlyDictionary<{0}, Attribute[]>(memberAttributesDictionary);",
            symbol.Name);
        writer.Indent--;
    }

    private static void WriteFieldInternal(
        INamedTypeSymbol symbol,
        List<IFieldSymbol> data,
        IndentedTextWriter writer,
        bool endWithSemicolon)
    {
        writer.Indent++;

        writer.WriteLine(
            "= new Dictionary<{0}, Attribute[]>()",
            symbol.Name);

        writer.WriteLine("{");
        writer.Indent++;

        foreach (var line in data)
        {
            writer.WriteLine("{");

            writer.Indent++;

            writer.WriteLine(
                "{0}.{1},",
                symbol.Name,
                line.Name);

            var attributes = line.GetAttributes();

            writer.WriteLine(
                "new Attribute[{0}]",
                attributes.Length);

            if (attributes.Length > 0)
            {
                writer.WriteLine("{");

                writer.Indent++;

                foreach (var attribute in attributes)
                {
                    writer.WriteAttribute(attribute);
                }

                writer.Indent--;

                writer.WriteLine("}");
            }

            writer.Indent--;

            writer.WriteLine("},");
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
            "{0} static FrozenDictionary<{1}, Attribute[]> GetMemberAttributes()",
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant(),
            symbol.Name);
        writer.WriteLineNoTabs("#else");
        writer.WriteLine(
            "{0} static Dictionary<{1}, Attribute[]> GetMemberAttributes()",
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant(),
            symbol.Name);
        writer.WriteLineNoTabs("#endif");
        writer.Indent++;
        writer.WriteLine("=> memberAttributes;");
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
            "{1} static FrozenDictionary<{0}, Attribute[]> GetMemberAttributes(this {0} enumValue)",
            symbol.Name,
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.WriteLineNoTabs("#else");
        writer.WriteLine(
            "{1} static Dictionary<{0}, Attribute[]> GetMemberAttributes(this {0} enumValue)",
            symbol.Name,
            symbol.DeclaredAccessibility.ToString().ToLowerInvariant());
        writer.WriteLineNoTabs("#endif");
        writer.Indent++;
        writer.WriteLine("=> memberAttributes;");
        writer.Indent--;
    }
}
