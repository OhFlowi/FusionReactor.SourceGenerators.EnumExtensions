// <copyright file="WriteAttributeExtension.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

// ReSharper disable ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
// ReSharper disable ExtractCommonBranchingCode
namespace FusionReactor.SourceGenerators.EnumExtensions.Extensions;

using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis;

/// <summary>
/// Provides extension methods for writing attributes in source code generation.
/// </summary>
public static class WriteAttributeExtension
{
    /// <summary>
    /// Writes an attribute to the indented text writer.
    /// </summary>
    /// <param name="writer">The <see cref="IndentedTextWriter"/> to write to.</param>
    /// <param name="attribute">The <see cref="AttributeData"/> representing the attribute to write.</param>
    public static void WriteAttribute(
        this IndentedTextWriter writer,
        AttributeData attribute)
    {
        if (writer == null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        if (attribute?.AttributeClass is null)
        {
            return;
        }

        if (attribute.ConstructorArguments.Length > 0)
        {
            writer.WriteLine(
                @"new {0}(",
                attribute.AttributeClass!.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));

            WriteAttributeConstructorArguments(writer, attribute);

            if (attribute.NamedArguments.Length > 0)
            {
                WriteAttributeNamedArguments(writer, attribute);
            }
        }
        else
        {
            writer.WriteLine(
                @"new {0}(){1}",
                attribute.AttributeClass!.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                attribute.NamedArguments.Length == 0 ? ',' : string.Empty);

            if (attribute.NamedArguments.Length > 0)
            {
                WriteAttributeNamedArguments(writer, attribute);
            }
        }
    }

    private static void WriteAttributeConstructorArguments(
        IndentedTextWriter writer,
        AttributeData attribute)
    {
        writer.Indent++;

        var argumentData = attribute
            .ConstructorArguments
            .Where(x => x.Type != null)
            .Select(x =>
            {
                if (x.Type!.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == "string")
                {
                    return $"\"{x.Value}\"";
                }

                if (x.Kind != TypedConstantKind.Array)
                {
                    return x.Value!.ToString();
                }

                var values = string.Empty;

                if (x.Values.Length <= 0)
                {
                    return
                        $"new {x.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}{values}";
                }

                if (x.Type.ToDisplayString() == "string[]")
                {
                    values = "{ " + string.Join(", ", x.Values.Select(y => $"\"{y.Value}\"")) + " }";
                }
                else
                {
                    values = "{ " + string.Join(", ", x.Values.Select(y => $"{y.Value}")) + " }";
                }

                return
                    $"new {x.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}{values}";
            })
            .ToList();

        foreach (var constructorArgument in argumentData)
        {
            writer.WriteLine(
                argumentData.Last() == constructorArgument
                    ? "{0}){1}"
                    : "{0},",
                constructorArgument,
                attribute.NamedArguments.Length == 0 ? ',' : string.Empty);
        }

        writer.Indent--;
    }

    private static void WriteAttributeNamedArguments(
        IndentedTextWriter writer,
        AttributeData attribute)
    {
        writer.WriteLine("{");

        writer.Indent++;

        var arguments = attribute.NamedArguments.ToArray();

        foreach (var argument in arguments)
        {
            var value = argument.Value.Value;

            if (argument.Value.Type!.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == "string")
            {
                value = "\"" + value + "\"";
            }

            writer.WriteLine(
                "{0} = {1},",
                argument.Key,
                value);
        }

        writer.Indent--;

        writer.WriteLine("},");
    }
}
