using System;
using System.Collections;
using System.Collections.Generic;

namespace Shastra.LazyGroupBy
{
    internal class LazyGroupedEnumerable<TSource, TKey, TElement, TResult> : IEnumerable<TResult>
    {
        private readonly IEnumerator<TSource> _source;
        private readonly Func<TSource, TKey> _keySelector;
        private readonly Func<TSource, TElement> _elementSelector;
        private readonly IEqualityComparer<TKey> _comparer;
        private readonly Func<TKey, IEnumerable<TElement>, TResult> _resultSelector;
        private bool _sourceIncomplete;

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
            _source = source.GetEnumerator();
            _keySelector = keySelector;
            _elementSelector = elementSelector;
            _comparer = comparer ?? EqualityComparer<TKey>.Default;
            _resultSelector = resultSelector;
        }

        public IEnumerator<TResult> GetEnumerator()
        {
            _sourceIncomplete = _source.MoveNext();
            while (_sourceIncomplete) {
                var key = _keySelector(_source.Current);
                var group = new ListBackedEnumerable<TElement>(SameKey(key).GetEnumerator());
                yield return _resultSelector(key, group);
                // Make sure we move on to the next key
                group.Enumerate();
            }
        }

        private IEnumerable<TElement> SameKey(TKey key)
        {
            do {
                yield return _elementSelector(_source.Current);
                _sourceIncomplete =_source.MoveNext();
            } while (_sourceIncomplete && _comparer.Equals(key, _keySelector(_source.Current)));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
