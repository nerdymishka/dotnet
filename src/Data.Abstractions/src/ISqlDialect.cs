using System;
using System.Data;
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

        string Write(object value);

        string WriteNull();

        string Write(string value);

        string Write(bool value);

        string Write(decimal value);

        string Write(double value);

        string Write(char[] value);

        string Write(byte[] value);

        string Write(short value);

        string Write(int value);

        string Write(long value);

        string Write(Guid value);

        string Write(DateTime datetime, DbType type = DbType.DateTime);

        string Write(DateTimeOffset value, DbType type = DbType.DateTimeOffset);

        string Write(TimeSpan value, DbType type = DbType.Int64);

        ISqlBuilder CreateBuilder();
    }
}