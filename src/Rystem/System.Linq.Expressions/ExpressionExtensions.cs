using System.Reflection;

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
        public static (LambdaExpression Expression, Type Type) DeserializeAsDynamicAndRetrieveType<T>(this string expressionAsString)
            => ExpressionSerializer.DeserializeAsDynamicAndRetrieveType<T>(expressionAsString);
        private static readonly MethodInfo PreGenericInvoke = typeof(ExpressionExtensions).FetchMethods().First(x => x.Name == nameof(InvokeAsync) && x.IsGenericMethod && x.GetParameters().Any(t => t.ParameterType == typeof(Delegate)));

        public static async ValueTask InvokeAsync(this LambdaExpression lambdaExpression, params object[] args)
        {
            var compiledLambda = lambdaExpression.Compile();
            if (compiledLambda != null)
            {
                var executedLambda = compiledLambda.DynamicInvoke(args);
                if (executedLambda is Task task)
                    await task.NoContext();
                else if (executedLambda is ValueTask valueTask)
                    await valueTask.NoContext();
            }
        }
        public static ValueTask<object?> InvokeAsync(this LambdaExpression lambdaExpression, Type type, params object[] args)
            => lambdaExpression.Compile().InvokeAsync(type, args);
        public static async ValueTask<object?> InvokeAsync(this Delegate method, Type type, params object[] args)
        {
            var invokedMethod = PreGenericInvoke.MakeGenericMethod(type).Invoke(null, new object[] { method, args });
            if (invokedMethod == null)
                return null!;
            var result = await (dynamic)invokedMethod;
            return result;
        }
        public static async ValueTask<T?> InvokeAsync<T>(this Delegate method, params object[] args)
        {
            var executedLambda = method.DynamicInvoke(args);
            if (executedLambda is Task<T> task)
                return await task.NoContext();
            else if (executedLambda is ValueTask<T> valueTask)
                return await valueTask.NoContext();
            else if (executedLambda != null)
                return (T)executedLambda;
            else
                return default;
        }
        public static ValueTask<T?> InvokeAsync<T>(this LambdaExpression lambdaExpression, params object[] args) 
            => lambdaExpression.Compile().InvokeAsync<T>(args);
    }
}