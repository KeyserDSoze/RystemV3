namespace System
{
    public static class CastExtensions
    {
        public static T Cast<T>(this object entity)
        {
            if (entity is IConvertible)
                return (T)Convert.ChangeType(entity, typeof(T));
            else
                return (T)entity;
        }
    }
}
