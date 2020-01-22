namespace NerdyMishka.Extensions.Logging.AppInsights
{
    public class AppInsightsLoggerOptions
    {
        public string InstrumentKey { get; set; }

        public bool UseCustomEvents { get; set; }

        public bool IncludeScopes { get; set; } = true;
    }
}