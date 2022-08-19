using System.Linq.Expressions;

namespace System.Linq
{
    public static class LambdaExpressionExtensions
    {
        public static LambdaExpression ChangeReturnType(this LambdaExpression expression, Type toChange)
        {
            if (expression.ReturnType != toChange)
                return Expression.Lambda(
                        Expression.Convert(expression.Body, toChange),
                        expression.Parameters);
            return expression;
        }
        public static LambdaExpression ChangeReturnType<T>(this LambdaExpression expression) 
            => expression.ChangeReturnType(typeof(T));
    }
}
