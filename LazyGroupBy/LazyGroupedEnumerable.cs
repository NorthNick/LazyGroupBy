using System;
using System.Collections;
using System.Collections.Generic;

namespace Shastra.LazyGroupBy
{
    internal class LazyGroupedEnumerable<TSource, TKey, TElement, TResult> : IEnumerable<TResult>
    {
        private readonly IEnumerable<TSource> _source;
        private readonly Func<TSource, TKey> _keySelector;
        private readonly Func<TSource, TElement> _elementSelector;
        private readonly IEqualityComparer<TKey> _comparer;
        private readonly Func<TKey, IEnumerable<TElement>, TResult> _resultSelector;

        public LazyGroupedEnumerable(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");
            _source = source;
            _keySelector = keySelector;
            _elementSelector = elementSelector;
            _comparer = comparer ?? EqualityComparer<TKey>.Default;
            _resultSelector = resultSelector;
        }

        public IEnumerator<TResult> GetEnumerator()
        {
            var source = _source.GetEnumerator();
            // We cannot use a simple bool here because it's modified within SameKey and iterator functions cannot take ref arguments
            BoolStruct sourceIncomplete = new BoolStruct {Value = source.MoveNext()};
            while (sourceIncomplete.Value) {
                var key = _keySelector(source.Current);
                var group = new ListBackedEnumerable<TElement>(SameKey(key, source, sourceIncomplete).GetEnumerator());
                yield return _resultSelector(key, group);
                // Make sure we move on to the next key
                group.Enumerate();
            }
        }

        private IEnumerable<TElement> SameKey(TKey key, IEnumerator<TSource> source, BoolStruct sourceIncomplete)
        {
            do {
                yield return _elementSelector(source.Current);
                sourceIncomplete.Value = source.MoveNext();
            } while (sourceIncomplete.Value && _comparer.Equals(key, _keySelector(source.Current)));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class BoolStruct
        {
            public bool Value;
        }
    }
}
