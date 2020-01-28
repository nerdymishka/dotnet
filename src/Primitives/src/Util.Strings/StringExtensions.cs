using System;
using System.Linq;
using System.Security.Cryptography;

namespace NerdyMishka.Util.Strings
{
    /// <summary>
    /// String extension methods. The extension methods in a specialized
    /// <c>Util</c> namespace so that consumers must opt into using them.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Asserts if the left and right are equal using
        /// <see cref="StringComparison.CurrentCultureIgnoreCase" /> by default.
        /// </summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <param name="ignoreCase">The ingore case instruction.</param>
        /// <returns><c>True</c> when equal; Otherwise, <c>False</c>.</returns>
        public static bool Equals(
            this string left,
            string right,
            bool ignoreCase)
        {
            if (ignoreCase)
                return string.Equals(
                    left,
                    right,
                    StringComparison.CurrentCultureIgnoreCase);

            return string.Equals(
                    left,
                    right,
                    StringComparison.CurrentCulture);
        }

        /// <summary>
        /// Asserts if the left and right are equal using
        /// <see cref="StringComparison.OrdinalIgnoreCase" /> by default. Ordinal
        /// compares the binary values.
        /// </summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <param name="ignoreCase">The ingore case instruction.</param>
        /// <returns><c>True</c> when equal; Otherwise, <c>False</c>.</returns>
        public static bool EqualsOrdinal(
            this string left,
            string right,
            bool ignoreCase = true)
        {
            if (ignoreCase)
                return string.Equals(
                    left,
                    right,
                    StringComparison.OrdinalIgnoreCase);

            return string.Equals(
                    left,
                    right,
                    StringComparison.Ordinal);
        }

        /// <summary>
        /// Asserts if the left and right are equal using
        /// <see cref="StringComparison.InvariantCultureIgnoreCase" /> by default.
        /// </summary>
        /// <param name="left">The left value.</param>
        /// <param name="right">The right value.</param>
        /// <param name="ignoreCase">The ingore case instruction.</param>
        /// <returns><c>True</c> when equal; Otherwise, <c>False</c>.</returns>
        public static bool EqualsInvariant(
            this string left,
            string right,
            bool ignoreCase = true)
        {
            if (ignoreCase)
                return string.Equals(
                    left,
                    right,
                    StringComparison.InvariantCultureIgnoreCase);

            return string.Equals(
                left,
                right,
                StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Strips the given values from the string.
        /// </summary>
        /// <param name="instance">The string instance.</param>
        /// <param name="oldValues">The values to remove.</param>
        /// <returns>A new instance of <see cref="string"/> with the values.</returns>
        public static string Strip(this string instance, params string[] oldValues)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            foreach (var oldValue in oldValues)
                instance = instance.Replace(oldValue, string.Empty);

            return instance;
        }
    }
}
