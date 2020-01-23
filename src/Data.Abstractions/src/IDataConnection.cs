

namespace NerdyMishka.Data
{
    public interface IDataConnection : IDataConnectionActions,
        IDataTransactionFactory, IDataCommandScope
    {

    }
}