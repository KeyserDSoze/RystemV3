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
        [Theory]
        [InlineData("ƒ => (((ƒ.X == \"dasda\") AndAlso ƒ.X.Contains(\"dasda\")) AndAlso ((ƒ.E == Guid.Parse(\"bf46510b-b7e6-4ba2-88da-cef208aa81f2\")) Or (ƒ.Id == 32)))", 5)]
        [InlineData("ƒ => ((((ƒ.X == \"dasda\") AndAlso ƒ.Sol) AndAlso ƒ.X.Contains(\"dasda\")) AndAlso ((ƒ.E == Guid.Parse(\"bf46510b-b7e6-4ba2-88da-cef208aa81f2\")) Or (ƒ.Id == 32)))", 5)]
        [InlineData("ƒ => (((((ƒ.X == \"dasda\") AndAlso ƒ.Sol) AndAlso ƒ.X.Contains(\"dasda\")) AndAlso ((ƒ.E == Guid.Parse(\"bf46510b-b7e6-4ba2-88da-cef208aa81f2\")) Or (ƒ.Id == 32))) AndAlso ((ƒ.Type == 1) OrElse (ƒ.Type == 2)))", 5)]
        [InlineData("ƒ => (ƒ.Type == 2)", 5)]
        [InlineData("ƒ => (((((ƒ.X == \"dasda\") AndAlso ƒ.Sol) AndAlso (ƒ.X.Contains(\"dasda\") OrElse ƒ.Sol.Equals(True))) AndAlso ((ƒ.E == Guid.Parse(\"bf46510b-b7e6-4ba2-88da-cef208aa81f2\")) Or (ƒ.Id == 32))) AndAlso ((ƒ.Type == 1) OrElse (ƒ.Type == 2)))", 5)]
        public void Test(string expressionAsString, int count)
        {
            var newExpression = expressionAsString.Deserialize<MakeIt, bool>();
            MakeIt makeIt = new()
            {
                B = "",
                Id = 32,
                E = Guid.Parse("bf46510b-b7e6-4ba2-88da-cef208aa81f2"),
                X = "dasda",
                Sol = true,
                Type = MakeType.Wrong
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
            Assert.Equal(count, x.Count);
        }
    }
}