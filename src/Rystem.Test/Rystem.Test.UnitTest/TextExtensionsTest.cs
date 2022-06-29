using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Rystem.Test.UnitTest
{
    public class TextExtensionsTest
    {
        [Fact]
        public void Test1()
        {
            string olfa = "daskemnlandxioasndslam dasmdpoasmdnasndaslkdmlasmv asmdsa";
            var bytes = olfa.ToByteArray();
            string value = bytes.ConvertToString();
            Assert.Equal(olfa, value);
        }
        [Fact]
        public void Test2()
        {
            string olfa = "daskemnlandxioasndslam dasmdpoasmdnasndaslkdmlasmv asmdsa";
            var stream = olfa.ToStream();
            string value = stream.ConvertToString();
            Assert.Equal(olfa, value);
        }
        [Fact]
        public async Task Test3()
        {
            string olfa = "daskemnlandxioasndslam\ndasmdpoasmdnasndaslkdmlasmv\nasmdsa";
            var stream = olfa.ToStream();
            var strings = new List<string>();
            await foreach (var x in stream.ReadLinesAsync())
            {
                strings.Add(x);
            }
            Assert.Equal(3, strings.Count);
        }
        [Fact]
        public async Task Test4()
        {
            string olfa = "dasda";
            var olfa2 = olfa.ToUpperCaseFirst();
            Assert.Equal("Dasda", olfa2);
        }
    }
}