

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

        public string Provider => this.SqlDialect.Name;

        public string ConnectionString
        {
            get => this.InnerConnection.ConnectionString;
            set => this.InnerConnection.ConnectionString = value;
        }

        public ConnectionState State => this.InnerConnection.State;

        public ISqlDialect SqlDialect { get; private set; }

        protected internal DbConnection InnerConnection { get; set; }

        public DataConnection(
            IDbConnection connection,
            ISqlDialect sqlDialect,
            bool autoClose = false)
        {
            this.InnerConnection = (DbConnection)connection;
            this.SqlDialect = sqlDialect;
        }

        public IDataTransactionActions BeginTransaction(IsolationLevel level = 0)
        {
            var transaction = this.InnerConnection.BeginTransaction(level);
            return new DataTransaction(this, transaction, level, false);
        }

        public void Close()
        {
            this.InnerConnection?.Close();
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

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DataConnection()
        {
            this.Dispose(false);
        }
    }
}