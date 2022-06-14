using System.Collections;

namespace System.Text.Csv
{
    internal class ArraySerializer : ICsvInterpreter
    {
        public int Priority => 1;

        public dynamic Deserialize(Type type, string value, int deep = int.MaxValue)
        {
            var list = value.Split((char)deep);
            var array = Activator.CreateInstance(type, list.Length);
            var currentType = type.GetElementType();
            for (int i = 0; i < list.Length; i++)
                (array as dynamic)![i] = Serializer.Instance.Deserialize(currentType!, list[i], deep - 1);
            return array!;
        }
        public string Serialize(Type type, object value, int deep = int.MaxValue)
        {
            return string.Join((char)deep, Read()
                .Select(x => Serializer.Instance.Serialize(x.GetType(), x, deep - 1)));

            IEnumerable<object> Read()
            {
                foreach (var item in (IEnumerable)value)
                    yield return item;
            }
        }
        public bool IsValid(Type type) => type.IsArray;
    }
}
