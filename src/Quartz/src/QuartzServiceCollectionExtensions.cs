using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;

namespace NerdyMishka.Extensions.Quartz
{
    public static class QuartzServiceCollectionExtensions
    {
        public static IServiceCollection AddHostedQuartzService(
            this IServiceCollection services,
            Action<QuartzServiceBuilderOptions> configureQuartzOptions = null)
        {
            if (configureQuartzOptions == null)
                configureQuartzOptions = (o) => { };

            var options = new QuartzServiceBuilderOptions();
            configureQuartzOptions(options);

            if (options.SchedulerFactorySettings != null &&
                options.SchedulerFactorySettings.Count > 0)
            {
                services.AddTransient((sp) =>
                {
                    return new InjectedSchedulerFactorySettings(
                        options.SchedulerFactorySettings);
                });
            }

            foreach (var svc in options.InitialJobs)
            {
                services.AddTransient((sp) => svc);
                if (svc.IsSingleton)
                    services.AddSingleton(svc.JobType);
                else
                    services.AddTransient(svc.JobType);
            }

            services.AddSingleton(typeof(ISchedulerFactory),
                options.SchedulerFactoryType);

            services.AddSingleton(typeof(IJobFactory),
                options.JobFactoryType);

            services.AddSingleton(typeof(IInjectedSchedulerFactory), typeof(InjectedSchedulerFactory));
            services.AddHostedService<QuartzService>();
            services.AddSingleton((sp) =>
            {
                var svc = sp.GetServices<IHostedService>()
                    .SingleOrDefault(o => o is QuartzService);

                if (svc is QuartzService quartzService)
                    return quartzService.Scheduler;

                return null;
            });

            return services;
        }
    }
}