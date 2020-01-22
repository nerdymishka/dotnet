using System;

namespace NerdyMishka.Extensions.AppInsights.Abstractions
{
    /// <summary>
    /// Base class for telemetry records.
    /// </summary>
    public abstract class TelemetryRecord : ITelemetryRecord
    {
        /// <summary>
        /// Gets or sets the value that defines absolute order of the
        /// telemetry item.
        /// </summary>
        /// <value>The order of the record.</value>
        public string Sequence { get; set; }

        /// <summary>
        /// Gets or sets the creation time of the record.
        /// </summary>
        /// <value>The creation time of the record.</value>
        public DateTimeOffset Timestamp { get; set; }
    }
}