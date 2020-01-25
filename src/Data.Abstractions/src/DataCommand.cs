using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace NerdyMishka.Data
{
    /// <summary>
    /// Represents an enhanced <see cref="IDbCommand"/>.
    /// </summary>
    /// <seealso cref="NerdyMishka.Data.IDataCommand" />
    public class DataCommand : IDataCommand
    {
        private IDbCommand command;
        private DbCommand dbCommand;
        private DbParametersReadOnlyCollection parameters;
        private bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataCommand"/> class.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="sqlDialect">The SQL dialect.</param>
        /// <param name="behavior">The behavior.</param>
        public DataCommand(
            IDbCommand command,
            ISqlDialect sqlDialect,
            CommandBehavior? behavior = null)
        {
            Check.NotNull(nameof(command), command);
            this.SqlDialect = sqlDialect;
            this.parameters = new DbParametersReadOnlyCollection(command.Parameters);
            this.command = command;
            this.dbCommand = command as DbCommand;
            this.Behavior = behavior;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DataCommand"/> class.
        /// </summary>
        ~DataCommand()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets or sets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        public IDataConnectionActions Connection { get; set; }

        /// <summary>
        /// Gets or sets the transaction.
        /// </summary>
        /// <value>
        /// The transaction.
        /// </value>
        public IDataTransactionActions Transaction { get; set; }

        /// <summary>
        /// Gets the SQL dialect.
        /// </summary>
        /// <value>
        /// The SQL dialect.
        /// </value>
        public ISqlDialect SqlDialect { get; private set; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public IReadOnlyCollection<IDbDataParameter> Parameters
        {
            get => this.parameters;
        }

        /// <summary>
        /// Gets or sets the SQL text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; }

        /// <summary>
        /// Gets the default command behavior for data readers.
        /// </summary>
        /// <value>
        /// The behavior.
        /// </value>
        public CommandBehavior? Behavior { get; }

        /// <summary>
        /// Gets or sets the command type. e.g. Text, StoredProcedure.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public CommandType Type { get; set; }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        /// <value>
        /// The timeout.
        /// </value>
        public int Timeout { get; set; }

        /// <summary>
        /// Adds the <see cref="IDbDataParameter" />.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public void Add(IDbDataParameter parameter)
        {
            this.command.Parameters.Add(parameter);
            this.parameters.Add(parameter);
        }

        /// <summary>
        /// Creates a typed <see cref="IDbDataParameter" />.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <returns>
        /// The db parameter.
        /// </returns>
        public TParameter CreateParameter<TParameter>()
            where TParameter : IDbDataParameter
                => (TParameter)this.command.CreateParameter();

        /// <summary>
        /// Creates the parameter.
        /// </summary>
        /// <returns>
        /// The db parameter.
        /// </returns>
        public IDbDataParameter CreateParameter()
            => this.command.CreateParameter();

        /// <summary>
        /// Executes a non query.
        /// </summary>
        /// <returns>
        /// The number of affected rows.
        /// </returns>
        public int Execute()
            => this.command.ExecuteNonQuery();

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <returns>The task that executes a non query.</returns>
        public Task<int> ExecuteAsync()
            => this.dbCommand?.ExecuteNonQueryAsync();

        /// <summary>
        /// Executes a non query asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The number of affected rows.
        /// </returns>
        public Task<int> ExecuteAsync(CancellationToken cancellationToken = default)
            => this.dbCommand?.ExecuteNonQueryAsync(cancellationToken);

        /// <summary>
        /// Executes a query and returns a data reader to iterate over the results.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns>
        /// The data reader.
        /// </returns>
        public IDataReader ExecuteReader(CommandBehavior? behavior = CommandBehavior.Default)
        {
            behavior = behavior ?? this.Behavior;
            if (!behavior.HasValue)
                return new DataReader(this.command.ExecuteReader());

            return new DataReader(this.command.ExecuteReader(behavior.Value));
        }

        /// <summary>
        /// Executes a query and returns a data reader to iterate over the results.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The data reader.
        /// </returns>
        public async Task<IDataReader> ExecuteReaderAsync(
            CommandBehavior? behavior = CommandBehavior.Default,
            CancellationToken cancellationToken = default)
        {
            behavior = behavior ?? this.Behavior;
            System.Data.IDataReader dr;
            if (!behavior.HasValue)
            {
                dr = await this.dbCommand.ExecuteReaderAsync(cancellationToken)
                    .ConfigureAwait(false);

                return new DataReader(dr);
            }

            dr = await this.dbCommand.ExecuteReaderAsync(behavior.Value, cancellationToken)
                   .ConfigureAwait(false);

            return new DataReader(dr);
        }

        /// <summary>
        /// Executes a query and returns one value.
        /// </summary>
        /// <returns>
        /// one value.
        /// </returns>
        public object ExecuteScalar()
        {
            return this.command.ExecuteScalar();
        }

        /// <summary>
        /// Executes a query and returns one value asynchronousy.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// One value.
        /// </returns>
        public Task<object> ExecuteScalarAsync(CancellationToken cancellationToken = default)
        {
            return this.dbCommand.ExecuteScalarAsync(cancellationToken);
        }

        /// <summary>
        /// Prepares this command.
        /// </summary>
        public void Prepare()
        {
            this.command.Prepare();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Unwraps the current object by returning the inner object.
        /// </summary>
        /// <returns>
        /// The inner object.
        /// </returns>
        object IUnwrap.Unwrap()
        {
            return this.command;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
                return;

            if (disposing)
            {
                this.command = null;
                this.parameters = null;
            }

            this.command?.Dispose();
            this.dbCommand = null;
        }
    }
}