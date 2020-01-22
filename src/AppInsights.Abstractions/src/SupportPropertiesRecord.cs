using System.Collections.Concurrent;
using System.Collections.Generic;

namespace NerdyMishka.Extensions.AppInsights.Abstractions
{
    /// <summary>
    /// Base class for records that support properties.
    /// </summary>
    public class SupportPropertiesRecord : TelemetryRecord, ISupportPropertiesRecord
    {
        private IDictionary<string, string> properties;

        /// <summary>
        /// Gets the collection of name values associated with this record.
        /// </summary>
        /// <value>Name values associated with this record.</value>
        public IDictionary<string, string> Properties
        {
            get
            {
                if (this.properties == null)
                    this.properties = new ConcurrentDictionary<string, string>();

                return this.properties;
            }
        }
    }
}