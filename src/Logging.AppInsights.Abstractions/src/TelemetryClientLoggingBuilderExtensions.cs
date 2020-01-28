using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NerdyMishka.Extensions.AppInsights.Abstractions;
using NerdyMishka.Extensions.Logging.AppInsights.Abstractions;

namespace Microsoft.Extensions.Logging
{
    /// <summary>
    /// Extension methods to add and configure the IRawTelemetryClient logger.
    /// </summary>
    public static class TelemetryClientLoggingBuilderExtensions
    {
        /// <summary>
        /// Adds an ApplicationInsights logger named 'ApplicationInsights' to the factory.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
        /// <param name="configureTelemetryClient">
        /// Action to configure the <see cref="IRawTelemetryClient"/> service.
        /// </param>
        /// <returns>Logging builder with Application Insights added to it.</returns>
        public static ILoggingBuilder AddTelemetryClient(
            this ILoggingBuilder builder,
            Func<IServiceProvider, IRawTelemetryClient> configureTelemetryClient)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            if (configureTelemetryClient is null)
                throw new ArgumentNullException(nameof(configureTelemetryClient));

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, TelemetryClientLoggerProvider>());
            builder.Services.Configure<TelemetryClientLoggerOptions>((options) => { });
            builder.Services.AddSingleton(configureTelemetryClient);

            return builder;
        }

        /// <summary>
        /// Adds an ApplicationInsights logger named 'ApplicationInsights' to the factory.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
        /// <param name="configureTelemetryClient">
        /// Action to configure the <see cref="IRawTelemetryClient"/> service.
        /// </param>
        /// <param name="configurationTelemetryClientOptions">
        /// Action to configure the <c>TelemetryClientLoggerOptions</c>.</param>
        /// <returns>Logging builder with Application Insights added to it.</returns>
        public static ILoggingBuilder AddTelemetryClient(
            this ILoggingBuilder builder,
            Func<IServiceProvider, IRawTelemetryClient> configureTelemetryClient,
            Action<TelemetryClientLoggerOptions> configurationTelemetryClientOptions)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            if (configurationTelemetryClientOptions is null)
                throw new ArgumentNullException(nameof(configurationTelemetryClientOptions));

            if (configureTelemetryClient is null)
                throw new ArgumentNullException(nameof(configureTelemetryClient));

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, TelemetryClientLoggerProvider>());
            builder.Services.Configure(configurationTelemetryClientOptions);
            builder.Services.AddSingleton(configureTelemetryClient);

            return builder;
        }
    }
}