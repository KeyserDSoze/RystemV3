namespace System.Text.Csv
{
    internal class Serializer : ICsvInterpreter
    {
        public int Priority => 0;
        public static Serializer Instance { get; } = new();
        private Serializer() { }
        private readonly List<ICsvInterpreter> _interpreters = new()
        {
            new ObjectSerializer(),
            new PrimitiveSerializer(),
            new ArraySerializer(),
            new DictionarySerializer(),
            new EnumerableSerializer()
        };
        public ICsvInterpreter? GetRightService(Type type)
           => _interpreters.OrderByDescending(x => x.Priority).FirstOrDefault(x => x.IsValid(type));
        public dynamic Deserialize(Type type, string value, int deep = int.MaxValue)
            => GetRightService(type)?.Deserialize(type, value, deep) ?? null!;
        public string Serialize(Type type, object value, int deep = int.MaxValue)
            => GetRightService(type)?.Serialize(type, value, deep) ?? null!;
        public bool IsValid(Type type) => true;
    }
}
