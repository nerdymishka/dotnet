using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using NerdyMishka.Text;

namespace NerdyMishka.Data
{
    /// <summary>
    /// Builds a SQL literal.
    /// </summary>
    /// <seealso cref="NerdyMishka.Data.ISqlBuilder" />
    public class DataSqlBuilder : ISqlBuilder
    {
        private StringBuilder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSqlBuilder"/> class.
        /// </summary>
        /// <param name="sqlDialect">The SQL dialect.</param>
        /// <param name="builder">The builder.</param>
        public DataSqlBuilder(
            ISqlDialect sqlDialect,
            StringBuilder builder = null)
        {
            this.SqlDialect = sqlDialect;
            this.builder = builder ?? new StringBuilder();
        }

        /// <summary>
        /// Gets the SQL dialect.
        /// </summary>
        /// <value>
        /// The SQL dialect.
        /// </value>
        public ISqlDialect SqlDialect { get; }

        /// <summary>
        /// Appends the <see cref="ISqlBuilder" />.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>
        /// The SQL Builder.
        /// </returns>
        public ISqlBuilder Append(ISqlBuilder builder)
        {
            Check.NotNull(nameof(builder), builder);
            return this.Append(builder);
        }

        /// <summary>
        /// Appends the <see cref="StringBuilder" />.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>
        /// The SQL Builder.
        /// </returns>
        public ISqlBuilder Append(StringBuilder builder)
        {
            Check.NotNull(nameof(builder), builder);
            this.builder.Append(builder);
            return this;
        }

        /// <summary>
        /// Appends the <see cref="char" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The SQL Builder.
        /// </returns>
        public ISqlBuilder Append(char value)
        {
            this.builder.Append(this.SqlDialect.Write(value));
            return this;
        }

        /// <summary>
        /// Appends the <see cref="string" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The SQL Builder.
        /// </returns>
        public ISqlBuilder Append(string value)
        {
            this.builder.Append(value);
            return this;
        }

        /// <summary>
        /// Appends the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="transformer">The transformer.</param>
        /// <returns>The SQL builder.</returns>
        public ISqlBuilder Append(string value, ITextTransform transformer)
        {
            Check.NotNull(nameof(transformer), transformer);
            this.builder.Append(transformer.Transform(value));

            return this;
        }

        /// <summary>
        /// Appends the <see cref="bool" />.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>
        /// The SQL Builder.
        /// </returns>
        public ISqlBuilder Append(bool value)
            => this.Append(this.SqlDialect.Write(value));

        /// <summary>
        /// Appends the <see cref="decimal" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The SQL Builder.
        /// </returns>
        public ISqlBuilder Append(decimal value)
             => this.Append(this.SqlDialect.Write(value));

        /// <summary>
        /// Appends the <see cref="double" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The SQL Builder.
        /// </returns>
        public ISqlBuilder Append(double value)
             => this.Append(this.SqlDialect.Write(value));

        /// <summary>
        /// Appends the <see cref="char[]" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The SQL Builder.
        /// </returns>
        public ISqlBuilder Append(char[] value)
             => this.Append(this.SqlDialect.Write(value));

        /// <summary>
        /// Appends the <see cref="byte[]" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The SQL Builder.
        /// </returns>
        public ISqlBuilder Append(byte[] value)
             => this.Append(this.SqlDialect.Write(value));

        /// <summary>
        /// Appends the <see cref="short" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The SQL Builder.
        /// </returns>
        public ISqlBuilder Append(short value)
             => this.Append(this.SqlDialect.Write(value));

        /// <summary>
        /// Appends the <see cref="int" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The SQL Builder.
        /// </returns>
        public ISqlBuilder Append(int value)
             => this.Append(this.SqlDialect.Write(value));

        /// <summary>
        /// Appends the <see cref="long" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The SQL Builder.
        /// </returns>
        public ISqlBuilder Append(long value)
             => this.Append(this.SqlDialect.Write(value));

