using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace NerdyMishka.Data
{
    /// <summary>
    /// A contract that represents an enhanced <see cref="System.Data.IDbCommand"/>.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="NerdyMishka.Data.IUnwrap" />
    public interface IDataCommand : IDisposable,
        IUnwrap
    {
        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        IDataConnectionActions Connection { get; }

        /// <summary>
        /// Gets or sets the transaction.
        /// </summary>
        /// <value>
        /// The transaction.
        /// </value>
        IDataTransactionActions Transaction { get; set; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        IReadOnlyCollection<IDbDataParameter> Parameters { get; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        string Text { get; set; }

        /// <summary>
        /// Gets the default command behavior for data readers.
        /// </summary>
        /// <value>
        /// The behavior.
        /// </value>
        CommandBehavior? Behavior { get; }

        /// <summary>
        /// Gets or sets the command type. e.g. Text, StoredProcedure.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        CommandType Type { get; set; }

        /// <summary>
        /// Gets the SQL dialect.
        /// </summary>
        /// <value>
        /// The SQL dialect.
        /// </value>
        ISqlDialect SqlDialect { get; }

        /// <summary>
        /// Gets the timeout.
        /// </summary>
        /// <value>
        /// The timeout.
        /// </value>
        int Timeout { get; }

        /// <summary>
        /// Creates a typed <see cref="IDbDataParameter"/>.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <returns>The db parameter.</returns>
        TParameter CreateParameter<TParameter>()
            where TParameter : IDbDataParameter;

        /// <summary>
        /// Creates the parameter.
        /// </summary>
        /// <returns>The db parameter.</returns>
        IDbDataParameter CreateParameter();

        /// <summary>
        /// Adds the <see cref="IDbDataParameter"/>.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        void Add(IDbDataParameter parameter);

        /// <summary>
        /// Executes a non query.
        /// </summary>
        /// <returns>The number of affected rows.</returns>
        int Execute();

        /// <summary>
        /// Executes a non query asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of affected rows.</returns>
        Task<int> ExecuteAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes a query and returns one value.
        /// </summary>
        /// <returns>one value.</returns>
        object ExecuteScalar();

        /// <summary>
        /// Executes a query and returns one value asynchronousy.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>One value.</returns>
        Task<object> ExecuteScalarAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes a query and returns a data reader to iterate over the results.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns>The data reader.</returns>
        IDataReader ExecuteReader(CommandBehavior? behavior = default);

        /// <summary>
        /// Executes a query and returns a data reader to iterate over the results.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The data reader.</returns>
        Task<IDataReader> ExecuteReaderAsync(
            CommandBehavior? behavior = default,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Prepares this command.
        /// </summary>
        void Prepare();
    }
}