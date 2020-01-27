using System.Collections.Specialized;

namespace NerdyMishka.Extensions.Quartz
{
    public class InjectedSchedulerFactorySettings
    {
        public InjectedSchedulerFactorySettings(NameValueCollection settings)
        {
            this.SchedulerFactorySettings = settings;
        }

        public NameValueCollection SchedulerFactorySettings { get; private set; }
    }
}