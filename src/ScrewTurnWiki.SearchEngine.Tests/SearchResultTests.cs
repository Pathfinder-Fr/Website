using System;
using NUnit.Framework;

namespace ScrewTurn.Wiki.SearchEngine.Tests
{
    [TestFixture]
    public class SearchResultTests : TestsBase
    {
        [Test]
        public void Constructor()
        {
            var doc = MockDocument("Doc", "Document", "ptdoc", DateTime.Now);
            var res = new SearchResult(doc);

            Assert.AreEqual(doc, res.Document, "Wrong document");
            Assert.AreEqual(0, res.Relevance.Value, "Wrong initial relevance value");
            Assert.IsFalse(res.Relevance.IsFinalized, "Initial relevance value should not be finalized");
        }
    }
}