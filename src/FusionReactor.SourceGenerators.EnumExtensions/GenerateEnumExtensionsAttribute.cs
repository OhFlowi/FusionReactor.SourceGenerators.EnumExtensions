// <copyright file="GenerateEnumExtensionsAttribute.cs" company="OhFlowi">
// Copyright (c) OhFlowi. All rights reserved.
// </copyright>

namespace FusionReactor.SourceGenerators.EnumExtensions;

using System;

/// <summary>
/// Attribute to indicate that extensions should be generated for the decorated enum.
/// </summary>
[AttributeUsage(AttributeTargets.Enum)]
public sealed class GenerateEnumExtensionsAttribute : Attribute
{
}
