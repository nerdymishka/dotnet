namespace NerdyMishka.Data
{
    /// <summary>
    /// A contract for an enhanced <see cref="System.Data.IDbConnection" />.
    /// </summary>
    /// <seealso cref="NerdyMishka.Data.IDataConnectionActions" />
    /// <seealso cref="NerdyMishka.Data.IDataTransactionFactory" />
    /// <seealso cref="NerdyMishka.Data.IDataCommandScope" />
    /// <seealso cref="NerdyMishka.Data.IUnwrap" />
    public interface IDataConnection : IDataConnectionActions,
        IDataTransactionFactory,
        IDataCommandScope,
        IUnwrap
    {
    }
}