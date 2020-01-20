using Xunit;

namespace Mettle
{
    [System.AttributeUsage(System.AttributeTargets.Method,
        Inherited = false,
        AllowMultiple = false)]
    public abstract class TestCaseAttribute : FactAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestCaseAttribute"/> class.
        /// Base constuctor
        /// </summary>
        public TestCaseAttribute()
        {
        }

        /// <summary>
        /// Gets or sets the SkipReason for this test.
        /// </summary>
        /// <value>The skip reason.</value>
        public string SkipReason { get; set; }

        /// <summary>
        /// Gets or sets a link to the ticket this test was created for.
        /// </summary>
        /// <value>The ticket uri.</value>
        public string Ticket { get; set; }

        /// <summary>
        /// Gets or sets the ticket id, which can be used as a filter
        /// </summary>
        /// <value>The id of the ticket or unique identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the tags/categories
        /// </summary>
        /// <value>The set of tags delimited by semicolon.</value>
        public string Tags { get; set; }
    }
}