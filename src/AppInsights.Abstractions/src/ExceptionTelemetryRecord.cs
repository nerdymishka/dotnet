using System;
using Microsoft.Extensions.Logging;

namespace NerdyMishka.Extensions.AppInsights.Abstractions
{
    // TODO: support exception detail info list constructor
    // https://docs.microsoft.com/en-us/dotnet/api/microsoft.applicationinsights.datacontracts.exceptiontelemetry.-ctor?view=azure-dotnet#Microsoft_ApplicationInsights_DataContracts_ExceptionTelemetry__ctor_System_Collections_Generic_IEnumerable_Microsoft_ApplicationInsights_DataContracts_ExceptionDetailsInfo__System_Nullable_Microsoft_ApplicationInsights_DataContracts_SeverityLevel__System_String_System_Collections_Generic_IDictionary_System_String_System_String__System_Collections_Generic_IDictionary_System_String_System_Double__

    /// <summary>
    /// The exception telemetry record.
    /// </summary>
    public class ExceptionTelemetryRecord : CommonRecord
    {
        /// <summary>
        /// Initializes a new instance of the  <see cref="ExceptionTelemetryRecord"/>
        /// class.
        /// </summary>
        public ExceptionTelemetryRecord()
        {
        }

        /// <summary>
        /// Initializes a new instance of the  <see cref="ExceptionTelemetryRecord"/>
        /// class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public ExceptionTelemetryRecord(Exception exception)
        {
            this.Exception = exception;
        }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>the exception.</value>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>the message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the problem id.
        /// </summary>
        /// <value>The problem id.</value>
        public string ProblemId { get; set; }

        /// <summary>
        /// Gets or sets the severity level.
        /// </summary>
        /// <value>The severity level.</value>
        public LogLevel SeverityLevel { get; set; }
    }
}