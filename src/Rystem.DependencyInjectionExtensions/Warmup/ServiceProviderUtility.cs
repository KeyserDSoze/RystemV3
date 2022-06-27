using Microsoft.Extensions.DependencyInjection;

namespace Rystem
{
    internal sealed class ServiceProviderUtility
    {
        public static ServiceProviderUtility Instance { get; set;} = new();
        private ServiceProviderUtility() { }
        public Func<IServiceProvider, Task>? AfterBuildEvent { get; set; }
        public async Task AfterBuildAsync(IServiceProvider providers)
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