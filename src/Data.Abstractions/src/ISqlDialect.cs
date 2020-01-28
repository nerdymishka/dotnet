using System;
using System.Data;
using NerdyMishka.Text;

namespace NerdyMishka.Data
{
    /// <summary>
    /// A contract that represents a SQL dialect such as T-SQL.
    /// </summary>
    public interface ISqlDialect
    {
        /// <summary>
        /// Gets the name of the dialect.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the parameter prefix token.
        /// </summary>
        /// <value>
        /// The parameter prefix token.
        /// </value>
        char ParameterPrefixToken { get; }

        /// <summary>
        /// Gets the left identifier escape token.
        /// </summary>
        /// <value>
        /// The left identifier token.
        /// </value>
        char LeftIdentifierEscapeToken { get; }

        /// <summary>
        /// Gets the right identifier escape token.
        /// </summary>
        /// <value>
        /// The right identifier token.
        /// </value>
        char RightIdentifierEscapeToken { get; }

        /// <summary>
        /// Gets the right escape token.
        /// </summary>
        /// <value>
        /// The right escape token.
        /// </value>
        char RightEscapeToken { get; }

        /// <summary>
        /// Gets the left escape token.
        /// </summary>
        /// <value>
        /// The left escape token.
        /// </value>
        char LeftEscapeToken { get; }

        /// <summary>
        /// Writes the object as a SQL literal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The SQL literal.</returns>
        string Write(object value);

        /// <summary>
        /// Writes a SQL literal for NULL.
        /// </summary>
        /// <returns>The SQL literal.</returns>
        string WriteNull();

        /// <summary>
        /// Writes the <see cref="string"/> value as a SQL literal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The SQL literal.</returns>
        string Write(string value);

        /// <summary>
        /// Writes the <see cref="bool"/> value as a SQL literal.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>The SQL literal.</returns>
        string Write(bool value);

        /// <summary>
        /// Writes the <see cref="decimal"/> value as a SQL literal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The SQL literal.</returns>
        string Write(decimal value);

        /// <summary>
        /// Writes the <see cref="double"/> value as a SQL literal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The SQL literal.</returns>
        string Write(double value);

        /// <summary>
        /// Writes the <see cref="char[]"/> value as a SQL literal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The SQL literal.</returns>
        string Write(char[] value);

        /// <summary>
        /// Writes the <see cref="byte[]"/> value as a SQL literal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The SQL literal.</returns>
        string Write(byte[] value);

        /// <summary>
        /// Writes the <see cref="short"/>  value as a SQL literal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The SQL literal.</returns>
        string Write(short value);

        /// <summary>
        /// Writes the <see cref="int"/> value as a SQL literal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The SQL literal.</returns>
        string Write(int value);

        /// <summary>
        /// Writes the <see cref="long"/> value as a SQL literal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The SQL literal.</returns>
        string Write(long value);

        /// <summary>
        /// Writes the <see cref="Guid"/> value as a SQL literal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The SQL literal.</returns>
        string Write(Guid value);

        /// <summary>
        /// Writes the <see cref="DateTime"/> value as a SQL literal.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="type">The type.</param>
        /// <returns>The SQL literal.</returns>
        string Write(DateTime datetime, DbType type = DbType.DateTime);

        /// <summary>
        /// Writes the <see cref="DateTimeOffset"/> value as a SQL literal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns>The SQL literal.</returns>
        string Write(DateTimeOffset value, DbType type = DbType.DateTimeOffset);

        /// <summary>
        /// Writes the <see cref="TimeSpan"/> value as a SQL literal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns>The SQL literal.</returns>
        string Write(TimeSpan value, DbType type = DbType.Int64);

        /// <summary>
        /// Creates the builder.
        /// </summary>
        /// <returns>The SQL builder.</returns>
        ISqlBuilder CreateBuilder();
    }
}