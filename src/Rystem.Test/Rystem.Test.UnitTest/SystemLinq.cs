using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Rystem.Test.UnitTest
{
    public class SystemLinq
    {
        internal enum MakeType
        {
            No,
            Yes,
            Wrong
        }
        internal sealed record MakeIt
        {
            public int Id { get; set; }
            public string? B { get; set; }
            public Guid E { get; set; }
            public bool Sol { get; set; }
            public MakeType Type { get; set; }
            public List<string>? Samules { get; set; }
            public DateTime ExpirationTime { get; set; }
            public TimeSpan TimeSpan { get; set; }
        }

        [Fact]
        public void Test1()
        {
            List<MakeIt> makes = new();
            for (int i = 0; i < 100; i++)
                makes.Add(new MakeIt { Id = i });

            Expression<Func<MakeIt, int>> expression = x => x.Id;
            string value = expression.Serialize();
            LambdaExpression newLambda = value.DeserializeAsDynamic<MakeIt>();
            var got = makes.AsQueryable();
            var cut = got.OrderByDescending(newLambda).ThenByDescending(newLambda).ToList();
            Assert.Equal(99, cut.First().Id);
            Assert.Equal(98, cut.Skip(1).First().Id);
            cut = cut.AsQueryable().OrderBy(newLambda).ThenBy(newLambda).ToList();
            Assert.Equal(0, cut.First().Id);
            Assert.Equal(1, cut.Skip(1).First().Id);
            var queryable = cut.AsQueryable();
            var average1 = (decimal)Convert.ChangeType(queryable.Average(x => x.Id), typeof(decimal));
            var average = queryable.Average(newLambda);
            Assert.Equal(average1, average);
            Expression<Func<MakeIt, bool>> expression2 = x => x.Id >= 10;
            string value2 = expression2.Serialize();
            LambdaExpression newLambda2 = value2.DeserializeAsDynamic<MakeIt>();
            var count = queryable.Count(newLambda2);
            var count2 = queryable.LongCount(newLambda2);
            Assert.Equal(count, count2);
            Assert.Equal(90, count);
            var max = queryable.Max(newLambda);
            Assert.Equal(99, max);
            var min = queryable.Min(newLambda);
            Assert.Equal(0, min);
            var sum1 = queryable.Sum(x => x.Id);
            var sum2 = queryable.Sum(newLambda);
            Assert.Equal(sum1, sum2);
            var where = queryable.Where(newLambda2);
            Assert.Equal(90, where.Count());
            Expression<Func<MakeIt, int>> expression3 = x => x.Id / 10;
            string value3 = expression3.Serialize();
            LambdaExpression newLambda3 = value3.DeserializeAsDynamic<MakeIt>();
            var distincted1 = queryable.DistinctBy(x => x.Id / 10);
            var distincted2 = queryable.DistinctBy(newLambda3);
            Assert.Equal(distincted1.Count(), distincted2.Count());
            Assert.Equal(10, distincted2.Count());
        }
    }
}