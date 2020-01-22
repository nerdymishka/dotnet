using System.Collections.Generic;
using Mettle;
using Xunit;

namespace Tests
{
    public static class AssertExtensions
    {
        public static IAssert Check(this IAssert assert)
        {
            if (assert is null)
                throw new System.ArgumentNullException(nameof(assert));

            return assert;
        }

        public static IAssert Contains<T>(
            this IAssert instance,
            T expected,
            IEnumerable<T> collection)
        {
            Xunit.Assert.Contains(expected, collection);
            return instance;
        }

        public static IAssert DoesNotContain<T>(
            this IAssert instance,
            T expected,
            IEnumerable<T> collection)
        {
            Xunit.Assert.DoesNotContain(expected, collection);
            return instance;
        }
    }
}