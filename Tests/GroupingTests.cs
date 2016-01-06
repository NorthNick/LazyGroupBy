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
    }
}
