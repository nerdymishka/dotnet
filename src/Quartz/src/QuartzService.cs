using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Spi;

namespace NerdyMishka.Extensions.Quartz
{
    public class QuartzService : IHostedService
    {
        private IEnumerable<JobSetup> initialJobs;

        private ILogger logger;

        public QuartzService(
            IScheduler scheduler,
            IEnumerable<JobSetup> initialJobs = null,
            ILogger<QuartzService> logger = null)
        {
            Check.NotNull(nameof(scheduler), scheduler);

            this.Scheduler = scheduler;
            this.initialJobs = initialJobs ?? Array.Empty<JobSetup>();
            this.logger = logger;
        }

        public IScheduler Scheduler { get; private set; }

        public async Task ScheduleJob(
            JobSetup jobSetup,
            CancellationToken cancellationToken = default)
        {
            Check.NotNull(nameof(jobSetup), jobSetup);

            await this.Scheduler.ScheduleJob(
                jobSetup.JobDetail,
                jobSetup.Trigger,
                cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task ScheduleJob(
            IJobDetail job,
            ITrigger trigger,
            CancellationToken cancellationToken = default)
        {
            await this.Scheduler.ScheduleJob(
                job,
                trigger,
                cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var jobSetup in this.initialJobs)
            {
                await this.ScheduleJob(jobSetup, cancellationToken)
                    .ConfigureAwait(false);
            }

            await this.Scheduler.Start(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            await this.Scheduler.Shutdown(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
