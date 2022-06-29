namespace System.Linq.Expressions
{
    internal sealed record ExpressionContext
    {
        public string ExpressionAsString { get; private set; }
        public List<Type> Arguments { get; } = new();
        public ExpressionContext(Expression expression)
        {
            ExpressionAsString = expression.ToString();
        }
        public void ReplaceWithValue(string key, object? value)
        {
            ExpressionAsString = ExpressionAsString.Replace(key, Interpretate(value), 1);
        }
        public void DirectReplace(string key, string value)
        {
            ExpressionAsString = ExpressionAsString.Replace(key, value, 1);
        }
        public bool IsAnArgument(Type? type) 
            => type != null && Arguments.Any(x => x == type);
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
        private static string Interpretate(object? value)
        {
            if (value is null)
                return "null";
            if (value is string)
                return $"\"{value}\"";
            else if (value is Guid)
                return $"Guid.Parse(\"{value}\")";
            else if (value is char)
                return $"'{value}'";
            else
                return value.ToString()!;
        }
    }
}