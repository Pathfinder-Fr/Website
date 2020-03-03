using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace ScrewTurn.Wiki.SearchEngine.Tests
{
    [TestFixture]
    public class DumpedChangeTests : TestsBase
    {
        [Test]
        public void Constructor()
        {
            var change = new DumpedChange(
                new DumpedDocument(MockDocument("doc", "Docum", "d", DateTime.Now)),
                new List<DumpedWord>(new[] {new DumpedWord(1, "word")}),
                new List<DumpedWordMapping>(new[] {new DumpedWordMapping(1, 1, 4, 1, WordLocation.Content.Location)}));

            Assert.AreEqual("doc", change.Document.Name, "Wrong name");
            Assert.AreEqual(1, change.Words.Count, "Wrong words count");
            Assert.AreEqual("word", change.Words[0].Text, "Wrong word");
            Assert.AreEqual(1, change.Mappings.Count, "Wrong mappings count");
            Assert.AreEqual(1, change.Mappings[0].WordID, "Wrong word index");
        }

        [Test]
        public void Constructor_EmptyMappings()
        {
            // Mappings can be empty
            var change = new DumpedChange(
                new DumpedDocument(MockDocument("doc", "Docum", "d", DateTime.Now)),
                new List<DumpedWord>(new[] {new DumpedWord(1, "word")}),
                new List<DumpedWordMapping>());
        }

        [Test]
        public void Constructor_EmptyWords()
        {
            // Words can be empty
            var change = new DumpedChange(
                new DumpedDocument(MockDocument("doc", "Docum", "d", DateTime.Now)),
                new List<DumpedWord>(),
                new List<DumpedWordMapping>(new[] {new DumpedWordMapping(1, 1, 4, 1, WordLocation.Content.Location)}));
        }
    }
}