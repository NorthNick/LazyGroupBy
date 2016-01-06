using System;
using System.Collections.Generic;
using System.Linq;

namespace Shastra.LazyGroupBy
{
    public static class LazyGroupByExtensions
    {

        public static IEnumerable<IGrouping<TKey, TSource>> LazyGroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return new LazyGroupedEnumerable<TSource, TKey, TSource, IGrouping<TKey, TSource>>(source, keySelector, IdentityFunction<TSource>.Instance, DefaultResultSelector, null);
        }

        public static IEnumerable<IGrouping<TKey, TSource>> LazyGroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new LazyGroupedEnumerable<TSource, TKey, TSource, IGrouping<TKey, TSource>>(source, keySelector, IdentityFunction<TSource>.Instance, DefaultResultSelector, comparer);
        }

        public static IEnumerable<IGrouping<TKey, TElement>> LazyGroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            return new LazyGroupedEnumerable<TSource, TKey, TElement, IGrouping<TKey, TElement>>(source, keySelector, elementSelector, DefaultResultSelector, null);
        }

        public static IEnumerable<IGrouping<TKey, TElement>> LazyGroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            return new LazyGroupedEnumerable<TSource, TKey, TElement, IGrouping<TKey, TElement>>(source, keySelector, elementSelector, DefaultResultSelector, comparer);
        }
        
        public static IEnumerable<TResult> LazyGroupBy<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector)
        {
            return new LazyGroupedEnumerable<TSource, TKey, TSource, TResult>(source, keySelector, IdentityFunction<TSource>.Instance, resultSelector, null);
        }

        public static IEnumerable<TResult> LazyGroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
        {
            return new LazyGroupedEnumerable<TSource, TKey, TElement, TResult>(source, keySelector, elementSelector, resultSelector, null);
        }

        public static IEnumerable<TResult> LazyGroupBy<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            return new LazyGroupedEnumerable<TSource, TKey, TSource, TResult>(source, keySelector, IdentityFunction<TSource>.Instance, resultSelector, comparer);
        }

        public static IEnumerable<TResult> LazyGroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            return new LazyGroupedEnumerable<TSource, TKey, TElement, TResult>(source, keySelector, elementSelector, resultSelector, comparer);
        }

        private static IGrouping<TKey, TElement> DefaultResultSelector<TKey, TElement>(TKey key, IEnumerable<TElement> elements)
        {
            return new Grouping<TKey, TElement>(key, elements);
        }
    }
}
