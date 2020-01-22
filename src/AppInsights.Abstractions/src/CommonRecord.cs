using System.Collections.Concurrent;
using System.Collections.Generic;

namespace NerdyMishka.Extensions.AppInsights.Abstractions
{
    /// <summary>
    /// Common base class for records that support metrics and sampling.
    /// </summary>
    public class CommonRecord : SupportPropertiesRecord, ISupportMetricsRecord, ISupportSamplingRecord
    {
        private IDictionary<string, double> metrics;

        /// <summary>
        /// Gets the collection of named metrics associated with this record.
        /// </summary>
        /// <value>Named metrics associated with this record.</value>
        public IDictionary<string, double> Metrics
        {
            get
            {
                if (this.metrics == null)
                    this.metrics = new ConcurrentDictionary<string, double>();

                return this.metrics;
            }
        }

        /// <summary>
        /// Gets or sets the sampling percentage.
        /// </summary>
        /// <value>The sampling percentage.</value>
        public double? SamplingPercentage { get; set; }
    }
}