﻿using System.Diagnostics.CodeAnalysis;

namespace Turret.Api.Utils;

public static class StringExtensions
{
    public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? s)
    {
        return string.IsNullOrWhiteSpace(s);
    }

    public static bool IsNullOrEmpty([NotNullWhen(false)]  string? s)
    {
        return string.IsNullOrEmpty(s);
    }
}