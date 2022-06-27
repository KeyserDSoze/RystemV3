using Cronos;

namespace System.Timers
{
    public static class BackgroundJobExtensions
    {
        private static string GetKey<T>(this T job, string? key = null)
            where T : class, IBackgroundJob
           => $"BackgroundWork_{key}_{job.GetType().FullName}";
        public static void Run<T>(this T job,
            BackgroundJobOptions options,
            Func<IBackgroundJob>? factory = null)
            where T : class, IBackgroundJob
        {
            string key = job.GetKey(options.Key);
            if (!BackgroundJobManager.Instance.IsRunning(key))
            {
                var entity = factory?.Invoke() ?? job;
                var expression = CronExpression.Parse(options.Cron, options.Cron?.Split(' ').Length > 5 ? CronFormat.IncludeSeconds : CronFormat.Standard);
                BackgroundJobManager.Instance.AddTaskAsync(async () =>
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
                key,
                () => expression.GetNextOccurrence(DateTime.UtcNow, true)?.Subtract(DateTime.UtcNow).TotalMilliseconds ?? 120,
                options.RunImmediately
            );
            }
        }

        public static async Task StopAsync<T>(this T job, string? key = null)
            where T : class, IBackgroundJob
           => await BackgroundJobManager.Instance.RemoveTaskAsync(job.GetKey(key));
    }
}