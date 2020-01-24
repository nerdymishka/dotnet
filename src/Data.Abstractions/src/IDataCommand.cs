using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace NerdyMishka.Data
{
    public interface IDataCommand : IDisposable
    {
        IDataConnectionActions Connection { get; }

        IDataTransactionActions Transaction { get; set; }

        IReadOnlyCollection<IDbDataParameter> Parameters { get; }

        string Text { get; set; }

        CommandBehavior? Behavior { get; }

        CommandType Type { get; set; }

        ISqlDialect SqlDialect { get; }

        int Timeout { get; set; }

        TParameter CreateParameter<TParameter>()
            where TParameter : IDbDataParameter;

        IDbDataParameter CreateParameter();

        void Add(IDbDataParameter parameter);

        int Execute();

        Task<int> ExecuteAsync(CancellationToken cancellationToken = default);

        object ExecuteScalar();

        Task<object> ExecuteScalarAsync(CancellationToken cancellationToken = default);

        IDataReader ExecuteReader(CommandBehavior? behavior = default);

        Task<IDataReader> ExecuteReaderAsync(
            CommandBehavior? behavior = default,
            CancellationToken cancellationToken = default);

        void Prepare();
    }
}