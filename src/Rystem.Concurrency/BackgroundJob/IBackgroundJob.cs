using System.Threading.Tasks;

namespace System.Threading
{
    public interface IBackgroundJob
    {
        Task ActionToDoAsync();
        Task OnException(Exception exception);
    }
}