using System;
using NUnit.Framework;

namespace ScrewTurn.Wiki.SearchEngine.Tests
{
    [TestFixture]
    public class DumpedDocumentTests : TestsBase
    {
        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void Constructor_WithParameters_InvalidName(string name)
        {
            var ddoc = new DumpedDocument(10, name, "Title", "doc", DateTime.Now);
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void Constructor_WithParameters_InvalidTitle(string title)
        {
            var ddoc = new DumpedDocument(1, "name", title, "doc", DateTime.Now);
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void Constructor_WithParameters_InvalidTypeTag(string typeTag)
        {
            var ddoc = new DumpedDocument(1, "name", "Title", typeTag, DateTime.Now);
        }

        [Test]
        public void Constructor_WithDocument()
        {
            var doc = MockDocument("name", "Title", "doc", DateTime.Now);
            var ddoc = new DumpedDocument(doc);

            Assert.AreEqual(doc.ID, ddoc.ID, "Wrong ID");
            Assert.AreEqual("name", ddoc.Name, "Wrong name");
            Assert.AreEqual("Title", ddoc.Title, "Wrong title");
            Assert.AreEqual(doc.DateTime, ddoc.DateTime, "Wrong date/time");
        }

        [Test]
        public void Constructor_WithParameters()
        {
            var doc = MockDocument("name", "Title", "doc", DateTime.Now);
            var ddoc = new DumpedDocument(doc.ID, doc.Name, doc.Title, doc.TypeTag, doc.DateTime);

            Assert.AreEqual(doc.ID, ddoc.ID, "Wrong ID");
            Assert.AreEqual("name", ddoc.Name, "Wrong name");
            Assert.AreEqual("Title", ddoc.Title, "Wrong title");
            Assert.AreEqual(doc.DateTime, ddoc.DateTime, "Wrong date/time");
        }
    }
}