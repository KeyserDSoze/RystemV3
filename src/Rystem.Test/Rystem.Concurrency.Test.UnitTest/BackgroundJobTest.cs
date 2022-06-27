using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Rystem.Concurrency.Test.UnitTest
{
    public class BackgroundJobTest
    {
        internal static int Counter;
        private static readonly IServiceProvider _serviceProvider;
        static BackgroundJobTest()
        {
            IServiceCollection services = new ServiceCollection()
                .AddBackgroundJob<BackgroundJob>(
                x =>
                {
                    x.Cron = "*/1 * * * * *";
                    x.RunImmediately = true;
                    x.Key = "alzo";
                }).AddSingleton<SingletonService>()
                .AddScoped<ScopedService>()
                .AddTransient<TransientService>();
            _serviceProvider = services.BuildServiceProvider();
            _serviceProvider.WarmUpAsync().ToResult();
        }
        [Fact]
        public async Task SingleRun()
        {
            await Task.Delay(2000).NoContext();
            Assert.True(Counter > 1);
        }
    }
}