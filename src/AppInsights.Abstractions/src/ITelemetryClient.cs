using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace NerdyMishka.Extensions.AppInsights.Abstractions
{
    /// <summary>
    /// Contract that represents a simplified instance of
    /// <c>Microsoft.ApplicationInsights.TelemetryClient</c> to enable Inversion
    /// of Control.
    /// </summary>
    public interface ITelemetryClient
    {
        /// <summary>
        /// Flushes any messages in the buffer.
        /// </summary>
        void Flush();

        /// <summary>
        /// Gets or creates a metric container that you can use to track, aggregate and send metric values.
        /// </summary>
        /// <param name="metricId">
        /// The identify (name) of the metric. The <c>DefaultMetricNamespace</c>
        /// will be used.
        /// </param>
        /// <returns>A Metric with the given Id and dimensions.</returns>
        IMetric GetMetric(string metricId);

        /// <summary>
        /// Gets or creates a metric container that you can use to track,
        /// aggregate, and send metric values.
        /// </summary>
        /// <param name="metricId">
        /// The identify (name) of the metric. The <c>DefaultMetricNamespace</c>
        /// will be used.
        /// </param>
        /// <param name="dimension1">
        /// The name of the 1st dimension.
        /// </param>
        /// <returns>A Metric with the given Id and dimensions.</returns>
        IMetric GetMetric(string metricId, string dimension1);

        /// <summary>
        /// Gets or creates a metric container that you can use to track,
        /// aggregate, and send metric values.
        /// </summary>
        /// <param name="metricId">
        /// The identify (name) of the metric. The <c>DefaultMetricNamespace</c>
        /// will be used.
        /// </param>
        /// <param name="dimension1">
        /// The name of the 1st dimension.
        /// </param>
        /// <param name="dimension2">
        /// The name of the 2nd dimension.
        /// </param>
        /// <returns>A Metric with the given Id and dimensions.</returns>
        IMetric GetMetric(string metricId, string dimension1, string dimension2);

        /// <summary>
        /// Gets or creates a metric container that you can use to track,
        /// aggregate, and send metric values.
        /// </summary>
        /// <param name="metricId">
        /// The identify (name) of the metric. The <c>DefaultMetricNamespace</c>
        /// will be used.
        /// </param>
        /// <param name="dimension1">
        /// The name of the 1st dimension.
        /// </param>
        /// <param name="dimension2">
        /// The name of the 2nd dimension.
        /// </param>
        /// <param name="dimension3">
        /// The name of the 3rd dimension.
        /// </param>
        /// <returns>A Metric with the given Id and dimensions.</returns>
        IMetric GetMetric(string metricId, string dimension1, string dimension2, string dimension3);

        /// <summary>
        /// Gets or creates a metric container that you can use to track,
        /// aggregate, and send metric values.
        /// </summary>
        /// <param name="metricId">
        /// The identify (name) of the metric. The <c>DefaultMetricNamespace</c>
        /// will be used.
        /// </param>
        /// <param name="dimension1">
        /// The name of the 1st dimension.
        /// </param>
        /// <param name="dimension2">
        /// The name of the 2nd dimension.
        /// </param>
        /// <param name="dimension3">
        /// The name of the 3rd dimension.
        /// </param>
        /// <param name="dimension4">
        /// The name of the 4th dimension.
        /// </param>
        /// <returns>A Metric with the given Id and dimensions.</returns>
        IMetric GetMetric(string metricId, string dimension1, string dimension2, string dimension3, string dimension4);

        /// <summary>
        /// Gets the enablement status of the client.
        /// </summary>
        /// <returns><c>True</c> If the client is enabled; Otherwise,
        /// <c>false</c>.
        /// </returns>
        bool IsEnabled();

        /// <summary>
        /// Send information about the availability of an application.
        /// </summary>
        /// <param name="name">Availability test name.</param>
        /// <param name="timeStamp">
        /// The availabilty test record submission timestamp.
        /// </param>
        /// <param name="duration">The avaiability test duration.</param>
        /// <param name="runLocation">Name of the location the availability test.</param>
        /// <param name="success">
        /// <c>True</c> when the test succeeds; Otherwise, <c>false</c>.
        /// </param>
        /// <param name="message">Error message for test failures.</param>
        /// <param name="properties">
        /// Named string values you can use to classify and search for this
        /// availability telemetry.
        /// </param>
        /// <param name="metrics">
        /// Metric values associated with this availability telemetry.
        /// </param>
        void TrackAvailability(
            string name,
            DateTimeOffset timeStamp,
            TimeSpan duration,
            string runLocation,
            bool success,
            string message = null,
            IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null);

        /// <summary>
        /// Send information about an external dependency(outgoing call) in
        /// the application.
        /// </summary>
        /// <param name="dependencyTypeName">
        /// External dependency type. Very low cardinality value for logical
        /// grouping and interpretation of fields. Examples are SQL, Azure
        /// table, and HTTP.
        /// </param>
        /// <param name="dependencyName">
        /// Name of the command initiated with this dependency call. Low
        /// cardinality value. Examples are stored procedure name and URL
        /// path template.
        /// </param>
        /// <param name="data">
        /// Command initiated by this dependency call. Examples are SQL
        /// statement and HTTP URL's with all query parameters.
        /// </param>
        /// <param name="startTime">
        /// The time when the dependency was called.
        /// </param>
        /// <param name="duration">
        /// The time taken by the external dependency to handle the call.
        /// </param>
        /// <param name="success">
        /// True if the dependency call was handled successfully.
        /// </param>
        void TrackDependency(
            string dependencyTypeName,
            string dependencyName,
            string data,
            DateTimeOffset startTime,
            TimeSpan duration,
            bool success);

        /// <summary>
        /// Send information about an external dependency(outgoing call) in
        /// the application.
        /// </summary>
        /// <param name="dependencyTypeName">
        /// External dependency type. Very low cardinality value for logical
        /// grouping and interpretation of fields. Examples are SQL, Azure
        /// table, and HTTP.
        /// </param>
        /// <param name="target">External dependency target.</param>
        /// <param name="dependencyName">
        /// Name of the command initiated with this dependency call. Low
        /// cardinality value. Examples are stored procedure name and URL
        /// path template.
        /// </param>
        /// <param name="data">
        /// Command initiated by this dependency call. Examples are SQL
        /// statement and HTTP URL's with all query parameters.
        /// </param>
        /// <param name="startTime">
        /// The time when the dependency was called.
        /// </param>
        /// <param name="duration">
        /// The time taken by the external dependency to handle the call.
        /// </param>
        /// <param name="resultCode">
        /// Result code of dependency call execution.
        /// </param>
        /// <param name="success">
        /// True if the dependency call was handled successfully.
        /// </param>
        void TrackDependency(
           string dependencyTypeName,
           string target,
           string dependencyName,
           string data,
           DateTimeOffset startTime,
           TimeSpan duration,
           string resultCode,
           bool success);

        /// <summary>
        /// Sends an event telemetry for display in the diagnostic search and
        /// in the analytics records.
        /// </summary>
        /// <param name="eventName">The name for the event.</param>
        /// <param name="properties">
        /// Named string values you can use to classify and search for this
        /// event.
        /// </param>
        /// <param name="metrics">
        /// Metric values associated with this event.
        /// </param>
        void TrackEvent(
            string eventName,
            IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null);

        /// <summary>
        /// Sends an exception records.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="properties">
        /// Named string values you can use to classify and search for this
        /// exception.
        /// </param>
        /// <param name="metrics">
        /// Metric values associated with this exception.
        /// </param>
        void TrackException(
            Exception exception,
            IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null);

        /// <summary>
        /// Sends a metric. <see cref="GetMetric(string, double)" /> is preferred.
        /// </summary>
        /// <param name="metricId">The id (name) of the metric.</param>
        /// <param name="value">The metric value.</param>
        void TrackMetric(string metricId, double value);

        /// <summary>
        /// Send information about the page viewed.
        /// </summary>
        /// <param name="name">The name of the page.</param>
        void TrackPageView(string name);

        /// <summary>
        /// Send information about a request handled by the application.
        /// </summary>
        /// <param name="name">The request name.</param>
        /// <param name="startTime">The process request start timestamp.</param>
        /// <param name="duration">The process request length of execution in time.</param>
        /// <param name="responseCode">The response status code.</param>
        /// <param name="success"><c>True</c>, if the request was handled;
        /// Otherwise, <c>False</c>.</param>
        void TrackRequest(
            string name,
            System.DateTimeOffset startTime,
            TimeSpan duration,
            string responseCode = "200",
            bool success = true);

        /// <summary>
        /// Send a trace message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logLevel">The serverity level.</param>
        /// <param name="properties">Named string values to search.</param>
        void TrackTrace(string message,
            LogLevel logLevel = LogLevel.Information,
            System.Collections.Generic.IDictionary<string, string> properties = null);

        /// <summary>
        /// Send a trace message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logLevel">The serverity level.</param>
        /// <param name="properties">Named string values to search.</param>
        void TrackTrace(string message,
            int logLevel = -1,
            System.Collections.Generic.IDictionary<string, string> properties = null);
    }
}