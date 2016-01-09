using NUnit.Framework;
using Shastra.LazyGroupBy;
using System.Linq;

namespace Tests
{
    [TestFixture]
    public class ElementSelectorTests
    {
        [Test]
        public void EmptySelectorTest()
        {
            var actual = Enumerable.Empty<string>().LazyGroupBy(x => x.Length > 12, x => x.Substring(0, 3));
            Assert.That(actual.Count(), Is.EqualTo(0));
        }

        [Test]
        public void SingleGroupSelectorTest()
        {
            var actual = Enumerable.Range(1, 5).LazyGroupBy(x => true, x => x.ToString());
            Assert.That(actual.Count(), Is.EqualTo(1));
            Assert.That(actual.First(), Is.EqualTo(Enumerable.Range(1, 5).Select(x => x.ToString())));
        }

        [Test]
        public void MultiGroupSelectorTest()
        {
            var actual = Enumerable.Range(10, 20).LazyGroupBy(x => x / 8, x => 7);
            Assert.That(actual.Count(), Is.EqualTo(3));
            Assert.That(actual.First(), Is.EqualTo(new[] {7, 7, 7, 7, 7, 7}));
        }
    }
}
