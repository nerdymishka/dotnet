using System.Data;

namespace NerdyMishka.Data
{
    /// <summary>
    /// A contract that represents a SQL Type mapping.
    /// </summary>
    /// <seealso cref="NerdyMishka.Data.ISqlTypeInfo" />
    public interface ISqlTypeMap : ISqlTypeInfo
    {
        /// <summary>
        /// Gets the format provider.
        /// </summary>
        /// <value>
        /// The format provider.
        /// </value>
        string FormatProvider { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is unicode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is unicode; otherwise, <c>false</c>.
        /// </value>
        bool IsUnicode { get; }

        /// <summary>
        /// Configures the parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        void ConfigureParameter(IDbDataParameter parameter);

        /// <summary>
        /// Writes the string literal for SQL for the given value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The SQL literal.</returns>
        string WriteLiteral(object instance);
    }
}