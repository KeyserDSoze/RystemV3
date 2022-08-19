using System;
using System.Collections.Generic;
using System.Text.Csv;
using System.Text.Json;
using Xunit;

namespace Rystem.Test.UnitTest.Csv
{
    public class CsvTest
    {
        internal sealed class CsvModel
        {
            public string? X { get; set; }
            public int Id { get; set; }
            public string? B { get; set; }
            public Guid E { get; set; }
            public bool Sol { get; set; }
        }
        private static readonly List<CsvModel> _models = new();
        static CsvTest()
        {
            for (int i = 0; i < 100; i++)
                _models.Add(new CsvModel { X = i.ToString(), Id = i, B = i.ToString(), E = Guid.NewGuid(), Sol = i % 2 == 0 });
        }
        [Fact]
        public void Test1()
        {
            var value = _models.ToCsv(';');
            Assert.True(value.Length < _models.ToJson().Length);
            var models2 = value.FromCsv<List<CsvModel>>(';');
            Assert.Equal(_models.Count, models2.Count);
        }
        [Fact]
        public void Test2()
        {
            var value = _models.ToCsv();
            Assert.True(value.Length < _models.ToJson().Length);
            var models2 = value.FromCsv<List<CsvModel>>();
            Assert.Equal(_models.Count, models2.Count);
        }
    }
}