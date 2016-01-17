using System;
using System.Collections;
using System.Collections.Generic;

namespace Shastra.LazyGroupBy
{
    internal class ListBackedEnumerable<TElement> : IEnumerable<TElement>
    {
        private readonly List<TElement> _elements = new List<TElement>();
        private readonly IEnumerator<TElement> _enumerator;
        private readonly object _lock = new object();

        private bool Incomplete { get; set; }

        internal ListBackedEnumerable(IEnumerator<TElement> enumerator)
        {
            _enumerator = enumerator;
            Incomplete = true;
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return Incomplete ? (IEnumerator<TElement>) new ListBackedEnumerator(FetchItem) : _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enumerate()
        {
            var dummy = default(TElement);
            while (Incomplete) {
                FetchItem(_elements.Count, ref dummy);
            }
        }

        private delegate bool FetchItemDelegate(int index, ref TElement value);

        private bool FetchItem(int index, ref TElement value)
        {
            // Another thread may modify the variables being tested can change during the test, but not in a way that
            // leads to incorrect results.
            if (index >= _elements.Count && Incomplete) {
                lock (_lock) {
                    // Need double-checking here in case another thread grabs the lock after the first check.
                    if (index >= _elements.Count && Incomplete) {
                        Incomplete = _enumerator.MoveNext();
                        if (Incomplete) _elements.Add(_enumerator.Current);
                    }
                }
            }
            
            if (index < _elements.Count) {
                value = _elements[index];
                return true;
            } else {
                return false;
            }
        }

        private class ListBackedEnumerator : IEnumerator<TElement>
        {
            private readonly FetchItemDelegate _fetcher;
            private int _nextIndex = 0;
            private TElement _current;

            public ListBackedEnumerator(FetchItemDelegate fetcher)
            {
                _fetcher = fetcher;
            }

            public void Dispose() {}

            public bool MoveNext()
            {
                if (_fetcher(_nextIndex, ref _current)) {
                    _nextIndex++;
                    return true;
                } else {
                    return false;
                }
            }

            public void Reset()
            {
                _nextIndex = 0;
            }

            public TElement Current { get { return _current; } }

            object IEnumerator.Current { get { return _current; } }
        }
    }
}
