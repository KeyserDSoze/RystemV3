namespace System.Text.Csv
{
    public static class CsvConvertExtensions
    {
        public static string ToCsv<T>(this T data)
           => Serializer.Instance.Serialize(data!.GetType(), data);
        public static T FromCsv<T>(this string value)
           => (T)Serializer.Instance.Deserialize(typeof(T), value);
    }
}
