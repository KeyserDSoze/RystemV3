using System;
using System.Linq.Expressions;
using Xunit;

namespace Rystem.Test.UnitTest
{
    public class SystemLinqExpressions
    {
        internal sealed class MakeIt
        {
            public string X { get; set; }
            public int Id { get; set; }
            public string B { get; set; }
            public Guid E { get; set; }
        }
        private const int V = 32;
        internal const string Result = "� => (((�.X == \"dasda\") AndAlso �.X.Contains(\"dasda\")) AndAlso ((�.E == Guid.Parse(\"bf46510b-b7e6-4ba2-88da-cef208aa81f2\")) Or (�.Id == 32)))";
        [Fact]
        public void Test()
        {
            var q = "dasda";
            var id = Guid.Parse("bf46510b-b7e6-4ba2-88da-cef208aa81f2");
            Expression<Func<MakeIt, bool>> expression = � => �.X == q && �.X.Contains(q) && (�.E == id | �.Id == V);
            var serialized = expression.Serialize();
            Assert.Equal(Result, serialized);
        }
    }
}