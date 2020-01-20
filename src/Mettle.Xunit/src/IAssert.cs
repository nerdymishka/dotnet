using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace Mettle
{
    public interface IAssert
    {
        IAssert Ok(bool condition, string message = null);

#pragma warning disable CA1716

        IAssert True(bool condition, string message = null);

        IAssert False(bool condition, string message = null);

#pragma warning restore CA1716

        IAssert Null(object instance);

        IAssert IsType<T>(object instance);

        IAssert IsType(Type expectedType, object instance);

        IAssert IsNotType<T>(object instance);

        IAssert IsNotType(Type expectedType, object instance);

        IAssert IsAssignableFrom<T>(object instance);

        IAssert IsAssignableFrom(Type expectedType, object instance);

        IAssert Throws<T>(Action action)
            where T : Exception;

        IAssert Throws<T>(Func<object> func)
            where T : Exception;

        IAssert Throws(Type expectedType, Action action);

        IAssert Throws(Type expectedType, Func<object> func);

        IAssert ThrowsAny<T>(Action action)
            where T : Exception;

        IAssert ThrowsAny<T>(Func<object> func)
            where T : Exception;

        IAssert Equal<T>(T expected, T actual);

        IAssert Equal(double expected, double actual, int precision);

        IAssert Equal<T>(T expected, T actual, IEqualityComparer<T> comparer);

        IAssert Equal(decimal expected, decimal actual, int precision);

        IAssert Equal(DateTime expected, DateTime actual, TimeSpan precision);

        IAssert Equal<T>(IEnumerable<T> expected,
            IEnumerable<T> actual,
            IEqualityComparer<T> comparer);

        IAssert Equal<T>(IEnumerable<T> expected, IEnumerable<T> actual);

        IAssert Equal(
            string expected,
            string actual,
            bool ignoreCase = false,
            bool ignoreLineEndingDifferences = false,
            bool ignoreWhiteSpaceDifferences = false);

        IAssert Equal(string expected, string actual);

        IAssert NotEmpty(IEnumerable collection);

        IAssert NotEqual<T>(T expected, T actual);

        IAssert NotEqual(decimal expected, decimal actual, int precision);

        IAssert NotEqual(double expected, double actual, int precision);

        IAssert NotEqual<T>(T expected, T actual, IEqualityComparer<T> comparer);

        IAssert NotEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual);

        IAssert NotEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T> comparer);

        IAssert NotNull(object instance);

        IAssert NotSame(object expected, object actual);

        IAssert Same(object expected, object actual);
    }
}