// <copyright file="EnumMember.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

namespace FusionReactor.SourceGenerators.EnumExtensions.Models;

using Microsoft.CodeAnalysis;

/// <summary>
/// Represents an enum member.
/// </summary>
public struct EnumMember : IEquatable<EnumMember>
{
    /// <summary>
    /// Gets or sets or initializes the name of the enum member.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets or initializes the value of the enum member.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Gets or sets or initializes the list of attributes associated with the enum member.
    /// </summary>
    public IReadOnlyList<AttributeData> Attributes { get; set; }

    /// <summary>
    /// Determines whether two <see cref="EnumDefinition"/> objects are equal.
    /// </summary>
    /// <param name="left">The first <see cref="EnumDefinition"/> object to compare.</param>
    /// <param name="right">The second <see cref="EnumDefinition"/> object to compare.</param>
    /// <returns>
    /// <c>true</c> if the specified objects are equal; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator ==(EnumMember left, EnumMember right) => left.Equals(right);

    /// <summary>
    /// Determines whether two <see cref="EnumDefinition"/> objects are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="EnumDefinition"/> object to compare.</param>
    /// <param name="right">The second <see cref="EnumDefinition"/> object to compare.</param>
    /// <returns>
    /// <c>true</c> if the specified objects are not equal; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator !=(EnumMember left, EnumMember right) => !(left == right);

    /// <inheritdoc />
    public override readonly bool Equals(object? obj) => obj is EnumMember member && this.Equals(member);

    /// <inheritdoc />
    public readonly bool Equals(EnumMember other)
        => this.Name == other.Name
           && EqualityComparer<IReadOnlyList<AttributeData>>.Default.Equals(this.Attributes, other.Attributes);

    /// <inheritdoc />
    public override readonly int GetHashCode()
    {
        var hashCode = -325042518;

        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.Name);
        hashCode = (hashCode * -1521134295) + EqualityComparer<IReadOnlyList<AttributeData>>.Default.GetHashCode(this.Attributes);

        return hashCode;
    }
}
