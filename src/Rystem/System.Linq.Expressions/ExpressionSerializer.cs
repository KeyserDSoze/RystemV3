using System.Linq.Dynamic.Core;

namespace System.Linq.Expressions
{
    internal static class ExpressionSerializer
    {
        private static readonly List<IExpressionInterpreter> interpreters = new();
        static ExpressionSerializer()
        {
            interpreters.Add(new BinaryExpressionInterpreter());
            interpreters.Add(new ConstantExpressionInterpreter());
            interpreters.Add(new MethodCallExpressionInterpreter());
            interpreters.Add(new MemberExpressionInterpreter());
            interpreters.Add(new ParameterExpressionInterpreter());
            interpreters.Add(new UnaryExpressionInterpreter());
            interpreters.Add(new LambdaExpressionInterpreter());
        }
        public static string Serialize(Expression expression)
        {
            ExpressionContext context = new(expression);
            Serialize(context, new ExpressionBearer(expression));
            return context.ExpressionAsString;
        }
        public static Expression<Func<T, TResult>> Deserialize<T, TResult>(string expressionAsString)
            => DynamicExpressionParser.ParseLambda<T, TResult>(ParsingConfig.Default, false, expressionAsString);
        public static Expression<Func<TResult>> Deserialize<TResult>(string expressionAsString)
            => DynamicExpressionParser.ParseLambda<TResult>(ParsingConfig.Default, false, expressionAsString);
        private static void Serialize(ExpressionContext context, ExpressionBearer bearer)
        {
            IEnumerable<ExpressionBearer> expressions = ReadExpressions(bearer, context);
            foreach (var exp in expressions)
                Serialize(context, exp);
        }
        private static readonly List<ExpressionBearer> Empty = new();
        private static IEnumerable<ExpressionBearer> ReadExpressions(ExpressionBearer bearer, ExpressionContext context)
        {
            var expressionType = bearer.Expression.GetType();
            var interpreter = interpreters.FirstOrDefault(x => x.Type == expressionType);
            while (interpreter == null && expressionType!.BaseType != typeof(object))
            {
                expressionType = expressionType.BaseType;
                interpreter = interpreters.FirstOrDefault(x => x.Type == expressionType);
            }
            if (interpreter != null)
                return interpreter.Read(bearer, context) ?? Empty;
            else
                throw new ArgumentException($"{bearer.Expression.GetType().Name} is not supported.");
        }
    }
}