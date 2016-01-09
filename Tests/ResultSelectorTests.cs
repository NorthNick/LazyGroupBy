using NUnit.Framework;
using Shastra.LazyGroupBy;
using System.Linq;

namespace Tests
{
    [TestFixture]
    public class ResultSelectorTests
    {
        [Test]
        public void EmptySelectorTest()
        {
            var actual = Enumerable.Empty<int>().LazyGroupBy(x => true, (b, xs) => xs.Count());
            Assert.That(actual.Count(), Is.EqualTo(0));
        }

        [Test]
        public void OneGroupSelectorTest()
        {
            var actual = Enumerable.Range(1, 10).LazyGroupBy(x => true, (b, xs) => xs.Count());
            Assert.That(actual, Is.EqualTo(new[] {10}));
        }
    }
}
