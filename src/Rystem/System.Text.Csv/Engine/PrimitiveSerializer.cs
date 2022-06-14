using System.Globalization;

namespace System.Text.Csv
{
    internal class PrimitiveSerializer : ICsvInterpreter
    {
        public int Priority => 6;
        public bool IsValid(Type type) => type == typeof(int) || type == typeof(int?) || type == typeof(uint) || type == typeof(uint?)
                || type == typeof(short) || type == typeof(short?) || type == typeof(ushort) || type == typeof(ushort?)
                || type == typeof(long) || type == typeof(long?) || type == typeof(ulong) || type == typeof(ulong?)
                || type == typeof(nint) || type == typeof(nint?) || type == typeof(nuint) || type == typeof(nuint?)
                || type == typeof(float) || type == typeof(float?) || type == typeof(double) || type == typeof(double?)
                || type == typeof(decimal) || type == typeof(decimal?) || type == typeof(Guid) || type == typeof(Guid?)
                || type == typeof(char) || type == typeof(char?) || type == typeof(byte) || type == typeof(byte?) || type == typeof(sbyte) || type == typeof(sbyte?)
                || type == typeof(bool) || type == typeof(bool?) || type == typeof(string) || type == typeof(DateTime) || type == typeof(DateTime?) || type == typeof(TimeSpan)
                || type == typeof(TimeSpan?) || type == typeof(DateTimeOffset) || type == typeof(DateTimeOffset?) || type.IsEnum;
        public dynamic Deserialize(Type type, string value, int deep = int.MaxValue)
        {
            if (!type.IsEnum)
            {
                return (!string.IsNullOrWhiteSpace(value) ?
                    (!type.IsGenericType ?
                        Convert.ChangeType(value, type, CultureInfo.InvariantCulture) :
                        Convert.ChangeType(value, type.GenericTypeArguments[0], CultureInfo.InvariantCulture)
                    )
                    : default)!;
            }
            else
                return Enum.Parse(type, value);
        }

        public string Serialize(Type type, object value, int deep = int.MaxValue)
            => value?.ToString() ?? string.Empty;
    }
}
