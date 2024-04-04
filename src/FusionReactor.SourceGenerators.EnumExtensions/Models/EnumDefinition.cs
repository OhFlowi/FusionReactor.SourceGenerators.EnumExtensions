// <copyright file="EnumDefinition.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

namespace FusionReactor.SourceGenerators.EnumExtensions.Models;

/// <summary>
/// Represents the definition of an enumeration.
/// </summary>
public struct EnumDefinition : IEquatable<EnumDefinition>
{
    /// <summary>
    /// Gets or sets the namespace of the enumeration.
    /// </summary>
    public string Namespace { get; set; }

    /// <summary>
    /// Gets or sets the access declaration of the enumeration.
    /// </summary>
    public string Access { get; set; }

    /// <summary>
    /// Gets or sets the name of the enumeration.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the underlying type of the enumeration.
    /// </summary>
    public string UnderlyingType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets a value, that indicates, whenever the enum has the <see cref="FlagsAttribute"/> or not.
    /// </summary>
    public bool HasFlags { get; set; }

    /// <summary>
    /// Gets or sets the list of members in the enumeration.
    /// </summary>
    public IReadOnlyList<EnumMember> Members { get; set; }

    /// <summary>
    /// Determines whether two <see cref="EnumDefinition"/> objects are equal.
    /// </summary>
    /// <param name="left">The first <see cref="EnumDefinition"/> object to compare.</param>
    /// <param name="right">The second <see cref="EnumDefinition"/> object to compare.</param>
    /// <returns>
    /// <c>true</c> if the specified objects are equal; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator ==(EnumDefinition left, EnumDefinition right) => left.Equals(right);

    /// <summary>
    /// Determines whether two <see cref="EnumDefinition"/> objects are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="EnumDefinition"/> object to compare.</param>
    /// <param name="right">The second <see cref="EnumDefinition"/> object to compare.</param>
    /// <returns>
    /// <c>true</c> if the specified objects are not equal; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator !=(EnumDefinition left, EnumDefinition right) => !(left == right);

    /// <inheritdoc />
    public override readonly bool Equals(object? obj) => obj is EnumDefinition definition && this.Equals(definition);

    /// <inheritdoc />
    public readonly bool Equals(EnumDefinition other)
        => this.Namespace == other.Namespace
           && this.Access == other.Access
           && this.Name == other.Name
           && EqualityComparer<IReadOnlyList<EnumMember>>.Default.Equals(
               this.Members,
               other.Members);

    /// <inheritdoc />
    public override readonly int GetHashCode()
    {
        var hashCode = -1544219359;

        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.Namespace);
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.Access);
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.Name);
        hashCode = (hashCode * -1521134295) + EqualityComparer<IReadOnlyList<EnumMember>>.Default.GetHashCode(this.Members);

        return hashCode;
    }
}
