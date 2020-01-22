using System.Collections.Generic;

namespace NerdyMishka.Extensions.AppInsights.Abstractions
{
    /// <summary>
    /// Contract for the root telemetry record type supports metrics.
    /// </summary>
    public interface ISupportMetricsRecord
    {
        /// <summary>
        /// Gets the collection of named metrics associated with this record.
        /// </summary>
        /// <value>Named metrics associated with this record.</value>
        IDictionary<string, string> Properties { get; }
    }
}