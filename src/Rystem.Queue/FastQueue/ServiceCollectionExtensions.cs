using Rystem.Queue;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddQueue<T>(this IServiceCollection services,
            Action<QueueProperty<T>>? options = null)
        {
            var settings = new QueueProperty<T>();
            options?.Invoke(settings);
            services.AddSingleton(settings);
            if (settings.Type == QueueType.FirstInFirstOut)
                services.AddSingleton<IQueue<T>, MemoryQueue<T>>();
            else if (settings.Type == QueueType.LastInFirstOut)
                services.AddSingleton<IQueue<T>, MemoryStack<T>>();
            services.AddBackgroundJob<QueueJobManager<T>>(x =>
            {
                x.Cron = settings.MaximumRetentionCronFormat;
                x.RunImmediately = true;
            });
            return services;
        }
    }
}