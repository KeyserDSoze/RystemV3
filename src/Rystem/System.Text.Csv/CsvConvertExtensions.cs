namespace System.Text.Csv
{
    public static class CsvConvertExtensions
    {
        private static readonly int _MaxValue = int.MaxValue;
        private static readonly char MaxValue = (char)_MaxValue;
        public static string ToCsv<T>(this T data, bool withHeader = false)
        {
            var stringBuilder = new StringBuilder();
            var result = Serializer.Instance.Serialize(data!.GetType(), data, int.MaxValue, stringBuilder);

            if (withHeader)
                return $"{stringBuilder}{MaxValue}{result}";
            else
                return result;
        }

        public static T FromCsv<T>(this string value, bool withHeader = false)
        {
            if (withHeader)
                value = string.Join(MaxValue, value.Split(MaxValue).Skip(1));
            return (T)Serializer.Instance.Deserialize(typeof(T), value, int.MaxValue);
        }
    }
}
