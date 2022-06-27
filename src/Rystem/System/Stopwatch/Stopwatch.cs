namespace System
{
    public static class Stopwatch
    {
        public static StopwatchStart Start() => new();
        public static StopwatchResult Monitor(this Action action)
        {
            var start = Start();
            action.Invoke();
            return start.Stop();
        }
        public static async Task<StopwatchResult> MonitorAsync(this Func<Task> action)
        {
            var start = Start();
            await action.Invoke().NoContext();
            return start.Stop();
        }
        public static async Task<StopwatchResult> MonitorAsync(this Task action)
        {
            var start = Start();
            await action;
            return start.Stop();
        }
    }
}