using System;

namespace NerdyMishka.Extensions.AppInsights.Abstractions
{
    /// <summary>
    /// Contract for the root telemetry record type supports properties.
    /// </summary>
    public interface ISupportSamplingRecord
    {
        /// <summary>
        /// Gets or sets the sampling percentage.
        /// </summary>
        /// <value>The sampling percentage.</value>
        double? SamplingPercentage { get; set; }
    }
}