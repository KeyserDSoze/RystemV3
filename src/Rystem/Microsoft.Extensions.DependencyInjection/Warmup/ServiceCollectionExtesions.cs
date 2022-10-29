using Rystem;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtesions
    {
        public static IServiceCollection AddWarmUp(this IServiceCollection services,
            Func<IServiceProvider, Task> actionAfterBuild)
        {
            ServiceProviderUtility.Instance.AfterBuildEvent += actionAfterBuild;
            return services;
        }
        public static IServiceCollection AddWarmUp(this IServiceCollection services,
            Action<IServiceProvider> actionAfterBuild)
        {
            ServiceProviderUtility.Instance.AfterBuildEvent += (serviceProvider) => 
            {
                actionAfterBuild.Invoke(serviceProvider); 
                return Task.CompletedTask; 
            };
            return services;
        }
        public static async Task<TServiceProvider> WarmUpAsync<TServiceProvider>(this TServiceProvider serviceProvider)
            where TServiceProvider : IServiceProvider
        {
            await ServiceProviderUtility.Instance.AfterBuildAsync(serviceProvider);
            return serviceProvider;
        }
    }
}
