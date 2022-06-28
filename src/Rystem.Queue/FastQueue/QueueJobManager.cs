using Cronos;
using System.Timers;

namespace Rystem.Queue
{
    internal sealed class QueueJobManager<T> : IBackgroundJob
    {
        private readonly IQueue<T> _queue;
        private readonly QueueProperty<T> _property;
        private DateTime _lastFlush = DateTime.UtcNow;
        public QueueJobManager(IQueue<T> queue, QueueProperty<T> property)
        {
            _queue = queue;
            _property = property;
        }
        public async Task ActionToDoAsync()
        {
            var expression = CronExpression.Parse(_property.MaximumRetentionCronFormat, _property.MaximumRetentionCronFormat?.Split(' ').Length > 5 ? CronFormat.IncludeSeconds : CronFormat.Standard);
            var nextRunningTime = expression.GetNextOccurrence(_lastFlush, true)?.Subtract(_lastFlush).TotalMilliseconds ?? 120;
            DateTime nextFlush = _lastFlush.AddMilliseconds(nextRunningTime);
            if (await _queue.CountAsync().NoContext() > _property.MaximumBuffer || nextFlush < DateTime.UtcNow)
            {
                List<T> items = new();
                foreach (var item in await _queue.DequeueAsync().NoContext())
                    items.Add(item);
                foreach (var action in _property.Actions)
                    await action.Invoke(items).NoContext();
                _lastFlush = DateTime.UtcNow;
            }
        }

        public Task OnException(Exception exception)
        {
            throw exception;
        }
    }
}