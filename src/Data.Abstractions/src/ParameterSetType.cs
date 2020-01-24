namespace NerdyMishka.Data
{
    /// <summary>
    /// The type of parameter set in used for the command.
    /// </summary>
    public enum ParameterSetType
    {
        /// <summary>
        /// No parameters.
        /// </summary>
        None = 0,

        /// <summary>
        /// Array of values.
        /// </summary>
        Array = 1,

        /// <summary>
        /// <see cref="System.Collections.Generic.KeyValuePair{String,Object}" />.
        /// </summary>
        KeyValue = 2,

        /// <summary>
        /// <see cref="System.Collections.IDictionary" />.
        /// </summary>
        Hashtable = 3,

        /// <summary>
        /// An enumerable of <see cref="System.Data.IDbDataParameter" />.
        /// </summary>
        DbParameters = 4,
    }
}