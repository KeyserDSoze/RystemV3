namespace System.Text.Csv
{
    internal interface ICsvInterpreter
    {
        string Serialize(Type type, object value, int deep = int.MaxValue);
        dynamic Deserialize(Type type, string value, int deep = int.MaxValue);
        bool IsValid(Type type);
        int Priority { get; }
    }
}
