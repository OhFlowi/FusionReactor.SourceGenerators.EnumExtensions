// <copyright file="NamespaceSymbolExtensions.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

namespace FusionReactor.SourceGenerators.EnumExtensions.Extensions;

using Microsoft.CodeAnalysis;

/// <summary>
/// Provides extension methods for <see cref="INamespaceSymbol"/> objects.
/// </summary>
public static class NamespaceSymbolExtensions
{
    /// <summary>
    /// Gets the full name of the namespace, including parent namespaces.
    /// </summary>
    /// <param name="namespaceSymbol">The namespace symbol.</param>
    /// <param name="fullName">Optional. The initial full name to start with.</param>
    /// <returns>The full name of the namespace.</returns>
    public static string FullNamespace(this INamespaceSymbol namespaceSymbol, string? fullName = null)
    {
        if (namespaceSymbol is null)
        {
            throw new ArgumentNullException(nameof(namespaceSymbol));
        }

        fullName ??= string.Empty;

        if (namespaceSymbol.ContainingNamespace != null)
        {
            fullName = namespaceSymbol.ContainingNamespace.FullNamespace(fullName);
        }

        if (!string.IsNullOrEmpty(fullName))
        {
            fullName += ".";
        }

        fullName += namespaceSymbol.Name;

        return fullName;
    }
}
