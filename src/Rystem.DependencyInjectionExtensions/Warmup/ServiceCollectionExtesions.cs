using Microsoft.AspNetCore.Builder;
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
        public static async Task<TApplicationBuilder> WarmUp<TApplicationBuilder>(this TApplicationBuilder builder)
            where TApplicationBuilder : IApplicationBuilder
        {
            await ServiceProviderUtility.Instance.AfterBuildAsync(builder.ApplicationServices);
            return builder;
        }
    }
}
