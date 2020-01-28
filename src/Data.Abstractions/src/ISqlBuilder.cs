using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using NerdyMishka.Text;

namespace NerdyMishka.Data
{
    /// <summary>
    /// A contract that represents a string builder for SQL.
    /// </summary>
    public interface ISqlBuilder
    {
        /// <summary>
        /// Gets the SQL dialect.
        /// </summary>
        /// <value>
        /// The SQL dialect.
        /// </value>
        ISqlDialect SqlDialect { get; }

        /// <summary>
        /// Appends the line.
        /// </summary>
        /// <returns>The SQL Builder.</returns>
        ISqlBuilder AppendLine();

        /// <summary>
        /// Appends the line.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The SQL Builder.</returns>
        ISqlBuilder AppendLine(char value);

        /// <summary>
        /// Appends the line.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The SQL Builder.</returns>
        ISqlBuilder AppendLine(string value);

        /// <summary>
        /// Appends the line.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="transform">The transform.</param>
        /// <returns>The SQL Builder.</returns>
        ISqlBuilder AppendLine(string value, ITextTransform transform);

        /// <summary>
        /// Appends the <see cref="ISqlBuilder"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The SQL Builder.</returns>
        ISqlBuilder Append(ISqlBuilder builder);

        /// <summary>
        /// Appends the <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The SQL Builder.</returns>
        ISqlBuilder Append(StringBuilder builder);

        /// <summary>
        /// Appends the <see cref="char"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The SQL Builder.</returns>
        ISqlBuilder Append(char value);

        /// <summary>
        /// Appends the <see cref="string"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The SQL Builder.</returns>
        ISqlBuilder Append(string value);

        /// <summary>
        /// Appends the <see cref="string"/> post transform.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="transform">The transform.</param>
        /// <returns>The SQL Builder.</returns>
        ISqlBuilder Append(string value, ITextTransform transform);

        /// <summary>
        /// Appends the <see cref="bool"/>.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>The SQL Builder.</returns>
        ISqlBuilder Append(bool value);

        /// <summary>
        /// Appends the <see cref="decimal"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The SQL Builder.</returns>
        ISqlBuilder Append(decimal value);

        /// <summary>
        /// Appends the <see cref="double"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The SQL Builder.</returns>
        ISqlBuilder Append(double value);

        /// <summary>
        /// Appends the <see cref="char[]"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The SQL Builder.</returns>
        ISqlBuilder Append(char[] value);

        /// <summary>
        /// Appends the <see cref="byte[]"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The SQL Builder.</returns>
        ISqlBuilder Append(byte[] value);

        /// <summary>
        /// Appends the <see cref="short"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The SQL Builder.</returns>
        ISqlBuilder Append(short value);

        /// <summary>
        /// Appends the <see cref="int"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The SQL Builder.</returns>
        ISqlBuilder Append(int value);

        /// <summary>
        /// Appends the <see cref="long"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The SQL Builder.</returns>
        ISqlBuilder Append(long value);

        /// <summary>
        /// Appends the <see cref="Guid"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The SQL Builder.</returns>
        ISqlBuilder Append(Guid value);

        /// <summary>
        /// Appends the <see cref="DateTime"/> value.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="type">The type.</param>
        /// <returns>The SQL Builder.</returns>
        ISqlBuilder Append(DateTime datetime, DbType type = DbType.DateTime);

        /// <summary>
        /// Appends the <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns>The SQL Builder.</returns>
        ISqlBuilder Append(DateTimeOffset value, DbType type = DbType.DateTimeOffset);

        /// <summary>
        /// Appends the <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns>The SQL Builder.</returns>
        ISqlBuilder Append(TimeSpan value, DbType type = DbType.Int64);

        /// <summary>
        /// Appends the name of the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The SQL Builder.</returns>
        ISqlBuilder AppendParameterName(string name);

        /// <summary>
        /// Appends the identifier.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The SQL Builder.</returns>
        ISqlBuilder AppendIdentifier(params string[] name);

        /// <summary>
        /// Appends the joined string.
        /// </summary>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="names">The names.</param>
        /// <returns>The SQL Builder.</returns>
        ISqlBuilder AppendJoin(string delimiter, params string[] names);

        /// <summary>
        /// Appends the joined string.
        /// </summary>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="names">The names.</param>
        /// <returns>The SQL Builder.</returns>
        ISqlBuilder AppendJoin(string delimiter, IEnumerable<string> names);

        /// <summary>
        /// Converts to stringbuilder.
        /// </summary>
        /// <returns>The string builder.</returns>
        StringBuilder ToStringBuilder();

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="clear">if set to <c>true</c> [clear].</param>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        string ToString(bool clear = false);
    }
}