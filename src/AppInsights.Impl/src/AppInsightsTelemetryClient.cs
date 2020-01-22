using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Logging;
using NerdyMishka.Extensions.AppInsights.Abstractions;

namespace NerdyMishka.Extensions.AppInsights.Impl
{
    public class AppInsightsTelemetryClient : ITelemetryClient,
        IRawTelemetryClient
    {
        private readonly TelemetryClient client;

        public AppInsightsTelemetryClient(TelemetryClient client)
        {
            if (client is null)
                throw new ArgumentNullException(nameof(client));

            this.client = client;
        }

        public void Flush() => this.client.Flush();

        public IMetric GetMetric(string metricId)
            => new MetricImpl(this.client.GetMetric(metricId));

        public IMetric GetMetric(string metricId, string dimension1)
            => new MetricImpl(this.client.GetMetric(metricId, dimension1));

        public IMetric GetMetric(string metricId, string dimension1, string dimension2)
            => new MetricImpl(this.client.GetMetric(metricId, dimension1, dimension2));

        public IMetric GetMetric(string metricId, string dimension1, string dimension2, string dimension3)
            => new MetricImpl(this.client.GetMetric(
                metricId,
                dimension1,
                dimension2,
                dimension3));

        public IMetric GetMetric(
                string metricId,
                string dimension1,
                string dimension2,
                string dimension3,
                string dimension4)
        {
            return new MetricImpl(this.client.GetMetric(
                metricId,
                dimension1,
                dimension2,
                dimension3,
                dimension4));
        }

        public bool IsEnabled()
            => this.client.IsEnabled();

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

        public void TrackEvent(
            string eventName,
            IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null)
        {
            this.client.TrackEvent(eventName, properties, metrics);
        }

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

        public void TrackException(
            Exception exception,
            IDictionary<string, string> properties = null,
            IDictionary<string, double> metrics = null)
        {
            this.TrackException(exception, properties, metrics);
        }

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

        public void TrackMetric(string metricId, double value)
        {
            this.client.TrackMetric(metricId, value);
        }

        public void TrackPageView(string name)
        {
            this.client.TrackPageView(name);
        }

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

        public void TrackTrace(
            string message,
            LogLevel logLevel = LogLevel.Information,
            IDictionary<string, string> properties = null)
        {
            var severity = this.GetSeverityLevel(logLevel);

            this.client.TrackTrace(message, severity, properties);
        }

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