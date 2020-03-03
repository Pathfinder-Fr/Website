using System;
using NUnit.Framework;

namespace ScrewTurn.Wiki.SearchEngine.Tests
{
    [TestFixture]
    public class DumpedWordTests
    {
        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void Constructor_WithParameters_InvalidText(string text)
        {
            var w = new DumpedWord(5, text);
        }

        [Test]
        public void Constructor_WithParameters()
        {
            var w = new DumpedWord(12, "word");
            Assert.AreEqual(12, w.ID, "Wrong ID");
            Assert.AreEqual("word", w.Text, "Wrong text");
        }

        [Test]
        public void Constructor_Word()
        {
            var w = new DumpedWord(new Word(23, "text"));
            Assert.AreEqual(23, w.ID, "Wrong ID");
            Assert.AreEqual("text", w.Text, "Wrong text");
        }
    }
}