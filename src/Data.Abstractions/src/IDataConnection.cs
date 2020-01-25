namespace NerdyMishka.Data
{
    /// <summary>
    /// A contract for an enhanced <see cref="System.Data.IDbConnection" />.
    /// </summary>
    public interface IDataConnection : IDataConnectionActions,
        IDataTransactionFactory,
        IDataCommandScope,
        IUnwrappable
    {
    }
}