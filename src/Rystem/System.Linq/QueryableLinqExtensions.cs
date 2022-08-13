using System.Collections;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq
{
    public static class QueryableLinqExtensions
    {
        private static readonly ConcurrentDictionary<string, MethodInfo> Methods = new();
        public static TResult CallMethod<TSource, TResult>(this IQueryable<TSource> query, string methodName, LambdaExpression expression)
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
            var has2Arguments = methodV.GetGenericArguments().Length == 2;
            MethodInfo genericMethod = has2Arguments ?
                methodV.MakeGenericMethod(entityType, expression.ReturnType) :
                methodV.MakeGenericMethod(entityType);

            var newQuery = genericMethod
                 .Invoke(genericMethod, new object[] { query, expression });
            if (newQuery == null)
                return default!;
            if (newQuery is IConvertible)
                return (TResult)Convert.ChangeType(newQuery!, resultType);
            else
                return (TResult)newQuery!;
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
                var grouped = new Grouping<TKey, TSource>(key, items);
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
        public static object? Max<TSource>(this IQueryable<TSource> source, LambdaExpression selector)
            => source.CallMethod<TSource, object>(nameof(Max), selector);
        public static object? Min<TSource>(this IQueryable<TSource> source, LambdaExpression selector)
            => source.CallMethod<TSource, object>(nameof(Min), selector);
        public static IOrderedQueryable<TSource> OrderByDescending<TSource>(this IQueryable<TSource> source, LambdaExpression keySelector)
            => source.CallMethod<TSource, IOrderedQueryable<TSource>>(nameof(OrderByDescending), keySelector);
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, LambdaExpression keySelector)
            => source.CallMethod<TSource, IOrderedQueryable<TSource>>(nameof(OrderBy), keySelector);
        public static IQueryable<object> Select<TSource>(this IQueryable<TSource> source, LambdaExpression selector)
            => source.CallMethod<TSource, IQueryable>(nameof(Select), selector).OfType<object>();
        public static IQueryable<TResult> Select<TSource, TResult>(this IQueryable<TSource> source, LambdaExpression selector)
            => source.CallMethod<TSource, IQueryable>(nameof(Select), selector).OfType<object>().Select(x => x.Cast<TResult>());
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
