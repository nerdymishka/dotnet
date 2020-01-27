using System;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace NerdyMishka.Extensions.Quartz
{
    public class JobFactory : IJobFactory
    {
        private readonly IServiceProvider serviceProvider;

        public JobFactory(IServiceProvider serviceProvider)
        {
            Check.NotNull(nameof(serviceProvider), serviceProvider);

            this.serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            Check.NotNull(nameof(bundle), bundle);

            var obj = this.serviceProvider
                .GetRequiredService(bundle.JobDetail.JobType);

            if (obj is IScopedJob scopedJob)
            {
                scopedJob.ServiceScope = this.serviceProvider.CreateScope();
                return scopedJob;
            }

            if (obj is IJob job)
                return job;

            throw new NotSupportedException(
                $"obj does not support Quartz.IJob: {obj.GetType().FullName}");
        }

        public virtual void ReturnJob(IJob job)
        {
            if (job is IScopedJob scopedJob)
            {
                scopedJob.ServiceScope.Dispose();
                return;
            }

            if (job is IDisposable disposable)
                disposable.Dispose();
        }
    }
}