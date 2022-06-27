namespace System.Linq.Expressions
{
    internal sealed class MethodCallExpressionInterpreter : IExpressionInterpreter
    {
        public Type Type { get; } = typeof(MethodCallExpression);

        public IEnumerable<ExpressionBearer>? Read(ExpressionBearer bearer, ExpressionContext context)
        {
            if (bearer.Expression is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Arguments.Count > 0)
                    foreach (var argument in methodCallExpression.Arguments)
                        context.CompileAndReplace(argument);
                else
                    context.CompileAndReplace(methodCallExpression);
            }
            return null;
        }
    }
}