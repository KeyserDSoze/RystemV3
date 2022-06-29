namespace System.Linq.Expressions
{
    internal sealed class UnaryExpressionInterpreter : IExpressionInterpreter
    {
        public Type Type { get; } = typeof(UnaryExpression);

        public IEnumerable<ExpressionBearer>? Read(ExpressionBearer bearer, ExpressionContext context)
        {
            var expressions = new List<ExpressionBearer>();
            if (bearer.Expression is UnaryExpression unaryExpression)
            {
                if (unaryExpression.Operand != null && unaryExpression.Operand is MemberExpression memberExpression)
                {
                    if (!context.IsAnArgument(memberExpression.Member.DeclaringType))
                        context.CompileAndReplace(unaryExpression);
                    else
                        context.DirectReplace(unaryExpression.ToString(), memberExpression.ToString());
                }
                else if (unaryExpression.Operand != null)
                    expressions.Add(new(unaryExpression.Operand));
            }
            return expressions;
        }
    }
}