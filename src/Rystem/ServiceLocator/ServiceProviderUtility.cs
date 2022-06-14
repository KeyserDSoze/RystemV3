using Microsoft.Extensions.DependencyInjection;

namespace Rystem
{
    internal static class ServiceProviderUtility
    {
        static internal Func<IServiceProvider, Task>? AfterBuildEvent;
        static internal async Task AfterBuildAsync(IServiceProvider providers)
        {
            var scope = providers.CreateAsyncScope();
            List<Task> tasks = new();
            if (AfterBuildEvent != null)
                tasks.Add(AfterBuildEvent.Invoke(scope.ServiceProvider));
            await Task.WhenAll(tasks);
            await scope.DisposeAsync();
        }
    }
}