using System;
using System.Linq.Expressions;
using Xunit;

namespace Rystem.Test.UnitTest
{
    public class SystemLinqExpressions
    {
        internal enum MakeType
        {
            No,
            Yes,
            Wrong
        }
        internal sealed class MakeIt
        {
            public string X { get; set; }
            public int Id { get; set; }
            public string B { get; set; }
            public Guid E { get; set; }
            public bool Sol { get; set; }
            public MakeType Type { get; set; }
        }
        private const int V = 32;
        [Fact]
        public void Test1()
        {
            string result = "� => (((�.X == \"dasda\") AndAlso �.X.Contains(\"dasda\")) AndAlso ((�.E == Guid.Parse(\"bf46510b-b7e6-4ba2-88da-cef208aa81f2\")) Or (�.Id == 32)))";
            var q = "dasda";
            var id = Guid.Parse("bf46510b-b7e6-4ba2-88da-cef208aa81f2");
            Expression<Func<MakeIt, bool>> expression = � => �.X == q && �.X.Contains(q) && (�.E == id | �.Id == V);
            var serialized = expression.Serialize();
            Assert.Equal(result, serialized);
        }
        [Fact]
        public void Test2()
        {
            string result = "� => ((((�.X == \"dasda\") AndAlso �.Sol) AndAlso �.X.Contains(\"dasda\")) AndAlso ((�.E == Guid.Parse(\"bf46510b-b7e6-4ba2-88da-cef208aa81f2\")) Or (�.Id == 32)))";
            var q = "dasda";
            var id = Guid.Parse("bf46510b-b7e6-4ba2-88da-cef208aa81f2");
            Expression<Func<MakeIt, bool>> expression = � => �.X == q && �.Sol && �.X.Contains(q) && (�.E == id | �.Id == V);
            var serialized = expression.Serialize();
            Assert.Equal(result, serialized);
        }
        [Fact]
        public void Test3()
        {
            string result = "� => (((((�.X == \"dasda\") AndAlso �.Sol) AndAlso �.X.Contains(\"dasda\")) AndAlso ((�.E == Guid.Parse(\"bf46510b-b7e6-4ba2-88da-cef208aa81f2\")) Or (�.Id == 32))) AndAlso ((�.Type == 1) OrElse (�.Type == 2)))";
            var q = "dasda";
            var id = Guid.Parse("bf46510b-b7e6-4ba2-88da-cef208aa81f2");
            var qq = MakeType.Wrong;
            Expression<Func<MakeIt, bool>> expression = � => �.X == q && �.Sol && �.X.Contains(q) && (�.E == id | �.Id == V) && (�.Type == MakeType.Yes || �.Type == qq);
            var serialized = expression.Serialize();
            Assert.Equal(result, serialized);
        }
        private const bool IsOk = true;
        [Fact]
        public void Test4()
        {
            string result = "� => (((((�.X == \"dasda\") AndAlso �.Sol) AndAlso (�.X.Contains(\"dasda\") OrElse �.Sol.Equals(True))) AndAlso ((�.E == Guid.Parse(\"bf46510b-b7e6-4ba2-88da-cef208aa81f2\")) Or (�.Id == 32))) AndAlso ((�.Type == 1) OrElse (�.Type == 2)))";
            var q = "dasda";
            var id = Guid.Parse("bf46510b-b7e6-4ba2-88da-cef208aa81f2");
            var qq = MakeType.Wrong;
            Expression<Func<MakeIt, bool>> expression = � => �.X == q && �.Sol && (�.X.Contains(q) || �.Sol.Equals(IsOk)) && (�.E == id | �.Id == V) && (�.Type == MakeType.Yes || �.Type == qq);
            var serialized = expression.Serialize();
            Assert.Equal(result, serialized);
        }
    }
}