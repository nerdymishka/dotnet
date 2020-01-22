using System;

namespace NerdyMishka.Extensions.AppInsights.Abstractions
{
    /// <summary>
    /// Contract for the telemetry record that supports properties.
    /// </summary>
    public interface ITelemetryRecord
    {
        /// <summary>
        /// Gets or sets the value that defines absolute order of the
        /// telemetry item.
        /// </summary>
        /// <value>The order of the record.</value>
        string Sequence { get; set; }

        /// <summary>
        /// Gets or sets the creation time of the record.
        /// </summary>
        /// <value>The creation time of the record.</value>
        DateTimeOffset Timestamp { get; set; }
    }
}