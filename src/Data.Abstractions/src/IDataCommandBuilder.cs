namespace NerdyMishka.Data
{
    /// <summary>
    /// A contract for a data command builder.
    /// </summary>
    public interface IDataCommandBuilder
    {
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        IDataCommandConfiguration Configuration { get; }

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        /// <value>
        /// The command.
        /// </value>
        IDataCommand Command { get; set; }
    }
}