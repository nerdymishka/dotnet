using System;
using Xunit;
using Xunit.Sdk;

namespace Mettle
{
    /// <summary>
    /// Notates a unit test for the Mettle test runner and annotates the
    /// tag and category traits as 'unit'
    /// </summary>
    [XunitTestCaseDiscoverer("Mettle.Xunit.Sdk.TestCaseDiscoverer", "Mettle.Xunit")]
    public class UnitTestAttribute : TestCaseAttribute
    {
        /// <summary>
        /// Creates a new instance of <see cref="UnitTestAttribute" />
        /// </summary>
        public UnitTestAttribute()
        {
            this.Tags = this.Tags ?? "unit";
        }
    }
}