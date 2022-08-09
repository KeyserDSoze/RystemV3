using System.Reflection;

namespace System
{
    public class MethodInfoWrapper
    {
        private readonly MethodInfo _method;
        public MethodInfoWrapper(MethodInfo methodInfo)
            => _method = methodInfo;
        public object? Invoke(params object[] inputs)
          => _method.Invoke(null, inputs);
        public TResult? Invoke<TResult>(params object[] inputs)
        {
            var value = _method.Invoke(null, inputs);
            return (TResult?)value;
        }
        public TResult? Invoke<TResult>(object obj, params object[] inputs)
        {
            var value = _method.Invoke(obj, inputs);
            return (TResult?)value;
        }
    }
}
