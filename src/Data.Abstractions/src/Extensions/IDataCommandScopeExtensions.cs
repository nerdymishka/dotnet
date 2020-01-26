using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NerdyMishka.Data.Extensions
{
    public static class IDataCommandScopeExtensions
    {
        public static IDataReader ExecuteReader(
            this IDataCommandScope scope,
            ISqlBuilder sqlBuilder,
            IDictionary parameters,
            CommandBehavior behavior = CommandBehavior.Default)
        {
            return scope.Execute((builder) =>
            {
                builder.Command = scope.CreateCommand(behavior);
                var cfg = builder.Configuration;
                cfg.Query = sqlBuilder;
                cfg.SetParameters(parameters);
            },
            (builder) => builder.Command.ExecuteReader());
        }

        public static int Execute(
            this IDataCommandScope scope,
            ISqlBuilder sqlBuilder,
            IList<IDbDataParameter> parameters,
            CommandBehavior behavior = CommandBehavior.Default)
        {
            return scope.Execute((builder) =>
            {
                builder.Command = scope.CreateCommand(behavior);
                var cfg = builder.Configuration;
                cfg.Query = sqlBuilder;
                cfg.SetParameters(parameters);
            },
            (builder) => builder.Command.Execute());
        }

        public static object ExecuteScalar(
            this IDataCommandScope scope,
            ISqlBuilder sqlBuilder,
            IList<IDbDataParameter> parameters,
            CommandBehavior behavior = CommandBehavior.Default)
        {
            return scope.Execute((builder) =>
            {
                builder.Command = scope.CreateCommand(behavior);
                var cfg = builder.Configuration;
                cfg.Query = sqlBuilder;
                cfg.SetParameters(parameters);
            },
            (builder) => builder.Command.ExecuteScalar());
        }

        public static object ExecuteScalar(
            this IDataCommandScope scope,
            ISqlBuilder sqlBuilder,
            IList<object> parameters,
            CommandBehavior behavior = CommandBehavior.Default)
        {
            return scope.Execute((builder) =>
            {
                builder.Command = scope.CreateCommand(behavior);
                var cfg = builder.Configuration;
                cfg.Query = sqlBuilder;
                cfg.SetParameters(parameters);
            },
            (builder) => builder.Command.ExecuteScalar());
        }

        public static object ExecuteScalar(
            this IDataCommandScope scope,
            ISqlBuilder sqlBuilder,
            IDictionary parameters,
            CommandBehavior behavior = CommandBehavior.Default)
        {
            return scope.Execute((builder) =>
            {
                builder.Command = scope.CreateCommand(behavior);
                var cfg = builder.Configuration;
                cfg.Query = sqlBuilder;
                cfg.SetParameters(parameters);
            },
            (builder) => builder.Command.ExecuteScalar());
        }

        public static object ExecuteScalar(
            this IDataCommandScope scope,
            ISqlBuilder sqlBuilder,
            IEnumerable<KeyValuePair<string, object>> parameters,
            CommandBehavior behavior = CommandBehavior.Default)
        {
            return scope.Execute((builder) =>
            {
                builder.Command = scope.CreateCommand(behavior);
                var cfg = builder.Configuration;
                cfg.Query = sqlBuilder;
                cfg.SetParameters(parameters);
            },
            (builder) => builder.Command.ExecuteScalar());
        }

        public static IDataReader ExecuteReader(
            this IDataCommandScope scope,
            ISqlBuilder sqlBuilder)
        {
            return scope.Execute((builder) =>
            {
                builder.Command = scope.CreateCommand(CommandBehavior.Default);
                var cfg = builder.Configuration;
                cfg.Query = sqlBuilder;
            },
            (builder) => builder.Command.ExecuteReader());
        }

        public static IDataReader ExecuteReader(
            this IDataCommandScope scope,
            ISqlBuilder sqlBuilder,
            CommandBehavior behavior)
        {
            return scope.Execute((builder) =>
            {
                builder.Command = scope.CreateCommand(behavior);
                var cfg = builder.Configuration;
                cfg.Query = sqlBuilder;
            },
            (builder) => builder.Command.ExecuteReader());
        }

        public static int Execute(
            this IDataCommandScope scope,
            ISqlBuilder sqlBuilder,
            IEnumerable<KeyValuePair<string, object>> parameters,
            CommandBehavior behavior = CommandBehavior.Default)
        {
            return scope.Execute((builder) =>
            {
                builder.Command = scope.CreateCommand(behavior);
                var cfg = builder.Configuration;
                cfg.Query = sqlBuilder;
                cfg.SetParameters(parameters);
            },
            (builder) => builder.Command.Execute());
        }

        public static IDataReader ExecuteReader(
            this IDataCommandScope scope,
            ISqlBuilder sqlBuilder,
            IList<IDbDataParameter> parameters)
        {
            return scope.Execute((builder) =>
            {
                builder.Command = scope.CreateCommand(CommandBehavior.Default);
                var cfg = builder.Configuration;
                cfg.Query = sqlBuilder;
                cfg.SetParameters(parameters);
            },
            (builder) => builder.Command.ExecuteReader());
        }

        public static IDataReader ExecuteReader(
            this IDataCommandScope scope,
            ISqlBuilder sqlBuilder,
            IEnumerable<KeyValuePair<string, object>> parameters)
        {
            return scope.Execute((builder) =>
            {
                builder.Command = scope.CreateCommand();
                var cfg = builder.Configuration;
                cfg.Query = sqlBuilder;
                cfg.SetParameters(parameters);
            },
            (builder) => builder.Command.ExecuteReader());
        }

        public static IDataReader ExecuteReader(
            this IDataCommandScope scope,
            ISqlBuilder sqlBuilder,
            IList<object> parameters)
        {
            return scope.Execute((builder) =>
            {
                builder.Command = scope.CreateCommand();
                var cfg = builder.Configuration;
                cfg.Query = sqlBuilder;
                cfg.SetParameters(parameters);
            },
            (builder) => builder.Command.ExecuteReader());
        }

        public static Task<IDataReader> ExecuteReaderAsync(
           this IDataCommandScope scope,
           ISqlBuilder sqlBuilder,
           IList<object> parameters,
           CancellationToken cancellationToken = default)
        {
            return scope.ExecuteAsync((builder) =>
            {
                builder.Command = scope.CreateCommand();
                var cfg = builder.Configuration;
                cfg.Query = sqlBuilder;
                cfg.SetParameters(parameters);
            },
            (builder) =>
            {
                return builder.Command.ExecuteReaderAsync(
                    CommandBehavior.CloseConnection,
                    cancellationToken);
            });
        }

        public static Task<IDataReader> ExecuteReaderAsync(
           this IDataCommandScope scope,
           ISqlBuilder sqlBuilder,
           IDictionary parameters,
           CancellationToken cancellationToken = default)
        {
            return scope.ExecuteAsync((builder) =>
            {
                builder.Command = scope.CreateCommand();
                var cfg = builder.Configuration;
                cfg.Query = sqlBuilder;
                cfg.SetParameters(parameters);
            },
            (builder) =>
            {
                return builder.Command.ExecuteReaderAsync(
                    CommandBehavior.CloseConnection,
                    cancellationToken);
            });
        }

        public static Task<IDataReader> ExecuteReaderAsync(
           this IDataCommandScope scope,
           ISqlBuilder sqlBuilder,
           IEnumerable<IDbDataParameter> parameters,
           CancellationToken cancellationToken = default)
        {
            return scope.ExecuteAsync((builder) =>
            {
                builder.Command = scope.CreateCommand(CommandBehavior.CloseConnection);
                var cfg = builder.Configuration;
                cfg.Query = sqlBuilder;
                cfg.SetParameters(parameters);
            },
            (builder) =>
            {
                return builder.Command.ExecuteReaderAsync(
                    CommandBehavior.CloseConnection,
                    cancellationToken);
            });
        }

        public static Task<IDataReader> ExecuteReaderAsync(
           this IDataCommandScope scope,
           ISqlBuilder sqlBuilder,
           IEnumerable<KeyValuePair<string, object>> parameters,
           CancellationToken cancellationToken = default)
        {
            return scope.ExecuteAsync((builder) =>
            {
                builder.Command = scope.CreateCommand(CommandBehavior.CloseConnection);
                var cfg = builder.Configuration;
                cfg.Query = sqlBuilder;
                cfg.SetParameters(parameters);
            },
            (builder) =>
            {
                return builder.Command.ExecuteReaderAsync(
                    CommandBehavior.CloseConnection,
                    cancellationToken);
            });
        }

        public static Task<T> ExecuteAsync<T>(
           this IDataCommandScope scope,
           Action<DataCommandBuilder> configure,
           Func<IDataCommandBuilder, Task<T>> executeAsync)
        {
            Check.NotNull(nameof(scope), scope);
            Check.NotNull(nameof(configure), configure);
            Check.NotNull(nameof(executeAsync), executeAsync);
            try
            {
                var statement = new DataCommandBuilder();
                configure(statement);
                scope.OnNext(statement);

                return executeAsync(statement);
            }
            catch
            {
                scope.OnError(null);
                throw;
            }
            finally
            {
                scope.OnCompleted();
            }
        }

        public static void Use(
            this IDataCommandScope scope,
            Action<IDataConnection> execute)
        {
            Check.NotNull(nameof(scope), scope);
            Check.NotNull(nameof(execute), execute);

            IDataTransaction tx = null;
            try
            {
                DataConnection c = null;
                if (scope is DataTransaction transaction)
                {
                    c = (DataConnection)transaction.Connection;
                    transaction.SetAutoCommit(true);
                    tx = transaction;
                }
                else if (scope is DataConnection connection)
                {
                    c = connection;
                }

                execute(c);
            }
            catch
            {
                tx?.OnError(null);
                scope.OnError(null);
                throw;
            }
            finally
            {
                tx?.OnCompleted();
                scope.OnCompleted();
            }
        }

        public static T Try<T>(
            this IDataCommandScope scope,
            Func<IDataConnection, T> execute)
        {
            Check.NotNull(nameof(scope), scope);
            Check.NotNull(nameof(execute), execute);
            IDataTransaction tx = null;
            try
            {
                DataConnection c = null;
                if (scope is DataTransaction transaction)
                {
                    c = (DataConnection)transaction.Connection;
                    transaction.SetAutoCommit(true);
                    tx = transaction;
                }
                else if (scope is DataConnection connection)
                {
                    c = connection;
                }

                if (c.State != ConnectionState.Open)
                {
                    c.SetAutoClose(true);
                    c.Open();
                }

                return execute(c);
            }
            catch
            {
                tx?.OnError(null);
                scope.OnError(null);
                throw;
            }
            finally
            {
                tx?.OnCompleted();
                scope.OnCompleted();
            }
        }

        public static T Execute<T>(
            this IDataCommandScope scope,
            Action<DataCommandBuilder> configure,
            Func<IDataCommandBuilder, T> execute)
        {
            Check.NotNull(nameof(scope), scope);
            Check.NotNull(nameof(configure), configure);
            Check.NotNull(nameof(execute), execute);

            try
            {
                var statement = new DataCommandBuilder();
                configure(statement);
                scope.OnNext(statement);
                return execute(statement);
            }
            catch
            {
                scope.OnError(null);
                throw;
            }
            finally
            {
                scope.OnCompleted();
            }
        }
    }
}