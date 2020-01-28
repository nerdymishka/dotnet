using System;

namespace NerdyMishka.Data
{
    /// <summary>
    /// A scope for <see cref="IDataCommand"/>.  The scope is either a <see cref="IDataCommand"/>
    /// or <see cref="IDataTransaction"/>. This interface enables extensions methods to have a common
    /// contract to create commands and manage the command's parent.
    /// </summary>
    /// <seealso cref="System.IObserver{NerdyMishka.Data.IDataCommandBuilder}" />
    /// <seealso cref="NerdyMishka.Data.IDataCommandFactory" />
    /// <seealso cref="System.IDisposable" />
    public interface IDataCommandScope : IObserver<IDataCommandBuilder>,
        IDataCommandFactory,
        IDisposable
    {
        /// <summary>
        /// Gets the <see cref="ISqlDialect"/>.
        /// </summary>
        /// <value>
        /// The SQL dialect.
        /// </value>
        ISqlDialect SqlDialect { get; }
    }
}