using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
    public static class RystemTaskExtensions
    {
        [Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "It's possible to change during startup.")]
        public static bool WaitCurrentThread;
        public static ConfiguredTaskAwaitable NoContext(this Task task)
            => task.ConfigureAwait(WaitCurrentThread);
        public static ConfiguredTaskAwaitable<T> NoContext<T>(this Task<T> task)
            => task.ConfigureAwait(WaitCurrentThread);
        public static ConfiguredValueTaskAwaitable NoContext(this ValueTask task)
          => task.ConfigureAwait(WaitCurrentThread);
        public static ConfiguredValueTaskAwaitable<T> NoContext<T>(this ValueTask<T> task)
            => task.ConfigureAwait(WaitCurrentThread);
        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> items)
        {
            List<T> entities = new();
            await foreach (var item in items)
                entities.Add(item);
            return entities;
        }
        public static void ToResult(this Task task)
            => task.NoContext().GetAwaiter().GetResult();
        public static T ToResult<T>(this Task<T> task)
            => task.NoContext().GetAwaiter().GetResult();
        public static void ToResult(this ValueTask task)
            => task.NoContext().GetAwaiter().GetResult();
        public static T ToResult<T>(this ValueTask<T> task)
            => task.NoContext().GetAwaiter().GetResult();
    }
}