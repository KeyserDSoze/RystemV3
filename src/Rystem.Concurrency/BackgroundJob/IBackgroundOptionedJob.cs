namespace System.Threading
{
    public interface IBackgroundOptionedJob : IBackgroundJob
    {
        BackgroundJobOptions Options { get; }
    }
}