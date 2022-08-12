using System.Collections;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq
{
    public static class QueryableLinqExtensions
    {
        private static readonly ConcurrentDictionary<string, MethodInfo> Methods = new();
        public static TResult CallMethod<TSource, TResult>(string methodName, IQueryable<TSource> query, LambdaExpression expression)
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
            => CallMethod<TSource, decimal>(nameof(Average), source, selector);
        public static int Count<TSource>(this IQueryable<TSource> source, LambdaExpression predicate)
            => CallMethod<TSource, int>(nameof(Count), source, predicate);
        public static IQueryable<TSource> DistinctBy<TSource>(this IQueryable<TSource> source, LambdaExpression keySelector)
            => CallMethod<TSource, IQueryable<TSource>>(nameof(DistinctBy), source, keySelector);
        public static IQueryable<IGrouping<object, TSource>> GroupBy<TSource>(this IQueryable<TSource> source, LambdaExpression keySelector)
            => source.GroupByAsEnumerable(keySelector).AsQueryable();
        private static IEnumerable<IGrouping<object, TSource>> GroupByAsEnumerable<TSource>(this IQueryable<TSource> source, LambdaExpression keySelector)
        {
            PropertyInfo? property = null;
            foreach (var item in CallMethod<TSource, dynamic>(nameof(GroupBy), source, keySelector))
            {
                if (property == null)
                {
                    Type type = item.GetType();
                    property = type.FetchProperties().First(x => x.Name == "Key");
                }
                var key = (object)property.GetValue(item);
                var items = GetEnumerable();
                var grouped = new Grouping<dynamic, TSource>(key, items);
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
            => CallMethod<TSource, long>(nameof(LongCount), source, predicate);
        public static object? Max<TSource>(this IQueryable<TSource> source, LambdaExpression selector)
            => CallMethod<TSource, object>(nameof(Max), source, selector);
        public static object? Min<TSource>(this IQueryable<TSource> source, LambdaExpression selector)
            => CallMethod<TSource, object>(nameof(Min), source, selector);
        public static IOrderedQueryable<TSource> OrderByDescending<TSource>(this IQueryable<TSource> source, LambdaExpression keySelector)
            => CallMethod<TSource, IOrderedQueryable<TSource>>(nameof(OrderByDescending), source, keySelector);
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, LambdaExpression keySelector)
            => CallMethod<TSource, IOrderedQueryable<TSource>>(nameof(OrderBy), source, keySelector);
        public static IQueryable<object> Select<TSource>(this IQueryable<TSource> source, LambdaExpression selector)
            => CallMethod<TSource, IQueryable>(nameof(Select), source, selector).OfType<object>();
        public static decimal Sum<TSource>(this IQueryable<TSource> source, LambdaExpression selector)
            => CallMethod<TSource, decimal>(nameof(Sum), source, selector);
        public static IOrderedQueryable<TSource> ThenByDescending<TSource>(this IOrderedQueryable<TSource> source, LambdaExpression keySelector)
            => CallMethod<TSource, IOrderedQueryable<TSource>>(nameof(ThenByDescending), source, keySelector);
        public static IOrderedQueryable<TSource> ThenBy<TSource>(this IOrderedQueryable<TSource> source, LambdaExpression keySelector)
            => CallMethod<TSource, IOrderedQueryable<TSource>>(nameof(ThenBy), source, keySelector);
        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, LambdaExpression predicate)
            => CallMethod<TSource, IQueryable<TSource>>(nameof(Where), source, predicate);
    }
}
