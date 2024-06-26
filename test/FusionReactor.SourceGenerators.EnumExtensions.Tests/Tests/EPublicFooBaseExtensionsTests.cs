namespace FusionReactor.SourceGenerators.EnumExtensions.Tests.Tests;
using System.Collections.Frozen;
using FusionReactor.SourceGenerators.EnumExtensions.Tests.Enums;

public class EPublicFooBaseExtensionsTests
{
    private static readonly IReadOnlyDictionary<EPublicFoo, int> ExpectedContent
        = new Dictionary<EPublicFoo, int>
            {
                { EPublicFoo.Foo, 0 },
                { EPublicFoo.Bar, 1 },
                { EPublicFoo.Batz, 2 },
            }
            .ToFrozenDictionary();
    private static readonly FrozenSet<string> ExpectedNames = new[]
    {
        "Foo",
        "Bar",
        "Batz",
    }
    .ToFrozenSet();
    private static readonly FrozenSet<EPublicFoo> ExpectedValues = new[]
    {
        EPublicFoo.Foo,
        EPublicFoo.Bar,
        EPublicFoo.Batz,
    }
    .ToFrozenSet();

    [Fact]
    public void GetContentReturnsExpectedDictionary()
    {
        var content = EPublicFooExtensions.GetContent();

        Assert.NotNull(content);
        Assert.NotEmpty(content);
        Assert.Equal(ExpectedContent, content);
    }

    [Fact]
    public void ThisGetContentReturnsExpectedDictionary()
    {
        var content = EPublicFoo.Foo.GetContent();

        Assert.NotNull(content);
        Assert.NotEmpty(content);
        Assert.Equal(ExpectedContent, content);
    }

    [Theory]
    [InlineData(EPublicFoo.Foo, "Foo")]
    [InlineData(EPublicFoo.Bar, "Bar")]
    [InlineData(EPublicFoo.Batz, "Batz")]
    public void GetNameReturnsExpectedString(EPublicFoo enumValue, string? expectedString) => Assert.Equal(expectedString, enumValue.GetName());

    [Fact]
    public void GetNamesReturnsExpectedEnumerable()
    {
        var names = EPublicFooExtensions.GetNames();

        Assert.NotNull(names);
        Assert.NotEmpty(names);
        Assert.Equal(ExpectedNames, names);
    }

    [Fact]
    public void ThisGetNamesReturnsExpectedEnumerable()
    {
        var names = EPublicFoo.Foo.GetNames();

        Assert.NotNull(names);
        Assert.NotEmpty(names);
        Assert.Equal(ExpectedNames, names);
    }

    [Fact]
    public void GetValuesReturnsExpectedEnumerable()
    {
        var values = EPublicFooExtensions.GetValues();

        Assert.NotNull(values);
        Assert.NotEmpty(values);
        Assert.Equal(ExpectedValues, values);
    }

    [Fact]
    public void ThisGetValuesReturnsExpectedEnumerable()
    {
        var values = EPublicFoo.Foo.GetValues();

        Assert.NotNull(values);
        Assert.NotEmpty(values);
        Assert.Equal(ExpectedValues, values);
    }

    /*
    [Theory]
    [InlineData(EPublicFoo.Foo, EPublicFoo.Foo, true)]
    [InlineData(EPublicFoo.Foo, EPublicFoo.Bar, false)]
    [InlineData(EPublicFoo.Bar, EPublicFoo.Bar, true)]
    [InlineData(EPublicFoo.Bar, EPublicFoo.Foo, false)]
    [InlineData(EPublicFoo.Bar, EPublicFoo.Batz, false)]
    public void HasFlagFastReturnsCorrectValue(EPublicFoo enumValue, EPublicFoo flag, bool expectedResult)
    {
        var result = enumValue.HasFlagFast(flag);

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(EPublicFoo.Foo | EPublicFoo.Bar, EPublicFoo.Foo, true)]
    [InlineData(EPublicFoo.Foo | EPublicFoo.Bar, EPublicFoo.Bar, true)]
    [InlineData(EPublicFoo.Foo | EPublicFoo.Bar, EPublicFoo.Batz, false)]
    public void HasFlagFastReturnsCorrectValueFlagValues(EPublicFoo enumValue, EPublicFoo flag, bool expectedResult)
    {
        var result = enumValue.HasFlagFast(flag);

        Assert.Equal(expectedResult, result);
    }
    */

