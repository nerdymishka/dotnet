namespace NerdyMishka.Data
{
    public interface IDataTransaction :
        IDataTransactionActions,
        IDataCommandScope,
        IUnwrappable
    {
        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>The connection.</value>
        IDataConnection Connection { get; }
    }
}