using NUnit.Framework;

namespace ScrewTurn.Wiki.SearchEngine.Tests
{
    [TestFixture]
    public class BasicWordInfoTests
    {
        [Test]
        public void CompareTo()
        {
            var info1 = new BasicWordInfo(0, 0, WordLocation.Content);
            var info2 = new BasicWordInfo(0, 0, WordLocation.Content);
            var info3 = new BasicWordInfo(10, 1, WordLocation.Content);
            var info4 = new BasicWordInfo(10, 1, WordLocation.Title);

            Assert.AreEqual(0, info1.CompareTo(info2), "info1 should equal info2");
            Assert.AreEqual(-1, info1.CompareTo(info3), "info1 should be smaller than info3");
            Assert.AreEqual(2, info3.CompareTo(info4), "info3 should be greater than info4");
            Assert.AreEqual(1, info1.CompareTo(null), "info1 should be greater than null");
        }

        [Test]
        public void Constructor()
        {
            var info = new BasicWordInfo(2, 0, WordLocation.Content);
            Assert.AreEqual(2, info.FirstCharIndex, "Wrong start index");
            Assert.AreEqual(0, info.WordIndex, "Wrong word index");
        }

        [Test]
        public void EqualityOperator()
        {
            var info1 = new BasicWordInfo(0, 0, WordLocation.Content);
            var info2 = new BasicWordInfo(0, 0, WordLocation.Content);
            var info3 = new BasicWordInfo(10, 1, WordLocation.Content);
            var info4 = new BasicWordInfo(10, 1, WordLocation.Title);

            Assert.IsTrue(info1 == info2, "info1 should equal info2");
            Assert.IsFalse(info1 == info3, "info1 should not equal info3");
            Assert.IsFalse(info3 == info4, "info3 should not equal info4");
            Assert.IsFalse(info1 == null, "info1 should not equal null");
        }

        [Test]
        public void Equals()
        {
            var info1 = new BasicWordInfo(0, 0, WordLocation.Content);
            var info2 = new BasicWordInfo(0, 0, WordLocation.Content);
            var info3 = new BasicWordInfo(10, 1, WordLocation.Content);
            var info4 = new BasicWordInfo(10, 1, WordLocation.Title);

            Assert.IsTrue(info1.Equals(info2), "info1 should equal info2");
            Assert.IsFalse(info1.Equals(info3), "info1 should not equal info3");
            Assert.IsFalse(info3.Equals(info4), "info3 should not equal info4");
            Assert.IsTrue(info1.Equals(info1), "info1 should equal itself");
            Assert.IsFalse(info1.Equals(null), "info1 should not equal null");
            Assert.IsFalse(info1.Equals("hello"), "info1 should not equal a string");
        }

        [Test]
        public void InequalityOperator()
        {
            var info1 = new BasicWordInfo(0, 0, WordLocation.Content);
            var info2 = new BasicWordInfo(0, 0, WordLocation.Content);
            var info3 = new BasicWordInfo(10, 1, WordLocation.Content);
            var info4 = new BasicWordInfo(10, 1, WordLocation.Title);

            Assert.IsFalse(info1 != info2, "info1 should equal info2");
            Assert.IsTrue(info1 != info3, "info1 should not equal info3");
            Assert.IsTrue(info3 != info4, "info3 should not equal info4");
            Assert.IsTrue(info1 != null, "info1 should not equal null");
        }
    }
}