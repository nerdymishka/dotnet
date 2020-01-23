
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using NerdyMishka.Text;

namespace NerdyMishka.Data
{
    public class DataSqlBuilder : ISqlBuilder
    {
        private StringBuilder builder;

        public DataSqlBuilder(
            ISqlDialect sqlDialect,
            StringBuilder builder = null)
        {
            this.SqlDialect = sqlDialect;
            this.builder = builder ?? new StringBuilder();
        }

        public ISqlDialect SqlDialect { get; }

        public ISqlBuilder Append(ISqlBuilder builder)
        {
            Check.NotNull(nameof(builder), builder);
            return this.Append(builder);
        }

        public ISqlBuilder Append(StringBuilder builder)
        {
            Check.NotNull(nameof(builder), builder);
            this.builder.Append(builder);
            return this;
        }

        public ISqlBuilder Append(char value)
        {
            throw new NotImplementedException();
        }

        public ISqlBuilder Append(string value)
        {
            this.builder.Append(value);
            return this;
        }

        public ISqlBuilder Append(string value, ITextTransform transformer)
        {
            Check.NotNull(nameof(transformer), transformer);
            this.builder.Append(transformer.Transform(value));

            return this;
        }

        public ISqlBuilder Append(bool value)
            => this.Append(this.SqlDialect.Write(value));

        public ISqlBuilder Append(decimal value)
             => this.Append(this.SqlDialect.Write(value));

        public ISqlBuilder Append(double value)
             => this.Append(this.SqlDialect.Write(value));

        public ISqlBuilder Append(char[] value)
             => this.Append(this.SqlDialect.Write(value));

        public ISqlBuilder Append(byte[] value)
             => this.Append(this.SqlDialect.Write(value));

        public ISqlBuilder Append(short value)
             => this.Append(this.SqlDialect.Write(value));

        public ISqlBuilder Append(int value)
             => this.Append(this.SqlDialect.Write(value));

        public ISqlBuilder Append(long value)
             => this.Append(this.SqlDialect.Write(value));

        public ISqlBuilder Append(Guid value)
             => this.Append(this.SqlDialect.Write(value));

        public ISqlBuilder Append(DateTime value, DbType type = DbType.DateTime)
             => this.Append(this.SqlDialect.Write(value));

        public ISqlBuilder Append(DateTimeOffset value, DbType type = DbType.DateTimeOffset)
            => this.Append(this.SqlDialect.Write(type));

        public ISqlBuilder Append(TimeSpan value, DbType type = DbType.Int64)
            => this.Append(this.SqlDialect.Write(value, type));

        public ISqlBuilder AppendJoin(string delimiter, params string[] names)
            => this.Append(string.Join(delimiter, names));

        public ISqlBuilder AppendJoin(string delimiter, IEnumerable<string> names)
                => this.Append(string.Join(delimiter, names));

        public ISqlBuilder AppendIdentity(params string[] names)
        {
            for (var i = 0; i < names.Length; i++)
            {
                var name = names[i];
                this.Append(this.SqlDialect.LeftIdentifierToken);
                this.Append(name);
                this.Append(this.SqlDialect.RightIdentifierToken);
                if (i < names.Length - 1)
                    this.Append(",");
            }

            return this;
        }

        public ISqlBuilder AppendLine()
        {
            this.builder.AppendLine();
            return this;
        }

        public ISqlBuilder AppendLine(char value)
        {
            this.builder.Append(value).AppendLine();
            return this;
        }

        public ISqlBuilder AppendLine(string value)
        {
            this.builder.AppendLine(value);
            return this;
        }

        public ISqlBuilder AppendLine(string value, ITextTransform transform)
        {
            Check.NotNull(nameof(transform), transform);
            this.builder.AppendLine(value);
            return this;
        }

        public ISqlBuilder AppendParameterName(string name)
            => this.Append(this.SqlDialect.ParameterPrefixToken)
                .Append(name);

        public StringBuilder ToStringBuilder()
            => this.builder;
    }
}