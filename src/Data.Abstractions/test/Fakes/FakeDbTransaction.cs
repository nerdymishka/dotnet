using System;
using System.Data;
using System.Data.Common;

namespace Fakes
{
    public class FakeDbTransaction : DbTransaction,
        IDbTransaction
    {
        private IsolationLevel isolationLevel;

        private DbConnection connection;

        private Action commit;

        private Action rollback;

        private bool isCommitted;

        public FakeDbTransaction()
        {
        }

        internal protected FakeDbTransaction(
            DbConnection connection,
            IsolationLevel il)
        {
            this.connection = connection;
            this.isolationLevel = il;
        }

        public bool IsCommitted { get; private set; }

        public bool IsRollback { get; private set; }

        public override IsolationLevel IsolationLevel
            => this.isolationLevel;

        protected override DbConnection DbConnection
            => this.connection;

        public override void Commit()
        {
            this.commit?.Invoke();
            this.IsCommitted = true;
        }

        public override void Rollback()
        {
            this.rollback?.Invoke();
            this.IsRollback = true;
        }
    }
}