using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace NerdyMishka.Extensions.Quartz
{
    public class QuartzServiceBuilderOptions
    {
        public IList<JobSetup> InitialJobs { get; }
            = new List<JobSetup>();

        public Type JobFactoryType { get; private set; } = typeof(JobFactory);

        public NameValueCollection SchedulerFactorySettings { get; private set; } = new NameValueCollection();

        public Type SchedulerFactoryType { get; private set; } = typeof(StdSchedulerFactory);

        public QuartzServiceBuilderOptions AddJob(JobSetup jobSetup)
        {
            this.InitialJobs.Add(jobSetup);
            return this;
        }

        public QuartzServiceBuilderOptions SetJobFactory(Type type)
        {
            this.JobFactoryType = type;
            return this;
        }

        public QuartzServiceBuilderOptions SetSchedulerFactory(Type type)
        {
            this.SchedulerFactoryType = type;
            return this;
        }

        public QuartzServiceBuilderOptions SetSettings(NameValueCollection settings)
        {
            this.SchedulerFactorySettings = settings;
            return this;
        }
    }
}