using Microsoft.CodeAnalysis;

namespace FusionReactor.SourceGenerators.EnumExtensions.Models;

/// <summary>
/// Represents an enum member.
/// </summary>
public struct EnumMember : IEquatable<EnumMember>
{
    /// <summary>
    /// Gets or initializes the name of the enum member.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or initializes the value of the enum member.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Gets or initializes the list of attributes associated with the enum member.
    /// </summary>
    public IReadOnlyList<AttributeData> Attributes { get; set; }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is EnumMember member && Equals(member);

    /// <inheritdoc />
    public bool Equals(EnumMember other)
        => Name == other.Name
           && EqualityComparer<IReadOnlyList<AttributeData>>.Default.Equals(Attributes, other.Attributes);

    /// <inheritdoc />
    public override readonly int GetHashCode()
    {
        var hashCode = -325042518;

        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
        hashCode = hashCode * -1521134295 + EqualityComparer<IReadOnlyList<AttributeData>>.Default.GetHashCode(Attributes);

        return hashCode;
    }

    public static bool operator ==(EnumMember left, EnumMember right) => left.Equals(right);
    public static bool operator !=(EnumMember left, EnumMember right) => !(left == right);
}
