using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Rystem.Queue.Test.UnitTest
{
    public class Sample
    {
        public string Id { get; set; }
    }
    public class QueueTest
    {
        internal static int Counter;
        private static readonly IServiceProvider _serviceProvider;
        static QueueTest()
        {
            IServiceCollection services = new ServiceCollection()
                .AddMemoryQueue<Sample>(x =>
                {
                    x.MaximumBuffer = 1000;
                    x.Actions.Add(t =>
                    {
                        var q = t;
                        return Task.CompletedTask;
                    });
                    x.MaximumRetentionCronFormat = "*/3 * * * * *";
                });
            _serviceProvider = services.BuildServiceProvider();
            _serviceProvider.WarmUpAsync().ToResult();
        }
        [Fact]
        public async Task ExpiryRun()
        {
            var queue = _serviceProvider.GetService<IQueue<Sample>>()!;
            for (int i = 0; i < 100; i++)
                await queue.AddAsync(new Sample() { Id = i.ToString() });
            Assert.Equal(100, await queue.CountAsync());
            await Task.Delay(7_000);
            Assert.Equal(0, await queue.CountAsync());
        }
        [Fact]
        public async Task MoreThan1000Run()
        {
            var queue = _serviceProvider.GetService<IQueue<Sample>>()!;
            for (int i = 0; i < 1001; i++)
                await queue.AddAsync(new Sample() { Id = i.ToString() });
            await Task.Delay(4_000);
            Assert.Equal(0, await queue.CountAsync());
        }
    }
}