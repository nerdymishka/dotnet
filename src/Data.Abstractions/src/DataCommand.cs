

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace NerdyMishka.Data
{
    public class DataCommand : IDataCommand
    {
        private IDbCommand command;
        private DbCommand dbCommand;
        private DbParametersReadOnlyCollection parameters;
        private bool isDisposed = false;

        public DataCommand(IDbCommand command)
        {
            Check.NotNull(nameof(command), command);
            this.parameters = new DbParametersReadOnlyCollection(command.Parameters);
            this.command = command;
            this.dbCommand = command as DbCommand;
        }

        public IDataConnectionActions Connection { get; set; }

        public IDataTransactionActions Transaction { get; set; }

        public IReadOnlyCollection<IDbDataParameter> Parameters
        {
            get => this.parameters;
        }

        public string Text { get; set; }

        public CommandBehavior? Behavior { get; }

        public CommandType Type { get; set; }

        public int Timeout { get; set; }

        public void Add(IDbDataParameter parameter)
        {
            this.command.Parameters.Add(parameter);
            this.parameters.Add(parameter);
        }

        public TParameter CreateParameter<TParameter>()
            where TParameter : IDbDataParameter
                => (TParameter)this.command.CreateParameter();

        public IDbDataParameter CreateParameter()
            => this.command.CreateParameter();

        public int Execute()
            => this.command.ExecuteNonQuery();

        public Task<int> ExecuteAsync()
            => this.dbCommand?.ExecuteNonQueryAsync();

        public Task<int> ExecuteAsync(CancellationToken cancellationToken = default)
            => this.dbCommand?.ExecuteNonQueryAsync(cancellationToken);

        public IDataReader ExecuteReader(CommandBehavior? behavior = CommandBehavior.Default)
        {
            behavior = behavior ?? this.Behavior;
            if (!behavior.HasValue)
                return new DataReader(this.command.ExecuteReader());

            return new DataReader(this.command.ExecuteReader(behavior.Value));
        }

        public async Task<IDataReader> ExecuteReaderAsync(
            CommandBehavior? behavior = CommandBehavior.Default,
            CancellationToken cancellationToken = default)
        {
            behavior = behavior ?? this.Behavior;
            System.Data.IDataReader dr = null;
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

        public object ExecuteScalar()
        {
            return this.command.ExecuteScalar();
        }

        public Task<object> ExecuteScalarAsync(CancellationToken cancellationToken = default)
        {
            return this.dbCommand.ExecuteScalarAsync(cancellationToken);
        }

        public void Prepare()
        {
            this.command.Prepare();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

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

        ~DataCommand()
        {
            this.Dispose(false);
        }
    }
}