using System.Reflection;
using Xunit;

namespace Rystem.Test.UnitTest
{
    public class MockTest
    {
        public abstract class Alzio
        {
            private protected string X { get; }
            public string O => X;
            public string A { get; set; }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3442:\"abstract\" classes should not have \"public\" constructors", Justification = "<Pending>")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            public Alzio(string x) => X = x;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        }
        [Fact]
        public void Test1()
        {
            var mocked = typeof(Alzio).CreateInstance("AAA") as Alzio;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            mocked.A = "rrrr";
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Assert.Equal("AAA", mocked.O);
            Assert.Equal("rrrr", mocked.A);
        }
        [Fact]
        public void Test2()
        {
            Alzio alzio = null!;
            var mocked = alzio.CreateInstance("AAA");
            mocked.A = "rrrr";
            Assert.Equal("AAA", mocked.O);
            Assert.Equal("rrrr", mocked.A);
        }
    }
}