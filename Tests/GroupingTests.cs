using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Shastra.LazyGroupBy;

namespace Tests
{
    [TestFixture]
    public class GroupingTests
    {
        [Test]
        public void EmptyTest()
        {
            var actual = Enumerable.Empty<int>().LazyGroupBy(x => x % 2);
            Assert.That(actual.Count(), Is.EqualTo(0));
        }

        [Test]
        public void OneGroupTest()
        {
            var contents = Enumerable.Range(1, 5);
            var actual = contents.LazyGroupBy(x => true);
            Assert.That(actual.Count(), Is.EqualTo(1));
            Assert.That(actual.First(), Is.EqualTo(contents));
        }

        [Test]
        public void TwoGroupsTest()
        {
            var actual = Enumerable.Range(1, 9).LazyGroupBy(x => x / 5);
            Assert.That(actual.Count(), Is.EqualTo(2));
            Assert.That(actual.First(), Is.EqualTo(Enumerable.Range(1, 4)));
            Assert.That(actual.Skip(1).First(), Is.EqualTo(Enumerable.Range(5, 5)));
        }

        [Test]
        public void EagerTwoGroupsTest()
        {
            var actual = Enumerable.Range(1, 9).LazyGroupBy(x => x / 5).ToList();
            Assert.That(actual.Count, Is.EqualTo(2));
            Assert.That(actual.First(), Is.EqualTo(Enumerable.Range(1, 4)));
            Assert.That(actual.Skip(1).First(), Is.EqualTo(Enumerable.Range(5, 5)));
        }

        [Test]
        public void LazyOneGroupTest()
        {
            var actual = Integers().LazyGroupBy(x => true).First().First();
            Assert.That(actual, Is.EqualTo(Integers().First()));
        }

        [Test]
        public void LazyMultiGroupTest()
        {
            var actual = Integers().LazyGroupBy(x => x / 10);
            Assert.That(actual.Skip(1).First(), Is.EqualTo(Enumerable.Range(10, 10)));
        }

        private static IEnumerable<int> Integers()
        {
            int val = 0;
            while (true) {
                yield return val;
                val++;
            }
        }
    }
}
