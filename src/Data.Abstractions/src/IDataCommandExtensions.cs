using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
            var cmd = builder.Command;
            var query = builder.Configuration.Query;
            var prefix = builder.Configuration.ParameterPrefix;

            switch (builder.Configuration.SetType)
            {
                case ParameterSetType.Array:
                    return cmd.ApplyParameters(
                        query,
                        prefix,
                        builder.Configuration.ParameterArray);
            }
        }

        public static IDataCommand ApplyParameters(
            this IDataCommand cmd,
            StringBuilder query,
            char parameterPrefix,
            IList parameters,
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


        public static IDataCommand ApplyParameters(
            this IDataCommand cmd,
            StringBuilder query,
            char parameterPrefix,
            IList<IDbDataParameter> parameters)
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
    }
}