using System.Timers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class BackgroundJobExtensions
    {
        public static IServiceCollection AddBackgroundJob<TJob>(this IServiceCollection services,
            Action<BackgroundJobOptions> options)
            where TJob : class, IBackgroundJob
        {
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
            var job = serviceProvider.CreateScope().ServiceProvider.GetService<TJob>();
            if (job != null)
                job.Run(options,
                    () => serviceProvider?.CreateScope()?.ServiceProvider?.GetService<TJob>()
                    ?? throw new ArgumentException($"Background job {typeof(TJob)} not found."));
            else
                throw new ArgumentException($"Background job {typeof(TJob)} not found.");
        }
    }
}