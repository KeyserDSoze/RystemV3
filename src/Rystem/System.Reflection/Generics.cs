using System.Collections.Concurrent;
using System.Reflection;

namespace System
{
    public sealed class Generics
    {
        private static readonly ConcurrentDictionary<string, MethodInfoWrapper> MethodsCache = new();
        public static MethodInfoWrapper With(Type containerType, string methodName, params Type[] generics)
        {
            string key = $"{containerType.Name}_{methodName}_{string.Join("_", generics.Select(x => x.FullName))}";
            if (MethodsCache.ContainsKey(key))
                return MethodsCache[key];
            var method = containerType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).FirstOrDefault(x => x.Name == methodName && x.IsGenericMethod && x.GetGenericArguments().Length == generics.Length)!;
            method = method.MakeGenericMethod(generics);
            MethodsCache.TryAdd(key, new(method));
            return MethodsCache[key];
        }
    }
}