using Microsoft.ApplicationInsights;
using NerdyMishka.Extensions.AppInsights.Abstractions;

namespace NerdyMishka.Extensions.AppInsights.Impl
{
    /// <summary>
    /// Data contract for representing a metric series.
    /// </summary>
    internal class MetricImpl : IMetric
    {
        private Metric metric;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetricImpl" /> class.
        /// </summary>
        /// <param name="metric">The <see cref="Metric"/>.</param>
        public MetricImpl(Metric metric)
        {
            this.metric = metric;
        }

        /// <summary>
        /// Tracks the given value.
        /// An aggregate representing tracked values will be automatically
        /// sent to the cloud ingestion endpoint at the end of each
        /// aggregation period.
        /// </summary>
        /// <param name="metricValue">The value to aggregate.</param>
        public void TrackValue(double metricValue)
            => this.metric.TrackValue(metricValue);

        /// <summary>
        /// Tracks the given value.
        /// An aggregate representing tracked values will be automatically
        /// sent to the cloud ingestion endpoint at the end of each
        /// aggregation period.
        /// </summary>
        /// <param name="metricValue">The value to aggregate.</param>
        /// <param name="dimension1">The value of the 1st dimension.</param>
        /// <returns><c>True</c> if the metric is added to the
        /// <c>MetricSeries</c>; otherwise, <c>False</c>.
        /// </returns>
        public bool TrackValue(double metricValue, string dimension1)
            => this.metric.TrackValue(metricValue, dimension1);

        /// <summary>
        /// Tracks the given value.
        /// An aggregate representing tracked values will be automatically
        /// sent to the cloud ingestion endpoint at the end of each
        /// aggregation period.
        /// </summary>
        /// <param name="metricValue">The value to aggregate.</param>
        /// <param name="dimension1">The value of the 1st dimension.</param>
        /// <param name="dimension2">The value of the 2nd dimension.</param>
        /// <returns><c>True</c> if the metric is added to the
        /// <c>MetricSeries</c>; otherwise, <c>False</c>.
        /// </returns>
        public bool TrackValue(double metricValue, string dimension1, string dimension2)
            => this.metric.TrackValue(metricValue, dimension1, dimension2);

        /// <summary>
        /// Tracks the given value.
        /// An aggregate representing tracked values will be automatically
        /// sent to the cloud ingestion endpoint at the end of each
        /// aggregation period.
        /// </summary>
        /// <param name="metricValue">The value to aggregate.</param>
        /// <param name="dimension1">The value of the 1st dimension.</param>
        /// <param name="dimension2">The value of the 2nd dimension.</param>
        /// <param name="dimension3">The value of the 3rd dimension.</param>
        /// <returns><c>True</c> if the metric is added to the
        /// <c>MetricSeries</c>; otherwise, <c>False</c>.
        /// </returns>
        public bool TrackValue(
            double metricValue,
            string dimension1,
            string dimension2,
            string dimension3)
        {
            return this.metric.TrackValue(
                metricValue,
                dimension1,
                dimension2,
                dimension3);
        }

        /// <summary>
        /// Tracks the given value.
        /// An aggregate representing tracked values will be automatically
        /// sent to the cloud ingestion endpoint at the end of each
        /// aggregation period.
        /// </summary>
        /// <param name="metricValue">The value to aggregate.</param>
        /// <param name="dimension1">The value of the 1st dimension.</param>
        /// <param name="dimension2">The value of the 2nd dimension.</param>
        /// <param name="dimension3">The value of the 3rd dimension.</param>
        /// <param name="dimension4">The value of the 4th dimension.</param>
        /// <returns><c>True</c> if the metric is added to the
        /// <c>MetricSeries</c>; otherwise, <c>False</c>.
        /// </returns>
        public bool TrackValue(
            double metricValue,
            string dimension1,
            string dimension2,
            string dimension3,
            string dimension4)
        {
            return this.metric.TrackValue(
                metricValue,
                dimension1,
                dimension2,
                dimension3,
                dimension4);
        }

        /// <summary>
        /// Tracks the given value.
        /// An aggregate representing tracked values will be automatically
        /// sent to the cloud ingestion endpoint at the end of each
        /// aggregation period.
        /// </summary>
        /// <param name="metricValue">The value to aggregate.</param>
        /// <param name="dimension1">The value of the 1st dimension.</param>
        /// <param name="dimension2">The value of the 2nd dimension.</param>
        /// <param name="dimension3">The value of the 3rd dimension.</param>
        /// <param name="dimension4">The value of the 4th dimension.</param>
        /// <param name="dimension5">The value of the 5th dimension.</param>
        /// <returns><c>True</c> if the metric is added to the
        /// <c>MetricSeries</c>; otherwise, <c>False</c>.
        /// </returns>
        public bool TrackValue(
            double metricValue,
            string dimension1,
            string dimension2,
            string dimension3,
            string dimension4,
            string dimension5)
        {
            return this.metric.TrackValue(
                metricValue,
                dimension1,
                dimension2,
                dimension3,
                dimension4,
                dimension5);
        }

        /// <summary>
        /// Tracks the given value.
        /// An aggregate representing tracked values will be automatically
        /// sent to the cloud ingestion endpoint at the end of each
        /// aggregation period.
        /// </summary>
        /// <param name="metricValue">The value to aggregate.</param>
        /// <param name="dimension1">The value of the 1st dimension.</param>
        /// <param name="dimension2">The value of the 2nd dimension.</param>
        /// <param name="dimension3">The value of the 3rd dimension.</param>
        /// <param name="dimension4">The value of the 4th dimension.</param>
        /// <param name="dimension5">The value of the 5th dimension.</param>
        /// <param name="dimension6">The value of the 6th dimension.</param>
        /// <returns><c>True</c> if the metric is added to the
        /// <c>MetricSeries</c>; otherwise, <c>False</c>.
        /// </returns>
        public bool TrackValue(
            double metricValue,
            string dimension1,
            string dimension2,
            string dimension3,
            string dimension4,
            string dimension5,
            string dimension6)
        {
            return this.metric.TrackValue(
                metricValue,
                dimension1,
                dimension2,
                dimension3,
                dimension4,
                dimension5,
                dimension6);
        }

        /// <summary>
        /// Tracks the given value.
        /// An aggregate representing tracked values will be automatically
        /// sent to the cloud ingestion endpoint at the end of each
        /// aggregation period.
        /// </summary>
        /// <param name="metricValue">The value to aggregate.</param>
        /// <param name="dimension1">The value of the 1st dimension.</param>
        /// <param name="dimension2">The value of the 2nd dimension.</param>
        /// <param name="dimension3">The value of the 3rd dimension.</param>
        /// <param name="dimension4">The value of the 4th dimension.</param>
        /// <param name="dimension5">The value of the 5th dimension.</param>
        /// <param name="dimension6">The value of the 6th dimension.</param>
        /// <param name="dimension7">The value of the 7th dimension.</param>
        /// <returns><c>True</c> if the metric is added to the
        /// <c>MetricSeries</c>; otherwise, <c>False</c>.
        /// </returns>
        public bool TrackValue(
            double metricValue,
            string dimension1,
            string dimension2,
            string dimension3,
            string dimension4,
            string dimension5,
            string dimension6,
            string dimension7)
        {
            return this.metric.TrackValue(
                metricValue,
                dimension1,
                dimension2,
                dimension3,
                dimension4,
                dimension5,
                dimension6,
                dimension7);
        }
    }
}