using System;
using System.Collections.Generic;
using System.Linq;
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
            public List<string> Samules { get; set; }
        }
        private const int V = 32;
        [Fact]
        public void Test1()
        {
            string result = "ƒ => (((ƒ.X == \"dasda\") AndAlso ƒ.X.Contains(\"dasda\")) AndAlso ((ƒ.E == Guid.Parse(\"bf46510b-b7e6-4ba2-88da-cef208aa81f2\")) Or (ƒ.Id == 32)))";
            var q = "dasda";
            var id = Guid.Parse("bf46510b-b7e6-4ba2-88da-cef208aa81f2");
            Expression<Func<MakeIt, bool>> expression = ƒ => ƒ.X == q && ƒ.X.Contains(q) && (ƒ.E == id | ƒ.Id == V);
            var serialized = expression.Serialize();
            Assert.Equal(result, serialized);
        }
        [Fact]
        public void Test2()
        {
            string result = "ƒ => ((((ƒ.X == \"dasda\") AndAlso ƒ.Sol) AndAlso ƒ.X.Contains(\"dasda\")) AndAlso ((ƒ.E == Guid.Parse(\"bf46510b-b7e6-4ba2-88da-cef208aa81f2\")) Or (ƒ.Id == 32)))";
            var q = "dasda";
            var id = Guid.Parse("bf46510b-b7e6-4ba2-88da-cef208aa81f2");
            Expression<Func<MakeIt, bool>> expression = ƒ => ƒ.X == q && ƒ.Sol && ƒ.X.Contains(q) && (ƒ.E == id | ƒ.Id == V);
            var serialized = expression.Serialize();
            Assert.Equal(result, serialized);
        }
        [Fact]
        public void Test3()
        {
            string result = "ƒ => (((((ƒ.X == \"dasda\") AndAlso ƒ.Sol) AndAlso ƒ.X.Contains(\"dasda\")) AndAlso ((ƒ.E == Guid.Parse(\"bf46510b-b7e6-4ba2-88da-cef208aa81f2\")) Or (ƒ.Id == 32))) AndAlso ((ƒ.Type == 1) OrElse (ƒ.Type == 2)))";
            var q = "dasda";
            var id = Guid.Parse("bf46510b-b7e6-4ba2-88da-cef208aa81f2");
            var qq = MakeType.Wrong;
            Expression<Func<MakeIt, bool>> expression = ƒ => ƒ.X == q && ƒ.Sol && ƒ.X.Contains(q) && (ƒ.E == id | ƒ.Id == V) && (ƒ.Type == MakeType.Yes || ƒ.Type == qq);
            var serialized = expression.Serialize();
            Assert.Equal(result, serialized);
        }
        private const bool IsOk = true;
        [Fact]
        public void Test4()
        {
            string result = "ƒ => (((((ƒ.X == \"dasda\") AndAlso ƒ.Sol) AndAlso (ƒ.X.Contains(\"dasda\") OrElse ƒ.Sol.Equals(True))) AndAlso ((ƒ.E == Guid.Parse(\"bf46510b-b7e6-4ba2-88da-cef208aa81f2\")) Or (ƒ.Id == 32))) AndAlso ((ƒ.Type == 1) OrElse (ƒ.Type == 2)))";
            var q = "dasda";
            var id = Guid.Parse("bf46510b-b7e6-4ba2-88da-cef208aa81f2");
            var qq = MakeType.Wrong;
            Expression<Func<MakeIt, bool>> expression = ƒ => ƒ.X == q && ƒ.Sol && (ƒ.X.Contains(q) || ƒ.Sol.Equals(IsOk)) && (ƒ.E == id | ƒ.Id == V) && (ƒ.Type == MakeType.Yes || ƒ.Type == qq);
            var serialized = expression.Serialize();
            Assert.Equal(result, serialized);
        }
        [Fact]
        public void Test5()
        {
            string result = "ƒ => ((((((ƒ.X == \"dasda\") AndAlso ƒ.Samules.Any(x => (x == \"ccccde\"))) AndAlso ƒ.Sol) AndAlso (ƒ.X.Contains(\"dasda\") OrElse ƒ.Sol.Equals(True))) AndAlso ((ƒ.E == Guid.Parse(\"bf46510b-b7e6-4ba2-88da-cef208aa81f2\")) Or (ƒ.Id == 32))) AndAlso ((ƒ.Type == 1) OrElse (ƒ.Type == 2)))";
            var q = "dasda";
            var k = "ccccde";
            var id = Guid.Parse("bf46510b-b7e6-4ba2-88da-cef208aa81f2");
            var qq = MakeType.Wrong;
            Expression<Func<MakeIt, bool>> expression = ƒ => ƒ.X == q && ƒ.Samules.Any(x => x == k) && ƒ.Sol && (ƒ.X.Contains(q) || ƒ.Sol.Equals(IsOk)) && (ƒ.E == id | ƒ.Id == V) && (ƒ.Type == MakeType.Yes || ƒ.Type == qq);
            var serialized = expression.Serialize();
            Assert.Equal(result, serialized);
        }
    }
}