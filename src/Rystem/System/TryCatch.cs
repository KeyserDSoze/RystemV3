namespace System
{
    public static class Try
    {
        public static T? WithDefaultOnCatch<T>(Func<T> function)
        {
            try
            {
                return function.Invoke();
            }
            catch
            {
                return default;
            }
        }
        public static async Task<T?> WithDefaultOnCatchAsync<T>(Func<Task<T>> function)
        {
            try
            {
                return await function.Invoke();
            }
            catch
            {
                return default;
            }
        }
    }
}
