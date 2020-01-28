using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace NerdyMishka.Data
{
    public interface IDataConnectionActions : IDisposable
    {
        /// <summary>
        /// Gets the name of the database provider.
        /// </summary>
        /// <value>
        /// The provider.
        /// </value>
        string Provider { get; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        string ConnectionString { get; set; }

        /// <summary>
        /// Gets the connection state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        ConnectionState State { get; }

        /// <summary>
        /// Opens a database connection.
        /// </summary>
        void Open();

        /// <summary>
        /// Opens a database connection.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task for opening the connection.</returns>
        Task OpenAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Closes the database connection.
        /// </summary>
        void Close();
    }
}