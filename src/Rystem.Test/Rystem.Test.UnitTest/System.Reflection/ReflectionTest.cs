using System.Reflection;
using Xunit;

namespace Rystem.Test.UnitTest.Reflection
{
    public class ReflectionTest
    {
        public class Zalo : Sulo
        {

        }
        public class Sulo
        {

        }
        public class Folli : Zalo
        {

        }
        [Fact]
        public void IsTheSameTypeOrASonTest()
        {
            Zalo zalo = new();
            Zalo zalo2 = new();
            Folli folli = new();
            Sulo sulo = new();
            object quo = new();
            int x = 2;
            decimal y = 3;
            Assert.True(zalo.IsTheSameTypeOrASon(sulo));
            Assert.True(folli.IsTheSameTypeOrASon(sulo));
            Assert.True(zalo.IsTheSameTypeOrASon(zalo2));
            Assert.True(zalo.IsTheSameTypeOrASon(quo));
            Assert.False(sulo.IsTheSameTypeOrASon(zalo));
            Assert.True(sulo.IsTheSameTypeOrAParent(zalo));
            Assert.False(y.IsTheSameTypeOrAParent(x));
        }
        [Fact]
        public void IsTheSameTypeOrAParentTest()
        {
            Zalo zalo = new();
            Sulo sulo = new();
            int x = 2;
            decimal y = 3;
            Assert.True(sulo.IsTheSameTypeOrAParent(zalo));
            Assert.False(y.IsTheSameTypeOrAParent(x));
        }
    }
}
