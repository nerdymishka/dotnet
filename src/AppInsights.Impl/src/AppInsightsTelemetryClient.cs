using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Logging;
using NerdyMishka.Extensions.AppInsights.Abstractions;

namespace NerdyMishka.Extensions.AppInsights.Impl
{
    /// <summary>
    /// Implementation of <see cref="IRawTelemetryClient" /> that wraps
    /// <see cref="TelemetryClient" />.
    /// </summary>
    /// <see cref="ITelemetryClient" />
    /// <see cref="IRawTelemetryClient" />
    public class AppInsightsTelemetryClient : ITelemetryClient,
        IRawTelemetryClient
    {
        private readonly TelemetryClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppInsightsTelemetryClient"/>
        /// class.
        /// </summary>
        /// <param name="client">The <see cref="TelemetryClient" />.</param>
        public AppInsightsTelemetryClient(TelemetryClient client)
        {
            if (client is null)
                throw new ArgumentNullException(nameof(client));

            this.client = client;
        }

        /// <summary>
        /// Flushes any messages in the buffer.
        /// </summary>
        public void Flush() => this.client.Flush();

        /// <summary>
        /// Gets or creates a metric container that you can use to track,
        /// aggregate, and send metric values.
        /// </summary>
        /// <param name="metricId">
        /// The identify (name) of the metric. The <c>DefaultMetricNamespace</c>
        /// will be used.
        /// </param>
        /// <returns>A Metric with the given Id and dimensions.</returns>
        public IMetric GetMetric(string metricId)
            => new MetricImpl(this.client.GetMetric(metricId));

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
        public IMetric GetMetric(string metricId, string dimension1)
            => new MetricImpl(this.client.GetMetric(metricId, dimension1));

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
        public IMetric GetMetric(string metricId, string dimension1, string dimension2)
            => new MetricImpl(this.client.GetMetric(metricId, dimension1, dimension2));

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
        public IMetric GetMetric(
            string metricId,
            string dimension1,
            string dimension2,
            string dimension3)
        {
            return new MetricImpl(
                this.client.GetMetric(
                    metricId,
                    dimension1,
                    dimension2,
                    dimension3));
        }

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
        public IMetric GetMetric(
            string metricId,
            string dimension1,
            string dimension2,
            string dimension3,
            string dimension4)
        {
            return new MetricImpl(
                this.client.GetMetric(
                    metricId,
                    dimension1,
                    dimension2,
                    dimension3,
                    dimension4));
        }

        /// <summary>
        /// Gets the enablement status of the client.
        /// </summary>
        /// <returns><c>True</c> If the client is enabled; Otherwise,
        /// <c>false</c>.
        /// </returns>
        public bool IsEnabled()
            => this.client.IsEnabled();

        /// <summary>
        /// Send any telemetry record.
        /// </summary>
        /// <param name="telemetry">The telemetry.</param>
        public void Track(ITelemetryRecord telemetry)
        {
            if (telemetry is null)
                throw new ArgumentNullException(nameof(telemetry));

            switch (telemetry)
            {
                case TraceTelemetryRecord trace:
                    this.TrackTrace(trace);
                    break;
                case EventTelemetryRecord eventTelemetry:
                    this.TrackEvent(eventTelemetry);
                    break;
                case ExceptionTelemetryRecord exception:
                    this.TrackException(exception);
                    break;
                default:
                    throw new NotSupportedException(telemetry.GetType().FullName);
            }
        }

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
        public void TrackAvailability(
            string name,
            DateTimeOffset timeStamp,
            TimeSpan duration,
            string runLocation,
            bool success,
            string message = null,
            IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null)
        {
            this.client.TrackAvailability(
                name,
                timeStamp,
                duration,
                runLocation,
                success,
                message,
                properties,
                metrics);
        }

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
        public void TrackDependency(
            string dependencyTypeName,
            string dependencyName,
            string data,
            DateTimeOffset startTime,
            TimeSpan duration,
            bool success)
        {
            this.client.TrackDependency(
                dependencyTypeName,
                dependencyName,
                data,
                startTime,
                duration,
                success);
        }

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
        public void TrackDependency(
            string dependencyTypeName,
            string target,
            string dependencyName,
            string data,
            DateTimeOffset startTime,
            TimeSpan duration,
            string resultCode,
            bool success)
        {
            this.client.TrackDependency(
                dependencyTypeName,
                target,
                dependencyTypeName,
                data,
                startTime,
                duration,
                resultCode,
                success);
        }

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
        public void TrackEvent(
            string eventName,
            IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null)
        {
            this.client.TrackEvent(eventName, properties, metrics);
        }

        /// <summary>
        /// Send an event.
        /// </summary>
        /// <param name="eventTelemetry">The event record.</param>
        public void TrackEvent(EventTelemetryRecord eventTelemetry)
        {
            if (eventTelemetry is null)
                throw new ArgumentNullException(nameof(eventTelemetry));

            var t = new EventTelemetry(
                    eventTelemetry.Name);

            foreach (var k in eventTelemetry.Properties.Keys)
                t.Properties.Add(k, eventTelemetry.Properties[k]);

            foreach (var k in eventTelemetry.Metrics.Keys)
                t.Metrics.Add(k, eventTelemetry.Metrics[k]);

            t.Timestamp = eventTelemetry.Timestamp;
            t.Sequence = eventTelemetry.Sequence;

            this.client.TrackEvent(t);
        }

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
        public void TrackException(
            Exception exception,
            IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null)
        {
            this.TrackException(exception, properties, metrics);
        }

        /// <summary>
        /// Send an exception.
        /// </summary>
        /// <param name="exceptionTelemetry">The exception telemetry.</param>
        public void TrackException(ExceptionTelemetryRecord exceptionTelemetry)
        {
            if (exceptionTelemetry is null)
                throw new ArgumentNullException(nameof(exceptionTelemetry));

            var t = new ExceptionTelemetry(
                    exceptionTelemetry.Exception);

            foreach (var k in exceptionTelemetry.Properties.Keys)
                t.Properties.Add(k, exceptionTelemetry.Properties[k]);

            foreach (var k in exceptionTelemetry.Metrics.Keys)
                t.Metrics.Add(k, exceptionTelemetry.Metrics[k]);

            t.Timestamp = exceptionTelemetry.Timestamp;
            t.Sequence = exceptionTelemetry.Sequence;

            this.client.TrackException(t);
        }

        /// <summary>
        /// Sends a metric. <see cref="GetMetric(string, double)" /> is preferred.
        /// </summary>
        /// <param name="metricId">The id (name) of the metric.</param>
        /// <param name="value">The metric value.</param>
        public void TrackMetric(string metricId, double value)
        {
            this.client.TrackMetric(metricId, value);
        }

        /// <summary>
        /// Send information about the page viewed.
        /// </summary>
        /// <param name="name">The name of the page.</param>
        public void TrackPageView(string name)
        {
            this.client.TrackPageView(name);
        }

        /// <summary>
        /// Send information about a request handled by the application.
        /// </summary>
        /// <param name="name">The request name.</param>
        /// <param name="startTime">The process request start timestamp.</param>
        /// <param name="duration">The process request length of execution in time.</param>
        /// <param name="responseCode">The response status code.</param>
        /// <param name="success"><c>True</c>, if the request was handled;
        /// Otherwise, <c>False</c>.</param>
        public void TrackRequest(
            string name,
            DateTimeOffset startTime,
            TimeSpan duration,
            string responseCode = "200",
            bool success = true)
        {
            this.client.TrackRequest(
                name,
                startTime,
                duration,
                responseCode,
                success);
        }

        /// <summary>
        /// Send a trace message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logLevel">The serverity level.</param>
        /// <param name="properties">Named string values to search.</param>
        public void TrackTrace(
            string message,
            LogLevel logLevel = LogLevel.Information,
            IDictionary<string, string> properties = null)
        {
            var severity = this.GetSeverityLevel(logLevel);

            this.client.TrackTrace(message, severity, properties);
        }

        /// <summary>
        /// Send a trace message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logLevel">The serverity level.</param>
        /// <param name="properties">Named string values to search.</param>
        public void TrackTrace(
            string message,
            int logLevel = -1,
            IDictionary<string, string> properties = null)
        {
            var severity = SeverityLevel.Information;
            if (logLevel > 0 && logLevel < 6)
                severity = (SeverityLevel)logLevel;

            this.client.TrackTrace(message, severity, properties);
        }

        /// <summary>
        /// Send a trace.
        /// </summary>
        /// <param name="traceTelemetry">The trace record.</param>
        public void TrackTrace(TraceTelemetryRecord traceTelemetry)
        {
            if (traceTelemetry is null)
                throw new ArgumentNullException(nameof(traceTelemetry));

            var t = new TraceTelemetry(
                    traceTelemetry.Message,
                    this.GetSeverityLevel(traceTelemetry.SeverityLevel));

            foreach (var k in traceTelemetry.Properties.Keys)
                t.Properties.Add(k, traceTelemetry.Properties[k]);

            t.Timestamp = traceTelemetry.Timestamp;
            t.Sequence = traceTelemetry.Sequence;

            this.client.TrackTrace(t);
        }

        private SeverityLevel GetSeverityLevel(LogLevel? logLevel)
        {
            SeverityLevel severity = SeverityLevel.Information;

            switch (logLevel)
            {
                case LogLevel.Debug:
                case LogLevel.Trace:
                    severity = SeverityLevel.Verbose;
                    break;

                case LogLevel.None:
                case LogLevel.Information:
                    severity = SeverityLevel.Information;
                    break;

                case LogLevel.Warning:
                    severity = SeverityLevel.Warning;
                    break;

                case LogLevel.Error:
                    severity = SeverityLevel.Error;
                    break;

                case LogLevel.Critical:
                    severity = SeverityLevel.Critical;
                    break;
            }

            return severity;
        }
    }
}