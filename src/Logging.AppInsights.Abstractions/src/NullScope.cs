using System;
#pragma warning disable CA1063

namespace NerdyMishka.Extensions.Logging.AppInsights.Abstractions
{
    /// <summary>
    /// An empty scope without any logic.
    /// </summary>
    internal class NullScope : IDisposable
    {
        private NullScope()
        {
        }

        public static NullScope Instance { get; } = new NullScope();

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
#pragma warning restore CA1063