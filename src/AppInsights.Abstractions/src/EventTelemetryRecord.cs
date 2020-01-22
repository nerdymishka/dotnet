namespace NerdyMishka.Extensions.AppInsights.Abstractions
{
    /// <summary>
    /// An event telemetry record.
    /// </summary>
    public class EventTelemetryRecord : CommonRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventTelemetryRecord"/>
        /// class.
        /// </summary>
        public EventTelemetryRecord()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTelemetryRecord"/>
        /// class.
        /// </summary>
        /// <param name="name">The name of the event.</param>
        public EventTelemetryRecord(string name)
            : this()
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        /// <value>The name of the event.</value>
        public string Name { get; set; }
    }
}