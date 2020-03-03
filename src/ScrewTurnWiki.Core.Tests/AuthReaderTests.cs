using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Rhino.Mocks;
using ScrewTurn.Wiki.AclEngine;
using ScrewTurn.Wiki.PluginFramework;

namespace ScrewTurn.Wiki.Tests
{
    [TestFixture]
    public class AuthReaderTests
    {
        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            // TODO: Verify if this is really needed
            Collectors.SettingsProvider = MockProvider();
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                Directory.Delete(testDir, true);
            }
            catch
            {
                //Console.WriteLine("Test: could not delete temp directory");
            }
            mocks.VerifyAll();
        }

        private MockRepository mocks;

        private readonly string testDir = Path.Combine(Environment.GetEnvironmentVariable("TEMP"),
            Guid.NewGuid().ToString());

        protected IHostV30 MockHost()
        {
            if (!Directory.Exists(testDir)) Directory.CreateDirectory(testDir);

            var host = mocks.DynamicMock<IHostV30>();
            Expect.Call(host.GetSettingValue(SettingName.PublicDirectory)).Return(testDir).Repeat.Any();

            mocks.Replay(host);

            return host;
        }

        private ISettingsStorageProviderV30 MockProvider(List<AclEntry> entries)
        {
            var provider = mocks.DynamicMock<ISettingsStorageProviderV30>();
            provider.Init(MockHost(), "");
            LastCall.On(provider).Repeat.Any();

            AclManagerBase aclManager = new StandardAclManager();
            Expect.Call(provider.AclManager).Return(aclManager).Repeat.Any();

            mocks.Replay(provider);

            foreach (var entry in entries)
            {
                aclManager.StoreEntry(entry.Resource, entry.Action, entry.Subject, entry.Value);
            }

            return provider;
        }

        private ISettingsStorageProviderV30 MockProvider()
        {
            return MockProvider(new List<AclEntry>());
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void RetrieveSubjectsForDirectory_InvalidDirectory(string d)
        {
            Collectors.SettingsProvider = MockProvider();

            var fProv = mocks.DynamicMock<IFilesStorageProviderV30>();
            mocks.Replay(fProv);
            AuthReader.RetrieveSubjectsForDirectory(fProv, d);
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void RetrieveGrantsForDirectory_Group_InvalidDirectory(string d)
        {
            Collectors.SettingsProvider = MockProvider();

            var fProv = mocks.DynamicMock<IFilesStorageProviderV30>();
            mocks.Replay(fProv);
            AuthReader.RetrieveGrantsForDirectory(new UserGroup("Group", "Group", null), fProv, d);
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void RetrieveGrantsForDirectory_User_InvalidDirectory(string d)
        {
            Collectors.SettingsProvider = MockProvider();

            var fProv = mocks.DynamicMock<IFilesStorageProviderV30>();
            mocks.Replay(fProv);
            AuthReader.RetrieveGrantsForDirectory(
                new UserInfo("User", "User", "user@users.com", true, DateTime.Now, null), fProv, d);
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void RetrieveDenialsForDirectory_Group_InvalidDirectory(string d)
        {
            Collectors.SettingsProvider = MockProvider();

            var fProv = mocks.DynamicMock<IFilesStorageProviderV30>();
            mocks.Replay(fProv);
            AuthReader.RetrieveDenialsForDirectory(new UserGroup("Group", "Group", null), fProv, d);
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void RetrieveDenialsForDirectory_User_InvalidDirectory(string d)
        {
            Collectors.SettingsProvider = MockProvider();

            var fProv = mocks.DynamicMock<IFilesStorageProviderV30>();
            mocks.Replay(fProv);
            AuthReader.RetrieveDenialsForDirectory(
                new UserInfo("User", "User", "user@users.com", true, DateTime.Now, null), fProv, d);
        }

        [Test]
        public void RetrieveDenialsForDirectory_Root_Group()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var filesProv = mocks.DynamicMock<IFilesStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            var dirName = Actions.ForDirectories.ResourceMasterPrefix + AuthTools.GetDirectoryName(filesProv, "/");
            Expect.Call(aclManager.RetrieveEntriesForSubject("G.Group")).Return(
                new[]
                {
                    new AclEntry(dirName, Actions.ForDirectories.List, "G.Group", Value.Deny),
                    new AclEntry(dirName, Actions.FullControl, "G.Group", Value.Grant),
                    new AclEntry("D." + AuthTools.GetDirectoryName(filesProv, "/Other/"),
                        Actions.ForDirectories.UploadFiles, "G.Group", Value.Deny)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;

            var grants = AuthReader.RetrieveDenialsForDirectory(new UserGroup("Group", "Group", null),
                filesProv, "/");

            Assert.AreEqual(1, grants.Length, "Wrong denial count");
            Assert.AreEqual(Actions.ForDirectories.List, grants[0], "Wrong denial");
        }

        [Test]
        public void RetrieveDenialsForDirectory_Root_User()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var filesProv = mocks.DynamicMock<IFilesStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            var dirName = Actions.ForDirectories.ResourceMasterPrefix + AuthTools.GetDirectoryName(filesProv, "/");
            Expect.Call(aclManager.RetrieveEntriesForSubject("U.User")).Return(
                new[]
                {
                    new AclEntry(dirName, Actions.ForDirectories.UploadFiles, "U.User", Value.Deny),
                    new AclEntry(dirName, Actions.FullControl, "U.User", Value.Grant),
                    new AclEntry("D." + AuthTools.GetDirectoryName(filesProv, "/Other/"), Actions.ForDirectories.List,
                        "U.User", Value.Deny)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;

            var grants =
                AuthReader.RetrieveDenialsForDirectory(
                    new UserInfo("User", "User", "user@users.com", true, DateTime.Now, null),
                    filesProv, "/");

            Assert.AreEqual(1, grants.Length, "Wrong denial count");
            Assert.AreEqual(Actions.ForDirectories.UploadFiles, grants[0], "Wrong denial");
        }

        [Test]
        public void RetrieveDenialsForDirectory_Sub_Group()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var filesProv = mocks.DynamicMock<IFilesStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            var dirName = Actions.ForDirectories.ResourceMasterPrefix +
                          AuthTools.GetDirectoryName(filesProv, "/Dir/Sub/");
            Expect.Call(aclManager.RetrieveEntriesForSubject("G.Group")).Return(
                new[]
                {
                    new AclEntry(dirName, Actions.ForDirectories.List, "G.Group", Value.Deny),
                    new AclEntry(dirName, Actions.FullControl, "G.Group", Value.Grant),
                    new AclEntry("D." + AuthTools.GetDirectoryName(filesProv, "/"), Actions.ForDirectories.UploadFiles,
                        "G.Group", Value.Deny)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;

            var grants = AuthReader.RetrieveDenialsForDirectory(new UserGroup("Group", "Group", null),
                filesProv, "/Dir/Sub/");

            Assert.AreEqual(1, grants.Length, "Wrong denial count");
            Assert.AreEqual(Actions.ForDirectories.List, grants[0], "Wrong denial");
        }

        [Test]
        public void RetrieveDenialsForDirectory_Sub_User()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var filesProv = mocks.DynamicMock<IFilesStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            var dirName = Actions.ForDirectories.ResourceMasterPrefix +
                          AuthTools.GetDirectoryName(filesProv, "/Dir/Sub/");
            Expect.Call(aclManager.RetrieveEntriesForSubject("U.User")).Return(
                new[]
                {
                    new AclEntry(dirName, Actions.ForDirectories.UploadFiles, "U.User", Value.Deny),
                    new AclEntry(dirName, Actions.FullControl, "U.User", Value.Grant),
                    new AclEntry("D." + AuthTools.GetDirectoryName(filesProv, "/"), Actions.ForDirectories.List,
                        "U.User", Value.Deny)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;

            var grants =
                AuthReader.RetrieveDenialsForDirectory(
                    new UserInfo("User", "User", "user@users.com", true, DateTime.Now, null),
                    filesProv, "/Dir/Sub/");

            Assert.AreEqual(1, grants.Length, "Wrong denial count");
            Assert.AreEqual(Actions.ForDirectories.UploadFiles, grants[0], "Wrong denial");
        }

        [Test]
        public void RetrieveDenialsForGlobals_Group()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            Expect.Call(aclManager.RetrieveEntriesForSubject("G.Group")).Return(
                new[]
                {
                    new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.ForGlobals.ManageFiles, "G.Group",
                        Value.Deny),
                    new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.ForGlobals.ManageConfiguration,
                        "G.Group", Value.Grant),
                    new AclEntry(Actions.ForDirectories.ResourceMasterPrefix + "/", Actions.ForDirectories.UploadFiles,
                        "G.Group", Value.Deny)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;
            var grants = AuthReader.RetrieveDenialsForGlobals(new UserGroup("Group", "Group", null));
            Assert.AreEqual(1, grants.Length, "Wrong denial count");
            Assert.AreEqual(Actions.ForGlobals.ManageFiles, grants[0], "Wrong denial");

            mocks.Verify(prov);
            mocks.Verify(aclManager);
        }

        [Test]
        public void RetrieveDenialsForGlobals_User()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            Expect.Call(aclManager.RetrieveEntriesForSubject("U.User")).Return(
                new[]
                {
                    new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.ForGlobals.ManageFiles, "U.User",
                        Value.Deny),
                    new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.ForGlobals.ManageAccounts, "U.User",
                        Value.Grant),
                    new AclEntry(Actions.ForDirectories.ResourceMasterPrefix + "/", Actions.ForDirectories.UploadFiles,
                        "U.User", Value.Deny)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;
            var grants =
                AuthReader.RetrieveDenialsForGlobals(new UserInfo("User", "User", "user@users.com", true, DateTime.Now,
                    null));
            Assert.AreEqual(1, grants.Length, "Wrong denial count");
            Assert.AreEqual(Actions.ForGlobals.ManageFiles, grants[0], "Wrong denial");

            mocks.Verify(prov);
            mocks.Verify(aclManager);
        }

        [Test]
        public void RetrieveDenialsForNamespace_Group_Root()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            Expect.Call(aclManager.RetrieveEntriesForSubject("G.Group")).Return(
                new[]
                {
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ManagePages,
                        "G.Group", Value.Deny),
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ManageCategories,
                        "G.Group", Value.Grant),
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Sub", Actions.ForNamespaces.ManagePages,
                        "G.Group", Value.Deny)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;

            var grants = AuthReader.RetrieveDenialsForNamespace(new UserGroup("Group", "Group", null), null);

            Assert.AreEqual(1, grants.Length, "Wrong grant count");
            Assert.AreEqual(Actions.ForNamespaces.ManagePages, grants[0], "Wrong grant");
        }

        [Test]
        public void RetrieveDenialsForNamespace_Group_Sub()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            Expect.Call(aclManager.RetrieveEntriesForSubject("G.Group")).Return(
                new[]
                {
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Sub", Actions.ForNamespaces.ManagePages,
                        "G.Group", Value.Deny),
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Sub",
                        Actions.ForNamespaces.ManageCategories, "G.Group", Value.Grant),
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ManagePages,
                        "G.Group", Value.Deny)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;

            var grants = AuthReader.RetrieveDenialsForNamespace(new UserGroup("Group", "Group", null),
                new NamespaceInfo("Sub", null, null));

            Assert.AreEqual(1, grants.Length, "Wrong grant count");
            Assert.AreEqual(Actions.ForNamespaces.ManagePages, grants[0], "Wrong grant");
        }

        [Test]
        public void RetrieveDenialsForNamespace_User_Root()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            Expect.Call(aclManager.RetrieveEntriesForSubject("U.User")).Return(
                new[]
                {
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ManagePages, "U.User",
                        Value.Deny),
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ManageCategories,
                        "U.User", Value.Grant),
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Sub", Actions.ForNamespaces.ManagePages,
                        "U.User", Value.Deny)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;

            var grants =
                AuthReader.RetrieveDenialsForNamespace(
                    new UserInfo("User", "User", "user@users.com", true, DateTime.Now, null), null);

            Assert.AreEqual(1, grants.Length, "Wrong grant count");
            Assert.AreEqual(Actions.ForNamespaces.ManagePages, grants[0], "Wrong grant");
        }

        [Test]
        public void RetrieveDenialsForNamespace_User_Sub()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            Expect.Call(aclManager.RetrieveEntriesForSubject("U.User")).Return(
                new[]
                {
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Sub", Actions.ForNamespaces.ManagePages,
                        "U.User", Value.Deny),
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Sub",
                        Actions.ForNamespaces.ManageCategories, "U.User", Value.Grant),
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ManagePages, "U.User",
                        Value.Deny)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;

            var grants =
                AuthReader.RetrieveDenialsForNamespace(
                    new UserInfo("User", "User", "user@users.com", true, DateTime.Now, null),
                    new NamespaceInfo("Sub", null, null));

            Assert.AreEqual(1, grants.Length, "Wrong grant count");
            Assert.AreEqual(Actions.ForNamespaces.ManagePages, grants[0], "Wrong grant");
        }

        [Test]
        public void RetrieveDenialsForPage_Group()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            Expect.Call(aclManager.RetrieveEntriesForSubject("G.Group")).Return(
                new[]
                {
                    new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage, "G.Group",
                        Value.Deny),
                    new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.FullControl, "G.Group",
                        Value.Grant),
                    new AclEntry(Actions.ForPages.ResourceMasterPrefix + "NS.Blah", Actions.ForPages.ManagePage,
                        "G.Group", Value.Deny)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;

            var grants = AuthReader.RetrieveDenialsForPage(new UserGroup("Group", "Group", null),
                new PageInfo("Page", null, DateTime.Now));

            Assert.AreEqual(1, grants.Length, "Wrong denial count");
            Assert.AreEqual(Actions.ForPages.ModifyPage, grants[0], "Wrong denial");
        }

        [Test]
        public void RetrieveDenialsForPage_User()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            Expect.Call(aclManager.RetrieveEntriesForSubject("U.User")).Return(
                new[]
                {
                    new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage, "U.User",
                        Value.Deny),
                    new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.FullControl, "U.User",
                        Value.Grant),
                    new AclEntry(Actions.ForPages.ResourceMasterPrefix + "NS.Blah", Actions.ForPages.ManagePage,
                        "U.User", Value.Deny)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;

            var grants =
                AuthReader.RetrieveDenialsForPage(
                    new UserInfo("User", "User", "user@users.com", true, DateTime.Now, null),
                    new PageInfo("Page", null, DateTime.Now));

            Assert.AreEqual(1, grants.Length, "Wrong denial count");
            Assert.AreEqual(Actions.ForPages.ModifyPage, grants[0], "Wrong denial");
        }

        [Test]
        public void RetrieveGrantsForDirectory_Root_Group()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var filesProv = mocks.DynamicMock<IFilesStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            var dirName = Actions.ForDirectories.ResourceMasterPrefix + AuthTools.GetDirectoryName(filesProv, "/");
            Expect.Call(aclManager.RetrieveEntriesForSubject("G.Group")).Return(
                new[]
                {
                    new AclEntry(dirName, Actions.ForDirectories.List, "G.Group", Value.Grant),
                    new AclEntry(dirName, Actions.FullControl, "G.Group", Value.Deny),
                    new AclEntry("D." + AuthTools.GetDirectoryName(filesProv, "/Other/"),
                        Actions.ForDirectories.UploadFiles, "G.Group", Value.Grant)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;

            var grants = AuthReader.RetrieveGrantsForDirectory(new UserGroup("Group", "Group", null),
                filesProv, "/");

            Assert.AreEqual(1, grants.Length, "Wrong grant count");
            Assert.AreEqual(Actions.ForDirectories.List, grants[0], "Wrong grant");
        }

        [Test]
        public void RetrieveGrantsForDirectory_Root_User()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var filesProv = mocks.DynamicMock<IFilesStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            var dirName = Actions.ForDirectories.ResourceMasterPrefix + AuthTools.GetDirectoryName(filesProv, "/");
            Expect.Call(aclManager.RetrieveEntriesForSubject("U.User")).Return(
                new[]
                {
                    new AclEntry(dirName, Actions.ForDirectories.UploadFiles, "U.User", Value.Grant),
                    new AclEntry(dirName, Actions.FullControl, "U.User", Value.Deny),
                    new AclEntry("D." + AuthTools.GetDirectoryName(filesProv, "/Other/"), Actions.ForDirectories.List,
                        "U.User", Value.Grant)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;

            var grants =
                AuthReader.RetrieveGrantsForDirectory(
                    new UserInfo("User", "User", "user@users.com", true, DateTime.Now, null),
                    filesProv, "/");

            Assert.AreEqual(1, grants.Length, "Wrong grant count");
            Assert.AreEqual(Actions.ForDirectories.UploadFiles, grants[0], "Wrong grant");
        }

        [Test]
        public void RetrieveGrantsForDirectory_Sub_Group()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var filesProv = mocks.DynamicMock<IFilesStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            var dirName = Actions.ForDirectories.ResourceMasterPrefix +
                          AuthTools.GetDirectoryName(filesProv, "/Dir/Sub/");
            Expect.Call(aclManager.RetrieveEntriesForSubject("G.Group")).Return(
                new[]
                {
                    new AclEntry(dirName, Actions.ForDirectories.List, "G.Group", Value.Grant),
                    new AclEntry(dirName, Actions.FullControl, "G.Group", Value.Deny),
                    new AclEntry("D." + AuthTools.GetDirectoryName(filesProv, "/"), Actions.ForDirectories.UploadFiles,
                        "G.Group", Value.Grant)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;

            var grants = AuthReader.RetrieveGrantsForDirectory(new UserGroup("Group", "Group", null),
                filesProv, "/Dir/Sub/");

            Assert.AreEqual(1, grants.Length, "Wrong grant count");
            Assert.AreEqual(Actions.ForDirectories.List, grants[0], "Wrong grant");
        }

        [Test]
        public void RetrieveGrantsForDirectory_Sub_User()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var filesProv = mocks.DynamicMock<IFilesStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            var dirName = Actions.ForDirectories.ResourceMasterPrefix +
                          AuthTools.GetDirectoryName(filesProv, "/Dir/Sub/");
            Expect.Call(aclManager.RetrieveEntriesForSubject("U.User")).Return(
                new[]
                {
                    new AclEntry(dirName, Actions.ForDirectories.UploadFiles, "U.User", Value.Grant),
                    new AclEntry(dirName, Actions.FullControl, "U.User", Value.Deny),
                    new AclEntry("D." + AuthTools.GetDirectoryName(filesProv, "/"), Actions.ForDirectories.List,
                        "U.User", Value.Grant)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;

            var grants =
                AuthReader.RetrieveGrantsForDirectory(
                    new UserInfo("User", "User", "user@users.com", true, DateTime.Now, null),
                    filesProv, "/Dir/Sub/");

            Assert.AreEqual(1, grants.Length, "Wrong grant count");
            Assert.AreEqual(Actions.ForDirectories.UploadFiles, grants[0], "Wrong grant");
        }

        [Test]
        public void RetrieveGrantsForGlobals_Group()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            Expect.Call(aclManager.RetrieveEntriesForSubject("G.Group")).Return(
                new[]
                {
                    new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.ForGlobals.ManageFiles, "G.Group",
                        Value.Deny),
                    new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.ForGlobals.ManageAccounts, "G.Group",
                        Value.Grant),
                    new AclEntry(Actions.ForDirectories.ResourceMasterPrefix + "/", Actions.ForDirectories.UploadFiles,
                        "G.Group", Value.Grant)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;
            var grants = AuthReader.RetrieveGrantsForGlobals(new UserGroup("Group", "Group", null));
            Assert.AreEqual(1, grants.Length, "Wrong grant count");
            Assert.AreEqual(Actions.ForGlobals.ManageAccounts, grants[0], "Wrong grant");

            mocks.Verify(prov);
            mocks.Verify(aclManager);
        }

        [Test]
        public void RetrieveGrantsForGlobals_User()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            Expect.Call(aclManager.RetrieveEntriesForSubject("U.User")).Return(
                new[]
                {
                    new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.ForGlobals.ManageFiles, "U.User",
                        Value.Deny),
                    new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.ForGlobals.ManageAccounts, "U.User",
                        Value.Grant),
                    new AclEntry(Actions.ForDirectories.ResourceMasterPrefix + "/", Actions.ForDirectories.UploadFiles,
                        "U.User", Value.Grant)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;
            var grants =
                AuthReader.RetrieveGrantsForGlobals(new UserInfo("User", "User", "user@users.com", true, DateTime.Now,
                    null));
            Assert.AreEqual(1, grants.Length, "Wrong grant count");
            Assert.AreEqual(Actions.ForGlobals.ManageAccounts, grants[0], "Wrong grant");

            mocks.Verify(prov);
            mocks.Verify(aclManager);
        }

        [Test]
        public void RetrieveGrantsForNamespace_Group_Root()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            Expect.Call(aclManager.RetrieveEntriesForSubject("G.Group")).Return(
                new[]
                {
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ManagePages,
                        "G.Group", Value.Grant),
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ManageCategories,
                        "G.Group", Value.Deny),
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Sub", Actions.ForNamespaces.ManagePages,
                        "G.Group", Value.Grant)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;

            var grants = AuthReader.RetrieveGrantsForNamespace(new UserGroup("Group", "Group", null), null);

            Assert.AreEqual(1, grants.Length, "Wrong grant count");
            Assert.AreEqual(Actions.ForNamespaces.ManagePages, grants[0], "Wrong grant");
        }

        [Test]
        public void RetrieveGrantsForNamespace_Group_Sub()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            Expect.Call(aclManager.RetrieveEntriesForSubject("G.Group")).Return(
                new[]
                {
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Sub", Actions.ForNamespaces.ManagePages,
                        "G.Group", Value.Grant),
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Sub",
                        Actions.ForNamespaces.ManageCategories, "G.Group", Value.Deny),
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ManagePages,
                        "G.Group", Value.Grant)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;

            var grants = AuthReader.RetrieveGrantsForNamespace(new UserGroup("Group", "Group", null),
                new NamespaceInfo("Sub", null, null));

            Assert.AreEqual(1, grants.Length, "Wrong grant count");
            Assert.AreEqual(Actions.ForNamespaces.ManagePages, grants[0], "Wrong grant");
        }

        [Test]
        public void RetrieveGrantsForNamespace_User_Root()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            Expect.Call(aclManager.RetrieveEntriesForSubject("U.User")).Return(
                new[]
                {
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ManagePages, "U.User",
                        Value.Grant),
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ManageCategories,
                        "U.User", Value.Deny),
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Sub", Actions.ForNamespaces.ManagePages,
                        "U.User", Value.Grant)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;

            var grants =
                AuthReader.RetrieveGrantsForNamespace(
                    new UserInfo("User", "User", "user@users.com", true, DateTime.Now, null), null);

            Assert.AreEqual(1, grants.Length, "Wrong grant count");
            Assert.AreEqual(Actions.ForNamespaces.ManagePages, grants[0], "Wrong grant");
        }

        [Test]
        public void RetrieveGrantsForNamespace_User_Sub()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            Expect.Call(aclManager.RetrieveEntriesForSubject("U.User")).Return(
                new[]
                {
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Sub", Actions.ForNamespaces.ManagePages,
                        "U.User", Value.Grant),
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Sub",
                        Actions.ForNamespaces.ManageCategories, "U.User", Value.Deny),
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ManagePages, "U.User",
                        Value.Grant)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;

            var grants =
                AuthReader.RetrieveGrantsForNamespace(
                    new UserInfo("User", "User", "user@users.com", true, DateTime.Now, null),
                    new NamespaceInfo("Sub", null, null));

            Assert.AreEqual(1, grants.Length, "Wrong grant count");
            Assert.AreEqual(Actions.ForNamespaces.ManagePages, grants[0], "Wrong grant");
        }

        [Test]
        public void RetrieveGrantsForPage_Group()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            Expect.Call(aclManager.RetrieveEntriesForSubject("G.Group")).Return(
                new[]
                {
                    new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage, "G.Group",
                        Value.Grant),
                    new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.FullControl, "G.Group",
                        Value.Deny),
                    new AclEntry(Actions.ForPages.ResourceMasterPrefix + "NS.Blah", Actions.ForPages.ManagePage,
                        "G.Group", Value.Grant)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;

            var grants = AuthReader.RetrieveGrantsForPage(new UserGroup("Group", "Group", null),
                new PageInfo("Page", null, DateTime.Now));

            Assert.AreEqual(1, grants.Length, "Wrong grant count");
            Assert.AreEqual(Actions.ForPages.ModifyPage, grants[0], "Wrong grant");
        }

        [Test]
        public void RetrieveGrantsForPage_User()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            Expect.Call(aclManager.RetrieveEntriesForSubject("U.User")).Return(
                new[]
                {
                    new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage, "U.User",
                        Value.Grant),
                    new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.FullControl, "U.User",
                        Value.Deny),
                    new AclEntry(Actions.ForPages.ResourceMasterPrefix + "NS.Blah", Actions.ForPages.ManagePage,
                        "U.User", Value.Grant)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;

            var grants =
                AuthReader.RetrieveGrantsForPage(
                    new UserInfo("User", "User", "user@users.com", true, DateTime.Now, null),
                    new PageInfo("Page", null, DateTime.Now));

            Assert.AreEqual(1, grants.Length, "Wrong grant count");
            Assert.AreEqual(Actions.ForPages.ModifyPage, grants[0], "Wrong grant");
        }

        [Test]
        public void RetrieveSubjectsForDirectory_Root()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var filesProv = mocks.DynamicMock<IFilesStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            var dirName = Actions.ForDirectories.ResourceMasterPrefix + AuthTools.GetDirectoryName(filesProv, "/");
            Expect.Call(aclManager.RetrieveEntriesForResource(dirName)).Return(
                new[]
                {
                    new AclEntry(dirName, Actions.ForDirectories.List, "U.User1", Value.Grant),
                    new AclEntry(dirName, Actions.ForDirectories.UploadFiles, "U.User", Value.Grant),
                    new AclEntry(dirName, Actions.ForDirectories.DownloadFiles, "G.Group", Value.Grant),
                    new AclEntry(dirName, Actions.ForDirectories.CreateDirectories, "U.User", Value.Deny)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;

            var infos = AuthReader.RetrieveSubjectsForDirectory(filesProv, "/");

            Assert.AreEqual(3, infos.Length, "Wrong info count");

            Array.Sort(infos, delegate(SubjectInfo x, SubjectInfo y) { return x.Name.CompareTo(y.Name); });
            Assert.AreEqual("Group", infos[0].Name, "Wrong subject name");
            Assert.AreEqual(SubjectType.Group, infos[0].Type, "Wrong subject type");
            Assert.AreEqual("User", infos[1].Name, "Wrong subject name");
            Assert.AreEqual(SubjectType.User, infos[1].Type, "Wrong subject type");
            Assert.AreEqual("User1", infos[2].Name, "Wrong subject name");
            Assert.AreEqual(SubjectType.User, infos[2].Type, "Wrong subject type");

            mocks.Verify(prov);
            mocks.Verify(aclManager);
        }

        [Test]
        public void RetrieveSubjectsForDirectory_Sub()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var filesProv = mocks.DynamicMock<IFilesStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            var dirName = Actions.ForDirectories.ResourceMasterPrefix +
                          AuthTools.GetDirectoryName(filesProv, "/Dir/Sub/");
            Expect.Call(aclManager.RetrieveEntriesForResource(dirName)).Return(
                new[]
                {
                    new AclEntry(dirName, Actions.ForDirectories.List, "U.User1", Value.Grant),
                    new AclEntry(dirName, Actions.ForDirectories.UploadFiles, "U.User", Value.Grant),
                    new AclEntry(dirName, Actions.ForDirectories.DownloadFiles, "G.Group", Value.Grant),
                    new AclEntry(dirName, Actions.ForDirectories.CreateDirectories, "U.User", Value.Deny)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;

            var infos = AuthReader.RetrieveSubjectsForDirectory(filesProv, "/Dir/Sub/");

            Assert.AreEqual(3, infos.Length, "Wrong info count");

            Array.Sort(infos, delegate(SubjectInfo x, SubjectInfo y) { return x.Name.CompareTo(y.Name); });
            Assert.AreEqual("Group", infos[0].Name, "Wrong subject name");
            Assert.AreEqual(SubjectType.Group, infos[0].Type, "Wrong subject type");
            Assert.AreEqual("User", infos[1].Name, "Wrong subject name");
            Assert.AreEqual(SubjectType.User, infos[1].Type, "Wrong subject type");
            Assert.AreEqual("User1", infos[2].Name, "Wrong subject name");
            Assert.AreEqual(SubjectType.User, infos[2].Type, "Wrong subject type");

            mocks.Verify(prov);
            mocks.Verify(aclManager);
        }

        [Test]
        public void RetrieveSubjectsForNamespace_Root()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            Expect.Call(aclManager.RetrieveEntriesForResource("N.")).Return(
                new[]
                {
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.DeletePages,
                        "U.User1", Value.Grant),
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ReadPages, "U.User",
                        Value.Grant),
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ManagePages, "U.User",
                        Value.Grant),
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ManagePages,
                        "G.Group", Value.Deny)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;

            var infos = AuthReader.RetrieveSubjectsForNamespace(null);

            Assert.AreEqual(3, infos.Length, "Wrong info count");

            Array.Sort(infos, delegate(SubjectInfo x, SubjectInfo y) { return x.Name.CompareTo(y.Name); });
            Assert.AreEqual("Group", infos[0].Name, "Wrong subject name");
            Assert.AreEqual(SubjectType.Group, infos[0].Type, "Wrong subject type");
            Assert.AreEqual("User", infos[1].Name, "Wrong subject name");
            Assert.AreEqual(SubjectType.User, infos[1].Type, "Wrong subject type");
            Assert.AreEqual("User1", infos[2].Name, "Wrong subject name");
            Assert.AreEqual(SubjectType.User, infos[2].Type, "Wrong subject type");

            mocks.Verify(prov);
            mocks.Verify(aclManager);
        }

        [Test]
        public void RetrieveSubjectsForNamespace_Sub()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            Expect.Call(aclManager.RetrieveEntriesForResource("N.Sub")).Return(
                new[]
                {
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.DeletePages,
                        "U.User1", Value.Grant),
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ReadPages, "U.User",
                        Value.Grant),
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ManagePages, "U.User",
                        Value.Grant),
                    new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ManagePages,
                        "G.Group", Value.Deny)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;

            var infos = AuthReader.RetrieveSubjectsForNamespace(new NamespaceInfo("Sub", null, null));

            Assert.AreEqual(3, infos.Length, "Wrong info count");

            Array.Sort(infos, delegate(SubjectInfo x, SubjectInfo y) { return x.Name.CompareTo(y.Name); });
            Assert.AreEqual("Group", infos[0].Name, "Wrong subject name");
            Assert.AreEqual(SubjectType.Group, infos[0].Type, "Wrong subject type");
            Assert.AreEqual("User", infos[1].Name, "Wrong subject name");
            Assert.AreEqual(SubjectType.User, infos[1].Type, "Wrong subject type");
            Assert.AreEqual("User1", infos[2].Name, "Wrong subject name");
            Assert.AreEqual(SubjectType.User, infos[2].Type, "Wrong subject type");

            mocks.Verify(prov);
            mocks.Verify(aclManager);
        }

        [Test]
        public void RetrieveSubjectsForPage()
        {
            var mocks = new MockRepository();
            var prov = mocks.DynamicMock<ISettingsStorageProviderV30>();
            var aclManager = mocks.DynamicMock<IAclManager>();

            Expect.Call(prov.AclManager).Return(aclManager).Repeat.Any();

            Expect.Call(aclManager.RetrieveEntriesForResource("P.NS.Page")).Return(
                new[]
                {
                    new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage, "U.User1",
                        Value.Grant),
                    new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ManageCategories,
                        "U.User", Value.Grant),
                    new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.DeleteAttachments,
                        "G.Group", Value.Grant),
                    new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.DeleteAttachments,
                        "U.User", Value.Deny)
                });

            mocks.Replay(prov);
            mocks.Replay(aclManager);

            Collectors.SettingsProvider = prov;

            var infos = AuthReader.RetrieveSubjectsForPage(new PageInfo("NS.Page", null, DateTime.Now));

            Assert.AreEqual(3, infos.Length, "Wrong info count");

            Array.Sort(infos, delegate(SubjectInfo x, SubjectInfo y) { return x.Name.CompareTo(y.Name); });
            Assert.AreEqual("Group", infos[0].Name, "Wrong subject name");
            Assert.AreEqual(SubjectType.Group, infos[0].Type, "Wrong subject type");
            Assert.AreEqual("User", infos[1].Name, "Wrong subject name");
            Assert.AreEqual(SubjectType.User, infos[1].Type, "Wrong subject type");
            Assert.AreEqual("User1", infos[2].Name, "Wrong subject name");
            Assert.AreEqual(SubjectType.User, infos[2].Type, "Wrong subject type");

            mocks.Verify(prov);
            mocks.Verify(aclManager);
        }
    }
}