using System;
using NUnit.Framework;

namespace ScrewTurn.Wiki.SearchEngine.Tests
{
    [TestFixture]
    public class SearchResultCollectionTests : TestsBase
    {
        [Test]
        public void AddAndCount()
        {
            var collection = new SearchResultCollection();

            Assert.AreEqual(0, collection.Count);

            var res = new SearchResult(MockDocument("d", "d", "d", DateTime.Now));
            var res2 = new SearchResult(MockDocument("d2", "d", "d", DateTime.Now));

            collection.Add(res);
            collection.Add(res2);
            Assert.AreEqual(2, collection.Count, "Wrong count (collection should contain 2 items)");
            Assert.AreEqual(res, collection[0], "Wrong item at index 0");
            Assert.AreEqual(res2, collection[1], "Wrong item at index 1");
        }

        [Test]
        public void Clear()
        {
            var collection = new SearchResultCollection();

            var res = new SearchResult(MockDocument("d", "d", "d", DateTime.Now));

            collection.Add(res);
            Assert.AreEqual(1, collection.Count, "Wrong count (collection should contain 1 item)");

            collection.Clear();
            Assert.AreEqual(0, collection.Count, "Wrong count (collection should be empty)");
        }

        [Test]
        public void Constructor_NoCapacity()
        {
            var collection = new SearchResultCollection();
            Assert.AreEqual(0, collection.Count, "Wrong count (collection should be empty)");
        }

        [Test]
        public void Constructor_WithCapacity()
        {
            var collection = new SearchResultCollection(15);
            Assert.AreEqual(0, collection.Count, "Wrong count (collection should be empty)");
            Assert.AreEqual(15, collection.Capacity, "Wrong capacity");
        }

        [Test]
        public void Contains()
        {
            var collection = new SearchResultCollection();

            var res = new SearchResult(MockDocument("d", "d", "d", DateTime.Now));
            var res2 = new SearchResult(MockDocument("d2", "d", "d", DateTime.Now));

            collection.Add(res);
            Assert.IsTrue(collection.Contains(res), "Collection should contain item");
            Assert.IsFalse(collection.Contains(res2), "Collection should not contain item");

            Assert.IsFalse(collection.Contains(null), "Contains should return false");
        }

        [Test]
        public void CopyTo()
        {
            var collection = new SearchResultCollection();

            var res = new SearchResult(MockDocument("d", "d", "d", DateTime.Now));
            var res2 = new SearchResult(MockDocument("d2", "d", "d", DateTime.Now));

            collection.Add(res);
            collection.Add(res2);

            var results = new SearchResult[2];
            collection.CopyTo(results, 0);

            Assert.AreEqual(res, results[0], "Wrong result item");
            Assert.AreEqual(res2, results[1], "Wrong result item");

            results = new SearchResult[3];
            collection.CopyTo(results, 0);

            Assert.AreEqual(res, results[0], "Wrong result item");
            Assert.AreEqual(res2, results[1], "Wrong result item");
            Assert.IsNull(results[2], "Non-null item");

            results = new SearchResult[3];
            collection.CopyTo(results, 1);

            Assert.IsNull(results[0], "Non-null item");
            Assert.AreEqual(res, results[1], "Wrong result item");
            Assert.AreEqual(res2, results[2], "Wrong result item");
        }

        [Test]
        public void GetEnumerator()
        {
            var collection = new SearchResultCollection();

            var res = new SearchResult(MockDocument("d", "d", "d", DateTime.Now));
            var res2 = new SearchResult(MockDocument("d2", "d", "d", DateTime.Now));

            collection.Add(res);
            collection.Add(res2);

            var count = 0;
            foreach (var r in collection)
            {
                count++;
            }
            Assert.AreEqual(2, count, "Wrong count - enumerator does not work");
        }

        [Test]
        public void GetSearchResult()
        {
            var collection = new SearchResultCollection();

            var doc1 = MockDocument("d", "d", "d", DateTime.Now);
            var doc2 = MockDocument("d2", "d", "d", DateTime.Now);
            var doc3 = MockDocument("d3", "d", "d", DateTime.Now);
            var res = new SearchResult(doc1);
            var res2 = new SearchResult(doc2);

            collection.Add(res);
            collection.Add(res2);

            Assert.AreEqual(res, collection.GetSearchResult(doc1), "Wrong search result object");
            Assert.IsNull(collection.GetSearchResult(doc3), "GetSearchResult should return null");
        }

        [Test]
        public void Indexer()
        {
            var collection = new SearchResultCollection();

            var res = new SearchResult(MockDocument("d", "d", "d", DateTime.Now));
            var res2 = new SearchResult(MockDocument("d2", "d", "d", DateTime.Now));

            collection.Add(res);
            collection.Add(res2);

            Assert.AreEqual(res, collection[0], "Wrong item");
            Assert.AreEqual(res2, collection[1], "Wrong item");
        }

        [Test]
        public void ReadOnly()
        {
            var collection = new SearchResultCollection();
            Assert.IsFalse(collection.IsReadOnly);
        }

        [Test]
        public void Remove()
        {
            var collection = new SearchResultCollection();

            var res = new SearchResult(MockDocument("d", "d", "d", DateTime.Now));
            var res2 = new SearchResult(MockDocument("d2", "d", "d", DateTime.Now));
            var res3 = new SearchResult(MockDocument("d3", "d", "d", DateTime.Now));

            collection.Add(res);
            collection.Add(res2);

            Assert.IsTrue(collection.Remove(res), "Remove should return true");
            Assert.IsFalse(collection.Remove(res3), "Remove should return false");
            Assert.AreEqual(1, collection.Count, "Wrong count");
            Assert.AreEqual(res2, collection[0], "Wrong item at index 0");
        }
    }
}