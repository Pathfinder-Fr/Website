using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using ScrewTurn.Wiki.AclEngine;

namespace ScrewTurn.Wiki.Tests
{
    [TestFixture]
    public class AclStorerTests
    {
        [TearDown]
        public void TearDown()
        {
            try
            {
                File.Delete(testFile);
            }
            catch
            {
            }
        }

        private readonly string testFile = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), "__ACL_File.dat");
        private readonly MockRepository mocks = new MockRepository();

        private IAclManager MockAclManager()
        {
            IAclManager manager = mocks.DynamicMock<AclManagerBase>();

            return manager;
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void Constructor_InvalidFile(string f)
        {
            var storer = new AclStorer(MockAclManager(), f);
        }

        private void AssertAclEntriesAreEqual(AclEntry expected, AclEntry actual)
        {
            Assert.AreEqual(expected.Resource, actual.Resource, "Wrong resource");
            Assert.AreEqual(expected.Action, actual.Action, "Wrong action");
            Assert.AreEqual(expected.Subject, actual.Subject, "Wrong subject");
            Assert.AreEqual(expected.Value, actual.Value, "Wrong value");
        }

        [Test]
        public void Constructor()
        {
            var manager = MockAclManager();

            var storer = new AclStorer(manager, testFile);
            Assert.AreSame(manager, storer.AclManager, "Wrong ACL Manager instance");
        }

        [Test]
        public void Delete_LoadData()
        {
            var manager = MockAclManager();

            var storer = new AclStorer(manager, testFile);

            manager.StoreEntry("Res1", "Action1", "U.User", Value.Grant);
            manager.StoreEntry("Res2", "Action2", "G.Group", Value.Deny);
            manager.StoreEntry("Res3", "Action1", "U.User", Value.Grant);
            manager.StoreEntry("Res3", "Action2", "G.Group", Value.Deny);

            manager.DeleteEntriesForResource("Res3");

            storer.Dispose();
            storer = null;

            manager = MockAclManager();

            storer = new AclStorer(manager, testFile);
            storer.LoadData();

            Assert.AreEqual(2, manager.TotalEntries, "Wrong entry count");

            var allEntries = manager.RetrieveAllEntries().ToArray();
            Assert.AreEqual(2, allEntries.Length, "Wrong entry count");

            Array.Sort(allEntries, delegate(AclEntry x, AclEntry y) { return x.Subject.CompareTo(y.Subject); });
            AssertAclEntriesAreEqual(new AclEntry("Res2", "Action2", "G.Group", Value.Deny), allEntries[0]);
            AssertAclEntriesAreEqual(new AclEntry("Res1", "Action1", "U.User", Value.Grant), allEntries[1]);
        }

        [Test]
        public void Overwrite_LoadData()
        {
            var manager = MockAclManager();

            var storer = new AclStorer(manager, testFile);

            manager.StoreEntry("Res1", "Action1", "U.User", Value.Grant);
            manager.StoreEntry("Res2", "Action2", "G.Group", Value.Grant);
            manager.StoreEntry("Res2", "Action2", "G.Group", Value.Deny); // Overwrite

            storer.Dispose();
            storer = null;

            manager = MockAclManager();

            storer = new AclStorer(manager, testFile);
            storer.LoadData();

            Assert.AreEqual(2, manager.TotalEntries, "Wrong entry count");

            var allEntries = manager.RetrieveAllEntries().ToArray();
            Assert.AreEqual(2, allEntries.Length, "Wrong entry count");

            Array.Sort(allEntries, delegate(AclEntry x, AclEntry y) { return x.Subject.CompareTo(y.Subject); });
            AssertAclEntriesAreEqual(new AclEntry("Res2", "Action2", "G.Group", Value.Deny), allEntries[0]);
            AssertAclEntriesAreEqual(new AclEntry("Res1", "Action1", "U.User", Value.Grant), allEntries[1]);
        }

        [Test]
        public void Store_LoadData()
        {
            var manager = MockAclManager();

            var storer = new AclStorer(manager, testFile);

            manager.StoreEntry("Res1", "Action1", "U.User", Value.Grant);
            manager.StoreEntry("Res2", "Action2", "G.Group", Value.Deny);

            storer.Dispose();
            storer = null;

            manager = MockAclManager();

            storer = new AclStorer(manager, testFile);
            storer.LoadData();

            Assert.AreEqual(2, manager.TotalEntries, "Wrong entry count");

            var allEntries = manager.RetrieveAllEntries().ToArray();
            Assert.AreEqual(2, allEntries.Length, "Wrong entry count");

            Array.Sort(allEntries, delegate(AclEntry x, AclEntry y) { return x.Subject.CompareTo(y.Subject); });
            AssertAclEntriesAreEqual(new AclEntry("Res2", "Action2", "G.Group", Value.Deny), allEntries[0]);
            AssertAclEntriesAreEqual(new AclEntry("Res1", "Action1", "U.User", Value.Grant), allEntries[1]);
        }
    }
}