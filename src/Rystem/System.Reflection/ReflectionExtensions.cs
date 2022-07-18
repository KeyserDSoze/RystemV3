namespace System.Reflection
{
    public static class ReflectionExtensions
    {
        private static readonly Dictionary<string, PropertyInfo[]> AllProperties = new();
        private static readonly Dictionary<string, ConstructorInfo[]> AllConstructors = new();
        private static readonly Dictionary<string, FieldInfo[]> AllFields = new();
        private static readonly Dictionary<string, MethodInfo[]> AllMethods = new();
        private static readonly Dictionary<string, MethodInfo[]> AllStaticMethods = new();
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
        public static FieldInfo[] FetchFields(this Type type)
        {
            if (!AllFields.ContainsKey(type.FullName!))
                lock (Semaphore)
                    if (!AllFields.ContainsKey(type.FullName!))
                        AllFields.Add(type.FullName!, type.GetFields());
            return AllFields[type.FullName!];
        }
        public static MethodInfo[] FetchMethods(this Type type)
        {
            if (!AllMethods.ContainsKey(type.FullName!))
                lock (Semaphore)
                    if (!AllMethods.ContainsKey(type.FullName!))
                        AllMethods.Add(type.FullName!, type.GetMethods());
            return AllMethods[type.FullName!];
        }
        public static MethodInfo[] FetchStaticMethods(this Type type)
        {
            if (!AllStaticMethods.ContainsKey(type.FullName!))
                lock (Semaphore)
                    if (!AllStaticMethods.ContainsKey(type.FullName!))
                        AllStaticMethods.Add(type.FullName!, type.GetMethods(BindingFlags.Public | BindingFlags.Static));
            return AllStaticMethods[type.FullName!];
        }
    }
}
