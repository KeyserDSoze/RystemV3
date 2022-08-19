using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Rystem.Test.UnitTest.Reflection
{
    public class SystemReflection
    {
        [Fact]
        public async Task StaticTest()
        {
            var result = await Generics
                .WithStatic<SystemReflection>(nameof(StaticCreateAsync), typeof(int))
                .InvokeAsync(3);
            Assert.Equal(3, result);
            var result2 = Generics
                .WithStatic<SystemReflection>(nameof(StaticCreate), typeof(int))
                .Invoke(3);
            Assert.Equal(3, result2);
            var result3 = Generics
                .WithStatic<SystemReflection>(nameof(StaticCreate), typeof(int))
                .Invoke<int>(3);
            Assert.Equal(3, result3);
        }
        [Fact]
        public async Task InstanceTest()
        {
            var result = await Generics
                .With<SystemReflection>(nameof(CreateAsync), typeof(int))
                .InvokeAsync(this, 3);
            Assert.Equal(3, result);
            var result2 = Generics
                .With<SystemReflection>(nameof(Create), typeof(int))
                .Invoke(this, 3);
            Assert.Equal(3, result2);
            var result3 = Generics
                .With<SystemReflection>(nameof(Create), typeof(int))
                .Invoke<int>(this, 3);
            Assert.Equal(3, result3);
        }
        public static async Task<T> StaticCreateAsync<T>(T x)
        {
            await Task.Delay(0);
            return x;
        }
        public static T StaticCreate<T>(T x)
        {
            return x;
        }
        public async Task<T> CreateAsync<T>(T x)
        {
            await Task.Delay(0);
            return x;
        }
        public T Create<T>(T x)
        {
            return x;
        }
    }
}