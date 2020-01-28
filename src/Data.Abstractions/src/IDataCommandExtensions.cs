using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NerdyMishka.Data
{
    public static class IDataCommandExtensions
    {
        private const char DefaultPrefix = '@';

        /// <summary>
        /// Adds the parameter to the command..
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>The data command.</returns>
        /// <exception cref="ArgumentNullException">command.</exception>
        public static IDataCommand AddParameter(this IDataCommand command,
            string name,
            object value)
        {
            if (command is null)
                throw new ArgumentNullException(nameof(command));

            var p = command.CreateParameter();
            p.ParameterName = name;
            p.Value = value;

            command.Add(p);
            return command;
        }

        internal static IDataCommand ApplyConfiguration(
            this IDataCommandBuilder builder)
        {
            Check.NotNull(nameof(builder), builder);
            Check.NotNull(nameof(builder.Command), builder.Command);
            Check.NotNull(nameof(builder.Configuration), builder.Configuration);
            Check.NotNull(nameof(builder.Configuration.Query), builder.Configuration.Query);

            var cfg = builder.Configuration;
            switch (builder.Configuration.SetType)
            {
                case ParameterSetType.List:
                    return builder.Command.ApplyParameters(
                        cfg.Query,
                        cfg.ParameterList,
                        cfg.ParameterPrefix);

                case ParameterSetType.Lookup:
                    return builder.Command.ApplyParameters(
                            cfg.Query,
                            cfg.ParameterLookup,
                            cfg.ParameterPrefix);

                case ParameterSetType.TypedList:
                    return builder.Command.ApplyParameters(
                        cfg.Query,
                        cfg.TypedParameterList,
                        cfg.ParameterPrefix);

                case ParameterSetType.TypedLookup:
                    return builder.Command.ApplyParameters(
                        cfg.Query,
                        cfg.TypedParameterLookup,
                        cfg.ParameterPrefix);

                case ParameterSetType.None:
                    {
                        var cmd = builder.Command;
                        cmd.Type = CommandType.Text;
                        cmd.Text = cfg.Query.ToString(true);
                        return cmd;
                    }
            }

            throw new NotSupportedException(ParameterSetType.None.ToString());
        }

        internal static IDataCommand ApplyParameters(
            this IDataCommand cmd,
            ISqlBuilder query,
            IList<object> parameters,
            char parameterPrefix = '@',
            string placeholder = "[?]")
        {
            if (cmd is null)
                throw new ArgumentNullException(nameof(cmd));

            if (query is null)
                throw new ArgumentNullException(nameof(query));

            if (parameterPrefix == char.MinValue)
                parameterPrefix = '@';

            var sql = query.ToString(true);
            if (parameters != null && parameters.Count > 0)
            {
                int index = 0;

                // TODO: write a parser
                sql = Regex.Replace(sql, placeholder, (m) =>
                {
                    var name = parameterPrefix + index.ToString(CultureInfo.InvariantCulture);
                    cmd.AddParameter(name, parameters[index]);
                    index++;

                    return name;
                });
            }

            cmd.Text = sql;
            cmd.Type = CommandType.Text;
            return cmd;
        }

        internal static IDataCommand ApplyParameters(
            this IDataCommand cmd,
            ISqlBuilder query,
            IEnumerable<KeyValuePair<string, object>> parameters,
            char parameterPrefix = '@')
        {
            if (cmd is null)
                throw new ArgumentNullException(nameof(cmd));

            if (query is null)
                throw new ArgumentNullException(nameof(query));

            cmd.Type = System.Data.CommandType.Text;

            if (parameters != null)
            {
                bool replace = parameterPrefix != DefaultPrefix;
                bool hasPrefix = parameters.First().Key[0] == DefaultPrefix;

                foreach (var set in parameters)
                {
                    var key = set.Key;
                    var parameterName = parameterPrefix + key;
                    var value = set.Value;

                    if (!replace)
                    {
                        cmd.AddParameter(key, value);
                        continue;
                    }

                    if (hasPrefix)
                    {
                        parameterName = parameterPrefix + key.Substring(1);
                        query.ToStringBuilder().Replace(key, parameterName);
                        cmd.AddParameter(parameterName, value);
                        continue;
                    }

                    query.ToStringBuilder().Replace(DefaultPrefix + key, parameterName);
                    cmd.AddParameter(parameterName, value);
                }
            }

            cmd.Text = query.ToString(true);
            return cmd;
        }

        internal static IDataCommand ApplyParameters(
            this IDataCommand cmd,
            ISqlBuilder query,
            IEnumerable<IDbDataParameter> parameters,
            char parameterPrefix = '@')
        {
            if (cmd is null)
                throw new ArgumentNullException(nameof(cmd));

            if (query is null)
                throw new ArgumentNullException(nameof(query));

            if (parameters != null)
            {
                bool replace = parameterPrefix != DefaultPrefix;

                foreach (var p in parameters)
                {
                    if (replace && p.ParameterName[0] == DefaultPrefix)
                    {
                        var name = parameterPrefix + p.ParameterName.Substring(1);
                        query.ToStringBuilder().Replace(p.ParameterName, name);
                        p.ParameterName = name;
                    }

                    cmd.Add(p);
                }
            }

            cmd.Text = query.ToString(true);
            cmd.Type = CommandType.Text;
            return cmd;
        }

        internal static IDataCommand ApplyParameters(
            this IDataCommand cmd,
            ISqlBuilder query,
            IDictionary parameters,
            char parameterPrefix = '@')
        {
            if (cmd is null)
                throw new ArgumentNullException(nameof(cmd));

            if (query is null)
                throw new ArgumentNullException(nameof(query));

            cmd.Type = System.Data.CommandType.Text;

            if (parameters != null && parameters.Count > 0)
            {
                bool replace = parameterPrefix != DefaultPrefix;
                var enumerator = parameters.GetEnumerator();
                enumerator.MoveNext();
                var hasPrefix = enumerator.Key.ToString()[0] == DefaultPrefix;

                foreach (string key in parameters.Keys)
                {
                    var parameterName = parameterPrefix + key;
                    var value = parameters[key];

                    if (!replace)
                    {
                        cmd.AddParameter(parameterName, value);
                        continue;
                    }

                    if (hasPrefix)
                    {
                        parameterName = parameterPrefix + key.Substring(1);
                        query.ToStringBuilder().Replace(key, parameterName);
                        cmd.AddParameter(parameterName, value);
                        continue;
                    }

                    query.ToStringBuilder().Replace(DefaultPrefix + key, parameterName);
                    cmd.AddParameter(parameterName, value);
                }
            }

            cmd.Text = query.ToString(true);
            return cmd;
        }
    }
}