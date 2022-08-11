using System.Reflection;

namespace System
{
    public class StaticMethodInfoWrapper
    {
        private readonly MethodInfo _method;
        public StaticMethodInfoWrapper(MethodInfo methodInfo)
            => _method = methodInfo;
        public object? Invoke(params object[] inputs)
          => _method.Invoke(null, inputs);
        public TResult? Invoke<TResult>(params object[] inputs)
        {
            var value = _method.Invoke(null, inputs);
            return (TResult?)value;
        }
    }
}
