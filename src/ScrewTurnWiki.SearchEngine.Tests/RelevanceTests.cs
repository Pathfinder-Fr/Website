using System;
using NUnit.Framework;

namespace ScrewTurn.Wiki.SearchEngine.Tests
{
    [TestFixture]
    public class RelevanceTests
    {
        [Test]
        public void Constructor()
        {
            var rel = new Relevance();
            Assert.AreEqual(0, rel.Value, "Wrong value");
            Assert.IsFalse(rel.IsFinalized, "Value should not be finalized");
        }

        [Test]
        public void Constructor_WithValue()
        {
            var rel = new Relevance(5);
            Assert.AreEqual(5, rel.Value, "Wrong value");
            Assert.IsFalse(rel.IsFinalized, "Value should not be finalized");
        }

        [Test]
        // Underscore to avoid interference with Destructor
        public void Finalize_()
        {
            var rel = new Relevance();
            rel.SetValue(12);
            Assert.AreEqual(12, rel.Value, "Wrong value");
            rel.Finalize(24);
            Assert.AreEqual(50, rel.Value, "Wrong finalized value");
            Assert.IsTrue(rel.IsFinalized, "Value should be finalized");
        }


        [Test]
        public void NormalizeAfterFinalization()
        {
            var rel = new Relevance(8);
            rel.Finalize(16);
            rel.NormalizeAfterFinalization(0.5F);
            Assert.AreEqual(25, rel.Value, 0.1, "Wrong value");
        }

        [Test]
        public void SetValue()
        {
            var rel = new Relevance();
            rel.SetValue(8);
            Assert.AreEqual(8, rel.Value, "Wrong value");
            Assert.IsFalse(rel.IsFinalized, "Value should not be finalized");
            rel.SetValue(14);
            Assert.AreEqual(14, rel.Value, "Wrong value");
            Assert.IsFalse(rel.IsFinalized, "Value should not be finalized");
        }
    }
}