    [Theory]
    [InlineData("Foo", EPublicFoo.Foo, true)]
    [InlineData("foo", EPublicFoo.Foo, false)]
    [InlineData("Bar", EPublicFoo.Bar, true)]
    [InlineData("bar", EPublicFoo.Bar, false)]
    [InlineData("Batz", EPublicFoo.Batz, true)]
    [InlineData("batz", EPublicFoo.Batz, false)]
    public void ParseFastValidValuesReturnsCorrectEnumValue(string value, EPublicFoo expected, bool success)
    {
        if (success)
        {
            var result = EPublicFooExtensions.ParseFast(value);

            Assert.Equal(expected, result);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => EPublicFooExtensions.ParseFast(value));
        }
    }

    [Theory]
    [InlineData("FOO", EPublicFoo.Foo)]
    [InlineData("FOo", EPublicFoo.Foo)]
    [InlineData("bAr", EPublicFoo.Bar)]
    [InlineData("BaR", EPublicFoo.Bar)]
    [InlineData("Batz", EPublicFoo.Batz)]
    [InlineData("BATZ", EPublicFoo.Batz)]
    public void ParseFastValidValuesIgnoreCaseReturnsCorrectEnumValue(string value, EPublicFoo expected)
    {
        var result = EPublicFooExtensions.ParseFast(value, ignoreCase: true);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Foo", EPublicFoo.Foo, true)]
    [InlineData("foo", EPublicFoo.Foo, false)]
    [InlineData("Bar", EPublicFoo.Bar, true)]
    [InlineData("bar", EPublicFoo.Bar, false)]
    [InlineData("Batz", EPublicFoo.Batz, true)]
    [InlineData("batz", EPublicFoo.Batz, false)]
    public void ThisParseFastValidValuesReturnsCorrectEnumValue(string value, EPublicFoo expected, bool success)
    {
        if (success)
        {
            var result = EPublicFoo.Bar.ParseFast(value);

            Assert.Equal(expected, result);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => EPublicFoo.Bar.ParseFast(value));
        }
    }

    [Theory]
    [InlineData("FOO", EPublicFoo.Foo)]
    [InlineData("FOo", EPublicFoo.Foo)]
    [InlineData("bAr", EPublicFoo.Bar)]
    [InlineData("BaR", EPublicFoo.Bar)]
    [InlineData("Batz", EPublicFoo.Batz)]
    [InlineData("BATZ", EPublicFoo.Batz)]
    public void ThisParseFastValidValuesIgnoreCaseReturnsCorrectEnumValue(string value, EPublicFoo expected)
    {
        var result = EPublicFoo.Bar.ParseFast(value, ignoreCase: true);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Foo", EPublicFoo.Foo)]
    [InlineData("foo", EPublicFoo.Foo)]
    [InlineData("FOO", EPublicFoo.Foo)]
    public void TryParseFastCaseInsensitiveSuccess(string value, EPublicFoo expected)
    {
        var result = EPublicFooExtensions.TryParseFast(value, true, out var parsed);

        Assert.True(result);
        Assert.Equal(expected, parsed);
    }

    [Theory]
    [InlineData("Bar", EPublicFoo.Bar)]
    [InlineData("bar", EPublicFoo.Bar)]
    [InlineData("BAR", EPublicFoo.Bar)]
    public void TryParseFastCaseInsensitiveSuccessBar(string value, EPublicFoo expected)
    {
        var result = EPublicFooExtensions.TryParseFast(value, true, out var parsed);

        Assert.True(result);
        Assert.Equal(expected, parsed);
    }

    [Fact]
    public void TryParseFastCaseInsensitiveFailure()
    {
        var result = EPublicFooExtensions.TryParseFast("invalid", true, out var parsed);

        Assert.False(result);
        Assert.Equal(default, parsed);
    }

    [Theory]
    [InlineData("Foo", EPublicFoo.Foo)]
    [InlineData("Bar", EPublicFoo.Bar)]
    [InlineData("Batz", EPublicFoo.Batz)]
    public void TryParseFastCaseSensitiveSuccess(string value, EPublicFoo expected)
    {
        var result = EPublicFooExtensions.TryParseFast(value, false, out var parsed);

        Assert.True(result);
        Assert.Equal(expected, parsed);
    }

    [Fact]
    public void TryParseFastCaseSensitiveFailure()
    {
        var result = EPublicFooExtensions.TryParseFast("invalid", false, out var parsed);

        Assert.False(result);
        Assert.Equal(default, parsed);
    }

    [Theory]
    [InlineData("Foo", EPublicFoo.Foo)]
    [InlineData("foo", EPublicFoo.Foo)]
    [InlineData("FOO", EPublicFoo.Foo)]
    public void ThisTryParseFastCaseInsensitiveSuccess(string value, EPublicFoo expected)
    {
        var result = EPublicFoo.Foo.TryParseFast(value, true, out var parsed);

        Assert.True(result);
        Assert.Equal(expected, parsed);
    }

    [Theory]
    [InlineData("Bar", EPublicFoo.Bar)]
    [InlineData("bar", EPublicFoo.Bar)]
    [InlineData("BAR", EPublicFoo.Bar)]
    public void ThisTryParseFastCaseInsensitiveSuccessBar(string value, EPublicFoo expected)
    {
        var result = EPublicFoo.Foo.TryParseFast(value, true, out var parsed);

        Assert.True(result);
        Assert.Equal(expected, parsed);
    }

    [Fact]
    public void ThisTryParseFastCaseInsensitiveFailure()
    {
        var result = EPublicFoo.Foo.TryParseFast("invalid", true, out var parsed);

        Assert.False(result);
        Assert.Equal(default, parsed);
    }

    [Theory]
    [InlineData("Foo", EPublicFoo.Foo)]
    [InlineData("Bar", EPublicFoo.Bar)]
    [InlineData("Batz", EPublicFoo.Batz)]
    public void ThisTryParseFastCaseSensitiveSuccess(string value, EPublicFoo expected)
    {
        var result = EPublicFoo.Foo.TryParseFast(value, false, out var parsed);

        Assert.True(result);
        Assert.Equal(expected, parsed);
    }

    [Fact]
    public void ThisTryParseFastCaseSensitiveFailure()
    {
        var result = EPublicFoo.Foo.TryParseFast("invalid", false, out var parsed);

        Assert.False(result);
        Assert.Equal(default, parsed);
    }
}
