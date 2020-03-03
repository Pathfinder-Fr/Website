using System;
using NUnit.Framework;

namespace ScrewTurn.Wiki.SearchEngine.Tests
{
    [TestFixture]
    public class InMemoryIndexBaseTests : IndexBaseTests
    {
        /// <summary>
        ///     Gets the instance of the index to test.
        /// </summary>
        /// <returns>The instance of the index.</returns>
        protected override IIndex GetIndex()
        {
            return MockInMemoryIndex();
        }

        [Test]
        public void InitializeData()
        {
            var index = (IInMemoryIndex) GetIndex();

            var d = MockDocument("doc", "Document", "doc", DateTime.Now);
            DumpedDocument[] documents = {new DumpedDocument(d)};

            DumpedWord[] words =
            {
                new DumpedWord(new Word(1, "document")),
                new DumpedWord(new Word(2, "this")),
                new DumpedWord(new Word(3, "is")),
                new DumpedWord(new Word(4, "some")),
                new DumpedWord(new Word(5, "content"))
            };

            DumpedWordMapping[] mappings =
            {
                new DumpedWordMapping(words[0].ID, documents[0].ID, new BasicWordInfo(0, 0, WordLocation.Title)),
                new DumpedWordMapping(words[1].ID, documents[0].ID, new BasicWordInfo(0, 0, WordLocation.Content)),
                new DumpedWordMapping(words[2].ID, documents[0].ID, new BasicWordInfo(5, 1, WordLocation.Content)),
                new DumpedWordMapping(words[3].ID, documents[0].ID, new BasicWordInfo(8, 2, WordLocation.Content)),
                new DumpedWordMapping(words[4].ID, documents[0].ID, new BasicWordInfo(13, 3, WordLocation.Content))
            };

            index.SetBuildDocumentDelegate(delegate { return d; });

            index.InitializeData(documents, words, mappings);

            Assert.AreEqual(1, index.TotalDocuments, "Wrong document count");
            Assert.AreEqual(5, index.TotalWords, "Wrong word count");
            Assert.AreEqual(5, index.TotalOccurrences, "Wrong occurrence count");

            var res = index.Search(new SearchParameters("document content"));
            Assert.AreEqual(1, res.Count, "Wrong result count");
            Assert.AreEqual(2, res[0].Matches.Count, "Wrong matches count");

            Assert.AreEqual("document", res[0].Matches[0].Text, "Wrong match text");
            Assert.AreEqual(0, res[0].Matches[0].FirstCharIndex, "Wrong match first char index");
            Assert.AreEqual(0, res[0].Matches[0].WordIndex, "Wrong match word index");
            Assert.AreEqual(WordLocation.Title, res[0].Matches[0].Location, "Wrong match location");

            Assert.AreEqual("content", res[0].Matches[1].Text, "Wrong match text");
            Assert.AreEqual(13, res[0].Matches[1].FirstCharIndex, "Wrong match first char index");
            Assert.AreEqual(3, res[0].Matches[1].WordIndex, "Wrong match word index");
            Assert.AreEqual(WordLocation.Content, res[0].Matches[1].Location, "Wrong match location");
        }

        [Test]
        public void InitializeData_DocumentNotAvailable()
        {
            var index = (IInMemoryIndex) GetIndex();

            var doc = MockDocument("doc", "Document", "doc", DateTime.Now);
            var inexistent = MockDocument2("inexistent", "Inexistent", "doc", DateTime.Now);

            DumpedDocument[] documents =
            {
                new DumpedDocument(doc),
                new DumpedDocument(inexistent)
            };

            DumpedWord[] words =
            {
                new DumpedWord(new Word(1, "document")),
                new DumpedWord(new Word(2, "this")),
                new DumpedWord(new Word(3, "is")),
                new DumpedWord(new Word(4, "some")),
                new DumpedWord(new Word(5, "content")),
                new DumpedWord(new Word(6, "inexistent")),
                new DumpedWord(new Word(7, "dummy")),
                new DumpedWord(new Word(8, "text")),
                new DumpedWord(new Word(9, "used")),
                new DumpedWord(new Word(10, "for")),
                new DumpedWord(new Word(11, "testing")),
                new DumpedWord(new Word(12, "purposes"))
            };

            DumpedWordMapping[] mappings =
            {
                new DumpedWordMapping(words[0].ID, documents[0].ID, new BasicWordInfo(0, 0, WordLocation.Title)),
                new DumpedWordMapping(words[1].ID, documents[0].ID, new BasicWordInfo(0, 0, WordLocation.Content)),
                new DumpedWordMapping(words[2].ID, documents[0].ID, new BasicWordInfo(5, 1, WordLocation.Content)),
                new DumpedWordMapping(words[3].ID, documents[0].ID, new BasicWordInfo(8, 2, WordLocation.Content)),
                new DumpedWordMapping(words[4].ID, documents[0].ID, new BasicWordInfo(13, 3, WordLocation.Content)),
                new DumpedWordMapping(words[5].ID, documents[1].ID, new BasicWordInfo(0, 0, WordLocation.Title)),
                new DumpedWordMapping(words[6].ID, documents[1].ID, new BasicWordInfo(0, 0, WordLocation.Content)),
                new DumpedWordMapping(words[7].ID, documents[1].ID, new BasicWordInfo(6, 1, WordLocation.Content)),
                new DumpedWordMapping(words[8].ID, documents[1].ID, new BasicWordInfo(11, 2, WordLocation.Content)),
                new DumpedWordMapping(words[9].ID, documents[1].ID, new BasicWordInfo(16, 3, WordLocation.Content)),
                new DumpedWordMapping(words[10].ID, documents[1].ID, new BasicWordInfo(20, 4, WordLocation.Content)),
                new DumpedWordMapping(words[11].ID, documents[1].ID, new BasicWordInfo(28, 5, WordLocation.Content))
            };

            index.SetBuildDocumentDelegate(delegate(DumpedDocument d)
            {
                if (d.Name == "doc") return doc;
                return null;
            });

            index.InitializeData(documents, words, mappings);

            Assert.AreEqual(1, index.Search(new SearchParameters("this")).Count, "Wrong result count");
            Assert.AreEqual(0, index.Search(new SearchParameters("dummy")).Count, "Wrong result count");
        }
    }
}