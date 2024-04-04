namespace FusionReactor.SourceGenerators.EnumExtensions.Tests.Enums;

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

[GenerateEnumExtensions]
[SuppressMessage("Style", "IDE0040:Add accessibility modifiers", Justification = "Reviewed.")]
// ReSharper disable once ArrangeTypeModifiers
enum EPrivateFoo
{
    [Display(
        ShortName = "Fo",
        Name = "Foo - 0",
        Description = "Zero",
        Prompt = "ooF",
        GroupName = "Foos",
        Order = 0)]
    Foo = 0,

    [Display(
        ShortName = "Ba",
        Name = "Bar - 1",
        Description = "One",
        Prompt = "raB",
        GroupName = "Bars",
        Order = 1)]
    Bar = 1,

    Batz = 2,
}
