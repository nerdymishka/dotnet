using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace NerdyMishka.Extensions.Quartz
{
    public interface IScopedJob : IJob
    {
        IServiceScope ServiceScope { get; set; }
    }
}