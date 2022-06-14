using System.Reflection;

namespace System.Reflection
{
    public static class ReflectionExtensions
    {
        private static readonly Dictionary<string, PropertyInfo[]> AllProperties = new();
        private static readonly Dictionary<string, ConstructorInfo[]> AllConstructors = new();
        private static readonly Dictionary<string, FieldInfo[]> AllFields = new();
        private static readonly object Semaphore = new();
        public static PropertyInfo[] FetchProperties(this Type type, params Type[] attributesToIgnore)
        {
            if (!AllProperties.ContainsKey(type.FullName!))
                lock (Semaphore)
                    if (!AllProperties.ContainsKey(type.FullName!))
                        AllProperties.Add(type.FullName!, type.GetProperties()
                            .Where(x =>
                            {
                                foreach (Type attributeToIgnore in attributesToIgnore)
                                    if (x.GetCustomAttribute(attributeToIgnore) != default)
                                        return false;
                                return true;
                            }).ToArray());
            return AllProperties[type.FullName!];
        }
        public static ConstructorInfo[] FecthConstructors(this Type type)
        {
            if (!AllConstructors.ContainsKey(type.FullName!))
                lock (Semaphore)
                    if (!AllConstructors.ContainsKey(type.FullName!))
                        AllConstructors.Add(type.FullName!, type.GetConstructors());
            return AllConstructors[type.FullName!];
        }
        public static ConstructorInfo[] FetchFields(this Type type)
        {
            if (!AllConstructors.ContainsKey(type.FullName!))
                lock (Semaphore)
                    if (!AllConstructors.ContainsKey(type.FullName!))
                        AllFields.Add(type.FullName!, type.GetFields());
            return AllConstructors[type.FullName!];
        }
    }
}
