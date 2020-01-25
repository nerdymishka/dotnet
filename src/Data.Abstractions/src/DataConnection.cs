

using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace NerdyMishka.Data
{
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

        ~DataConnection()
        {
            this.Dispose(false);
        }

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

        public ConnectionState State =>
            this.InnerConnection?.State ?? ConnectionState.Closed;

        public ISqlDialect SqlDialect { get; private set; }

        public int TimeOut
        {
            get => this.InnerConnection.ConnectionTimeout;
        }

        protected internal DbConnection InnerConnection { get; set; }

        public IDataTransactionActions BeginTransaction(IsolationLevel level = 0)
        {
            var transaction = this.InnerConnection.BeginTransaction(level);
            return new DataTransaction(this, transaction, level, false);
        }

        public void Close()
        {
            this.InnerConnection?.Close();
        }

        public virtual Task CloseAsync(CancellationToken cancellationToken = default)
        {
            return Task.Run(() => this.Close(), cancellationToken);
        }

        public IDataCommand CreateCommand(CommandBehavior? behavior = default)
        {
            var cmd = this.InnerConnection.CreateCommand();
            return new DataCommand(cmd, this.SqlDialect, behavior);
        }

        public void OnCompleted()
        {
            if (this.autoClose)
                this.Close();
        }

        public void OnError(Exception error)
        {
            this.Close();
        }

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

            if (builder.Command == null)
                builder.Command = this.CreateCommand();

            builder.ApplyConfiguration();
        }

        public void Open()
        {
            this.InnerConnection?.Open();
        }

        public Task OpenAsync()
        {
            return this.InnerConnection?.OpenAsync();
        }

        public Task OpenAsync(CancellationToken cancellationToken)
        {
            return this.InnerConnection?.OpenAsync(cancellationToken);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        object IUnwrappable.Unwrap()
        {
            return this.InnerConnection;
        }

        protected internal void SetAutoClose(bool value)
        {
            this.autoClose = value;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposedValue)
                return;

            if (disposing)
            {
                this.SqlDialect = null;
            }

            this.InnerConnection?.Dispose();
            this.InnerConnection = null;
            this.disposedValue = true;
        }
    }
}