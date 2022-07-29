namespace System.Reflection
{
    public static class PrimitiveExtensions
    {
        private static readonly Type[] PrimitiveTypes = new Type[] {
            typeof(string),
            typeof(decimal),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(Guid)
        };
        public static bool IsPrimitive<T>(this T entity)
            => entity?.GetType().IsPrimitive() ?? typeof(T).IsPrimitive();
        public static bool IsPrimitive(this Type type)
            => type.IsPrimitive || PrimitiveTypes.Contains(type) || type.IsEnum || Convert.GetTypeCode(type) != TypeCode.Object ||
            (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsPrimitive(type.GetGenericArguments()[0]));
    }
}