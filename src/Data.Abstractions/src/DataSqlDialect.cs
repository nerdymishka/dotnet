using System;
using System.Data;
using System.Globalization;
using System.Text;

namespace NerdyMishka.Data
{
    public class DataSqlDialect : ISqlDialect
    {
        internal const string SqliteDateTimeOffset = @"'{0:yyyy\-MM\-dd HH\:mm\:ss.FFFFFFFzzz}'";
        internal const string SqliteDateTimeFormat = @"'{0:yyyy\-MM\-dd HH\:mm\:ss.FFFFFFF}'";

        public virtual string Name => "Sqlite";

        public char ParameterPrefixToken => '@';

        public char LeftIdentifierEscapeToken => '[';

        public char RightIdentifierEscapeToken => ']';

        public char RightEscapeToken => '\'';

        public char LeftEscapeToken => '\'';

        protected virtual string Null => "NULL";

        public ISqlBuilder CreateBuilder()
            => new DataSqlBuilder(this);

        public string Write(object value)
        {
            if (value is null || value is DBNull)
                return this.Null;

            switch (value)
            {
                case bool bit:
                    return this.Write(bit);
                case decimal dec:
                    return this.Write(dec);
            }

            throw new NotSupportedException(value.GetType().FullName);
        }

        public string Write(string value)
            => value;

        public virtual string Write(bool value)
            => value ? "1" : "0";

        public string Write(decimal value)
            => string.Format(CultureInfo.InvariantCulture, "'{0:0.0###########################}'", value);

        public string Write(double value)
        {
            // https://github.com/dotnet/efcore/blob/master/src/EFCore.Relational/Storage/DoubleTypeMapping.cs
            var doubleValue = Convert.ToDouble(value);
            var literal = doubleValue.ToString("G17", CultureInfo.InvariantCulture);

            return !literal.Contains("E")
                && !literal.Contains("e")
                && !literal.Contains(".")
                && !double.IsNaN(doubleValue)
                && !double.IsInfinity(doubleValue)
                    ? literal + ".0"
                    : literal;
        }

        public string Write(char[] value)
            => new string(value);

        public string Write(byte[] value)
        {
            Check.NotNull(nameof(value), value);

            var stringBuilder = new StringBuilder();
            stringBuilder.Append("X'");

            foreach (var @byte in value)
            {
                stringBuilder.Append(@byte.ToString("X2", CultureInfo.InvariantCulture));
            }

            stringBuilder.Append("'");
            return stringBuilder.ToString();
        }

        public string Write(short value)
            => string.Format(CultureInfo.InvariantCulture, "{0}", value);

        public string Write(int value)
            => string.Format(CultureInfo.InvariantCulture, "{0}", value);

        public string Write(long value)
            => string.Format(CultureInfo.InvariantCulture, "{0}", value);

        public virtual string Write(Guid value)
            => $"'{value}'";

        public string Write(DateTime datetime, DbType type = DbType.DateTime)
        {
            switch (type)
            {
                case DbType.DateTime:
                    return string.Format(
                        CultureInfo.InvariantCulture,
                        SqliteDateTimeFormat,
                        datetime);

                case DbType.Int64:
                    return datetime.ToUniversalTime().Ticks
                        .ToString(CultureInfo.InvariantCulture);

                default:
                    throw new NotSupportedException(type.ToString());
            }
        }

        public string Write(DateTimeOffset value, DbType type = DbType.DateTimeOffset)
        {
            if (type != DbType.DateTimeOffset)
                throw new NotSupportedException(type.ToString());

            return string.Format(
                CultureInfo.InvariantCulture,
                SqliteDateTimeOffset,
                value);
        }

        public string Write(TimeSpan value, DbType type = DbType.Int64)
        {
            if (type != DbType.DateTimeOffset)
                throw new NotSupportedException(type.ToString());

            return this.Write(value.Ticks);
        }

        public string WriteNull()
            => "NULL";
    }
}