// <copyright file="StringExtensions.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

namespace FusionReactor.SourceGenerators.EnumExtensions.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="string"/> class.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Removes the specified suffix from the end of the source string, if present.
    /// </summary>
    /// <param name="source">The source string from which to remove the suffix.</param>
    /// <param name="trim">The suffix to remove from the end of the <paramref name="source"/> string.</param>
    /// <param name="comparisonType">
    /// An enumeration value that specifies the rules for the comparison.
    /// Defaults to <see cref="StringComparison.Ordinal"/>, which performs a case-sensitive,
    /// culture-insensitive comparison.
    /// </param>
    /// <returns>
    /// The <paramref name="source"/> string with the <paramref name="trim"/> suffix removed
    /// if <paramref name="source"/> ends with <paramref name="trim"/>.
    /// Otherwise, the original <paramref name="source"/> string.
    /// Returns the original <paramref name="source"/> string if <paramref name="source"/>
    /// or <paramref name="trim"/> is <see langword="null"/> or empty.
    /// </returns>
    /// <remarks>
    /// If either <paramref name="source"/> or <paramref name="trim"/> is <see langword="null"/> or an empty string,
    /// the <paramref name="source"/> string is returned unchanged.
    /// The comparison to determine if the string ends with the suffix is performed using the specified <paramref name="comparisonType"/>.
    /// </remarks>
    public static string TrimEnd(
        this string source,
        string trim,
        StringComparison comparisonType = StringComparison.Ordinal)
    {
        if (string.IsNullOrEmpty(source)
            || string.IsNullOrEmpty(trim))
        {
            return source;
        }

        return source.EndsWith(
            trim,
            comparisonType)
            ? source.Substring(
                0,
                source.Length - trim.Length)
            : source;
    }
}
