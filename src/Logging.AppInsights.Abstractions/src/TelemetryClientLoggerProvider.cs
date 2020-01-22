using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NerdyMishka.Extensions.AppInsights.Abstractions;

namespace NerdyMishka.Extensions.Logging.AppInsights.Abstractions
{
    /// <summary>
    /// Implementation of <see cref="ILoggerProvider" /> for <see cref="IRawTelemetryClient" />
    /// which wraps the application insights telemetry client.
    /// </summary>
    /// <seealso cref="ILoggerProvider" />
    /// <seealso cref="ISupportExternalScope" />
    [ProviderAlias("NerdyMishka.TelemetryClient")]
    public class TelemetryClientLoggerProvider : ILoggerProvider,
        ISupportExternalScope
    {
        private readonly TelemetryClientLoggerOptions options;

        private readonly IRawTelemetryClient client;

        private IExternalScopeProvider externalScopeProvider;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TelemetryClientLoggerProvider" /> class.
        /// </summary>
        /// <param name="rawTelemetryClient">The <see cref="IRawTelemetryClient" />.</param>
        /// <param name="TelemetryClientLoggerOptions">The <see cref="IOptions{TelemetryClientLoggerOptions}" />.</param>
        public TelemetryClientLoggerProvider(
            IRawTelemetryClient rawTelemetryClient,
            IOptions<TelemetryClientLoggerOptions> telemetryClientLoggerOptions)
        {
            this.client = rawTelemetryClient;

            if (telemetryClientLoggerOptions?.Value == null)
                throw new ArgumentNullException(nameof(telemetryClientLoggerOptions));

            this.options = telemetryClientLoggerOptions.Value;
            this.client = rawTelemetryClient;
        }

        /// <summary>
        /// Creates a new <see cref="ILogger" /> instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns>An <see cref="ILogger"/> instance to be used for logging.</returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new TelemetryClientLogger(
                    categoryName,
                    this.client,
                    this.options)
            {
                ExternalScopeProvider = this.externalScopeProvider,
            };
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Sets the scope provider. This method also updates all the existing logger to also use the new ScopeProvider.
        /// </summary>
        /// <param name="externalScopeProvider">The external scope provider.</param>
        public void SetScopeProvider(IExternalScopeProvider externalScopeProvider)
        {
            this.externalScopeProvider = externalScopeProvider;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="releasedManagedResources">Release managed resources.</param>
        protected virtual void Dispose(bool releasedManagedResources)
        {
            if (releasedManagedResources)
            {
                this.client?.Flush();

                // With the ServerTelemetryChannel, Flush pushes buffered telemetry to the Transmitter,
                // but it doesn't guarantee that all events have been transmitted to the endpoint.
                // TODO: Should we sleep here? Should that be controlled by options?
            }
        }
    }
}