namespace NerdyMishka.Extensions.AppInsights.Abstractions
{
    /// <summary>
    /// Contract for a telemtry client implementation the supports
    /// core telemetry records.
    /// </summary>
    public interface IRawTelemetryClient : ITelemetryClient
    {
        /// <summary>
        /// Send any telemetry record.
        /// </summary>
        /// <param name="telemetry">The telemetry.</param>
        void Track(ITelemetryRecord telemetry);

        /// <summary>
        /// Send an exception.
        /// </summary>
        /// <param name="telemetry">The exception telemetry.</param>
        void TrackException(ExceptionTelemetryRecord exceptionTelemetry);

        /// <summary>
        /// Send an event.
        /// </summary>
        /// <param name="eventTelemetry">The event record.</param>
        void TrackEvent(EventTelemetryRecord eventTelemetry);

        /// <summary>
        /// Send a trace.
        /// </summary>
        /// <param name="traceTelemetry">The trace record.</param>
        void TrackTrack(TraceTelemetryRecord traceTelemetry);
    }
}