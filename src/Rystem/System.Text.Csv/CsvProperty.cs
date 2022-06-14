namespace System.Text.Csv
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CsvProperty : Attribute
    {
        public string Name { get; }
        public CsvProperty(string name)
            => this.Name = name;
    }
}
