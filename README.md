[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square)](https://raw.githubusercontent.com/OhFlowi/FusionReactor.SourceGenerators.EnumExtensions/master/LICENSE)
[![Nuget](https://img.shields.io/nuget/dt/FusionReactor.SourceGenerators.EnumExtensions?label=Nuget.org%20Downloads&style=flat-square&color=blue)](https://www.nuget.org/packages/FusionReactor.SourceGenerators.EnumExtensions)
[![Nuget](https://img.shields.io/nuget/vpre/FusionReactor.SourceGenerators.EnumExtensions.svg?label=NuGet)](https://www.nuget.org/packages/FusionReactor.SourceGenerators.EnumExtensions)

# FusionReactor.SourceGenerators.EnumExtensions
# The best Source Generator for working with enums in C#
A C# source generator to create extensions for an enum type.
The extensions should be very fast and without reflections.

**Package** - [FusionReactor.SourceGenerators.EnumExtensions](https://www.nuget.org/packages/FusionReactor.SourceGenerators.EnumExtensions/)

Add the package to your application using

```bash
dotnet add package FusionReactor.SourceGenerators.EnumExtensions
```

Adding the package will automatically add a marker attribute, `[GenerateEnumExtensions]`, to your project.

To use the generator, add the `[GenerateEnumExtensions]` attribute to an enum. For example:

```csharp
[GenerateEnumExtensions]
public enum EPublicFoo
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

```

This will generate a class called `EPublicFooExtensions` (`EPublicFoo` + `Extensions`), which contains a number of helper methods.
For example:

```csharp

/// <summary>
/// Extension methods for the <see cref = "EPublicFoo"/> enum.
/// </summary>
public static class EPublicFooExtensions
{
    /// <summary>
    /// Returns the count of elements in the <see cref = "EPublicFoo"/> enum.
    /// </summary>
    /// <param name = "enumValue">The enum value.</param>
    /// <returns>The count of elements.</returns>
    public static uint Count(this EPublicFoo enumValue)
    {
        return 3;
    }

    /// <summary>
    /// Retrieves all available Names of the specified public enumeration type.
    /// </summary>
    /// <param name = "enumValue">The enumeration value.</param>
    /// <returns>An enumerable collection of <see cref = "EPublicFoo"/> Names.</returns>
    public static IEnumerable<string> Names(this EPublicFoo enumValue)
    {
        return new[]
        {
            "Foo",
            "Bar",
            "Batz",
        };
    }

    /// <summary>
    /// Retrieves all available values of the specified public enumeration type.
    /// </summary>
    /// <param name = "enumValue">The enumeration value.</param>
    /// <returns>An enumerable collection of <see cref = "EPublicFoo"/> values.</returns>
    public static IEnumerable<EPublicFoo> Values(this EPublicFoo enumValue)
    {
        return new[]
        {
            EPublicFoo.Foo,
            EPublicFoo.Bar,
            EPublicFoo.Batz,
        };
    }

    /// <summary>
    /// Determines whether the specified string representation of an enum type contains a valid value of the <see cref = "EPublicFoo"/> enumeration.
    /// </summary>
    /// <param name = "enumValue">The enum value.</param>
    /// <param name = "stringValue">The string representation of the enum value to check.</param>
    /// <returns>
    ///   <c>true</c> if the specified string value is a valid value of the <see cref = "EPublicFoo"/> enumeration; otherwise, <c>false</c>.
    /// </returns>
    public static bool Contains(this EPublicFoo enumValue, string stringValue)
    {
        return stringValue switch
        {
            nameof(EPublicFoo.Foo) => true,
            nameof(EPublicFoo.Bar) => true,
            nameof(EPublicFoo.Batz) => true,
            _ => false
        };
    }

    /// <summary>
    /// Checks whether the <see cref = "EPublicFoo"/> contains the given <see cref = "System.Enum"/> value.
    /// </summary>
    /// <param name = "enumValue">The enum value to check.</param>
    /// <param name = "value">The Enum value to search for.</param>
    /// <returns>True if the enum contains the specified value; otherwise, false.</returns>
    public static bool Contains(this EPublicFoo enumValue, Enum value)
    {
        return value switch
        {
            EPublicFoo.Foo => true,
            EPublicFoo.Bar => true,
            EPublicFoo.Batz => true,
            _ => false
        };
    }

    /// <summary>
    /// Converts the specified <see cref = "EPublicFoo"/> enum value to its corresponding string representation quickly.
    /// </summary>
    /// <param name = "enumValue">The enum value to convert.</param>
    /// <returns>
    /// The string representation of the enum value if it is one of the defined enum values; otherwise, <c>null</c>.
    /// </returns>
    public static string? ToStringFast(this EPublicFoo enumValue)
    {
        return enumValue switch
        {
            EPublicFoo.Foo => nameof(EPublicFoo.Foo),
            EPublicFoo.Bar => nameof(EPublicFoo.Bar),
            EPublicFoo.Batz => nameof(EPublicFoo.Batz),
            _ => null
        };
    }

    /// <summary>
    /// Returns the <see cref = "System.ComponentModel.DataAnnotations.DisplayAttribute.ShortName"/> of the <see cref = "EPublicFoo"/> enum.
    /// </summary>
    /// <param name = "enumValue">The enum value.</param>
    /// <returns>The display name or the enum value.</returns>
    public static string? DisplayShortName(this EPublicFoo enumValue)
    {
        return enumValue switch
        {
            EPublicFoo.Foo => "Fo",
            EPublicFoo.Bar => "Ba",
            EPublicFoo.Batz => "Batz",
            _ => null
        };
    }

    /// <summary>
    /// Returns the <see cref = "System.ComponentModel.DataAnnotations.DisplayAttribute.Name"/> of the <see cref = "EPublicFoo"/> enum.
    /// </summary>
    /// <param name = "enumValue">The enum value.</param>
    /// <returns>The name or the enum value.</returns>
    public static string? DisplayName(this EPublicFoo enumValue)
    {
        return enumValue switch
        {
            EPublicFoo.Foo => "Foo - 0",
            EPublicFoo.Bar => "Bar - 1",
            EPublicFoo.Batz => "Batz",
            _ => null
        };
    }

    /// <summary>
    /// Returns the <see cref = "System.ComponentModel.DataAnnotations.DisplayAttribute.Description"/> of the <see cref = "EPublicFoo"/> enum.
    /// </summary>
    /// <param name = "enumValue">The enum value.</param>
    /// <returns>The display name or the enum value.</returns>
    public static string? DisplayDescription(this EPublicFoo enumValue)
    {
        return enumValue switch
        {
            EPublicFoo.Foo => "Zero",
            EPublicFoo.Bar => "One",
            EPublicFoo.Batz => null,
            _ => null
        };
    }

    /// <summary>
    /// Returns the <see cref = "System.ComponentModel.DataAnnotations.DisplayAttribute.Prompt"/> of the <see cref = "EPublicFoo"/> enum.
    /// </summary>
    /// <param name = "enumValue">The enum value.</param>
    /// <returns>The display name or the enum value.</returns>
    public static string? DisplayPrompt(this EPublicFoo enumValue)
    {
        return enumValue switch
        {
            EPublicFoo.Foo => "ooF",
            EPublicFoo.Bar => "raB",
            EPublicFoo.Batz => null,
            _ => null
        };
    }

    /// <summary>
    /// Returns the <see cref = "System.ComponentModel.DataAnnotations.DisplayAttribute.GroupName"/> of the <see cref = "EPublicFoo"/> enum.
    /// </summary>
    /// <param name = "enumValue">The enum value.</param>
    /// <returns>The display name or the enum value.</returns>
    public static string? DisplayGroupName(this EPublicFoo enumValue)
    {
        return enumValue switch
        {
            EPublicFoo.Foo => "Foos",
            EPublicFoo.Bar => "Bars",
            EPublicFoo.Batz => null,
            _ => null
        };
    }

    /// <summary>
    /// Returns the <see cref = "System.ComponentModel.DataAnnotations.DisplayAttribute.Order"/> of the <see cref = "EPublicFoo"/> enum.
    /// </summary>
    /// <param name = "enumValue">The enum value.</param>
    /// <returns>The display name or the enum value.</returns>
    public static int? DisplayOrder(this EPublicFoo enumValue)
    {
        return enumValue switch
        {
            EPublicFoo.Foo => 0,
            EPublicFoo.Bar => 1,
            EPublicFoo.Batz => null,
            _ => null
        };
    }

    /// <summary>
    /// Generates a read-only dictionary containing <see cref = "System.ComponentModel.DataAnnotations.DisplayAttribute.ShortName"/> corresponding to values of the <see cref = "EPublicFoo"/> enumeration.
    /// </summary>
    /// <param name = "enumValue">The value of the <see cref = "EPublicFoo"/> enumeration.</param>
    /// <returns>A read-only dictionary containing <see cref = "System.ComponentModel.DataAnnotations.DisplayAttribute.ShortName"/> for each value of the <see cref = "EPublicFoo"/> enumeration.</returns>
    public static IReadOnlyDictionary<EPublicFoo, string> DisplayShortNameDictionary(this EPublicFoo enumValue)
    {
        return new ReadOnlyDictionary<EPublicFoo, string>(new Dictionary<EPublicFoo, string> { { EPublicFoo.Foo, "Fo" }, { EPublicFoo.Bar, "Ba" }, });
    }

    /// <summary>
    /// Generates a read-only dictionary containing <see cref = "System.ComponentModel.DataAnnotations.DisplayAttribute.Name"/> corresponding to values of the <see cref = "EPublicFoo"/> enumeration.
    /// </summary>
    /// <param name = "enumValue">The value of the <see cref = "EPublicFoo"/> enumeration.</param>
    /// <returns>A read-only dictionary containing <see cref = "System.ComponentModel.DataAnnotations.DisplayAttribute.Name"/> for each value of the <see cref = "EPublicFoo"/> enumeration.</returns>
    public static IReadOnlyDictionary<EPublicFoo, string> DisplayNameDictionary(this EPublicFoo enumValue)
    {
        return new ReadOnlyDictionary<EPublicFoo, string>(new Dictionary<EPublicFoo, string> { { EPublicFoo.Foo, "Foo - 0" }, { EPublicFoo.Bar, "Bar - 1" }, });
    }

    /// <summary>
    /// Generates a read-only dictionary containing <see cref = "System.ComponentModel.DataAnnotations.DisplayAttribute.Description"/> corresponding to values of the <see cref = "EPublicFoo"/> enumeration.
    /// </summary>
    /// <param name = "enumValue">The value of the <see cref = "EPublicFoo"/> enumeration.</param>
    /// <returns>A read-only dictionary containing <see cref = "System.ComponentModel.DataAnnotations.DisplayAttribute.Description"/> for each value of the <see cref = "EPublicFoo"/> enumeration.</returns>
    public static IReadOnlyDictionary<EPublicFoo, string> DisplayDescriptionDictionary(this EPublicFoo enumValue)
    {
        return new ReadOnlyDictionary<EPublicFoo, string>(new Dictionary<EPublicFoo, string> { { EPublicFoo.Foo, "Zero" }, { EPublicFoo.Bar, "One" }, });
    }

    /// <summary>
    /// Generates a read-only dictionary containing <see cref = "System.ComponentModel.DataAnnotations.DisplayAttribute.Prompt"/> corresponding to values of the <see cref = "EPublicFoo"/> enumeration.
    /// </summary>
    /// <param name = "enumValue">The value of the <see cref = "EPublicFoo"/> enumeration.</param>
    /// <returns>A read-only dictionary containing <see cref = "System.ComponentModel.DataAnnotations.DisplayAttribute.Prompt"/> for each value of the <see cref = "EPublicFoo"/> enumeration.</returns>
    public static IReadOnlyDictionary<EPublicFoo, string> DisplayPromptDictionary(this EPublicFoo enumValue)
    {
        return new ReadOnlyDictionary<EPublicFoo, string>(new Dictionary<EPublicFoo, string> { { EPublicFoo.Foo, "ooF" }, { EPublicFoo.Bar, "raB" }, });
    }

    /// <summary>
    /// Generates a read-only dictionary containing <see cref = "System.ComponentModel.DataAnnotations.DisplayAttribute.GroupName"/> corresponding to values of the <see cref = "EPublicFoo"/> enumeration.
    /// </summary>
    /// <param name = "enumValue">The value of the <see cref = "EPublicFoo"/> enumeration.</param>
    /// <returns>A read-only dictionary containing <see cref = "System.ComponentModel.DataAnnotations.DisplayAttribute.GroupName"/> for each value of the <see cref = "EPublicFoo"/> enumeration.</returns>
    public static IReadOnlyDictionary<EPublicFoo, string> DisplayGroupNameDictionary(this EPublicFoo enumValue)
    {
        return new ReadOnlyDictionary<EPublicFoo, string>(new Dictionary<EPublicFoo, string> { { EPublicFoo.Foo, "Foos" }, { EPublicFoo.Bar, "Bars" }, });
    }

    /// <summary>
    /// Generates a read-only dictionary containing <see cref = "System.ComponentModel.DataAnnotations.DisplayAttribute.Order"/> corresponding to values of the <see cref = "EPublicFoo"/> enumeration.
    /// </summary>
    /// <param name = "enumValue">The value of the <see cref = "EPublicFoo"/> enumeration.</param>
    /// <returns>A read-only dictionary containing <see cref = "System.ComponentModel.DataAnnotations.DisplayAttribute.Order"/> for each value of the <see cref = "EPublicFoo"/> enumeration.</returns>
    public static IReadOnlyDictionary<EPublicFoo, int> DisplayOrderDictionary(this EPublicFoo enumValue)
    {
        return new ReadOnlyDictionary<EPublicFoo, int>(new Dictionary<EPublicFoo, int> { { EPublicFoo.Foo, 0 }, { EPublicFoo.Bar, 1 }, });
    }
}
```

The generated extension files are available in your IDE under the Source Generators files.

## Contributing

Create an [issue](https://github.com/OhFlowi/FusionReactor.SourceGenerators.EnumExtensions/issues/new) if you find a BUG or have a Suggestion or Question. If you want to develop this project :

1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request

## License

FusionReactor.SourceGenerators.EnumExtensions is Copyright © 2024 [OhFlowi](https://github.com/OhFlowi) under the MIT License.
