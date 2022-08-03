namespace System.Linq.Expressions
{
    public static class ExpressionExtensions
    {
        public static string Serialize(this Expression expression) 
            => ExpressionSerializer.Serialize(expression);
        public static Expression<Func<T, TResult>> Deserialize<T, TResult>(this string expressionAsString)
            => ExpressionSerializer.Deserialize<T, TResult>(expressionAsString);
        public static Expression<Func<TResult>> Deserialize<TResult>(this string expressionAsString)
            => ExpressionSerializer.Deserialize<TResult>(expressionAsString);
        public static Func<T, TResult> DeserializeAndCompile<T, TResult>(this string expressionAsString)
            => ExpressionSerializer.Deserialize<T, TResult>(expressionAsString).Compile();
        public static Func<TResult> DeserializeAndCompile<TResult>(this string expressionAsString)
            => ExpressionSerializer.Deserialize<TResult>(expressionAsString).Compile();
        public static LambdaExpression DeserializeAsDynamic<T>(this string expressionAsString)
            => ExpressionSerializer.DeserializeAsDynamic<T>(expressionAsString);
        public static Delegate DeserializeAndCompileAsDynamic<T>(this string expressionAsString)
            => ExpressionSerializer.DeserializeAsDynamic<T>(expressionAsString).Compile();
    }
}