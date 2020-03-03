using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace ScrewTurn.Wiki.SearchEngine.Tests
{
    [TestFixture]
    public class OccurrenceDictionaryTests : TestsBase
    {
        [Test]
        public void Add_KV()
        {
            var dic = new OccurrenceDictionary();
            dic.Add(MockDocument("Doc1", "Doc 1", "d", DateTime.Now), new SortedBasicWordInfoSet());
            Assert.AreEqual(1, dic.Count, "Wrong count");
            dic.Add(MockDocument("Doc2", "Doc 2", "d", DateTime.Now), new SortedBasicWordInfoSet());
            Assert.AreEqual(2, dic.Count, "Wrong count");
        }

        [Test]
        public void Add_Pair()
        {
            var dic = new OccurrenceDictionary();
            dic.Add(new KeyValuePair<IDocument, SortedBasicWordInfoSet>(
                MockDocument("Doc1", "Doc 1", "d", DateTime.Now), new SortedBasicWordInfoSet()));
            Assert.AreEqual(1, dic.Count, "Wrong count");
            dic.Add(MockDocument("Doc2", "Doc 2", "d", DateTime.Now), new SortedBasicWordInfoSet());
            Assert.AreEqual(2, dic.Count, "Wrong count");
        }

        [Test]
        public void Clear()
        {
            var dic = new OccurrenceDictionary();
            dic.Add(MockDocument("Doc1", "Doc1", "d", DateTime.Now), new SortedBasicWordInfoSet());
            dic.Clear();
            Assert.AreEqual(0, dic.Count, "Wrong count");
        }

        [Test]
        public void Constructor_NoCapacity()
        {
            var dic = new OccurrenceDictionary();
            Assert.AreEqual(0, dic.Count, "Wrong count (Dictionary should be empty)");
        }

        [Test]
        public void Constructor_WithCapacity()
        {
            var dic = new OccurrenceDictionary(10);
            Assert.AreEqual(0, dic.Count, "Wrong count (Dictionary should be empty)");
        }

        [Test]
        public void Contains()
        {
            var dic = new OccurrenceDictionary();
            var doc = MockDocument("Doc", "Doc", "d", DateTime.Now);
            Assert.IsFalse(
                dic.Contains(new KeyValuePair<IDocument, SortedBasicWordInfoSet>(doc, new SortedBasicWordInfoSet())),
                "Contains should return false");
            dic.Add(doc, new SortedBasicWordInfoSet());
            Assert.IsTrue(
                dic.Contains(new KeyValuePair<IDocument, SortedBasicWordInfoSet>(doc, new SortedBasicWordInfoSet())),
                "Contains should return true");
            Assert.IsFalse(
                dic.Contains(
                    new KeyValuePair<IDocument, SortedBasicWordInfoSet>(
                        MockDocument("Doc2", "Doc 2", "d", DateTime.Now), new SortedBasicWordInfoSet())),
                "Contains should return false");

            var doc2 = MockDocument("Doc", "Doc", "d", DateTime.Now);
            Assert.IsTrue(
                dic.Contains(new KeyValuePair<IDocument, SortedBasicWordInfoSet>(doc, new SortedBasicWordInfoSet())),
                "Contains should return true");
        }

        [Test]
        public void ContainsKey()
        {
            var dic = new OccurrenceDictionary();
            var doc = MockDocument("Doc", "Doc", "d", DateTime.Now);
            Assert.IsFalse(dic.ContainsKey(doc), "ContainsKey should return false");
            dic.Add(doc, new SortedBasicWordInfoSet());
            Assert.IsTrue(dic.ContainsKey(doc), "ContainsKey should return true");
            Assert.IsFalse(dic.ContainsKey(MockDocument("Doc2", "Doc 2", "d", DateTime.Now)),
                "ContainsKey should return false");

            var doc2 = MockDocument("Doc", "Doc", "d", DateTime.Now);
            Assert.IsTrue(dic.ContainsKey(doc2), "ContainsKey should return true");
        }

        [Test]
        public void CopyTo()
        {
            var dic = new OccurrenceDictionary();
            var set = new SortedBasicWordInfoSet();
            set.Add(new BasicWordInfo(1, 1, WordLocation.Title));
            dic.Add(MockDocument("Doc", "Doc", "d", DateTime.Now), set);
            var array = new KeyValuePair<IDocument, SortedBasicWordInfoSet>[1];
            dic.CopyTo(array, 0);

            Assert.IsNotNull(array[0], "Array[0] should not be null");
            Assert.AreEqual("Doc", array[0].Key.Name, "Wrong array item");
            Assert.AreEqual(1, array[0].Value.Count, "Wrong count");
            Assert.AreEqual(1, array[0].Value[0].FirstCharIndex, "Wrong first char index");
        }

        [Test]
        public void GetEnumerator()
        {
            var dic = new OccurrenceDictionary();
            var doc1 = MockDocument("Doc1", "Doc1", "d", DateTime.Now);
            var doc2 = MockDocument("Doc2", "Doc2", "d", DateTime.Now);
            dic.Add(doc1, new SortedBasicWordInfoSet());
            dic.Add(doc2, new SortedBasicWordInfoSet());

            Assert.IsNotNull(dic.GetEnumerator(), "GetEnumerator should not return null");

            var count = 0;
            foreach (var pair in dic)
            {
                count++;
            }

            Assert.AreEqual(2, count, "Wrong count");
        }

        [Test]
        public void Indexer_Get()
        {
            var dic = new OccurrenceDictionary();
            var doc1 = MockDocument("Doc1", "Doc1", "d", DateTime.Now);
            var set1 = new SortedBasicWordInfoSet();
            set1.Add(new BasicWordInfo(1, 1, WordLocation.Content));

            dic.Add(doc1, set1);

            var output = dic[MockDocument("Doc1", "Doc1", "d", DateTime.Now)];
            Assert.IsNotNull(output, "Output should not be null");
            Assert.AreEqual(1, set1.Count, "Wrong count");
            Assert.AreEqual(1, set1[0].FirstCharIndex, "Wrong first char index");
        }

        [Test]
        public void Indexer_Set()
        {
            var dic = new OccurrenceDictionary();
            dic.Add(MockDocument("Doc1", "Doc1", "d", DateTime.Now), new SortedBasicWordInfoSet());

            var set1 = new SortedBasicWordInfoSet();
            set1.Add(new BasicWordInfo(1, 1, WordLocation.Content));

            dic[MockDocument("Doc1", "Doc1", "d", DateTime.Now)] = set1;

            var output = dic[MockDocument("Doc1", "Doc1", "d", DateTime.Now)];
            Assert.AreEqual(1, output.Count, "Wrong count");
            Assert.AreEqual(1, output[0].FirstCharIndex, "Wrong first char index");
        }

        [Test]
        public void IsReadOnly()
        {
            var dic = new OccurrenceDictionary();
            Assert.IsFalse(dic.IsReadOnly, "IsReadOnly should always return false");
        }

        [Test]
        public void Keys()
        {
            var dic = new OccurrenceDictionary();
            var doc1 = MockDocument("Doc1", "Doc1", "d", DateTime.Now);
            var doc2 = MockDocument("Doc2", "Doc2", "d", DateTime.Now);
            dic.Add(doc1, new SortedBasicWordInfoSet());
            dic.Add(doc2, new SortedBasicWordInfoSet());

            Assert.AreEqual(2, dic.Keys.Count, "Wrong key count");

            bool doc1Found = false, doc2Found = false;
            foreach (var d in dic.Keys)
            {
                if (d.Name == "Doc1") doc1Found = true;
                if (d.Name == "Doc2") doc2Found = true;
            }

            Assert.IsTrue(doc1Found, "Doc1 not found");
            Assert.IsTrue(doc2Found, "Doc2 not found");
        }

        [Test]
        public void Remove_KV()
        {
            var dic = new OccurrenceDictionary();
            var set = new SortedBasicWordInfoSet();
            set.Add(new BasicWordInfo(5, 0, WordLocation.Content));
            dic.Add(MockDocument("Doc1", "Doc1", "d", DateTime.Now), set);
            dic.Add(MockDocument("Doc2", "Doc2", "d", DateTime.Now), new SortedBasicWordInfoSet());
            Assert.AreEqual(2, dic.Count, "Wrong initial count");
            Assert.IsFalse(dic.Remove(MockDocument("Doc3", "Doc3", "d", DateTime.Now)), "Remove should return false");
            Assert.IsTrue(dic.Remove(MockDocument("Doc1", "Doc1", "d", DateTime.Now)), "Remove should return true");
            Assert.AreEqual(1, dic.Count, "Wrong count");
        }

        [Test]
        public void Remove_Pair()
        {
            var dic = new OccurrenceDictionary();
            dic.Add(MockDocument("Doc1", "Doc1", "d", DateTime.Now), new SortedBasicWordInfoSet());
            dic.Add(MockDocument("Doc2", "Doc2", "d", DateTime.Now), new SortedBasicWordInfoSet());
            Assert.AreEqual(2, dic.Count, "Wrong initial count");
            Assert.IsFalse(dic.Remove(
                new KeyValuePair<IDocument, SortedBasicWordInfoSet>(MockDocument("Doc3", "Doc3", "d", DateTime.Now),
                    new SortedBasicWordInfoSet())),
                "Remove should return false");
            Assert.IsTrue(dic.Remove(
                new KeyValuePair<IDocument, SortedBasicWordInfoSet>(MockDocument("Doc2", "Doc2", "d", DateTime.Now),
                    new SortedBasicWordInfoSet())),
                "Remove should return true");
            Assert.AreEqual(1, dic.Count, "Wrong count");
        }

        [Test]
        public void RemoveExtended()
        {
            var dic = new OccurrenceDictionary();
            var set1 = new SortedBasicWordInfoSet();
            set1.Add(new BasicWordInfo(5, 0, WordLocation.Content));
            set1.Add(new BasicWordInfo(12, 1, WordLocation.Keywords));
            var set2 = new SortedBasicWordInfoSet();
            set2.Add(new BasicWordInfo(1, 0, WordLocation.Content));
            set2.Add(new BasicWordInfo(4, 1, WordLocation.Title));
            dic.Add(MockDocument("Doc1", "Doc", "doc", DateTime.Now), set1);
            dic.Add(MockDocument("Doc2", "Doc", "doc", DateTime.Now), set2);

            var dm = dic.RemoveExtended(MockDocument("Doc1", "Doc", "doc", DateTime.Now), 1);
            Assert.AreEqual(2, dm.Count, "Wrong count");

            Assert.IsTrue(dm.Find(delegate(DumpedWordMapping m)
            {
                return m.WordID == 1 && m.DocumentID == 1 &&
                       m.FirstCharIndex == 5 && m.WordIndex == 0 &&
                       m.Location == WordLocation.Content.Location;
            }) != null, "Mapping not found");

            Assert.IsTrue(dm.Find(delegate(DumpedWordMapping m)
            {
                return m.WordID == 1 && m.DocumentID == 1 &&
                       m.FirstCharIndex == 12 && m.WordIndex == 1 &&
                       m.Location == WordLocation.Keywords.Location;
            }) != null, "Mapping not found");
        }

        [Test]
        public void TryGetValue()
        {
            var dic = new OccurrenceDictionary();
            var doc1 = MockDocument("Doc1", "Doc1", "d", DateTime.Now);
            var doc2 = MockDocument("Doc2", "Doc2", "d", DateTime.Now);

            SortedBasicWordInfoSet set = null;

            Assert.IsFalse(dic.TryGetValue(doc1, out set), "TryGetValue should return false");
            Assert.IsNull(set, "Set should be null");

            dic.Add(doc1, new SortedBasicWordInfoSet());
            Assert.IsTrue(dic.TryGetValue(MockDocument("Doc1", "Doc1", "d", DateTime.Now), out set),
                "TryGetValue should return true");
            Assert.IsNotNull(set, "Set should not be null");

            Assert.IsFalse(dic.TryGetValue(doc2, out set), "TryGetValue should return false");
            Assert.IsNull(set, "Set should have been set to null");
        }

        [Test]
        public void Values()
        {
            var dic = new OccurrenceDictionary();
            var doc1 = MockDocument("Doc1", "Doc1", "d", DateTime.Now);
            var doc2 = MockDocument("Doc2", "Doc2", "d", DateTime.Now);
            var set1 = new SortedBasicWordInfoSet();
            set1.Add(new BasicWordInfo(0, 0, WordLocation.Content));
            var set2 = new SortedBasicWordInfoSet();
            set2.Add(new BasicWordInfo(1, 1, WordLocation.Title));
            dic.Add(doc1, set1);
            dic.Add(doc2, set2);

            Assert.AreEqual(2, dic.Values.Count, "Wrong value count");

            bool set1Found = false, set2Found = false;
            foreach (var set in dic.Values)
            {
                if (set[0].FirstCharIndex == 0) set1Found = true;
                if (set[0].FirstCharIndex == 1) set2Found = true;
            }

            Assert.IsTrue(set1Found, "Set1 not found");
            Assert.IsTrue(set2Found, "Set2 not found");
        }
    }
}