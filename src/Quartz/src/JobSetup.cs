using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Quartz;

namespace NerdyMishka.Extensions.Quartz
{
    public class JobSetup
    {
        public JobSetup(Type jobType, ITrigger trigger, bool? isSingleton = null)
            : this(jobType, trigger, null, null, null, null, isSingleton)
        {
        }

        public JobSetup(
            Type jobType,
            string cronExpression,
            string identity = null,
            string description = null,
            bool? isSingleton = null)
            : this(jobType,
            null,
            null,
            cronExpression,
            identity,
            description,
            isSingleton)
        {
        }

        public JobSetup(
            Type jobType,
            ITrigger trigger = null,
            JobDataMap jobData = null,
            string cronExpression = null,
            string identity = null,
            string description = null,
            bool? isSingleton = false)
        {
            Check.NotNull(nameof(jobType), jobType);

            if (trigger == null && string.IsNullOrWhiteSpace(cronExpression))
                throw new ArgumentException(
                    "trigger or cronExpression must be specified");

            this.JobType = jobType;
            this.Trigger = trigger;
            this.CronExpression = cronExpression;
            this.Identity = identity ?? jobType.FullName;
            this.Description = description ?? jobType.FullName;
            this.JobData = jobData;
            this.Init(isSingleton);
        }

        public Type JobType { get; private set; }

        public string Description { get; private set; }

        public string Identity { get; private set; }

        public ITrigger Trigger { get; private set; }

        public string CronExpression { get; private set; }

        public JobDataMap JobData { get; private set; }

        public bool IsSingleton { get; private set; } = false;

        protected internal IJobDetail JobDetail { get; set; }

        protected void Init(bool? isSingleton = null)
        {
            var contracts = this.JobType.GetInterfaces();
            if (!contracts.Any(o => o is IJob))
                throw new InvalidCastException(
                    this.JobType.FullName + " does not implement Quartz.IJob");

            if (isSingleton.HasValue && isSingleton.Value == true)
            {
                this.IsSingleton = true;
            }
            else
            {
                var isScoped = contracts.Any(o => o is IScopedJob);
                var isDisposable = contracts.Any(o => o is IDisposable);

                this.IsSingleton = !(isScoped || isDisposable);
            }

            var builder = JobBuilder.Create(this.JobType)
                .WithIdentity(this.Identity)
                .WithDescription(this.Description);

            if (this.JobData != null)
                builder.SetJobData(this.JobData);

            this.JobDetail = builder.Build();

            if (this.Trigger == null)
            {
                this.Trigger = TriggerBuilder.Create()
                    .WithCronSchedule(this.CronExpression)
                    .WithIdentity(this.Identity + ".Trigger")
                    .WithDescription(this.Description)
                    .Build();
            }
        }
    }
}