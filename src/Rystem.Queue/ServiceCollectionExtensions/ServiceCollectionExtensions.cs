using Rystem.Queue;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMemoryQueue<T>(this IServiceCollection services,
            Action<QueueProperty<T>>? options = null)
            => services.AddQueueIntegration<T, MemoryQueue<T>>(options);
        public static IServiceCollection AddMemoryStackQueue<T>(this IServiceCollection services,
            Action<QueueProperty<T>>? options = null)
            => services.AddQueueIntegration<T, MemoryStackQueue<T>>(options);
        public static IServiceCollection AddQueueIntegration<T, TQueue>(this IServiceCollection services,
            Action<QueueProperty<T>>? options = null)
            where TQueue : class, IQueue<T>
        {
            var settings = new QueueProperty<T>();
            options?.Invoke(settings);
            services.AddSingleton(settings);
            services.AddSingleton<IQueue<T>, TQueue>();
            services.AddBackgroundJob<QueueJobManager<T>>(x =>
            {
                x.Cron = settings.MaximumRetentionCronFormat;
                x.RunImmediately = false;
            });
            return services;
        }
    }
}