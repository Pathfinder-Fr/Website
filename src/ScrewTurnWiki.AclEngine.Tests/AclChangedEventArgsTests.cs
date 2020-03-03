using System;
using NUnit.Framework;

namespace ScrewTurn.Wiki.AclEngine.Tests
{
    [TestFixture]
    public class AclChangedEventArgsTests
    {
        [Test]
        public void Constructor()
        {
            var entry = new AclEntry("Res", "Action", "U.User", Value.Grant);

            var args = new AclChangedEventArgs(new[] {entry}, Change.EntryStored);

            Assert.AreEqual(1, args.Entries.Length, "Wrong entry count");
            Assert.AreSame(entry, args.Entries[0], "Wrong Entry instance");
            Assert.AreEqual(Change.EntryStored, args.Change, "Wrong change");

            args = new AclChangedEventArgs(new[] {entry}, Change.EntryDeleted);

            Assert.AreEqual(1, args.Entries.Length, "Wrong entry count");
            Assert.AreSame(entry, args.Entries[0], "Wrong Entry instance");
            Assert.AreEqual(Change.EntryDeleted, args.Change, "Wrong change");
        }

        [Test]
        public void Constructor_EmptyEntries()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var args = new AclChangedEventArgs(new AclEntry[0], Change.EntryDeleted);
            });
        }

        [Test]
        public void Constructor_NullEntries()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var args = new AclChangedEventArgs(null, Change.EntryDeleted);
            });
        }
    }
}