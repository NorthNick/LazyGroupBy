using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Shastra.LazyGroupBy;

namespace Tests
{
    [TestFixture]
    public class GroupingEnumerableTests
    {
        [Test]
        public void MultipleIteratorsTest()
        {
            var gp = Enumerable.Range(1, 100).LazyGroupBy(x => x / 10).First();
            var iters = new IEnumerator<int>[2];
            iters[0] = gp.GetEnumerator();
            iters[1] = gp.GetEnumerator();
            // Check that independent MoveNext calls give the same result
            int current = 1;
            iters[0].MoveNext();
            Assert.That(iters[0].Current, Is.EqualTo(current));
            iters[1].MoveNext();
            Assert.That(iters[1].Current, Is.EqualTo(current));
            // Fully enumerate iters[0] then iters[1]
            foreach (var iter in iters) {
                current = 1;
                while (iter.MoveNext()) {
                    current++;
                    Assert.That(iter.Current, Is.EqualTo(current));
                }
            }
            Assert.That(current, Is.EqualTo(9));
        }

        [Test]
        public void FullyEnumerateThenIterateTest()
        {
            var gp = Enumerable.Range(1, 100).LazyGroupBy(x => x / 10).First();
            var iter = gp.GetEnumerator();
            // Fully enumerate the group
            while (iter.MoveNext()) {}
            // Check that then iterating behaves as expected
            iter = gp.GetEnumerator();
            int current = 0;
            while (iter.MoveNext()) {
                current++;
                Assert.That(iter.Current, Is.EqualTo(current));
            }
            Assert.That(current, Is.EqualTo(9));
        }
    }
}
