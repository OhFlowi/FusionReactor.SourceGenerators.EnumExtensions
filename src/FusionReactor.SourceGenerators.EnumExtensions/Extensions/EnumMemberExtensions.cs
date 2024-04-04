// <copyright file="EnumMemberExtensions.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

namespace FusionReactor.SourceGenerators.EnumExtensions.Extensions;

using FusionReactor.SourceGenerators.EnumExtensions.Models;

/// <summary>
/// Provides extension methods for <see cref="EnumMember"/> objects.
/// </summary>
public static class EnumMemberExtensions
{
    /// <summary>
    /// Extracts the value of the specified property from the DisplayAttribute of an enum member.
    /// </summary>
    /// <param name="enumMember">The enum member from which to extract the value.</param>
    /// <param name="property">The property to extract.</param>
    /// <param name="value">When this method returns, contains the extracted value, if found; otherwise, null.</param>
    /// <returns>True if the value was successfully extracted; otherwise, false.</returns>
    public static bool ExtractStringValue(this EnumMember enumMember, string property, out string? value)
    {
        value = null;

        var attribute = enumMember
            .Attributes
            .FirstOrDefault(x => x.AttributeClass is { Name: "DisplayAttribute" });

        if (attribute is null)
        {
            return false;
        }

        var nameArgument = attribute
            .NamedArguments
            .FirstOrDefault(x => x.Key == property);

        if (string.IsNullOrWhiteSpace(nameArgument.Value.Value?.ToString()))
        {
            return false;
        }

        value = nameArgument.Value.Value?.ToString();

        return true;
    }

    /// <summary>
    /// Extracts the value of the specified property from the DisplayAttribute of an enum member.
    /// </summary>
    /// <param name="enumMember">The enum member from which to extract the value.</param>
    /// <param name="property">The property to extract.</param>
    /// <param name="value">When this method returns, contains the extracted value, if found; otherwise, null.</param>
    /// <returns>True if the value was successfully extracted; otherwise, false.</returns>
    public static bool ExtractIntValue(this EnumMember enumMember, string property, out int? value)
    {
        value = null;

        var attribute = enumMember
            .Attributes
            .FirstOrDefault(x => x.AttributeClass is { Name: "DisplayAttribute" });

        if (attribute is null)
        {
            return false;
        }

        var nameArgument = attribute
            .NamedArguments
            .FirstOrDefault(x => x.Key == property);

        if (string.IsNullOrWhiteSpace(nameArgument.Value.Value?.ToString())
            || !int.TryParse(nameArgument.Value.Value?.ToString(), out var intValue))
        {
            return false;
        }

        value = intValue;

        return true;
    }
}
