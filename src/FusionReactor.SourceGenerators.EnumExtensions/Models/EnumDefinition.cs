
namespace FusionReactor.SourceGenerators.EnumExtensions.Models;

/// <summary>
/// Represents the definition of an enumeration.
/// </summary>
public struct EnumDefinition : IEquatable<EnumDefinition>
{
    /// <summary>
    /// Gets the namespace of the enumeration.
    /// </summary>
    public string Namespace { get; set; }

    /// <summary>
    /// Gets the access declaration of the enumeration.
    /// </summary>
    public string Access { get; set; }

    /// <summary>
    /// Gets the name of the enumeration.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets the underlying type of the enumeration.
    /// </summary>
    public string UnderlyingType { get; set; }

    /// <summary>
    /// Gets or sets a value, that indicates, whenever the enum has the <see cref="FlagsAttribute"/> or not.
    /// </summary>
    public bool HasFlags { get; set; }

    /// <summary>
    /// Gets the list of members in the enumeration.
    /// </summary>
    public IReadOnlyList<EnumMember> Members { get; set; }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is EnumDefinition definition && Equals(definition);

    /// <inheritdoc />
    public bool Equals(EnumDefinition other)
        => Namespace == other.Namespace
           && Access == other.Access
           && Name == other.Name
           && EqualityComparer<IReadOnlyList<EnumMember>>.Default.Equals(
               Members,
               other.Members);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hashCode = -1544219359;

        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Namespace);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Access);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
        hashCode = hashCode * -1521134295 + EqualityComparer<IReadOnlyList<EnumMember>>.Default.GetHashCode(Members);

        return hashCode;
    }

    public static bool operator ==(EnumDefinition left, EnumDefinition right) => left.Equals(right);

    public static bool operator !=(EnumDefinition left, EnumDefinition right) => !(left == right);
}
