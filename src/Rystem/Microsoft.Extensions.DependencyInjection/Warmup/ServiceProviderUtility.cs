using Microsoft.Extensions.DependencyInjection;

namespace Rystem
{
    internal sealed class ServiceProviderUtility
    {
        public static ServiceProviderUtility Instance { get; set; } = new();
        private ServiceProviderUtility() { }
        public Func<IServiceProvider, Task>? AfterBuildEvent { get; set; }
        public async Task AfterBuildAsync(IServiceProvider providers)
        {
            var scope = providers.CreateScope();
            if (AfterBuildEvent != null)
                _ = await Try.WithDefaultOnCatchAsync(() =>
                {
                    return AfterBuildEvent.Invoke(scope.ServiceProvider);
                });
            scope.Dispose();
        }
    }
}