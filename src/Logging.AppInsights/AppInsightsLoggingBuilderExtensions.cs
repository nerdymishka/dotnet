using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NerdyMishka.Extensions.AppInsights.Abstractions;
using NerdyMishka.Extensions.Logging.AppInsights;

namespace Microsoft.Extensions.Logging
{
    /// <summary>
    /// Extension methods to add and configure the IRawTelemetryClient logger.
    /// </summary>
    public static class AppInsightsLoggingBuilderExtensions
    {
        /// <summary>
        /// Adds an ApplicationInsights logger named 'ApplicationInsights' to the factory.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
        /// <param name="configureTelemetryClient">
        /// Action to configure the <see cref="IRawTelemetryClient"/> service.
        /// </param>
        /// <returns>Logging builder with Application Insights added to it.</returns>
        public static ILoggingBuilder AddAppInsights(
            this ILoggingBuilder builder,
            Func<TelemetryClient> createTelemetryClient,
            Action<TelemetryClientLoggerOptions> createTelemetryClientOptions)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            if (createTelemetryClient is null)
                throw new ArgumentNullException(nameof(configureTelemetryClient));

            if (createTelemetryClientOptions)
                throw new ArgumentNullException(nameof(createTelemetryClientOptions));

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, AppInsightsLoggerProvider>());
            builder.Services.Configure(createTelemetryClientOptions);
            builder.Services.AddSingleton(() =>
            {
                new AppInsightsTelemetryClient(createTelemetryClient());
            });

            return builder;
        }

        /// <summary>
        /// Adds an ApplicationInsights logger named 'ApplicationInsights' to the factory.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
        /// <param name="configureTelemetryClient">
        /// Action to configure the <see cref="IRawTelemetryClient"/> service.
        /// </param>
        /// <returns>Logging builder with Application Insights added to it.</returns>
        public static ILoggingBuilder AddAppInsights(
            this ILoggingBuilder builder, string instrumentKey)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            if (createTelemetryClient is null)
                throw new ArgumentNullException(nameof(configureTelemetryClient));

            if (createTelemetryClientOptions)
                throw new ArgumentNullException(nameof(createTelemetryClientOptions));

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, AppInsightsLoggerProvider>());
            builder.Services.Configure<TelemetryClientLoggerOptions>((options) => { });
            builder.Services.AddSingleton(() =>
            {
                new AppInsightsTelemetryClient(
                    new TelemetryClient(instrumentKey));
            });

            return builder;
        }
    }
}