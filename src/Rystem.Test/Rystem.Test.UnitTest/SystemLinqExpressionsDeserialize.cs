using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using static Rystem.Test.UnitTest.SystemLinqExpressions;

namespace Rystem.Test.UnitTest
{
    public class SystemLinqExpressionsDeserialize
    {
        [Fact]
        public void Test()
        {
            var newExpression = Result.Deserialize<MakeIt, bool>();
            MakeIt makeIt = new()
            {
                B = "",
                Id = 32,
                E = Guid.Parse("bf46510b-b7e6-4ba2-88da-cef208aa81f2"),
                X = "dasda"
            };
            List<MakeIt> makes = new()
            {
                makeIt,
                makeIt,
                makeIt,
                makeIt,
                makeIt,
            };
            var x = makes.Where(newExpression.Compile()).ToList();
            Assert.Equal(5, x.Count);
        }
    }
}