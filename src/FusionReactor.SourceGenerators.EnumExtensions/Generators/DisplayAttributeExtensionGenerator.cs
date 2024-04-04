﻿using FusionReactor.SourceGenerators.EnumExtensions.Generators.Strategies.DisplayAttribute;
using FusionReactor.SourceGenerators.EnumExtensions.Models;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace FusionReactor.SourceGenerators.EnumExtensions.Generators;

public static class DisplayAttributeExtensionGenerator
{
    private static readonly IEnumerable<IExtensionGeneratorStrategy> Strategies = new[]
    {
        PropertyDisplayResultStrategy.Factory,
        DisplayShortNameStrategy.Factory,
        DisplayNameStrategy.Factory,
        DisplayDescriptionStrategy.Factory,
        DisplayPromptStrategy.Factory,
        DisplayGroupNameStrategy.Factory,
        DisplayOrderStrategy.Factory,
    };

    public static string Generate(IncrementalGeneratorInitializationContext context, EnumDefinition node)
    {
        var strategyResults = Strategies
            .Select(x => x.GetMethod(context, node));

        var stringBuilder = new StringBuilder();

        foreach (var strategyResult in strategyResults)
        {
            stringBuilder.AppendLine(strategyResult);
        }

        var returnValue = new StringBuilder();

        returnValue.AppendLine("// <auto-generated />");
        returnValue.AppendLine();
        returnValue.AppendLine("#nullable enable");
        returnValue.AppendLine();
        returnValue.AppendLine("using System;");
        returnValue.AppendLine("using System.Collections;");
        returnValue.AppendLine("#if NET8_0_OR_GREATER");
        returnValue.AppendLine("using System.Collections.Frozen;");
        returnValue.AppendLine("#endif");
        returnValue.AppendLine("using System.Collections.Generic;");
        returnValue.AppendLine("using System.Collections.ObjectModel;");
        returnValue.AppendLine("using FusionReactor.SourceGenerators.EnumExtensions;");
        returnValue.AppendLine();
        returnValue.AppendLine($"namespace {node.Namespace};");
        returnValue.AppendLine();
        returnValue.AppendLine($"{node.Access} static partial class {node.Name}Extensions");
        returnValue.AppendLine("{");
        returnValue.AppendLine(stringBuilder.ToString());
        returnValue.AppendLine("}");
        returnValue.AppendLine();

        return CSharpSyntaxTree
            .ParseText(returnValue.ToString())
            .GetRoot()
            .NormalizeWhitespace()
            .ToFullString();
    }
}
