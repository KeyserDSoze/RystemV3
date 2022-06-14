using System.Reflection;

namespace System.Text.Csv
{
    internal class ObjectSerializer : ICsvInterpreter
    {
        public int Priority => 0;
        private static readonly Type Ignore = typeof(CsvIgnore);
        public bool IsValid(Type type) => !type.IsInterface && !type.IsAbstract;
        public dynamic Deserialize(Type type, string value, int deep = int.MaxValue)
        {
            var constructor = type.FecthConstructors()
               .OrderBy(x => x.GetParameters().Length)
               .FirstOrDefault();
            if (constructor == null)
                return null!;
            else
            {
                var instance = Activator.CreateInstance(type, constructor.GetParameters().Select(x => x.DefaultValue!).ToArray())!;
                var enumerator = value.Split((char)deep).GetEnumerator();
                foreach (var property in type.FetchProperties())
                {
                    enumerator.MoveNext();
                    if (property.SetMethod != null)
                        property.SetValue(instance, Serializer.Instance.Deserialize(property.PropertyType, enumerator.Current.ToString()!, deep - 1));
                }
                return instance;
            }
        }

        public string Serialize(Type type, object value, int deep, StringBuilder? header)
        {
            header?.Append($"{(char)deep}{type.Name}");
            return string.Join((char)deep,
                           type.FetchProperties(Ignore)
                               .Select(x => Serializer.Instance.Serialize(x.PropertyType, x.GetValue(value)!, deep - 1, header)));
        }
    }
}
