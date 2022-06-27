﻿namespace System.Threading.Concurrent
{
    public sealed class MemoryLock : ILockable
    {
        private readonly object Semaphore = new();
        private bool IsLocked { get; set; }
        public Task<bool> AcquireAsync(string key)
        {
            if (!IsLocked)
                lock (Semaphore)
                {
                    if (!IsLocked)
                    {
                        IsLocked = true;
                        return Task.FromResult(true);
                    }
                }
            return Task.FromResult(false);
        }
        public Task<bool> IsAcquiredAsync(string key)
            => Task.FromResult(IsLocked);
        public Task<bool> ReleaseAsync(string key)
        {
            IsLocked = false;
            return Task.FromResult(true);
        }
    }
}
