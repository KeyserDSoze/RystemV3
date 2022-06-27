using System.Linq.Dynamic.Core;
using System.Reflection;

namespace System.Linq.Expressions
{
    internal interface IExpressionInterpreter
    {
        Type Type { get; }
        IEnumerable<ExpressionBearer>? Read(ExpressionBearer bearer, ExpressionContext context);
    }
    internal sealed class LambdaExpressionInterpreter : IExpressionInterpreter
    {
        public Type Type { get; } = typeof(LambdaExpression);

        public IEnumerable<ExpressionBearer>? Read(ExpressionBearer bearer, ExpressionContext context)
        {
            if (bearer.Expression is LambdaExpression lambdaExpression)
                return new List<ExpressionBearer>() { new ExpressionBearer(lambdaExpression.Body) };
            return null;
        }
    }
    internal sealed class BinaryExpressionInterpreter : IExpressionInterpreter
    {
        public Type Type { get; } = typeof(BinaryExpression);
        public IEnumerable<ExpressionBearer>? Read(ExpressionBearer bearer, ExpressionContext context)
        {
            var expressions = new List<ExpressionBearer>();
            if (bearer.Expression is BinaryExpression binaryExpression)
            {
                expressions.Add(new(binaryExpression.Left));
                expressions.Add(new(binaryExpression.Right));
            }
            return expressions;
        }
    }
    internal sealed class ConstantExpressionInterpreter : IExpressionInterpreter
    {
        public Type Type { get; } = typeof(ConstantExpression);

        public IEnumerable<ExpressionBearer>? Read(ExpressionBearer bearer, ExpressionContext context)
        {
            if (bearer.Key != null && bearer.Member != null && bearer.Expression is ConstantExpression constantExpression)
                context.ReplaceWithValue(bearer.Key,
                    (bearer.Member as FieldInfo)!.GetValue(constantExpression.Value)!);
            return null;
        }
    }
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

    internal sealed class MemberExpressionInterpreter : IExpressionInterpreter
    {
        public Type Type { get; } = typeof(MemberExpression);

        public IEnumerable<ExpressionBearer>? Read(ExpressionBearer bearer, ExpressionContext context)
        {
            var expressions = new List<ExpressionBearer>();
            if (bearer.Expression is MemberExpression memberExpression)
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
            return expressions;
        }
    }
    internal sealed class ParameterExpressionInterpreter : IExpressionInterpreter
    {
        public Type Type { get; } = typeof(ParameterExpression);

        public IEnumerable<ExpressionBearer>? Read(ExpressionBearer bearer, ExpressionContext context)
        {
            //if (parameterExpression.IsByRef)
            //{
            //}
            return null;
        }
    }
    internal sealed class ExpressionSerializer
    {
        private readonly List<IExpressionInterpreter> interpreters = new();
        public static ExpressionSerializer Instance { get; } = new();
        private ExpressionSerializer()
        {
            interpreters.Add(new BinaryExpressionInterpreter());
            interpreters.Add(new ConstantExpressionInterpreter());
            interpreters.Add(new MethodCallExpressionInterpreter());
            interpreters.Add(new MemberExpressionInterpreter());
            interpreters.Add(new ParameterExpressionInterpreter());
            interpreters.Add(new LambdaExpressionInterpreter());
        }
        public string Serialize(Expression expression)
        {
            ExpressionContext context = new(expression.ToString());
            Serialize(context, new ExpressionBearer(expression));
            return context.ExpressionAsString;
        }
        public Expression<Func<T, TResult>> Deserialize<T, TResult>(string expressionAsString)
            => DynamicExpressionParser.ParseLambda<T, TResult>(ParsingConfig.Default, false, expressionAsString);
        public Expression<Func<TResult>> Deserialize<TResult>(string expressionAsString)
            => DynamicExpressionParser.ParseLambda<TResult>(ParsingConfig.Default, false, expressionAsString);
        private void Serialize(ExpressionContext context, ExpressionBearer bearer)
        {
            IEnumerable<ExpressionBearer> expressions = ReadExpressions(bearer, context);
            foreach (var exp in expressions)
                Serialize(context, exp);
        }
        private readonly List<ExpressionBearer> Empty = new();
        private IEnumerable<ExpressionBearer> ReadExpressions(ExpressionBearer bearer, ExpressionContext context)
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