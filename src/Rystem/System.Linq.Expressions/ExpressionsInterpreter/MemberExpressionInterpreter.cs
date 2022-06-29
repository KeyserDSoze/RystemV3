namespace System.Linq.Expressions
{
    internal sealed class MemberExpressionInterpreter : IExpressionInterpreter
    {
        public Type Type { get; } = typeof(MemberExpression);

        public IEnumerable<ExpressionBearer>? Read(ExpressionBearer bearer, ExpressionContext context)
        {
            var expressions = new List<ExpressionBearer>();
            if (bearer.Expression is MemberExpression memberExpression)
            {
                if (!context.IsAnArgument(memberExpression.Member.DeclaringType))
                {
                    if (memberExpression.Expression == null)
                        context.CompileAndReplace(memberExpression);
                    else
                        expressions.Add(new(memberExpression.Expression)
                        {
                            Key = memberExpression.ToString(),
                            Member = memberExpression.Member,
                        });
                }                
            }
            return expressions;
        }
    }
}