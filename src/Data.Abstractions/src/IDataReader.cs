using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NerdyMishka.Data
{
    public interface IDataReader : IDataRecord,
        IEnumerable<IDataRecord>,
        IDisposable
    {
        /// <summary>
        /// Gets the depth of nesting for the current row.
        /// </summary>
        /// <value>The depth.</value>
        int Depth { get; }

        /// <summary>
        /// Gets whether or not this data set has rows.
        /// </summary>
        /// <value><see cref="System.Boolean">.</value>
        bool HasRows { get; }

        /// <summary>
        /// Gets whether or not the data reader is closed.
        /// </summary>
        /// <value><see cref="System.Boolean">.</value>
        bool IsClosed { get; }

        /// <summary>
        /// Reads the next row.
        /// </summary>
        /// <returns><c>True</c> if the read moved to the next row; 
        /// Otherwise, <c>False</c></returns>
        bool Read();

        /// <summary>
        /// Reads the next row.
        /// </summary>
        /// <returns>The task.</returns>
        Task<bool> ReadAsync();

        /// <summary>
        /// Reads the next row.
        /// </summary>
        /// <param name="cancellationToken">The cancel token.</param>
        /// <returns>The task.</returns>
        Task<bool> ReadAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Moves to the next result set if one exists.
        /// </summary>
        /// <returns><c>True</c> if the read moves to the next result set; 
        /// Otherwise, <c>False</c></returns>
        bool NextResult();

        /// <summary>
        /// Moves to the next result set if one exists.
        /// </summary>
        /// <returns>The task.</returns>
        Task<bool> NextResultAsync();

        /// <summary>
        /// Moves to the next result set if one exists.
        /// </summary>
        /// <param name="cancellationToken">The cancel token.</param>
        /// <returns>The task.</returns>
        Task<bool> NextResultAsync(CancellationToken cancellationToken);
    }
}