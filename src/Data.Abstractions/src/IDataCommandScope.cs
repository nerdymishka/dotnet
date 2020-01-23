using System;

namespace NerdyMishka.Data
{
    public interface IDataCommandScope : IObserver<IDataCommandBuilder>,
        IDataCommandFactory,
        IDisposable
    {
        ISqlDialect SqlDialect { get; }
    }
}