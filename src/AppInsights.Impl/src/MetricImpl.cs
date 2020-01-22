using Microsoft.ApplicationInsights;
using NerdyMishka.Extensions.AppInsights.Abstractions;

namespace NerdyMishka.Extensions.AppInsights.Impl
{
    internal class MetricImpl : IMetric
    {
        private Metric metric;

        public MetricImpl(Metric metric)
        {
            this.metric = metric;
        }

        public void TrackValue(double metricValue)
            => this.metric.TrackValue(metricValue);

        public bool TrackValue(double metricValue, string dimension1)
            => this.metric.TrackValue(metricValue, dimension1);

        public bool TrackValue(double metricValue, string dimension1, string dimension2)
            => this.metric.TrackValue(metricValue, dimension1, dimension2);
    }
}