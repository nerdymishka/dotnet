#if !NET35

using System.Threading.Tasks;

internal static class CommonTasks
{
    internal static readonly Task Completed = Task.FromResult(0);
}

#endif