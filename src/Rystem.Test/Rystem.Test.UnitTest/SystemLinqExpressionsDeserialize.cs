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
        [InlineData("� => (((�.X == \"dasda\") AndAlso �.X.Contains(\"dasda\")) AndAlso ((�.E == Guid.Parse(\"bf46510b-b7e6-4ba2-88da-cef208aa81f2\")) Or (�.Id == 32)))", 5)]
        [InlineData("� => ((((�.X == \"dasda\") AndAlso �.Sol) AndAlso �.X.Contains(\"dasda\")) AndAlso ((�.E == Guid.Parse(\"bf46510b-b7e6-4ba2-88da-cef208aa81f2\")) Or (�.Id == 32)))", 5)]
        [InlineData("� => (((((�.X == \"dasda\") AndAlso �.Sol) AndAlso �.X.Contains(\"dasda\")) AndAlso ((�.E == Guid.Parse(\"bf46510b-b7e6-4ba2-88da-cef208aa81f2\")) Or (�.Id == 32))) AndAlso ((�.Type == 1) OrElse (�.Type == 2)))", 5)]
        [InlineData("� => (�.Type == 2)", 5)]
        [InlineData("� => (((((�.X == \"dasda\") AndAlso �.Sol) AndAlso (�.X.Contains(\"dasda\") OrElse �.Sol.Equals(True))) AndAlso ((�.E == Guid.Parse(\"bf46510b-b7e6-4ba2-88da-cef208aa81f2\")) Or (�.Id == 32))) AndAlso ((�.Type == 1) OrElse (�.Type == 2)))", 5)]
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