namespace System.Linq.Expressions
{
    internal sealed class MethodCallExpressionInterpreter : IExpressionInterpreter
    {
        public Type Type { get; } = typeof(MethodCallExpression);

        public IEnumerable<ExpressionBearer>? Read(ExpressionBearer bearer, ExpressionContext context)
        {
            List<ExpressionBearer> expressions = new();
            if (bearer.Expression is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Arguments.Count > 0)
                    foreach (var argument in methodCallExpression.Arguments)
                    {
                        if (argument is Expression)
                            expressions.Add(new(argument));
                        else
                            context.CompileAndReplace(argument);
                    }
                else
                    context.CompileAndReplace(methodCallExpression);
            }
            return expressions;
        }
    }
}