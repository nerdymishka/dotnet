using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace NerdyMishka.Extensions.Quartz
{
    public class InjectedSchedulerFactory : ISchedulerFactory
    {
        private ISchedulerFactory schedulerFactory;
        private IJobFactory jobFactory;

        public InjectedSchedulerFactory(
            ISchedulerFactory schedulerFactory = null,
            IJobFactory jobFactory = null,
            InjectedSchedulerFactorySettings settings = null)
        {
            this.jobFactory = jobFactory;
            this.schedulerFactory = schedulerFactory;
            if (this.schedulerFactory == null)
            {
                if (settings != null && settings.SchedulerFactorySettings != null)
                    this.schedulerFactory = new StdSchedulerFactory(settings.SchedulerFactorySettings);
                else
                    this.schedulerFactory = new StdSchedulerFactory();
            }
        }

        public async Task<IReadOnlyList<IScheduler>> GetAllSchedulers(CancellationToken cancellationToken = default)
        {
            var schedulers = await this.schedulerFactory.GetAllSchedulers(cancellationToken)
                .ConfigureAwait(false);

            if (this.jobFactory != null)
            {
                foreach (var scheduler in schedulers)
                {
                    scheduler.JobFactory = this.jobFactory;
                }
            }

            return schedulers;
        }

        public async Task<IScheduler> GetScheduler(CancellationToken cancellationToken = default)
        {
            var scheduler = await this.schedulerFactory.GetScheduler(cancellationToken)
                 .ConfigureAwait(false);

            if (this.jobFactory != null)
                scheduler.JobFactory = this.jobFactory;

            return scheduler;
        }

        public async Task<IScheduler> GetScheduler(string schedulerName, CancellationToken cancellationToken = default)
        {
            var scheduler = await this.schedulerFactory.GetScheduler(schedulerName, cancellationToken)
                .ConfigureAwait(false);

            if (this.jobFactory != null)
                scheduler.JobFactory = this.jobFactory;

            return scheduler;
        }
    }
}