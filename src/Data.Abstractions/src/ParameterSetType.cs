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
        /// A set of parameters stored as a <see cref="System.Collections.IList"/>.
        /// </summary>
        List = 1,

        /// <summary>
        /// A set of parameters stored as a <see cref="System.Collections.IDictionary"/>.
        /// </summary>
        Lookup = 2,

        /// <summary>
        /// A set of parameters stored as a <see cref="System.Collections.Generic.IEnumerable{System.Data.IDbDataParameter}"/>.
        /// </summary>
        TypedList = 3,

        /// <summary>
        /// A set of parameters stored as a <see cref="System.Collections.Generic.KeyValuePair{String,Object}" />.
        /// </summary>
        TypedLookup = 4,
    }
}