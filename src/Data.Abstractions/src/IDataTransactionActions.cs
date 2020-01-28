using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NerdyMishka.Data
{
    /// <summary>
    /// A contract for transaction methods.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IDataTransactionActions : IDisposable
    {
        /// <summary>
        /// Sets a value to note an automatic commit.
        /// </summary>
        /// <param name="autoCommit">if set to <c>true</c> [automatic commit].</param>
        void SetAutoCommit(bool autoCommit = true);

        /// <summary>
        /// Commits all changes within the transaction to the database.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rolls all the changes within the transaction back.
        /// </summary>
        void Rollback();
    }
}