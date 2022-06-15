namespace System.Threading
{
    public sealed record BackgroundJobManager(string Id = "")
    {
        /// <summary>
        /// Method that allows task to run continuously in background.
        /// </summary>
        /// <param name="task">Action to perform.</param>
        /// <param name="nextRunningTime">Function that has to return a value in milliseconds. Default time is 120ms.</param>
        /// <param name="runImmediately">Run immediately at the start.</param>
        public Task RunAsync(Action task, Func<double> nextRunningTime = default, bool runImmediately = false)
            => RunInBackgroundAsync(task, Id, nextRunningTime, runImmediately);
        /// <summary>
        /// Method that allows task to run continuously in background.
        /// </summary>
        /// <param name="task">Action to perform.</param>
        /// <param name="id">Task id that runs in background.</param>
        /// <param name="nextRunningTime">Function that has to return a value in milliseconds. Default time is 120ms.</param>
        /// <param name="runImmediately">Run immediately at the start.</param>
        public Task RunAsync(Func<Task> task,Func<double> nextRunningTime = default, bool runImmediately = false)
            => task.RunInBackgroundAsync(Id, nextRunningTime, runImmediately);
        /// <summary>
        /// Remove a task from the continuously running by its id.
        /// </summary>
        /// <param name="id">Task id that runs in background.</param>
        public Task StopAsync()
            => BackgroundJobExtensions.StopRunningInBackgroundAsync(null, Id);
        /// <summary>
        /// Get if your task is running.
        /// </summary>
        /// <param name="id">Task id that runs in background.</param>
        public bool IsRunning()
            => BackgroundJobExtensions.IsRunning(null, Id);
    }
}