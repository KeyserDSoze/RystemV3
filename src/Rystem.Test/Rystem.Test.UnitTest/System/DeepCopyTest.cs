using Rystem.System;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Rystem.Test.UnitTest
{
    public class DeepCopyTest
    {
        private class A
        {
            public int Id { get; set; }
        }
        private sealed class B : A { }
        [Fact]
        public async Task Test1()
        {
            B b = new B()
            {
                Id = 3
            };
            B a = b.ToDeepCopy();
            Assert.False(a == b);
            Assert.True(a.Id == b.Id);
        }
    }
}