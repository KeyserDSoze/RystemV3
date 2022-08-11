using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq
{
    public static class QueryableLinqExtensions
    {
        private static readonly ConcurrentDictionary<string, MethodInfo> Methods = new();
        public static TResult CallMethod<TSource, TResult>(string methodName, IQueryable<TSource> query,
            LambdaExpression expression, bool checkReturnTypeOfFunction)
        {
            var resultType = typeof(TResult);
            var keyName = $"{methodName}_{expression.ReturnType.FullName}";
            if (!Methods.ContainsKey(keyName))
            {
                MethodInfo? method = null;
                foreach (var m in typeof(Queryable).FetchMethods()
                 .Where(m => m.Name == methodName && m.IsGenericMethodDefinition
                 && m.GetParameters().Length == 2))
                {
                    var type = m.GetParameters().LastOrDefault()?.ParameterType.GetGenericArguments().LastOrDefault()?.GetGenericArguments().LastOrDefault();
                    if (type == null)
                        continue;
                    if (type.FullName == null || type == expression.ReturnType)
                    {
                        method = m;
                        break;
                    }
                }
                if (method == null)
                    throw new InvalidOperationException($"It's not possibile to use lambda expressions with return type {expression.ReturnType.FullName} in method {methodName}.");
                Methods.TryAdd(keyName, method);
            }
            var entityType = typeof(TSource);
            var methodV = Methods[keyName];
            MethodInfo genericMethod = methodV.GetGenericArguments().Length == 2 ?
                methodV.MakeGenericMethod(entityType, expression.ReturnType) :
                methodV.MakeGenericMethod(entityType);

            var newQuery = genericMethod
                 .Invoke(genericMethod, new object[] { query, expression });
            return checkReturnTypeOfFunction ? (TResult)Convert.ChangeType(newQuery!, resultType) : (TResult)newQuery!;
        }
        public static decimal Average<TSource>(this IQueryable<TSource> source, LambdaExpression selector)
            => CallMethod<TSource, decimal>(nameof(Average), source, selector, true);

        public static int Count<TSource>(this IQueryable<TSource> source, LambdaExpression predicate)
            => CallMethod<TSource, int>(nameof(Count), source, predicate, true);

        public static IQueryable<TSource> DistinctBy<TSource>(this IQueryable<TSource> source, LambdaExpression keySelector)
            => CallMethod<TSource, IQueryable<TSource>>(nameof(DistinctBy), source, keySelector, false);
        public static IQueryable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IQueryable<TSource> source, LambdaExpression keySelector)
        {
            throw new NotImplementedException();
        }
        public static long LongCount<TSource>(this IQueryable<TSource> source, LambdaExpression predicate)
            => CallMethod<TSource, long>(nameof(LongCount), source, predicate, true);

        public static object? Max<TSource>(this IQueryable<TSource> source, LambdaExpression selector)
            => CallMethod<TSource, object>(nameof(Max), source, selector, false);
        public static object? Min<TSource>(this IQueryable<TSource> source, LambdaExpression selector)
            => CallMethod<TSource, object>(nameof(Min), source, selector, false);
        public static IOrderedQueryable<TSource> OrderByDescending<TSource>(this IQueryable<TSource> source, LambdaExpression keySelector)
            => CallMethod<TSource, IOrderedQueryable<TSource>>(nameof(OrderByDescending), source, keySelector, false);

        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, LambdaExpression keySelector)
            => CallMethod<TSource, IOrderedQueryable<TSource>>(nameof(OrderBy), source, keySelector, false);

        public static IQueryable<TResult> Select<TSource, TResult>(this IQueryable<TSource> source, LambdaExpression selector)
        {
            throw new NotImplementedException();
        }
        public static decimal Sum<TSource>(this IQueryable<TSource> source, LambdaExpression selector)
            => CallMethod<TSource, decimal>(nameof(Sum), source, selector, true);
        public static IOrderedQueryable<TSource> ThenByDescending<TSource>(this IOrderedQueryable<TSource> source, LambdaExpression keySelector)
            => CallMethod<TSource, IOrderedQueryable<TSource>>(nameof(ThenByDescending), source, keySelector, false);

        public static IOrderedQueryable<TSource> ThenBy<TSource>(this IOrderedQueryable<TSource> source, LambdaExpression keySelector)
            => CallMethod<TSource, IOrderedQueryable<TSource>>(nameof(ThenBy), source, keySelector, false);
        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, LambdaExpression predicate)
            => CallMethod<TSource, IQueryable<TSource>>(nameof(Where), source, predicate, false);
    }
}
