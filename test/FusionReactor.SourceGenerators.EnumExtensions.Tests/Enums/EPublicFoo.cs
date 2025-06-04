// ReSharper disable UseCollectionExpression
#pragma warning disable CA1515
#pragma warning disable IDE0300

namespace FusionReactor.SourceGenerators.EnumExtensions.Tests.Enums;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using FusionReactor.SourceGenerators.EnumExtensions.Tests.Attributes;

[GenerateEnumExtensions]
[Description("Test 123")]
[IntArray(new[] { 123 })]
[IntArray(new[] { 456 })]
[StringArray(new[] { "Foo", "Bar", "Baz" })]
public enum EPublicFoo
{
    [EnumMember]
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
