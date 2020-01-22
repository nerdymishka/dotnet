namespace NerdyMishka.Extensions.Logging.AppInsights.Abstractions
{
    public class TelemetryClientLoggerOptions
    {
        public string InstrumentKey { get; set; }

        public bool UseCustomEvents { get; set; }

        public bool IncludeScopes { get; set; } = true;
    }
}