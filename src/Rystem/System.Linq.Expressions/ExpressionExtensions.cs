namespace System.Linq.Expressions
{
    public static class ExpressionExtensions
    {
        public static string Serialize(this Expression expression) 
            => ExpressionSerializer.Instance.Serialize(expression);
        public static Expression<Func<T, TResult>> Deserialize<T, TResult>(this string expressionAsString)
            => ExpressionSerializer.Instance.Deserialize<T, TResult>(expressionAsString);
        public static Expression<Func<TResult>> Deserialize<TResult>(this string expressionAsString)
            => ExpressionSerializer.Instance.Deserialize<TResult>(expressionAsString);
    }
}