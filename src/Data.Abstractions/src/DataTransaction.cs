using System;
using System.Data;

namespace NerdyMishka.Data
{
    /// <summary>
    /// Represents a database transaction.
    /// </summary>
    /// <seealso cref="NerdyMishka.Data.IDataTransaction" />
    public class DataTransaction : IDataTransaction
    {
        private IDbTransaction transaction;

        private bool autoCommit = false;

        private IsolationLevel il;

        private bool isUsed = false;

        private bool isDisposed = false;

        private DataConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTransaction"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="il">The il.</param>
        /// <param name="autoCommit">if set to <c>true</c> [automatic commit].</param>
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

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        public IDataConnection Connection => this.connection;

        /// <summary>
        /// Gets the <see cref="ISqlDialect" />.
        /// </summary>
        /// <value>
        /// The SQL dialect.
        /// </value>
        public ISqlDialect SqlDialect => this.connection.SqlDialect;

        /// <summary>
        /// Commits all changes within the transaction to the database.
        /// </summary>
        public void Commit()
        {
            this.isUsed = true;
            this.transaction.Commit();
        }

        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns>
        /// The data command.
        /// </returns>
        public IDataCommand CreateCommand(CommandBehavior? behavior = CommandBehavior.Default)
        {
            var cmd = this.Connection.CreateCommand(behavior);
            cmd.Transaction = this;
            return cmd;
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
        /// Notifies the observer that the provider has finished sending push-based notifications.
        /// </summary>
        public void OnCompleted()
        {
            if (this.autoCommit && !this.isUsed)
                this.Commit();
        }

        /// <summary>
        /// Notifies the observer that the provider has experienced an error condition.
        /// </summary>
        /// <param name="error">An object that provides additional information about the error.</param>
        public void OnError(Exception error)
        {
            if (!this.isUsed)
                this.Rollback();
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

            if (builder.Configuration.Query == null)
                builder.Configuration.Query = this.SqlDialect.CreateBuilder();

            if (builder.Command == null)
                builder.Command = this.CreateCommand();

            builder.Configuration.ParameterPrefix = this.SqlDialect.ParameterPrefixToken;

            builder.ApplyConfiguration();
        }

        /// <summary>
        /// Rolls all the changes within the transaction back.
        /// </summary>
        public void Rollback()
        {
            this.transaction.Rollback();
            this.isUsed = true;
        }

        /// <summary>
        /// Sets a value to note an automatic commit.
        /// </summary>
        /// <param name="autoCommit">if set to <c>true</c> [automatic commit].</param>
        public virtual void SetAutoCommit(bool autoCommit = true)
        {
            this.autoCommit = autoCommit;
        }

        /// <summary>
        /// Unwraps the current object by returning the inner object.
        /// </summary>
        /// <returns>
        /// The inner object.
        /// </returns>
        object IUnwrap.Unwrap()
        {
            return this.transaction;
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
                if (!this.isUsed)
                    this.Commit();

                this.connection = null;
                this.transaction?.Dispose();
                this.transaction = null;
            }

            this.isDisposed = true;
        }
    }
}