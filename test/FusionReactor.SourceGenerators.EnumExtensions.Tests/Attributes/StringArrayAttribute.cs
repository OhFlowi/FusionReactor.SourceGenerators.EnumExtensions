namespace FusionReactor.SourceGenerators.EnumExtensions.Tests.Attributes;

using System.Diagnostics.CodeAnalysis;

/// <inheritdoc />
[AttributeUsage(AttributeTargets.Enum)]
[SuppressMessage("Style", "IDE0290:Primären Konstruktor verwenden", Justification = "Reviewed.")]
internal sealed class StringArrayAttribute : Attribute
{
    public string[] Data { get; }

    /// <inheritdoc />
    public StringArrayAttribute(string[] data) => this.Data = data;
}
