using FusionReactor.SourceGenerators.EnumExtensions.Tests.Enums;

namespace FusionReactor.SourceGenerators.EnumExtensions.Tests.Tests;

public class EPublicFooDisplayAttributeExtensionsTests
{
    [Theory]
    [InlineData(EPublicFoo.Foo, "Fo")]
    [InlineData(EPublicFoo.Bar, "Ba")]
    [InlineData(EPublicFoo.Batz, "Batz")]
    public void DisplayShortNameShouldReturnCorrectValue(EPublicFoo enumValue, string expected)
    {
        // Act
        var result = enumValue.DisplayShortName();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(EPublicFoo.Foo, "Foo - 0")]
    [InlineData(EPublicFoo.Bar, "Bar - 1")]
    [InlineData(EPublicFoo.Batz, "Batz")]
    public void DisplayNameShouldReturnCorrectValue(EPublicFoo enumValue, string expected)
    {
        // Act
        var result = enumValue.DisplayName();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(EPublicFoo.Foo, "Zero")]
    [InlineData(EPublicFoo.Bar, "One")]
    [InlineData(EPublicFoo.Batz, null)]
    public void DisplayDescriptionShouldReturnCorrectValue(EPublicFoo enumValue, string? expected)
    {
        // Act
        var result = enumValue.DisplayDescription();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(EPublicFoo.Foo, "ooF")]
    [InlineData(EPublicFoo.Bar, "raB")]
    [InlineData(EPublicFoo.Batz, null)]
    public void DisplayPromptShouldReturnCorrectValue(EPublicFoo enumValue, string? expected)
    {
        // Act
        var result = enumValue.DisplayPrompt();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(EPublicFoo.Foo, "Foos")]
    [InlineData(EPublicFoo.Bar, "Bars")]
    [InlineData(EPublicFoo.Batz, null)]
    public void DisplayGroupNameShouldReturnCorrectValue(EPublicFoo enumValue, string? expected)
    {
        // Act
        var result = enumValue.DisplayGroupName();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(EPublicFoo.Foo, 0)]
    [InlineData(EPublicFoo.Bar, 1)]
    [InlineData(EPublicFoo.Batz, null)]
    public void DisplayOrderShouldReturnCorrectValue(EPublicFoo enumValue, int? expected)
    {
        // Act
        var result = enumValue.DisplayOrder();

        // Assert
        Assert.Equal(expected, result);
    }
}
