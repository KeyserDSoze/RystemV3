using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

namespace System.Text.Csv
{
    internal sealed class CsvEngine
    {
        private sealed class ColumnKeeper
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
        private sealed class RowKeeper
        {
            public List<ColumnKeeper> Columns { get; set; }
        }
        private sealed class TableKeeper
        {
            public List<RowKeeper> Rows { get; set; }
        }
        private static readonly Regex Regex = new(@"\[[^\]]*\]");
        public static string Convert<T>(IEnumerable<T> values)
        {
            var showcase = typeof(T).ToShowcase();
            var tableKeeper = new TableKeeper() { Rows = new() };
            Dictionary<string, int> maxColumns = new();
            foreach (var value in values)
            {
                RowKeeper rowKeeper = new() { Columns = new() };
                ConvertOne(showcase.Properties, Array.Empty<int>());
                tableKeeper.Rows.Add(rowKeeper);
                void ConvertOne(List<BaseProperty> properties, int[] indexes)
                {
                    foreach (var property in properties)
                    {
                        if (!maxColumns.ContainsKey(property.NavigationPath) && property.Type != PropertyType.Complex)
                            maxColumns.Add(property.NavigationPath, -1);
                        if (property.Type == PropertyType.Primitive)
                        {
                            var entry = property.NamedValue(value, indexes);
                            rowKeeper.Columns.Add(new()
                            {
                                Key = entry.Name,
                                Value = entry.Value?.ToString() ?? string.Empty,
                            });
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
                            if (innerIndex > maxColumns[property.NavigationPath])
                                maxColumns[property.NavigationPath] = innerIndex;
                        }
                    }
                }
            }
            var keys = maxColumns.Select(x => x.Key).ToList();
            foreach (var key in keys)
            {
                if (maxColumns[key] > 0)
                {
                    var columnsWithThatValue = maxColumns.Where(x => x.Key != key && x.Key.Contains(key)).Select(x => x.Key).ToList();
                    if (columnsWithThatValue.Count > 0)
                    {
                        foreach (var innerKey in columnsWithThatValue)
                            maxColumns[innerKey] = maxColumns[key];
                        maxColumns.Remove(key);
                    }
                }
            }
            StringBuilder header = new();
            StringBuilder[] rows = new StringBuilder[tableKeeper.Rows.Count];
            foreach (var max in maxColumns)
            {
                if (max.Value == -1)
                {
                    if (header.Length > 0)
                        header.Append(',');
                    header.Append(CheckIfContainsEscapeCharacters(max.Key));
                }
                else
                {
                    for (int i = 0; i < max.Value; i++)
                    {
                        if (header.Length > 0)
                            header.Append(',');
                        header.Append(CheckIfContainsEscapeCharacters($"{max.Key}[{i}]"));
                    }
                }
                int counter = 0;
                foreach (var row in tableKeeper.Rows)
                {
                    if (rows[counter] == null)
                        rows[counter] = new StringBuilder();
                    var stringBuilder = rows[counter];
                    if (stringBuilder.Length > 0)
                        stringBuilder.Append(',');
                    if (max.Value == -1)
                    {
                        var value = row.Columns.First(x => x.Key == max.Key).Value;
                        stringBuilder.Append(CheckIfContainsEscapeCharacters(value));
                    }
                    else
                    {
                        var list = row.Columns.Where(x => Regex.Replace(x.Key, string.Empty) == max.Key).ToList();
                        stringBuilder.Append(string.Join(',', list.Select(x => CheckIfContainsEscapeCharacters(x.Value))));
                        for (int i = list.Count; i < max.Value; i++)
                            stringBuilder.Append(',');
                    }
                    counter++;
                }
            }

            string CheckIfContainsEscapeCharacters(string value)
                => value.Contains(',') || value.Contains('"') ? $"\"{value}\"" : value;
            return $"{header}{'\n'}{string.Join('\n', rows.Select(x => x.ToString()))}";
        }
    }
}
