using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

internal static class Check
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T NotNull<T>(string parameterName, T value)
    {
        if (value == null)
            throw new ArgumentNullException(parameterName);

        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IList<T> NotNullOrEmpty<T>(string parameterName, IList<T> value)
    {
        if (value == null || value.Count == 0)
            throw new ArgumentNullOrEmptyException(parameterName);

        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string NotNullOrEmpty(string parameterName, string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullOrEmptyException(parameterName);

        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string NotNullOrWhiteSpace(string parameterName, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullOrWhiteSpaceException(parameterName);

        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Range<T>(string parameterName, T value, T min, T max)
        where T : struct, IComparable<T>
    {
        if (value.CompareTo(min) == -1)
            throw new ArgumentOutOfRangeException(parameterName, $"The parameter {parameterName} must not be less than {min}.");

        if (value.CompareTo(max) == 1)
            throw new ArgumentOutOfRangeException(parameterName, $"Argument {parameterName} must not be greater than {max}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Slice<T>(string parameterName, IList<T> value, int start, int count)
    {
        if (start < 0)
            throw new ArgumentOutOfRangeException(nameof(start), $"Argument {start} must be greater than zero.");

        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(start), $"Argument {count} must be greater than zero.");

        if (start >= value.Count)
        {
            throw new ArgumentOutOfRangeException(parameterName,
                            $"Argument {parameterName} has less items than required for the staring item at index {start}");
        }

        if (start + count > value.Count)
        {
            throw new ArgumentOutOfRangeException(parameterName,
                            $"Argument {parameterName} requires at least {start + count} items. It has {value.Count}");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Count<T>(string parameterName, IList<T> value, int count)
    {
        if (value.Count > count)
        {
            throw new ArgumentOutOfRangeException(
                parameterName, $"Argument {parameterName} must have exactly {count} items.");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Max<T>(string parameterName, IList<T> value, int max)
    {
        if (value.Count > max)
        {
            throw new ArgumentOutOfRangeException(
                parameterName, $"Argument {parameterName} must be not exceed {max}.");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Min<T>(string parameterName, IList<T> value, int min)
    {
        if (value.Count < min)
        {
            throw new ArgumentOutOfRangeException(
                parameterName, $"Argument {parameterName} must have more than {min} items.");
        }
    }
}
