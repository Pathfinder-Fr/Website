using System;
using NUnit.Framework;

namespace ScrewTurn.Wiki.SearchEngine.Tests
{
    [TestFixture]
    public class SearchParametersTests
    {
        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void Constructor_QueryOnly_InvalidQuery(string q)
        {
            var par = new SearchParameters(q);
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void Constructor_QueryDocumentTypeTags_InvalidQuery(string q)
        {
            var par = new SearchParameters(q, "blah", "doc");
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void Constructor_QueryDocumentTypeTags_InvalidDocumentTypeTagsElement(string e)
        {
            var par = new SearchParameters("query", "blah", e);
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void Constructor_QueryOptions_InvalidQuery(string q)
        {
            var par = new SearchParameters(q, SearchOptions.ExactPhrase);
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void Constructor_Full_InvalidQuery(string q)
        {
            var par = new SearchParameters(q, new[] {"blah", "doc"}, SearchOptions.AllWords);
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void Constructor_Full_InvalidDocumentTypeTagsElement(string e)
        {
            var par = new SearchParameters("query", new[] {"blah", e}, SearchOptions.ExactPhrase);
        }

        [Test]
        public void Constructor_Full()
        {
            var par = new SearchParameters("query", new[] {"blah", "doc"}, SearchOptions.AllWords);
            Assert.AreEqual("query", par.Query, "Wrong query");
            Assert.AreEqual(2, par.DocumentTypeTags.Length, "Wrong DocumentTypeTag count");
            Assert.AreEqual("blah", par.DocumentTypeTags[0], "Wrong type tag");
            Assert.AreEqual("doc", par.DocumentTypeTags[1], "Wrong type tag");
            Assert.AreEqual(SearchOptions.AllWords, par.Options);
        }

        [Test]
        public void Constructor_Full_NullDocumentTypeTags()
        {
            var par = new SearchParameters("query", null, SearchOptions.AllWords);
            Assert.AreEqual("query", par.Query, "Wrong query");
            Assert.IsNull(par.DocumentTypeTags, "DocumentTypeTags should be null");
            Assert.AreEqual(SearchOptions.AllWords, par.Options);
        }

        [Test]
        public void Constructor_QueryDocumentTypeTags()
        {
            var par = new SearchParameters("query", "blah", "doc");
            Assert.AreEqual("query", par.Query, "Wrong query");
            Assert.AreEqual(2, par.DocumentTypeTags.Length, "Wrong DocumentTypeTag count");
            Assert.AreEqual("blah", par.DocumentTypeTags[0], "Wrong type tag");
            Assert.AreEqual("doc", par.DocumentTypeTags[1], "Wrong type tag");
            Assert.AreEqual(SearchOptions.AtLeastOneWord, par.Options);
        }

        [Test]
        public void Constructor_QueryDocumentTypeTags_NullDocumentTypeTags()
        {
            var par = new SearchParameters("query", null);
            Assert.AreEqual("query", par.Query, "Wrong query");
            Assert.IsNull(par.DocumentTypeTags, "DocumentTypeTags should be null");
            Assert.AreEqual(SearchOptions.AtLeastOneWord, par.Options);
        }

        [Test]
        public void Constructor_QueryOnly()
        {
            var par = new SearchParameters("query");
            Assert.AreEqual("query", par.Query, "Wrong query");
            Assert.IsNull(par.DocumentTypeTags, "DocumentTypeTags should be null");
            Assert.AreEqual(SearchOptions.AtLeastOneWord, par.Options);
        }

        [Test]
        public void Constructor_QueryOptions()
        {
            var par = new SearchParameters("query", SearchOptions.ExactPhrase);
            Assert.AreEqual("query", par.Query, "Wrong query");
            Assert.IsNull(par.DocumentTypeTags, "DocumentTypeTags should be null");
            Assert.AreEqual(SearchOptions.ExactPhrase, par.Options);
        }
    }
}