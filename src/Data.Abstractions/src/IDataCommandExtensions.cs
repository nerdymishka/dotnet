using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;

namespace NerdyMishka.Data
{
    public static class IDataCommandExtensions
    {
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

        public static IDataCommand ApplyConfiguration(
            this IDataCommandBuilder builder)
        {
            Check.NotNull(nameof(builder), builder);
            Check.NotNull(nameof(builder.Command), builder.Command);
            Check.NotNull(nameof(builder.Configuration), builder.Configuration);
            Check.NotNull(nameof(builder.Configuration.Query), builder.Configuration.Query);

            var cfg = builder.Configuration;
            switch (builder.Configuration.SetType)
            {
                case ParameterSetType.Array:
                    return builder.Command.ApplyParameters(
                        cfg.Query,
                        cfg.ParameterArray,
                        cfg.ParameterPrefix);

                case ParameterSetType.DbParameters:
                    return builder.Command.ApplyParameters(
                        cfg.Query,
                        cfg.DbParameters,
                        cfg.ParameterPrefix);

                case ParameterSetType.Hashtable:
                    return builder.Command.ApplyParameters(
                            cfg.Query,
                            cfg.Hastable,
                            cfg.ParameterPrefix);

                case ParameterSetType.KeyValue:
                    return builder.Command.ApplyParameters(
                        cfg.Query,
                        cfg.DbParameters,
                        cfg.ParameterPrefix);
            }

            throw new NotSupportedException(ParameterSetType.None.ToString());
        }

        public static IDataCommand ApplyParameters(
            this IDataCommand cmd,
            StringBuilder query,
            IList parameters,
            char parameterPrefix = '@',
            string placeholder = "/?")
        {
            if (cmd is null)
                throw new ArgumentNullException(nameof(cmd));

            if (query is null)
                throw new ArgumentNullException(nameof(query));

            if (parameterPrefix == char.MinValue)
                parameterPrefix = '@';

            var sql = query.ToString();
            if (parameters != null && parameters.Count > 0)
            {
                int index = 0;
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
                    IDataCommand cmd,
                    StringBuilder query,
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
                bool? replace = null;

                foreach (var set in parameters)
                {
                    var key = set.Key;
                    var parameterName = key;
                    var value = set.Value;
                    if (!replace.HasValue)
                    {
                        var prefix = key[0];
                        replace = char.IsLetterOrDigit(prefix) || prefix != parameterPrefix;
                    }

                    if (!replace.Value)
                    {
                        cmd.AddParameter(key, value);
                        continue;
                    }

                    // TODO: do an insert
                    parameterName = parameterPrefix + key;
                    query.Replace(key, parameterName);
                    cmd.AddParameter(parameterName, value);
                }
            }

            cmd.Text = query.ToString();
            return cmd;
        }

        public static IDataCommand ApplyParameters(
            this IDataCommand cmd,
            StringBuilder query,
            IEnumerable<IDbDataParameter> parameters,
            char parameterPrefix = '@')
        {
            if (cmd is null)
                throw new ArgumentNullException(nameof(cmd));

            if (query is null)
                throw new ArgumentNullException(nameof(query));

            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    if (p.ParameterName[0] != parameterPrefix)
                    {
                        var name = p.ParameterName.Substring(1);
                        var corrected = parameterPrefix + name;
                        query.Replace(p.ParameterName, corrected);

                        p.ParameterName = corrected;
                    }

                    cmd.Add(p);
                }
            }

            cmd.Text = query.ToString();
            cmd.Type = CommandType.Text;
            return cmd;
        }

        public static IDataCommand ApplyParameters(
            this IDataCommand cmd,
            StringBuilder query,
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
                bool? replace = null;

                foreach (string key in parameters.Keys)
                {
                    var parameterName = key;
                    var value = parameters[key];
                    if (!replace.HasValue)
                    {
                        var prefix = key[0];
                        replace = char.IsLetterOrDigit(prefix) || prefix != parameterPrefix;
                    }

                    if (!replace.Value)
                    {
                        cmd.AddParameter(key, value);
                        continue;
                    }

                    // TODO: do an insert
                    parameterName = parameterPrefix + key;
                    query.Replace(key, parameterName);
                    cmd.AddParameter(parameterName, value);
                }
            }

            cmd.Text = query.ToString();
            return cmd;
        }
    }
}