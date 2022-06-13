using Microsoft.AspNetCore.Builder;
using Rystem;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtesions
    {
        public static IServiceCollection AddBuildCallback(this IServiceCollection services, Func<IServiceProvider, Task> actionAfterBuild)
        {
            ServiceLocator.AfterBuildEvent += actionAfterBuild;
            return services;
        }
        public static async Task<TApplicationBuilder> RunCallbacksAfterBuild<TApplicationBuilder>(this TApplicationBuilder builder)
            where TApplicationBuilder : IApplicationBuilder
        {
            await ServiceLocator.AfterBuildAsync(builder.ApplicationServices);
            return builder;
        }
    }
}
