using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace NerdyMishka.Data
{
    /// <summary>
    /// An enhanced DbConnection.
    /// </summary>
    /// <seealso cref="NerdyMishka.Data.IDataConnection" />
    public class DataConnection : IDataConnection
    {
        private bool disposedValue = false;

        private bool autoClose = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataConnection" /> class.
        /// </summary>
        /// <param name="connection">The inner connection.</param>
        /// <param name="sqlDialect">The sql dialect for the inner connection.</param>
        /// <param name="autoClose">A indicating to automatically closed the connections.</param>
        public DataConnection(
            IDbConnection connection,
            ISqlDialect sqlDialect,
            bool autoClose = false)
        {
            Check.NotNull(nameof(connection), connection);
            Check.NotNull(nameof(sqlDialect), sqlDialect);

            this.InnerConnection = (DbConnection)connection;
            this.SqlDialect = sqlDialect;
            this.autoClose = autoClose;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DataConnection"/> class.
        /// </summary>
        ~DataConnection()
        {
            this.Dispose(false);
        }

        public bool AutoClose => this.autoClose;

        /// <summary>
        /// Gets the provider name.
        /// </summary>
        public string Provider => this.SqlDialect.Name;

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString
        {
            get => this.InnerConnection.ConnectionString;
            set => this.InnerConnection.ConnectionString = value;
        }

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public ConnectionState State =>
            this.InnerConnection?.State ?? ConnectionState.Closed;

        /// <summary>
        /// Gets the SQL dialect.
        /// </summary>
        /// <value>
        /// The SQL dialect.
        /// </value>
        public ISqlDialect SqlDialect { get; private set; }

        /// <summary>
        /// Gets the time out.
        /// </summary>
        /// <value>
        /// The time out.
        /// </value>
        public int TimeOut
        {
            get => this.InnerConnection.ConnectionTimeout;
        }

        /// <summary>
        /// Gets or sets the inner connection.
        /// </summary>
        /// <value>
        /// The inner connection.
        /// </value>
        protected internal DbConnection InnerConnection { get; set; }

        /// <summary>
        /// Creates and starts the transaction.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns>The transaction.</returns>
        public IDataTransactionActions BeginTransaction(IsolationLevel level = 0)
        {
            var transaction = this.InnerConnection.BeginTransaction(level);
            return new DataTransaction(this, transaction, level, false);
        }

        /// <summary>
        /// Closes this connection.
        /// </summary>
        public void Close()
        {
            this.InnerConnection?.Close();
        }

        /// <summary>
        /// Closes the connection asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that closes the connection.</returns>
        public virtual Task CloseAsync(CancellationToken cancellationToken = default)
        {
            return Task.Run(() => this.Close(), cancellationToken);
        }

        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns>The db command.</returns>
        public IDataCommand CreateCommand(CommandBehavior? behavior = default)
        {
            var cmd = this.InnerConnection.CreateCommand();
            return new DataCommand(cmd, this.SqlDialect, behavior);
        }

        /// <summary>
        /// Called when [completed].
        /// </summary>
        public void OnCompleted()
        {
            if (this.autoClose)
                this.Close();
        }

        /// <summary>
        /// Called when [error].
        /// </summary>
        /// <param name="error">The error.</param>
        public void OnError(Exception error)
        {
            this.Close();
        }

        /// <summary>
        /// Called when [next].
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <exception cref="ArgumentNullException">builder.</exception>
        /// <exception cref="NullReferenceException">
        /// Configuration
        /// or
        /// Query.
        /// </exception>
        public void OnNext(IDataCommandBuilder builder)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            if (builder.Configuration is null)
                throw new NullReferenceException(nameof(builder.Configuration));

            if (builder.Configuration.Query is null)
                throw new NullReferenceException(nameof(builder.Configuration.Query));

            if (this.State != ConnectionState.Open)
            {
                this.autoClose = true;
                this.Open();
            }

            if (builder.Configuration.Query == null)
                builder.Configuration.Query = this.SqlDialect.CreateBuilder();

            if (builder.Command == null)
                builder.Command = this.CreateCommand();

            builder.Configuration.ParameterPrefix = this.SqlDialect.ParameterPrefixToken;

            builder.ApplyConfiguration();
        }

        /// <summary>
        /// Opens the connection.
        /// </summary>
        public void Open()
        {
            this.InnerConnection?.Open();
        }

        /// <summary>
        /// Opens the connection asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that opens the connection.</returns>
        public Task OpenAsync(CancellationToken cancellationToken = default)
        {
            return this.InnerConnection?.OpenAsync(cancellationToken);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Unwraps this instance.
        /// </summary>
        /// <returns>The inner connection.</returns>
        object IUnwrap.Unwrap()
        {
            return this.InnerConnection;
        }

        /// <summary>
        /// Sets the connection close to automatic.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        protected internal void SetAutoClose(bool value)
        {
            this.autoClose = value;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposedValue)
                return;

            if (disposing)
            {
                this.SqlDialect = null;
            }

            if (this.autoClose)
                this.InnerConnection?.Dispose();

            this.InnerConnection = null;
            this.disposedValue = true;
        }
    }
}