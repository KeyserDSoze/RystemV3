namespace System.Text.Csv
{
    public static class CsvConvertExtensions
    {
        public static string ToCsv<T>(this T data, char? startSeparator = null) 
            => Serializer.Instance.Serialize(data!.GetType(), data, startSeparator == null ? int.MaxValue : (int)startSeparator);

        public static T FromCsv<T>(this string value, char? startSeparator = null) 
            => (T)Serializer.Instance.Deserialize(typeof(T), value, startSeparator == null ? int.MaxValue : (int)startSeparator);
    }
}
