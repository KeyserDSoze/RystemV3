using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Xunit;

namespace Rystem.Test.UnitTest.Security.Cryptography
{
    public class HashTest
    {
        public class Foo
        {
            public IEnumerable<string> Values { get; init; }
            public bool X { get; init; }
        }

        [Fact]
        public void Run()
        {
            var foo = new Foo()
            {
                Values = new List<string>() { "aa", "bb", "cc" },
                X = true
            };
            Assert.Equal(foo.ToHash(), foo.ToHash());
            var message = Guid.NewGuid();
            Assert.Equal(message.ToHash(), message.ToHash());
        }
    }
}
