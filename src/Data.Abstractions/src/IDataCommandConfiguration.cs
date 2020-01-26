using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace NerdyMishka.Data
{
    /// <summary>
    /// A contract that represents a data command configuration.
    /// </summary>
    public interface IDataCommandConfiguration
    {
        /// <summary>
        /// Gets the parameter set type such as list or dictionary.
        /// </summary>
        /// <value>
        /// The type of the set.
        /// </value>
        ParameterSetType SetType { get; }

        /// <summary>
        /// Gets the database parameters. This represets a list of <see cref="System.Data.IDbDataParameter"/>.
        /// </summary>
        /// <value>
        /// The database parameters.
        /// </value>
        IEnumerable<IDbDataParameter> TypedParameterList { get; }

        /// <summary>
        /// Gets the typed parameter lookup. This represents a parameter set stored
        /// as a set of name value pairs.
        /// </summary>
        /// <value>
        /// The typed parameter lookup.
        /// </value>
        IEnumerable<KeyValuePair<string, object>> TypedParameterLookup { get; }

        char ParameterPrefix { get; set; }

        /// <summary>
        /// Gets the parameter lookup. This property represents a parameter set stored
        /// as dictionary of name value pairs. The name is assumed to be a string.
        /// </summary>
        /// <value>
        /// The parameter lookup.
        /// </value>
        IDictionary ParameterLookup { get; }

        /// <summary>
        /// Gets the parameter list. This property represents a parameter set stored
        /// as a list of parameter values in order from 0 to x.
        /// </summary>
        /// <value>
        /// The parameter list.
        /// </value>
        IList<object> ParameterList { get; }

        /// <summary>
        /// Gets or sets the query.
        /// </summary>
        /// <value>
        /// The query.
        /// </value>
        ISqlBuilder Query { get; set; }

        /// <summary>
        /// Gets or sets the command behavior.
        /// </summary>
        /// <value>
        /// The command behavior.
        /// </value>
        CommandBehavior CommandBehavior { get; set; }

        /// <summary>
        /// Gets or sets the type of the command.
        /// </summary>
        /// <value>
        /// The type of the command.
        /// </value>
        CommandType CommandType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is completeable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is completeable; otherwise, <c>false</c>.
        /// </value>
        bool IsCompleteable { get; set; }

        /// <summary>
        /// Sets the parameters.
        /// </summary>
        /// <param name="value">The value.</param>
        void SetParameters(IList<object> value);

        /// <summary>
        /// Sets the parameters.
        /// </summary>
        /// <param name="value">The value.</param>
        void SetParameters(IDictionary value);

        /// <summary>
        /// Sets the parameters.
        /// </summary>
        /// <param name="value">The value.</param>
        void SetParameters(IEnumerable<IDbDataParameter> value);

        /// <summary>
        /// Sets the parameters.
        /// </summary>
        /// <param name="value">The value.</param>
        void SetParameters(IEnumerable<KeyValuePair<string, object>> value);
    }
}