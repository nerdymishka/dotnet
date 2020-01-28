using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Fakes
{
    public class FakeDbConnection : DbConnection,
        IDbConnection
    {
        private string database;

        private string datasource;

        private string serverVersion;

        private string connectionString;

        private ConnectionState state;

        private Func<DbConnection, IsolationLevel, DbTransaction> transactionFactory;

        private Func<DbConnection, DbCommand> commandFactory;

        public FakeDbConnection()
        {
            this.transactionFactory = (conn, il) =>
            {
                return new FakeDbTransaction(this, il);
            };

            this.commandFactory = (db) =>
                new FakeDbCommand(db);
        }

        public int Delay { get; private set; }

        public override string ConnectionString
        {
            get => this.connectionString;
            set
            {
                if (!string.IsNullOrWhiteSpace(value) &&
                    this.State == ConnectionState.Closed)
                {
                    var builder = new DbConnectionStringBuilder
                    {
                        ConnectionString = value,
                    };

                    this.datasource = builder["Data Source"] as string;
                    this.database = builder["Initial Catalog"] as string;

                    if (this.database == null)
                        this.database = builder["Database"] as string;

                    if (this.datasource == null)
                        this.datasource = builder["Server"] as string;
                }

                this.connectionString = value;
            }
        }

        public override string Database => this.database;

        public override string DataSource => this.datasource;

        public override string ServerVersion => this.serverVersion;

        public override ConnectionState State => this.state;

        public override void ChangeDatabase(string databaseName)
        {
            this.database = databaseName;
        }

        public override void Close()
        {
            if (this.Delay > 0)
                Task.Delay(this.Delay).Wait();

            this.SetState(ConnectionState.Closed);
        }

        public override void Open()
        {
            this.SetState(ConnectionState.Connecting);

            if (this.Delay > 0)
                Task.Delay(this.Delay).Wait();

            this.SetState(ConnectionState.Open);
        }

        protected internal virtual void SetState(ConnectionState state)
        {
            this.state = state;
        }

        protected internal virtual void SetDelay(int delay)
        {
            this.Delay = delay;
        }

        protected internal virtual void SetTransactionFactory(
            Func<DbConnection, IsolationLevel, DbTransaction> factory)
        {
            this.transactionFactory = factory;
        }

        protected internal virtual void SetCommandFactory(Func<DbConnection, DbCommand> factory)
        {
            this.commandFactory = factory;
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            if (this.transactionFactory is null)
                return null;

            return this.transactionFactory(this, isolationLevel);
        }

        protected override DbCommand CreateDbCommand()
        {
            if (this.commandFactory is null)
                return null;

            return this.commandFactory(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Close();
            }
        }
    }
}