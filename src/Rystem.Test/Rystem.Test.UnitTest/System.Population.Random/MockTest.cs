using System.Collections.Generic;
using System;
using System.Reflection;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Population.Random;
using System.Linq;
using System.Text.RegularExpressions;

namespace Rystem.Test.UnitTest.Reflection
{
    public class PopulationTest
    {
        public class DelegationPopulation
        {
            public int Id { get; set; }
            public int A { get; set; }
            public int? AA { get; set; }
            public uint B { get; set; }
            public uint? BB { get; set; }
            public byte C { get; set; }
            public byte? CC { get; set; }
            public sbyte D { get; set; }
            public sbyte? DD { get; set; }
            public short E { get; set; }
            public short? EE { get; set; }
            public ushort F { get; set; }
            public ushort? FF { get; set; }
            public long G { get; set; }
            public long? GG { get; set; }
            public nint H { get; set; }
            public nint? HH { get; set; }
            public nuint L { get; set; }
            public nuint? LL { get; set; }
            public float M { get; set; }
            public float? MM { get; set; }
            public double N { get; set; }
            public double? NN { get; set; }
            public decimal O { get; set; }
            public decimal? OO { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            public string P { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            public string? PP { get; set; }
            public bool Q { get; set; }
            public bool? QQ { get; set; }
            public char R { get; set; }
            public char? RR { get; set; }
            public Guid S { get; set; }
            public Guid? SS { get; set; }
            public DateTime T { get; set; }
            public DateTime? TT { get; set; }
            public TimeSpan U { get; set; }
            public TimeSpan? UU { get; set; }
            public DateTimeOffset V { get; set; }
            public DateTimeOffset? VV { get; set; }
            public Range Z { get; set; }
            public Range? ZZ { get; set; }
            public IEnumerable<InnerDelegationPopulation>? X { get; set; }
            public IDictionary<string, InnerDelegationPopulation>? Y { get; set; }
            public InnerDelegationPopulation[]? W { get; set; }
            public ICollection<InnerDelegationPopulation>? J { get; set; }
        }
        public class InnerDelegationPopulation
        {
            public string? A { get; set; }
            public int? B { get; set; }
        }
        [Fact]
        public void Test1()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddPopulationService();
            var serviceProvider = services.BuildServiceProvider().CreateScope().ServiceProvider;
            var populatedModel = serviceProvider.GetService<IPopulation<DelegationPopulation>>();
            var all = populatedModel!
                .Populate(90, 8)
                        .WithAutoIncrement(x => x.Id, 0)
                        .WithPattern(x => x.A, "[1-9]{1,2}")
                        .WithPattern(x => x.AA, "[1-9]{1,2}")
                        .WithPattern(x => x.B, "[1-9]{1,2}")
                        .WithPattern(x => x.BB, "[1-9]{1,2}")
                        .WithPattern(x => x.C, "[1-9]{1,2}")
                        .WithPattern(x => x.CC, "[1-9]{1,2}")
                        .WithPattern(x => x.D, "[1-9]{1,2}")
                        .WithPattern(x => x.DD, "[1-9]{1,2}")
                        .WithPattern(x => x.E, "[1-9]{1,2}")
                        .WithPattern(x => x.EE, "[1-9]{1,2}")
                        .WithPattern(x => x.F, "[1-9]{1,2}")
                        .WithPattern(x => x.FF, "[1-9]{1,2}")
                        .WithPattern(x => x.G, "[1-9]{1,2}")
                        .WithPattern(x => x.GG, "[1-9]{1,2}")
                        .WithPattern(x => x.H, "[1-9]{1,3}")
                        .WithPattern(x => x.HH, "[1-9]{1,3}")
                        .WithPattern(x => x.L, "[1-9]{1,3}")
                        .WithPattern(x => x.LL, "[1-9]{1,3}")
                        .WithPattern(x => x.M, "[1-9]{1,2}")
                        .WithPattern(x => x.MM, "[1-9]{1,2}")
                        .WithPattern(x => x.N, "[1-9]{1,2}")
                        .WithPattern(x => x.NN, "[1-9]{1,2}")
                        .WithPattern(x => x.O, "[1-9]{1,2}")
                        .WithPattern(x => x.OO, "[1-9]{1,2}")
                        .WithPattern(x => x.P, "[1-9a-zA-Z]{5,20}")
                        .WithPattern(x => x.Q, "true")
                        .WithPattern(x => x.QQ, "true")
                        .WithPattern(x => x.R, "[a-z]{1}")
                        .WithPattern(x => x.RR, "[a-z]{1}")
                        .WithPattern(x => x.S, "([0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12})")
                        .WithPattern(x => x.SS, "([0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12})")
                        .WithPattern(x => x.T, @"(?:2018|2019|2020|2021|2022)/(?:10|11|12)/(?:06|07|08) (00:00:00)")
                        .WithPattern(x => x.TT, @"(?:2018|2019|2020|2021|2022)/(?:10|11|12)/(?:06|07|08) (00:00:00)")
                        .WithPattern(x => x.U, "[1-9]{1,4}")
                        .WithPattern(x => x.UU, "[1-9]{1,4}")
                        .WithPattern(x => x.V, @"(?:10|11|12)/(?:06|07|08)/(?:2018|2019|2020|2021|2022) (00:00:00 AM \+00:00)")
                        .WithPattern(x => x.VV, @"(?:10|11|12)/(?:06|07|08)/(?:2018|2019|2020|2021|2022) (00:00:00 AM \+00:00)")
                        .WithPattern(x => x.Z, "[1-9]{1,2}", "[1-9]{1,2}")
                        .WithPattern(x => x.ZZ, "[1-9]{1,2}", "[1-9]{1,2}")
                        .WithPattern(x => x.J!.First().A, "[a-z]{4,5}")
                        .WithPattern(x => x.Y!.First().Value.A, "[a-z]{4,5}")
                        .Execute();
            var theFirst = all.First()!;
            Assert.Equal(90, all.Count);
            Assert.Equal(0, all.First().Id);
            Assert.Equal(89, all.Last().Id);
            Assert.NotEqual(0, theFirst.A);
            Assert.NotNull(theFirst.AA);
            Assert.NotEqual((uint)0, theFirst.B);
            Assert.NotNull(theFirst.BB);
            Assert.NotEqual((byte)0, theFirst.C);
            Assert.NotNull(theFirst.CC);
            Assert.NotEqual((sbyte)0, theFirst.D);
            Assert.NotNull(theFirst.DD);
            Assert.NotEqual((short)0, theFirst.E);
            Assert.NotNull(theFirst.EE);
            Assert.NotEqual((ushort)0, theFirst.F);
            Assert.NotNull(theFirst.FF);
            Assert.NotEqual((long)0, theFirst.G);
            Assert.NotNull(theFirst.GG);
            Assert.NotEqual((nint)0, theFirst.H);
            Assert.NotNull(theFirst.HH);
            Assert.NotEqual((nuint)0, theFirst.L);
            Assert.NotNull(theFirst.LL);
            Assert.NotEqual((float)0, theFirst.M);
            Assert.NotNull(theFirst.MM);
            Assert.NotEqual((double)0, theFirst.N);
            Assert.NotNull(theFirst.NN);
            Assert.NotEqual((decimal)0, theFirst.O);
            Assert.NotNull(theFirst.OO);
            Assert.NotNull(theFirst.P);
            Assert.NotNull(theFirst.PP);
            Assert.NotNull(theFirst.QQ);
            Assert.NotEqual((char)0, theFirst.R);
            Assert.NotNull(theFirst.RR);
            Assert.NotEqual(Guid.Empty, theFirst.S);
            Assert.NotNull(theFirst.SS);
            Assert.NotEqual(new DateTime(), theFirst.T);
            Assert.NotNull(theFirst.TT);
            Assert.NotEqual(TimeSpan.FromTicks(0), theFirst.U);
            Assert.NotNull(theFirst.UU);
            Assert.NotEqual(new DateTimeOffset(), theFirst.V);
            Assert.NotNull(theFirst.VV);
            Assert.NotEqual(new Range(), theFirst.Z);
            Assert.NotNull(theFirst.ZZ);
            Assert.NotNull(theFirst.X);
            Assert.Equal(8, theFirst?.X?.Count());
            Assert.NotNull(theFirst?.Y);
            Assert.Equal(8, theFirst?.Y?.Count);
            Assert.NotNull(theFirst?.W);
            Assert.Equal(8, theFirst?.W?.Length);
            Assert.NotNull(theFirst?.J);
            Assert.Equal(8, theFirst?.J?.Count);
            var regex = new Regex("[a-z]{4,5}");
            foreach (var check in theFirst!.J!)
            {
                Assert.Equal(check.A,
                    regex.Matches(check!.A!).OrderByDescending(x => x.Length).First().Value);
            }
            foreach (var check in theFirst!.Y!)
            {
                Assert.Equal(check.Value.A,
                    regex.Matches(check!.Value!.A!).OrderByDescending(x => x.Length).First().Value);
            }
        }
    }
}