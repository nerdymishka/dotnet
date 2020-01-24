

using System;
using System.Data;

namespace NerdyMishka.Data
{
    public class DataTransaction : IDataTransaction
    {
        private IDbTransaction transaction;
        private bool autoCommit = false;
        private IsolationLevel il;

        private DataConnection connection;

        public IDataConnection Connection => this.connection;

        public ISqlDialect SqlDialect => this.connection.SqlDialect;

        protected internal DataTransaction(
            DataConnection connection,
            IDbTransaction transaction = null,
            IsolationLevel? il = null,
            bool autoCommit = false)
        {
            if (il.HasValue)
                this.il = il.Value;

            this.connection = connection;
            this.transaction = transaction;
            this.autoCommit = autoCommit;
        }

        public void Commit()
        {
            this.transaction.Commit();
        }

        public IDataCommand CreateCommand(CommandBehavior? behavior = CommandBehavior.Default)
        {
            var cmd = this.Connection.CreateCommand(behavior);
            cmd.Transaction = this;
            return cmd;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            if (this.autoCommit)
                this.Commit();
        }

        public void OnError(Exception error)
        {
            this.Rollback();
        }

        public void OnNext(IDataCommandBuilder builder)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            if (builder.Configuration is null)
                throw new NullReferenceException(nameof(builder.Configuration));

            if (builder.Configuration.Query is null)
                throw new NullReferenceException(nameof(builder.Configuration.Query));

            if (this.Connection.State != ConnectionState.Open)
            {
                this.autoCommit = true;
                this.Connection.Open();
                this.transaction = this.connection
                    .InnerConnection
                    .BeginTransaction(this.il);
            }

            if (this.transaction == null)
            {
                this.transaction = this.connection
                    .InnerConnection
                    .BeginTransaction(this.il);
            }

            if (builder.Command == null)
                builder.Command = this.CreateCommand();

            builder.ApplyConfiguration();
        }

        public void Rollback()
        {
            this.transaction.Rollback();
        }

        public void SetAutoCommit(bool autoCommit = true)
        {
            this.autoCommit = autoCommit;
        }
    }
}