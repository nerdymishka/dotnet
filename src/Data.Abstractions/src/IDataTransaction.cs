namespace NerdyMishka.Data
{
    /// <summary>
    /// Contract that represents a database transaction.
    /// </summary>
    /// <seealso cref="NerdyMishka.Data.IDataTransactionActions" />
    /// <seealso cref="NerdyMishka.Data.IDataCommandScope" />
    /// <seealso cref="NerdyMishka.Data.IUnwrap" />
    public interface IDataTransaction :
        IDataTransactionActions,
        IDataCommandScope,
        IUnwrap
    {
        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>The connection.</value>
        IDataConnection Connection { get; }
    }
}