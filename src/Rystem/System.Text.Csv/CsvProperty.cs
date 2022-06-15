namespace System.Text.Csv
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CsvPropertyAttribute : Attribute
    {
        public int Column { get; }
        public CsvPropertyAttribute(int column)
            => Column = column;
    }
}
