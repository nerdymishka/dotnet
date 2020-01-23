using System;
using System.Linq;
using System.Security.Cryptography;

namespace NerdyMishka.Util.Strings
{
    public static class StringExtensions
    {
        public static ReadOnlySpan<char> AsReadOnlySpan(this string instance)
        {
            return (ReadOnlySpan<char>)instance;
        }

        public static bool Equals(
            this string instance,
            string pattern,
            bool ignoreCase)
        {
            if (ignoreCase)
                return string.Equals(
                    instance,
                    pattern,
                    StringComparison.CurrentCultureIgnoreCase);

            return string.Equals(
                    instance,
                    pattern,
                    StringComparison.CurrentCulture);
        }

        public static bool EqualsOrdinal(
            this string instance,
            string pattern,
            bool ignoreCase = true)
        {
            if (ignoreCase)
                return string.Equals(
                    instance,
                    pattern,
                    StringComparison.OrdinalIgnoreCase);

            return string.Equals(
                    instance,
                    pattern,
                    StringComparison.Ordinal);
        }

        public static bool EqualsInvariant(
            this string instance,
            string pattern,
            bool ignoreCase = true)
        {
            if (ignoreCase)
                return String.Equals(
                    instance,
                    pattern,
                    StringComparison.InvariantCultureIgnoreCase);

            return string.Equals(
                instance,
                pattern,
                StringComparison.InvariantCulture);
        }

        public static string Strip(this string instance, string oldValue)
        {
            if (instance is null)
                throw new ArgumentNullException(nameof(instance));

            return instance.Replace(oldValue, "");
        }

        public static string Strip(this string instance, char oldValue)
        {
            if (instance is null)
                throw new ArgumentNullException(nameof(instance));

            return instance.Replace(oldValue, string.Empty);
        }

        public static string Strip(this string instance, params string[] oldValues)
        {
            if (instance is null)
                throw new ArgumentNullException(nameof(instance));

            stirng target = instance;
            foreach (var oldValue in oldValues)
                target = target.Replace(oldValue, "");

            return target;
        }
    }
}
