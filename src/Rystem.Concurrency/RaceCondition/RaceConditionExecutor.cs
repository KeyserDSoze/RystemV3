namespace System.Threading.Concurrent
{
    internal sealed class RaceConditionExecutor
    {
        private DateTime LastExecutionPlusExpirationTime;
        internal bool IsExpired => DateTime.UtcNow > LastExecutionPlusExpirationTime;
        private readonly string Key;
        private readonly TimeSpan TimeWindow;
        public RaceConditionExecutor(string id, TimeSpan timeWindow)
        {
            Key = id;
            TimeWindow = timeWindow == default ? TimeSpan.FromMinutes(1) : timeWindow;
        }
        private readonly MemoryLock Memory = new();
        public async Task<RaceConditionResponse> ExecuteAsync(Func<Task> action, ILockable lockable)
        {
            lockable ??= Memory;
            LastExecutionPlusExpirationTime = DateTime.UtcNow.Add(TimeWindow);
            var isTheFirst = false;
            var isWaiting = false;
            await WaitAsync().NoContext();
            if (!isWaiting)
            {
                if (await lockable.AcquireAsync(Key).NoContext())
                    isTheFirst = true;
                if (!isTheFirst)
                    await WaitAsync().NoContext();
            }
            Exception exception = default!;
            if (isTheFirst && !isWaiting)
            {
                try
                {
                    await action.Invoke().NoContext();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                await lockable.ReleaseAsync(Key).NoContext();
            }
            return new RaceConditionResponse(isTheFirst && !isWaiting, exception != default ? new List<Exception>() { exception } : new());

            async Task WaitAsync()
            {
                while (await lockable.IsAcquiredAsync(Key).NoContext())
                {
                    isWaiting = true;
                    await Task.Delay(4).NoContext();
                }
            }
        }
    }
}
