using System.Text;

namespace NerdyMishka.Text
{
    public class Utf8
    {
        public readonly static Encoding NoBom = new UTF8Encoding(false, true);

        public readonly static Encoding NoBomNoThrow = new UTF8Encoding(false, false);
    }
}