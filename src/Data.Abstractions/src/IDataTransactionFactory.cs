using System.Data;

namespace NerdyMishka.Data
{
    /// <summary>
    /// A contract for a transaction factory.
    /// </summary>
    public interface IDataTransactionFactory
    {
        /// <summary>
        /// Creates and starts a database transaction.
        /// </summary>
        /// <param name="level">The isolation level.</param>
        /// <returns>The transaction.</returns>
        IDataTransactionActions BeginTransaction(IsolationLevel level = default);
    }
}