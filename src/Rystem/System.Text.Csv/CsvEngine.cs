using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace System.Text.Csv
{
    internal sealed class CsvEngine
    {
        private sealed class TableHandler
        {
            public Dictionary<string, RowHandler> Map { get; set; }
            public void Add(int valueIndex, BasePropertyNameValue basePropertyNameValue)
            {
                if (!Map.ContainsKey(basePropertyNameValue.NavigationPath))
                    Map.Add(basePropertyNameValue.NavigationPath, new RowHandler
                    {
                        Max = 1,
                        NavigationPath = basePropertyNameValue.NavigationPath,
                        Rows = new()
                    });
                var map = Map[basePropertyNameValue.NavigationPath];
                if (map.Rows.Count <= valueIndex)
                    map.Rows.Add(new() { Columns = new() });
                var row = map.Rows[valueIndex];
                if (!row.Columns.ContainsKey(basePropertyNameValue.Name))
                    row.Columns.Add(basePropertyNameValue.Name, new());
                var listOfValues = row.Columns[basePropertyNameValue.Name];
                listOfValues.Add(basePropertyNameValue.Value?.ToString() ?? string.Empty);
                if (listOfValues.Count > map.Max)
                    map.Max = listOfValues.Count;
            }
        }
        private static readonly Regex Regex = new(@"\[[^\]]*\]");
        private sealed class ColumnHandler
        {
            public Dictionary<string, List<string>> Columns { get; set; }
        }
        private sealed class RowHandler
        {
            public required string NavigationPath { get; init; }
            public required List<ColumnHandler> Rows { get; init; }
            public required int Max { get; set; }

        }
        public static string Convert<T>(IEnumerable<T> values)
        {
            var showcase = typeof(T).ToShowcase();
            var tableHandler = new TableHandler() { Map = new() };
            int counter = 0;
            foreach (var value in values)
            {
                ConvertOne(showcase.Properties, Array.Empty<int>());
                counter++;

                void ConvertOne(List<BaseProperty> properties, int[] indexes)
                {
                    foreach (var property in properties)
                    {
                        if (property.Type == PropertyType.Primitive)
                        {
                            var entry = property.NamedValue(value, indexes);
                            tableHandler.Add(counter, entry);
                        }
                        else if (property.Type == PropertyType.Complex)
                        {
                            ConvertOne(property.Sons, indexes);
                        }
                        else
                        {
                            var innerValues = property.Value(value, indexes) as IEnumerable;
                            var innerIndex = 0;
                            if (innerValues != null)
                            {
                                var innerIndexes = new int[indexes.Length + 1];
                                indexes.CopyTo(innerIndexes, 0);
                                foreach (var innerValue in innerValues)
                                {
                                    innerIndexes[innerIndexes.Length - 1] = innerIndex;
                                    ConvertOne(property.Sons, innerIndexes);
                                    innerIndex++;
                                }
                            }
                        }
                    }
                }
            }
            StringBuilder header = new();
            foreach (var map in tableHandler.Map)
            {
                if (map.Value.Max < 2)
                {
                    foreach (var value in map.Value.Rows.First().Columns)
                        if (string.IsNullOrWhiteSpace(map.Value.NavigationPath))
                            header.Append($"{value.Key},");
                        else
                            header.Append($"{map.Value.NavigationPath}.{value.Key},");
                }
                else
                {
                    foreach (var value in map.Value.Rows.First().Columns)
                        for (int i = 0; i < map.Value.Max; i++)
                        {
                            header.Append($"{map.Value.NavigationPath}[{i}].{value.Key},");
                        }
                }
            }
            StringBuilder[] rows = new StringBuilder[counter];
            foreach (var map in tableHandler.Map)
            {
                int internalCounter = 0;
                foreach (var row in map.Value.Rows)
                {
                    if (rows[internalCounter] == null)
                        rows[internalCounter] = new StringBuilder();
                    var stringBuilder = rows[internalCounter];
                    foreach (var column in row.Columns)
                    {
                        if (stringBuilder.Length > 0)
                            stringBuilder.Append(',');
                        stringBuilder.Append(string.Join(',', column.Value.Select(x => CheckIfContainsEscapeCharacters(x))));
                        for (int i = column.Value.Count; i < map.Value.Max; i++)
                            stringBuilder.Append(',');
                    }
                    internalCounter++;
                }
            }

            string CheckIfContainsEscapeCharacters(string value)
                => value.Contains(',') || value.Contains('"') ? $"\"{value}\"" : value;
            return $"{header.ToString().Trim(',')}{'\n'}{string.Join('\n', rows.Select(x => x.ToString()))}";
        }
    }
}
