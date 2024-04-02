using FusionReactor.SourceGenerators.EnumExtensions.Models;

using Microsoft.CodeAnalysis;

namespace FusionReactor.SourceGenerators.EnumExtensions.Generators;

public interface IExtensionGeneratorStrategy
{
    string GetMethod(IncrementalGeneratorInitializationContext context, EnumDefinition enumDeclarationSyntax);
}
