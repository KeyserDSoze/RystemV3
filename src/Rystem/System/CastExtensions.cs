using System.Reflection;

namespace System
{
    public static class CastExtensions
    {
        public static T? Cast<T>(this object? entity)
        {
            if (entity == null)
                return default;
            if (entity is T casted)
                return casted;
            else if (entity is IConvertible)
                return (T)Convert.ChangeType(entity, typeof(T));
            else
                return (T)entity;
        }
        public static dynamic Cast(this object? entity, Type typeToCast) 
            => Generics.WithStatic(typeof(CastExtensions), nameof(Cast), typeToCast)
                .Invoke(entity!);
    }
}
