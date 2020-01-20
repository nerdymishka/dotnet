using System;
using Xunit.Sdk;

namespace Mettle
{
    [TestFrameworkDiscoverer(
        "Mettle.Xunit.Sdk.MettleTestFrameworkTypeDiscoverer",
        "Mettle.Xunit")]
    [System.AttributeUsage(System.AttributeTargets.Assembly,
        Inherited = false,
        AllowMultiple = false)]
    public sealed class MettleXunitFrameworkAttribute : System.Attribute,
        ITestFrameworkAttribute
    {
        public MettleXunitFrameworkAttribute()
        {
        }

        public Type Type { get; set; }

        public string Assembly { get; set; }
    }
}