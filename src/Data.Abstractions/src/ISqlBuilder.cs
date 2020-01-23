using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using NerdyMishka.Text;

namespace NerdyMishka.Data
{
    public interface ISqlBuilder
    {
        ISqlDialect SqlDialect { get; }

        ISqlBuilder AppendLine();


        ISqlBuilder AppendLine(char value);

        ISqlBuilder AppendLine(string value);

        ISqlBuilder AppendLine(string value, ITextTransform transform);

        ISqlBuilder Append(ISqlBuilder builder);

        ISqlBuilder Append(StringBuilder builder);

        ISqlBuilder Append(char value);

        ISqlBuilder Append(string value);

        ISqlBuilder Append(string value, ITextTransform transform);

        ISqlBuilder Append(bool value);

        ISqlBuilder Append(decimal value);

        ISqlBuilder Append(double value);

        ISqlBuilder Append(char[] value);

        ISqlBuilder Append(byte[] value);

        ISqlBuilder Append(short value);

        ISqlBuilder Append(int value);

        ISqlBuilder Append(long value);

        ISqlBuilder Append(Guid value);

        ISqlBuilder Append(DateTime datetime, DbType type = DbType.DateTime);

        ISqlBuilder Append(DateTimeOffset value, DbType type = DbType.DateTimeOffset);

        ISqlBuilder Append(TimeSpan value, DbType type = DbType.Int64);

        ISqlBuilder AppendParameterName(string name);
        ISqlBuilder AppendIdentity(params string[] name);
        ISqlBuilder AppendJoin(string delimiter, params string[] names);

        ISqlBuilder AppendJoin(string delimiter, IEnumerable<string> names);
        StringBuilder ToStringBuilder();
    }
}