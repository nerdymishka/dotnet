using System.Collections.Generic;

namespace NerdyMishka.Extensions.AppInsights.Abstractions
{
    /// <summary>
    /// Contract for the root telemetry record type supports properties.
    /// </summary>
    public interface ISupportPropertiesRecord
    {
        /// <summary>
        /// Gets the collection of name values associated with this record.
        /// </summary>
        /// <value>Name values associated with this record.</value>
        IDictionary<string, string> Properties { get; }
    }
}