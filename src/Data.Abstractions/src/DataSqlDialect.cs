using System;
using System.Data;
using System.Globalization;
using System.Text;

namespace NerdyMishka.Data
{
    /// <summary>
    /// The SQL dialect such as SQLite or T-SQL.
    /// </summary>
    /// <seealso cref="NerdyMishka.Data.ISqlDialect" />
    public class DataSqlDialect : ISqlDialect
    {
        internal const string SqliteDateTimeOffset = @"'{0:yyyy\-MM\-dd HH\:mm\:ss.FFFFFFFzzz}'";
        internal const string SqliteDateTimeFormat = @"'{0:yyyy\-MM\-dd HH\:mm\:ss.FFFFFFF}'";

        /// <summary>
        /// Gets the name of the dialect.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public virtual string Name => "Sqlite";

        /// <summary>
        /// Gets the parameter prefix token.
        /// </summary>
        /// <value>
        /// The parameter prefix token.
        /// </value>
        public char ParameterPrefixToken => ':';

        /// <summary>
        /// Gets the left identifier escape token.
        /// </summary>
        /// <value>
        /// The left identifier token.
        /// </value>
        public char LeftIdentifierEscapeToken => '[';

        /// <summary>
        /// Gets the right identifier escape token.
        /// </summary>
        /// <value>
        /// The right identifier token.
        /// </value>
        public char RightIdentifierEscapeToken => ']';

        /// <summary>
        /// Gets the right escape token.
        /// </summary>
        /// <value>
        /// The right escape token.
        /// </value>
        public char RightEscapeToken => '\'';

        /// <summary>
        /// Gets the left escape token.
        /// </summary>
        /// <value>
        /// The left escape token.
        /// </value>
        public char LeftEscapeToken => '\'';

        /// <summary>
        /// Gets the SQL literal for NULL.
        /// </summary>
        /// <value>
        /// The null.
        /// </value>
        protected virtual string Null => "NULL";

        /// <summary>
        /// Creates the SQL builder.
        /// </summary>
        /// <returns>
        /// The SQL builder.
        /// </returns>
        public ISqlBuilder CreateBuilder()
            => new DataSqlBuilder(this);

        /// <summary>
        /// Writes the object as a SQL literal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The SQL literal.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Throws when the value is not supported for writing a SQL literal.
        /// </exception>
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

        /// <summary>
        /// Writes the <see cref="string" /> value as a SQL literal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The SQL literal.
        /// </returns>
        public string Write(string value)
            => value;

        /// <summary>
        /// Writes the <see cref="bool" /> value as a SQL literal.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>
        /// The SQL literal.
        /// </returns>
        public virtual string Write(bool value)
            => value ? "1" : "0";

        /// <summary>
        /// Writes the <see cref="decimal" /> value as a SQL literal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The SQL literal.
        /// </returns>
        public string Write(decimal value)
            => string.Format(CultureInfo.InvariantCulture, "'{0:0.0###########################}'", value);

        /// <summary>
        /// Writes the <see cref="double" /> value as a SQL literal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The SQL literal.
        /// </returns>
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

        /// <summary>
        /// Writes the <see cref="char[]" /> value as a SQL literal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The SQL literal.
        /// </returns>
        public string Write(char[] value)
            => new string(value);

        /// <summary>
        /// Writes the <see cref="byte[]" /> value as a SQL literal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The SQL literal.
        /// </returns>
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

        /// <summary>
        /// Writes the <see cref="short" />  value as a SQL literal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The SQL literal.
        /// </returns>
        public string Write(short value)
            => string.Format(CultureInfo.InvariantCulture, "{0}", value);

        /// <summary>
        /// Writes the <see cref="int" /> value as a SQL literal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The SQL literal.
        /// </returns>
        public string Write(int value)
            => string.Format(CultureInfo.InvariantCulture, "{0}", value);

        /// <summary>
        /// Writes the <see cref="long" /> value as a SQL literal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The SQL literal.
        /// </returns>
        public string Write(long value)
            => string.Format(CultureInfo.InvariantCulture, "{0}", value);

        /// <summary>
        /// Writes the <see cref="Guid" /> value as a SQL literal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The SQL literal.
        /// </returns>
        public virtual string Write(Guid value)
            => $"'{value}'";

        /// <summary>
        /// Writes the <see cref="DateTime" /> value as a SQL literal.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="type">The type.</param>
        /// <returns>
        /// The SQL literal.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Throws when the type is not supported for converting this
        /// datetime to a given DbType.
        /// </exception>
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

        /// <summary>
        /// Writes the <see cref="DateTimeOffset" /> value as a SQL literal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns>
        /// The SQL literal.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Throws when the type is not supported for converting this
        /// <c>DateTimeOffset</c> to a given DbType.
        /// </exception>
        public string Write(DateTimeOffset value, DbType type = DbType.DateTimeOffset)
        {
            if (type != DbType.DateTimeOffset)
                throw new NotSupportedException(type.ToString());

            return string.Format(
                CultureInfo.InvariantCulture,
                SqliteDateTimeOffset,
                value);
        }

        /// <summary>
        /// Writes the <see cref="TimeSpan" /> value as a SQL literal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns>
        /// The SQL literal.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// Throws when the type is not supported for converting this
        /// <c>TimeSpan</c> to a given DbType.
        /// </exception>
        public string Write(TimeSpan value, DbType type = DbType.Int64)
        {
            if (type != DbType.DateTimeOffset)
                throw new NotSupportedException(type.ToString());

            return this.Write(value.Ticks);
        }

        /// <summary>
        /// Writes a SQL literal for NULL.
        /// </summary>
        /// <returns>
        /// The SQL literal.
        /// </returns>
        public string WriteNull()
            => "NULL";
    }
}