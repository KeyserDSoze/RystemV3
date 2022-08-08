namespace System.Linq.Expressions
{
    public static partial class ExpressionExtensions
    {
        public static Expression<Func<T>> AsExpression<T>(this LambdaExpression lambdaExpression)
            => Expression.Lambda<Func<T>>(lambdaExpression.Body, lambdaExpression.Parameters);
        public static Expression<Func<T, T1>> AsExpression<T, T1>(this LambdaExpression lambdaExpression)
                => Expression.Lambda<Func<T, T1>>(lambdaExpression.Body, lambdaExpression.Parameters);
        public static Expression<Func<T, T1, T2>> AsExpression<T, T1, T2>(this LambdaExpression lambdaExpression)
            => Expression.Lambda<Func<T, T1, T2>>(lambdaExpression.Body, lambdaExpression.Parameters);
        public static Expression<Func<T, T1, T2, T3>> AsExpression<T, T1, T2, T3>(this LambdaExpression lambdaExpression)
            => Expression.Lambda<Func<T, T1, T2, T3>>(lambdaExpression.Body, lambdaExpression.Parameters);
    }
}