        /// <summary>
        /// Appends the <see cref="Guid" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The SQL Builder.
        /// </returns>
        public ISqlBuilder Append(Guid value)
             => this.Append(this.SqlDialect.Write(value));

        /// <summary>
        /// Appends the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns>The SQL builder.</returns>
        public ISqlBuilder Append(DateTime value, DbType type = DbType.DateTime)
             => this.Append(this.SqlDialect.Write(value));

        /// <summary>
        /// Appends the <see cref="DateTimeOffset" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns>
        /// The SQL Builder.
        /// </returns>
        public ISqlBuilder Append(DateTimeOffset value, DbType type = DbType.DateTimeOffset)
            => this.Append(this.SqlDialect.Write(type));

        /// <summary>
        /// Appends the <see cref="TimeSpan" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns>
        /// The SQL Builder.
        /// </returns>
        public ISqlBuilder Append(TimeSpan value, DbType type = DbType.Int64)
            => this.Append(this.SqlDialect.Write(value, type));

        /// <summary>
        /// Appends the joined string.
        /// </summary>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="names">The names.</param>
        /// <returns>
        /// The SQL Builder.
        /// </returns>
        public ISqlBuilder AppendJoin(string delimiter, params string[] names)
            => this.Append(string.Join(delimiter, names));

        /// <summary>
        /// Appends the joined string.
        /// </summary>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="names">The names.</param>
        /// <returns>
        /// The SQL Builder.
        /// </returns>
        public ISqlBuilder AppendJoin(string delimiter, IEnumerable<string> names)
                => this.Append(string.Join(delimiter, names));

        /// <summary>
        /// Appends the identifier.
        /// </summary>
        /// <param name="names">The names.</param>
        /// <returns>The SQL builder.</returns>
        public ISqlBuilder AppendIdentifier(params string[] names)
        {
            for (var i = 0; i < names.Length; i++)
            {
                var name = names[i];
                this.Append(this.SqlDialect.LeftIdentifierEscapeToken);
                this.Append(name);
                this.Append(this.SqlDialect.RightIdentifierEscapeToken);
                if (i < names.Length - 1)
                    this.Append(",");
            }

            return this;
        }

        /// <summary>
        /// Appends the line.
        /// </summary>
        /// <returns>
        /// The SQL Builder.
        /// </returns>
        public ISqlBuilder AppendLine()
        {
            this.builder.AppendLine();
            return this;
        }

        /// <summary>
        /// Appends the line.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The SQL Builder.
        /// </returns>
        public ISqlBuilder AppendLine(char value)
        {
            this.builder.Append(value).AppendLine();
            return this;
        }

        /// <summary>
        /// Appends the line.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The SQL Builder.
        /// </returns>
        public ISqlBuilder AppendLine(string value)
        {
            this.builder.AppendLine(value);
            return this;
        }

        /// <summary>
        /// Appends the line.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="transform">The transform.</param>
        /// <returns>
        /// The SQL Builder.
        /// </returns>
        public ISqlBuilder AppendLine(string value, ITextTransform transform)
        {
            Check.NotNull(nameof(transform), transform);
            this.builder.AppendLine(value);
            return this;
        }

        /// <summary>
        /// Appends the name of the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// The SQL Builder.
        /// </returns>
        public ISqlBuilder AppendParameterName(string name)
            => this.Append(this.SqlDialect.ParameterPrefixToken)
                .Append(name);

        /// <summary>
        /// Converts to <see cref="StringBuilder"/>.
        /// </summary>
        /// <returns>
        /// The string builder.
        /// </returns>
        public StringBuilder ToStringBuilder()
            => this.builder;

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="clear">if set to <c>true</c> [clear].</param>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public string ToString(bool clear = false)
        {
            var state = this.builder.ToString();
            if (clear)
                this.builder.Clear();

            return state;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.builder.ToString();
        }
    }
}