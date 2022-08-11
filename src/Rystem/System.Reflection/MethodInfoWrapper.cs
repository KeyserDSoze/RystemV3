using System.Reflection;

namespace System
{
    public class MethodInfoWrapper
    {
        private readonly MethodInfo _method;
        public MethodInfoWrapper(MethodInfo methodInfo)
            => _method = methodInfo;
        public TResult? Invoke<TResult>(object obj, params object[] inputs)
        {
            var value = _method.Invoke(obj, inputs);
            return (TResult?)value;
        }
    }
}
