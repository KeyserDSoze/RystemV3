using System;
using System.Threading.Tasks;
using Xunit;

namespace Rystem.Test.UnitTest
{
    public class TryCatchTest
    {
        [Fact]
        public void Test1()
        {
            var t = Try.WithDefaultOnCatch(() =>
             {
                 throw new Exception();
                 return 1;
             });
            Assert.Equal(0, t);
        }
        [Fact]
        public void Test2()
        {
            var t = Try.WithDefaultOnCatch(() =>
            {
                return 1;
            });
            Assert.Equal(1, t);
        }
        [Fact]
        public async Task Test3()
        {
            var t = await Try.WithDefaultOnCatchAsync(() =>
            {
                throw new Exception();
                return Task.FromResult(1);
            });
            Assert.Equal(0, t);
        }
        [Fact]
        public async Task Test4()
        {
            var t = await Try.WithDefaultOnCatchAsync(() =>
             {
                 return Task.FromResult(1);
             });
            Assert.Equal(1, t);
        }
    }
}