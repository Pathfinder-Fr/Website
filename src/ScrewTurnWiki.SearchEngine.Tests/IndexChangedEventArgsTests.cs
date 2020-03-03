using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace ScrewTurn.Wiki.SearchEngine.Tests
{
    [TestFixture]
    public class IndexChangedEventArgsTests : TestsBase
    {
        [Test]
        public void Constructor()
        {
            var doc = MockDocument("Doc", "Document", "ptdoc", DateTime.Now);
            var change = new DumpedChange(new DumpedDocument(doc), new List<DumpedWord>(),
                new List<DumpedWordMapping>(new[] {new DumpedWordMapping(1, 1, 1, 1, 1)}));

            var args = new IndexChangedEventArgs(doc, IndexChangeType.DocumentAdded, change, null);

            Assert.AreSame(doc, args.Document, "Invalid document instance");
            Assert.AreEqual(IndexChangeType.DocumentAdded, args.Change, "Wrong change");
        }

        [Test]
        public void Constructor_IndexCleared()
        {
            var args = new IndexChangedEventArgs(null, IndexChangeType.IndexCleared, null, null);
        }
    }
}