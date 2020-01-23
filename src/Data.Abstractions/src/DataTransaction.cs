

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

        public ISqlDialect SqlDialect { get; internal protected set; }

        public void Commit()
        {
            this.transaction.Commit();
        }

        public IDataCommand CreateCommand(CommandBehavior behavior = CommandBehavior.Default)
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

            var config = builder.Configuration;
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