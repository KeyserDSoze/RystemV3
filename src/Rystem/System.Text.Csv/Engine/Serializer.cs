namespace System.Text.Csv
{
    internal class Serializer : ICsvInterpreter
    {
        public int Priority => 0;
        public static Serializer Instance { get; } = new();
        private Serializer() { }
        private readonly List<ICsvInterpreter> _interpreters = new List<ICsvInterpreter>()
        {
            new ObjectSerializer(),
            new PrimitiveSerializer(),
            new ArraySerializer(),
            new DictionarySerializer(),
            new EnumerableSerializer()
        }.OrderByDescending(x => x.Priority).ToList();
        public ICsvInterpreter? GetRightService(Type type)
           => _interpreters.FirstOrDefault(x => x.IsValid(type));
        public dynamic Deserialize(Type type, string value, int deep)
            => GetRightService(type)?.Deserialize(type, value, deep) ?? null!;
        public string Serialize(Type type, object value, int deep)
            => GetRightService(type)?.Serialize(type, value, deep) ?? null!;
        public bool IsValid(Type type) => true;
    }
}
