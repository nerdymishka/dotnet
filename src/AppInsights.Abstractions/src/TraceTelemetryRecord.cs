using Microsoft.Extensions.Logging;

namespace NerdyMishka.Extensions.AppInsights.Abstractions
{
    /// <summary>
    /// A trace telemetry record.
    /// </summary>
    public class TraceTelemetryRecord : CommonRecord
    {
        /// <summary>
        /// Initializes a new instance of the  <see cref="TraceTelemetryRecord"/>
        /// class.
        /// </summary>
        public TraceTelemetryRecord()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceTelemetryRecord"/>
        /// class.
        /// </summary>
        /// <param name="message">The trace message.</param>
        public TraceTelemetryRecord(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceTelemetryRecord"/>
        /// class.
        /// </summary>
        /// <param name="message">The trace message.</param>
        /// <param name="severityLevel">The severity level.</param>
        public TraceTelemetryRecord(string message, LogLevel severityLevel)
            : this(message)
        {
            this.SeverityLevel = severityLevel;
        }

        /// <summary>
        /// Gets or sets the trace message.
        /// </summary>
        /// <value>the trace message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the severity level.
        /// </summary>
        /// <value>the severity level.</value>
        public LogLevel? SeverityLevel { get; set; }
    }
}