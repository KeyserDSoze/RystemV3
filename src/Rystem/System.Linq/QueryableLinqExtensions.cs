using System.Collections;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Linq
{
    public static class QueryableLinqExtensions
    {
        private static readonly ConcurrentDictionary<string, MethodInfo> Methods = new();
        private static MethodInfo GetMethod(Type entityType, Type sourceType, string methodName, LambdaExpression? expression, bool isAsync)
        {
            int numberOfParameters = 2;
            if (expression == null && !isAsync)
                numberOfParameters = 1;
            else if (expression != null && isAsync)
                numberOfParameters = 3;
            var keyName = $"{methodName}_{sourceType.FullName}_{expression?.ReturnType.FullName}_{numberOfParameters}";
            if (!Methods.ContainsKey(keyName))
            {
                MethodInfo? method = null;
                foreach (var m in sourceType.FetchMethods()
                 .Where(m => m.Name == methodName && m.IsGenericMethodDefinition
                 && m.GetParameters().Length == numberOfParameters
                 && (!isAsync || (m.ReturnType.Name.StartsWith("Task") || m.ReturnType.Name.StartsWith("ValueTask")))))
                {
                    var type =
                        !isAsync ?
                        m.GetParameters().LastOrDefault()?.ParameterType.GetGenericArguments().LastOrDefault()?.GetGenericArguments().LastOrDefault()
                        : m.GetParameters()[^2]?.ParameterType.GetGenericArguments().LastOrDefault()?.GetGenericArguments().LastOrDefault();
                    if (type == null && expression != null)
                        continue;
                    if (type?.FullName == null || (expression != null && type == expression.ReturnType))
                    {
                        method = m;
                        break;
                    }
                }
                if (method == null)
                    throw new InvalidOperationException($"It's not possibile to find a method {methodName} in {sourceType.FullName}.");
                Methods.TryAdd(keyName, method);
            }
            var methodV = Methods[keyName];
            var has2Arguments = methodV.GetGenericArguments().Length == 2 && expression != null;
            MethodInfo genericMethod = has2Arguments ?
                methodV.MakeGenericMethod(entityType, expression!.ReturnType) :
                methodV.MakeGenericMethod(entityType);
            return genericMethod;
        }
        public static TResult CallMethod<TSource, TResult>(this IQueryable<TSource> query, string methodName, LambdaExpression? expression = null, Type? typeWhereToSearchTheMethod = null)
        {
            if (typeWhereToSearchTheMethod == null)
                typeWhereToSearchTheMethod = typeof(Queryable);
            var newQuery = GetMethod(typeof(TSource), typeWhereToSearchTheMethod, methodName, expression, false)
                 .Invoke(null, expression != null ? new object[] { query, expression } : new object[] { query });
            if (newQuery == null)
                return default!;
            if (newQuery is IConvertible)
                return (TResult)Convert.ChangeType(newQuery!, typeof(TResult));
            else
                return (TResult)newQuery!;
        }
        public static ValueTask<TSource> CallMethodAsync<TSource>(this IQueryable<TSource> query, string methodName, CancellationToken cancellation = default)
            => CallMethodAsync<TSource, TSource>(query, methodName, cancellation);
        public static ValueTask<TResult> CallMethodAsync<TSource, TResult>(this IQueryable<TSource> query, string methodName, CancellationToken cancellation = default)
            => CallMethodAsync<TSource, TResult>(query, methodName, null, null, cancellation);
        public static ValueTask<TSource> CallMethodAsync<TSource>(this IQueryable<TSource> query, string methodName, Type? typeWhereToSearchTheMethod = null, CancellationToken cancellation = default)
            => CallMethodAsync<TSource, TSource>(query, methodName, null, typeWhereToSearchTheMethod, cancellation);
        public static ValueTask<TResult> CallMethodAsync<TSource, TResult>(this IQueryable<TSource> query, string methodName, Type? typeWhereToSearchTheMethod = null, CancellationToken cancellation = default)
            => CallMethodAsync<TSource, TResult>(query, methodName, null, typeWhereToSearchTheMethod, cancellation);
        public static async ValueTask<TResult> CallMethodAsync<TSource, TResult>(this IQueryable<TSource> query, string methodName, LambdaExpression? expression = null, Type? typeWhereToSearchTheMethod = null, CancellationToken cancellation = default)
        {
            if (typeWhereToSearchTheMethod == null)
                typeWhereToSearchTheMethod = typeof(Queryable);
            var newQuery = GetMethod(typeof(TSource), typeWhereToSearchTheMethod, methodName, expression, true)
                 .Invoke(null, expression != null ? new object[] { query, expression, cancellation } : new object[] { query, cancellation })!;
            object? result = null;
            if (newQuery is Task<TResult> task)
            {
                await task;
                result = task.Result;
            }
            else if (newQuery is ValueTask<TResult> valueTask)
            {
                await valueTask!;
                result = valueTask!.Result;
            }
            return result.Cast<TResult>()!;
        }
        public static decimal Average<TSource>(this IQueryable<TSource> source, LambdaExpression selector)
            => source.CallMethod<TSource, decimal>(nameof(Average), selector);
        public static int Count<TSource>(this IQueryable<TSource> source, LambdaExpression predicate)
            => source.CallMethod<TSource, int>(nameof(Count), predicate);
        public static IQueryable<TSource> DistinctBy<TSource>(this IQueryable<TSource> source, LambdaExpression keySelector)
            => source.CallMethod<TSource, IQueryable<TSource>>(nameof(DistinctBy), keySelector);
        public static IQueryable<IGrouping<object, TSource>> GroupBy<TSource>(this IQueryable<TSource> source, LambdaExpression keySelector)
            => source.GroupByAsEnumerable<object, TSource>(keySelector).AsQueryable();
        public static IQueryable<IGrouping<TKey, TSource>> GroupBy<TKey, TSource>(this IQueryable<TSource> source, LambdaExpression keySelector)
            => source.GroupByAsEnumerable<TKey, TSource>(keySelector).AsQueryable();
        private static IEnumerable<IGrouping<TKey, TSource>> GroupByAsEnumerable<TKey, TSource>(this IQueryable<TSource> source, LambdaExpression keySelector)
        {
            PropertyInfo? property = null;
            foreach (var item in source.CallMethod<TSource, dynamic>(nameof(GroupBy), keySelector))
            {
                if (property == null)
                {
                    Type type = item.GetType();
                    property = type.FetchProperties().First(x => x.Name == "Key");
                }
                var key = ((object)property.GetValue(item)).Cast<TKey>();
                var items = GetEnumerable();
                var grouped = new Grouping<TKey, TSource>(key!, items);
                yield return grouped;

                IEnumerable<TSource> GetEnumerable()
                {
                    foreach (var it in item)
                        yield return (TSource)it;
                }
            }
        }
        private sealed class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
        {
            private readonly IEnumerable<TElement> _enumerable;
            public Grouping(TKey key, IEnumerable<TElement> enumerable)
            {
                Key = key;
                _enumerable = enumerable;
            }
            public TKey Key { get; }

            public IEnumerator<TElement> GetEnumerator()
                => _enumerable.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();
        }
        public static long LongCount<TSource>(this IQueryable<TSource> source, LambdaExpression predicate)
            => source.CallMethod<TSource, long>(nameof(LongCount), predicate);
        public static dynamic? Max<TSource>(this IQueryable<TSource> source, LambdaExpression selector)
            => source.CallMethod<TSource, dynamic>(nameof(Max), selector);
        public static dynamic? Min<TSource>(this IQueryable<TSource> source, LambdaExpression selector)
            => source.CallMethod<TSource, dynamic>(nameof(Min), selector);
        public static IOrderedQueryable<TSource> OrderByDescending<TSource>(this IQueryable<TSource> source, LambdaExpression keySelector)
            => source.CallMethod<TSource, IOrderedQueryable<TSource>>(nameof(OrderByDescending), keySelector);
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, LambdaExpression keySelector)
            => source.CallMethod<TSource, IOrderedQueryable<TSource>>(nameof(OrderBy), keySelector);
        public static IQueryable<dynamic> Select<TSource>(this IQueryable<TSource> source, LambdaExpression selector)
        {
            selector = Expression.Lambda(
                        Expression.Convert(selector.Body, typeof(object)),
                        selector.Parameters);
            var value = Generics
                .WithStatic(typeof(QueryableLinqExtensions), nameof(Select), typeof(TSource), selector.ReturnType)
                .Invoke(source, selector) as IQueryable;
            return value!.OfType<dynamic>();
        }
        public static IQueryable<TResult> Select<TSource, TResult>(this IQueryable<TSource> source, LambdaExpression selector)
            => source.CallMethod<TSource, IQueryable<TResult>>(nameof(Select), selector);
        public static decimal Sum<TSource>(this IQueryable<TSource> source, LambdaExpression selector)
            => source.CallMethod<TSource, decimal>(nameof(Sum), selector);
        public static IOrderedQueryable<TSource> ThenByDescending<TSource>(this IOrderedQueryable<TSource> source, LambdaExpression keySelector)
            => source.CallMethod<TSource, IOrderedQueryable<TSource>>(nameof(ThenByDescending), keySelector);
        public static IOrderedQueryable<TSource> ThenBy<TSource>(this IOrderedQueryable<TSource> source, LambdaExpression keySelector)
            => source.CallMethod<TSource, IOrderedQueryable<TSource>>(nameof(ThenBy), keySelector);
        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, LambdaExpression predicate)
            => source.CallMethod<TSource, IQueryable<TSource>>(nameof(Where), predicate);
    }
}
