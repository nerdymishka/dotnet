using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Fakes
{
    public class FakeDbCommand : DbCommand,
        IDbCommand
    {
        private object scalar;

        private int affectedRecords = 0;

        private FakeDataReader reader;

        public FakeDbCommand(DbConnection connection)
        {
            this.Connection = connection;
        }

        public override string CommandText { get; set; }

        public override int CommandTimeout { get; set; }

        public override CommandType CommandType { get; set; }

        public override bool DesignTimeVisible { get; set; }

        public override UpdateRowSource UpdatedRowSource { get; set; }

        public virtual int Delay { get; private set; }

        protected override DbConnection DbConnection { get; set; }

        protected override DbParameterCollection DbParameterCollection { get; }
            = new FakeDbParameterCollection();

        protected override DbTransaction DbTransaction { get; set; }

        public override void Cancel()
        {
        }

        public override int ExecuteNonQuery()
        {
            if (this.Delay > 0)
                Task.Delay(this.Delay).Wait();

            return this.affectedRecords;
        }

        public override object ExecuteScalar()
        {
            if (this.Delay > 0)
                Task.Delay(this.Delay).Wait();

            return this.scalar;
        }

        public override void Prepare()
        {
        }

        protected internal virtual FakeDbCommand SetNextScalar(object value)
        {
            this.scalar = value;
            return this;
        }

        protected internal virtual FakeDbCommand SetNextAffectedResults(int value)
        {
            this.affectedRecords = value;
            return this;
        }

        protected internal virtual FakeDbCommand SetNextReader(FakeDataReader value)
        {
            // TODO: consider a factory method instead.
            this.reader = value;
            return this;
        }

        protected override DbParameter CreateDbParameter()
            => new FakeDbParameter();

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return null;
        }
    }
}