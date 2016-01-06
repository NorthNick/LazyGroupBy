using System.Collections;
using System.Collections.Generic;

namespace Shastra.LazyGroupBy
{
    internal class ListBackedEnumerable<TElement> : IEnumerable<TElement>
    {
        private readonly List<TElement> _elements = new List<TElement>();
        private readonly IEnumerator<TElement> _enumerator;
        private readonly object _lock = new object();

        private bool IsEnumerated { get; set; }

        internal ListBackedEnumerable(IEnumerator<TElement> enumerator)
        {
            _enumerator = enumerator;
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            if (IsEnumerated) {
                return _elements.GetEnumerator();
            } else {
                return new ListBackedEnumerator(this);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enumerate()
        {
            while (!IsEnumerated) {
                FetchNext(_elements.Count);
            }
        }

        private bool FetchNext(int enumeratedElements)
        {
            lock (_lock) {
                if (enumeratedElements < _elements.Count) {
                    // Although enumeratedElements < _elements.Count is always false when this is called, another thread may grab the
                    // lock and add an element first, so this needs to be rechecked.
                    return true;
                } else if (IsEnumerated) {
                    return false;
                } else {
                    IsEnumerated = _enumerator.MoveNext();
                    if (!IsEnumerated) _elements.Add(_enumerator.Current);
                    return IsEnumerated;
                }
            }
        }

        private class ListBackedEnumerator : IEnumerator<TElement>
        {
            private readonly ListBackedEnumerable<TElement> _base;
            private int _enumeratedElements = 0;

            public ListBackedEnumerator(ListBackedEnumerable<TElement> @base)
            {
                _base = @base;
            }

            public void Dispose() {}

            public bool MoveNext()
            {
                if (_enumeratedElements < _base._elements.Count || _base.FetchNext(_enumeratedElements)) {
                    Current = _base._elements[_enumeratedElements];
                    _enumeratedElements++;
                    return true;
                } else {
                    return false;
                }
            }

            public void Reset()
            {
                _enumeratedElements = 0;
            }

            public TElement Current { get; private set; }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }
    }
}
