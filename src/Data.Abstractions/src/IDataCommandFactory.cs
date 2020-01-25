using System.Data;

namespace NerdyMishka.Data
{
    /// <summary>
    /// A contract for creating <see cref="IDataCommand"/>.
    /// </summary>
    public interface IDataCommandFactory
    {
        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns>The data command.</returns>
        IDataCommand CreateCommand(CommandBehavior? behavior = default);
    }
}