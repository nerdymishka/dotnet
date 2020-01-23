using System.Data;

namespace NerdyMishka.Data
{
    public interface IDataTransactionFactory
    {
        IDataTransactionActions BeginTransaction(IsolationLevel level = default);
    }
}