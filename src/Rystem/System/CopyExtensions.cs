using System.Text.Json;

namespace Rystem.System
{
    public static class CopyExtensions
    {
        public static T? ToDeepCopy<T>(this T? source)
        {
            if (source == null)
                return default;
            else
                return source.ToJson().FromJson<T>();
        }
    }
}
