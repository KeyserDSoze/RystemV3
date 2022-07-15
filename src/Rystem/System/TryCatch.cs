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
        [Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S2486:Generic exceptions should not be ignored", Justification = "To skip the exception.")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S108:Nested blocks of code should not be left empty", Justification = "For empty catch.")]
        public static void WithDefaultOnCatch(Action function)
        {
            try
            {
                function.Invoke();
            }
            catch
            {
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
        [Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S2486:Generic exceptions should not be ignored", Justification = "To skip the exception.")]
        [Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S108:Nested blocks of code should not be left empty", Justification = "For empty catch.")]
        public static async Task WithDefaultOnCatchAsync(Func<Task> function)
        {
            try
            {
                await function.Invoke();
            }
            catch
            {
            }
        }
    }
}
