using System.Text;

namespace NerdyMishka.Text
{
    public sealed class Utf8
    {
        public static readonly Encoding NoBom = new UTF8Encoding(false, true);

        public static readonly Encoding NoBomNoThrow = new UTF8Encoding(false, false);
    }
}