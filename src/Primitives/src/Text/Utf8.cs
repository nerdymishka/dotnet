using System.Text;

namespace NerdyMishka.Text
{
    /// <summary>
    /// Common UTF-8 encoding options.
    /// </summary>
    public sealed class Utf8
    {
        /// <summary>
        /// Gets an Utf8 encoding instance with no Byte Order Mark ("BOM")
        /// configured.
        /// </summary>
        /// <returns>The encoding.</returns>
        public static readonly Encoding NoBom = new UTF8Encoding(false, true);

        /// <summary>
        /// Gets an UTF-8 encoding instance with no Byte Order Mark ("BOM")
        /// and the 'no throw on invalid bytes' configured.
        /// </summary>
        /// <returns>The encoding.</returns>
        public static readonly Encoding NoBomNoThrow = new UTF8Encoding(false, false);
    }
}