using Microsoft.AspNetCore.Builder;
using Rystem;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtesions
    {
        public static IServiceCollection AddWarmUp(this IServiceCollection services, Func<IServiceProvider, Task> actionAfterBuild)
        {
            ServiceProviderUtility.AfterBuildEvent += actionAfterBuild;
            return services;
        }
        public static async Task<TApplicationBuilder> WarmUp<TApplicationBuilder>(this TApplicationBuilder builder)
            where TApplicationBuilder : IApplicationBuilder
        {
            await ServiceProviderUtility.AfterBuildAsync(builder.ApplicationServices);
            return builder;
        }
    }
}
