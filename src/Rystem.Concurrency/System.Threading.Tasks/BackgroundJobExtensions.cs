using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Timers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class BackgroundJobExtensions
    {
        public static IServiceCollection AddBackgroundJobManager<TJobManager>(this IServiceCollection services)
            where TJobManager : class, IBackgroundJobManager
            => services.AddSingleton<IBackgroundJobManager, TJobManager>();
        public static IServiceCollection AddBackgroundJob<TJob>(this IServiceCollection services,
            Action<BackgroundJobOptions> options)
            where TJob : class, IBackgroundJob
        {
            services.TryAddSingleton<IBackgroundJobManager, BackgroundJobManager>();
            services.AddTransient<TJob>();
            var bOptions = new BackgroundJobOptions()
            {
                Key = Guid.NewGuid().ToString(),
                Cron = "0 1 * * *",
                RunImmediately = false
            };
            options.Invoke(bOptions);
            services.AddWarmUp(serviceProvider => Start<TJob>(serviceProvider, bOptions));
            return services;
        }
        private static void Start<TJob>(IServiceProvider serviceProvider,
            BackgroundJobOptions options)
            where TJob : class, IBackgroundJob
        {
            var backgroundJob = serviceProvider.CreateScope().ServiceProvider.GetService<IBackgroundJobManager>();
            if (backgroundJob != null)
            {
                string key = $"BackgroundWork_{options.Key}_{typeof(TJob).FullName}";
                backgroundJob.AddTaskAsync(
                    async () =>
                    {
                        try
                        {
                            if (entity != default)
                                await entity.ActionToDoAsync().NoContext();
                        }
                        catch (Exception exception)
                        {
                            if (entity != default)
                                await entity.OnException(exception).NoContext();
                        }
                    },
                    typeof(TJob).FullName,
                    () => expression.GetNextOccurrence(DateTime.UtcNow, true)?.Subtract(DateTime.UtcNow).TotalMilliseconds ?? 120,
                    options.RunImmediately
                    );
            }
            else
                throw new ArgumentException("Background job manager not found.");
        }
    }
}