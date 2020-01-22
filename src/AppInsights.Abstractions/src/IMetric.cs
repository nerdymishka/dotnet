namespace NerdyMishka.Extensions.AppInsights.Abstractions
{
    /// <summary>
    /// Interface that represents the Metric object in Application Insights.
    /// </summary>
    public interface IMetric
    {
        /// <summary>
        /// Tracks the given value.
        /// An aggregate representing tracked values will be automatically
        /// sent to the cloud ingestion endpoint at the end of each
        /// aggregation period.
        /// </summary>
        /// <param name="metricValue">The value to aggregate.</param>
        void TrackValue(double metricValue);

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
        bool TrackValue(double metricValue, string dimension1);

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
        bool TrackValue(double metricValue, string dimension1, string dimension2);

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
        bool TrackValue(
            double metricValue,
            string dimension1,
            string dimension2,
            string dimension3);

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
        bool TrackValue(
            double metricValue,
            string dimension1,
            string dimension2,
            string dimension3,
            string dimension4);

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
        bool TrackValue(
            double metricValue,
            string dimension1,
            string dimension2,
            string dimension3,
            string dimension4,
            string dimension5);

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
        bool TrackValue(
            double metricValue,
            string dimension1,
            string dimension2,
            string dimension3,
            string dimension4,
            string dimension5,
            string dimension6);

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
        bool TrackValue(
            double metricValue,
            string dimension1,
            string dimension2,
            string dimension3,
            string dimension4,
            string dimension5,
            string dimension6,
            string dimension7);
    }
}