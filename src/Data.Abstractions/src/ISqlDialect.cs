using NerdyMishka.Text;

namespace NerdyMishka.Data
{
    public interface ISqlDialect
    {
        string Name { get; }

        char ParameterPrefixToken { get; }

        char LeftIdentifierToken { get; }

        char RightIdentifierToken { get; }

        char RightEscapeToken { get; }

        char LeftEscapeToken { get; }

        ISqlBuilder CreateBuilder();
    }
}