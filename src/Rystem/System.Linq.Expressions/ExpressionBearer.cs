using System.Reflection;

namespace System.Linq.Expressions
{
    internal sealed record ExpressionContext
    {
        public string ExpressionAsString { get; private set; }
        public ExpressionContext(string expressionAsString)
        {
            ExpressionAsString = expressionAsString;
        }
        public void ReplaceWithValue(string key, object? value)
        {
            ExpressionAsString = ExpressionAsString.Replace(key, Interpretate(value));
        }
        public void CompileAndReplace(Expression argument)
        {
            try
            {
                var argumentKey = argument.ToString();
                var value = Expression.Lambda(argument).Compile().DynamicInvoke();
                ReplaceWithValue(argumentKey, value);
            }
            catch { }
        }
        public string Interpretate(object? value)
        {
            if (value is null)
                return "null";
            if (value is string)
                return $"\"{value}\"";
            else if (value is Guid)
                return $"Guid.Parse(\"{value}\")";
            else
                return value.ToString()!;
        }
    }
    internal sealed record ExpressionBearer(Expression Expression)
    {
        public MemberInfo? Member { get; set; }
        public string? Key { get; set; }
    }
}