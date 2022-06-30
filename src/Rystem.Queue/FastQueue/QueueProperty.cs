namespace Rystem.Queue
{
    public sealed class QueueProperty<T>
    {
        public string Name { get; set; } = string.Empty;
        public int MaximumBuffer { get; set; } = 5000;
        public string MaximumRetentionCronFormat { get; set; } = "*/1 * * * *";
        public List<Func<IEnumerable<T>, Task>> Actions { get; set; } = new();
    }
}