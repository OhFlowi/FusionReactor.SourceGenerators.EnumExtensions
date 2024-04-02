// The .NET Foundation licenses this file to you under the MIT license.

using FusionReactor.SourceGenerators.EnumExtensions.Constants;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace FusionReactor.SourceGenerators.EnumExtensions.Extensions;

/// <summary>
/// Contains extension methods for the <see cref="IncrementalGeneratorInitializationContext"/> class.
/// </summary>
public static class IncrementalGeneratorInitializationContextExtensions
{
    /// <summary>
    /// Adds an enum fast attribute to the generator execution context.
    /// </summary>
    /// <param name="context">The generator execution context.</param>
    public static void AddGenerateEnumExtensionsAttribute(this IncrementalGeneratorInitializationContext context)
    {
        var content = CSharpSyntaxTree
            .ParseText(GenerateEnumExtensionsAttributeConstants.Content)
            .GetRoot()
            .NormalizeWhitespace()
            .ToFullString();

        context.RegisterPostInitializationOutput(
            initializationContext =>
                initializationContext.AddSource(
                    GenerateEnumExtensionsAttributeConstants.Name + "Attribute.g.cs",
                    SourceText.From(
                        content,
                        Encoding.UTF8)));
    }

    /// <summary>
    /// Adds an enum fast attribute to the generator execution context.
    /// </summary>
    /// <param name="context">The generator execution context.</param>
    public static void AddDisplayResult(this IncrementalGeneratorInitializationContext context)
    {
        var content = CSharpSyntaxTree
            .ParseText(DisplayResultConstants.Content)
            .GetRoot()
            .NormalizeWhitespace()
            .ToFullString();

        context.RegisterPostInitializationOutput(
            initializationContext =>
                initializationContext.AddSource(
                    DisplayResultConstants.Name + ".g.cs",
                    SourceText.From(
                        content,
                        Encoding.UTF8)));
    }
}
