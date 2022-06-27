using System;
using System.Threading.Tasks;
using System.Timers;

namespace Rystem.Concurrency.Test.UnitTest
{
    public abstract class BasicService
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
    public class SingletonService : BasicService
    {
    }
    public class ScopedService : BasicService
    {
    }
    public class TransientService : BasicService
    {
    }
    public class BackgroundJob : IBackgroundJob
    {
        private readonly SingletonService _singleton;
        private readonly ScopedService _scoped;
        private readonly TransientService _transient;
        private readonly TransientService _transient1;
        private static Guid? LastSingleton;
        private static Guid? LastScoped;
        private static Guid? LastTransient;
        private static Guid? LastTransient1;
        public BackgroundJob(SingletonService singleton,
            ScopedService scoped,
            TransientService transient,
            TransientService transient1)
        {
            _singleton = singleton;
            _scoped = scoped;
            _transient = transient;
            _transient1 = transient1;
        }
        public Task ActionToDoAsync()
        {
            if (LastScoped != null && LastScoped == _scoped.Id)
                throw new ArgumentException("Scope service is not correct.");
            LastScoped = _scoped.Id;
            if (LastSingleton != null && LastSingleton != _singleton.Id)
                throw new ArgumentException("Singleton service is not correct.");
            LastSingleton = _singleton.Id;
            if (_transient.Id == _transient1.Id)
                throw new ArgumentException("Transient service is not correct.");
            if (LastTransient != null && LastTransient == _transient.Id)
                throw new ArgumentException("Transient service is not correct.");
            LastTransient = _transient.Id;
            if (LastTransient1 != null && LastTransient1 == _transient1.Id)
                throw new ArgumentException("Transient1 service is not correct.");
            LastTransient1 = _transient1.Id;
            BackgroundJobTest.Counter++;
            return Task.CompletedTask;
        }

        public Task OnException(Exception exception)
        {
            throw exception;
        }
    }
}