namespace FusionReactor.SourceGenerators.EnumExtensions.Tests.Attributes;

using System.Diagnostics.CodeAnalysis;

/// <inheritdoc />
[AttributeUsage(AttributeTargets.Enum)]
[SuppressMessage("Style", "IDE0290:Primären Konstruktor verwenden", Justification = "Reviewed.")]
internal sealed class IntArrayAttribute : Attribute
{
    public int[] Data { get; }

    /// <inheritdoc />
    public IntArrayAttribute(int[] data) => this.Data = data;
}
