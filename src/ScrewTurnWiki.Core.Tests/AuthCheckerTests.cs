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
    public class AuthCheckerTests
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
        [TestCase("Blah", ExpectedException = typeof (ArgumentException))]
        [TestCase("*", ExpectedException = typeof (ArgumentException))]
        public void CheckActionForGlobals_InvalidAction(string a)
        {
            Collectors.SettingsProvider = MockProvider();
            AuthChecker.CheckActionForGlobals(a, "User", new string[0]);
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void CheckActionForGlobals_InvalidUser(string u)
        {
            Collectors.SettingsProvider = MockProvider();
            AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageAccounts, u, new string[0]);
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        [TestCase("Blah", ExpectedException = typeof (ArgumentException))]
        [TestCase("*", ExpectedException = typeof (ArgumentException))]
        public void CheckActionForNamespace_InvalidAction(string a)
        {
            Collectors.SettingsProvider = MockProvider();
            AuthChecker.CheckActionForNamespace(new NamespaceInfo("NS", null, null), a, "User", new string[0]);
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void CheckActionForNamespace_InvalidUser(string u)
        {
            Collectors.SettingsProvider = MockProvider();
            AuthChecker.CheckActionForNamespace(new NamespaceInfo("NS", null, null), Actions.ForNamespaces.ManagePages,
                u, new string[0]);
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        [TestCase("Blah", ExpectedException = typeof (ArgumentException))]
        [TestCase("*", ExpectedException = typeof (ArgumentException))]
        public void CheckActionForPage_InvalidAction(string a)
        {
            Collectors.SettingsProvider = MockProvider();
            AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), a, "User", new string[0]);
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void CheckActionForPage_InvalidUser(string u)
        {
            Collectors.SettingsProvider = MockProvider();
            AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.DeleteAttachments,
                u, new string[0]);
        }

        private IFilesStorageProviderV30 MockFilesProvider()
        {
            var prov = mocks.DynamicMock<IFilesStorageProviderV30>();
            mocks.Replay(prov);
            return prov;
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void CheckActionForDirectory_InvalidDirectory(string d)
        {
            Collectors.SettingsProvider = MockProvider();
            AuthChecker.CheckActionForDirectory(MockFilesProvider(), d, Actions.ForDirectories.CreateDirectories, "User",
                new string[0]);
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        [TestCase("Blah", ExpectedException = typeof (ArgumentException))]
        [TestCase("*", ExpectedException = typeof (ArgumentException))]
        public void CheckActionForDirectory_InvalidAction(string a)
        {
            Collectors.SettingsProvider = MockProvider();
            AuthChecker.CheckActionForDirectory(MockFilesProvider(), "/Dir", a, "User", new string[0]);
        }

        [TestCase(null, ExpectedException = typeof (ArgumentNullException))]
        [TestCase("", ExpectedException = typeof (ArgumentException))]
        public void CheckActionForDirectory_InvalidUser(string u)
        {
            Collectors.SettingsProvider = MockProvider();
            AuthChecker.CheckActionForDirectory(MockFilesProvider(), "/Dir", Actions.ForDirectories.CreateDirectories, u,
                new string[0]);
        }

        [Test]
        public void CheckActionForDirectory_AdminBypass()
        {
            Collectors.SettingsProvider = MockProvider();
            Assert.IsTrue(
                AuthChecker.CheckActionForDirectory(MockFilesProvider(), "/", Actions.ForDirectories.DeleteFiles,
                    "admin", new string[0]), "Admin account should bypass security");
        }

        [Test]
        public void CheckActionForDirectory_DenyGroupExplicit()
        {
            var filesProv = MockFilesProvider();

            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.ForDirectories.UploadFiles, "G.Group", Value.Deny));
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.ForDirectories.UploadFiles, "G.Group100", Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.DeleteFiles, "User", new[] {"Group"}), "Permission should be denied");

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.UploadFiles, "User", new[] {"Group"}), "Permission should be denied");

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.UploadFiles, "User", new[] {"Group2"}), "Permission should be denied");

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir2",
                Actions.ForDirectories.UploadFiles, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForDirectory_DenyGroupFullControl()
        {
            var filesProv = MockFilesProvider();

            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.FullControl, "G.Group", Value.Deny));
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.FullControl, "G.Group100", Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.DeleteFiles, "User", new[] {"Group"}), "Permission should be denied");

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.UploadFiles, "User", new[] {"Group"}), "Permission should be denied");

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.UploadFiles, "User", new[] {"Group2"}), "Permission should be denied");

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir2",
                Actions.ForDirectories.UploadFiles, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForDirectory_DenyUserExplicit()
        {
            var filesProv = MockFilesProvider();

            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.ForDirectories.UploadFiles, "U.User", Value.Deny));
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.ForDirectories.UploadFiles, "U.User100", Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.DeleteFiles, "User", new[] {"Group"}), "Permission should be denied");

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.UploadFiles, "User", new[] {"Group"}), "Permission should be denied");

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.UploadFiles, "User2", new[] {"Group"}), "Permission should be denied");

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir2",
                Actions.ForDirectories.UploadFiles, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForDirectory_DenyUserFullControl()
        {
            var filesProv = MockFilesProvider();

            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.FullControl, "U.User", Value.Deny));
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.FullControl, "U.User100", Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.DeleteFiles, "User", new[] {"Group"}), "Permission should be denied");

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.UploadFiles, "User", new[] {"Group"}), "Permission should be denied");

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.UploadFiles, "User2", new[] {"Group"}), "Permission should be denied");

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir2",
                Actions.ForDirectories.UploadFiles, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForDirectory_GrantGroupDirectoryEscalator()
        {
            var filesProv = MockFilesProvider();

            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.ForDirectories.DownloadFiles, "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.ForDirectories.DownloadFiles, "G.Group100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(AuthChecker.CheckActionForDirectory(filesProv, "/Dir/Sub",
                Actions.ForDirectories.DownloadFiles, "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForDirectory_GrantGroupDirectoryEscalator_DenyUserExplicit()
        {
            var filesProv = MockFilesProvider();

            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.ForDirectories.DownloadFiles, "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir/Sub"),
                Actions.ForDirectories.DownloadFiles, "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir/Sub",
                Actions.ForDirectories.DownloadFiles, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForDirectory_GrantGroupExplicit()
        {
            var filesProv = MockFilesProvider();

            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.ForDirectories.UploadFiles, "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.ForDirectories.UploadFiles, "G.Group100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.DeleteFiles, "User", new[] {"Group"}), "Permission should be denied");

            Assert.IsTrue(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.UploadFiles, "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.UploadFiles, "User", new[] {"Group2"}), "Permission should be denied");

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir2",
                Actions.ForDirectories.UploadFiles, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForDirectory_GrantGroupFullControl()
        {
            var filesProv = MockFilesProvider();

            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.FullControl, "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.FullControl, "G.Group100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.DeleteFiles, "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsTrue(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.UploadFiles, "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.UploadFiles, "User", new[] {"Group2"}), "Permission should be denied");

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir2",
                Actions.ForDirectories.UploadFiles, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForDirectory_GrantGroupGlobalEscalator()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix,
                Actions.ForGlobals.ManageFiles, "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix,
                Actions.ForGlobals.ManageFiles, "G.Group100", Value.Deny));

            var filesProv = MockFilesProvider();

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.UploadFiles, "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForDirectory_GrantGroupGlobalEscalator_DenyUserExplicit()
        {
            var filesProv = MockFilesProvider();

            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix,
                Actions.ForGlobals.ManageFiles, "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"), Actions.ForDirectories.DownloadFiles,
                "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.DownloadFiles, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForDirectory_GrantGroupGlobalFullControl()
        {
            var filesProv = MockFilesProvider();

            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix,
                Actions.FullControl, "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix,
                Actions.FullControl, "G.Group100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.List, "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForDirectory_GrantGroupGlobalFullControl_DenyUserExplicit()
        {
            var filesProv = MockFilesProvider();

            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix,
                Actions.FullControl, "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"), Actions.ForDirectories.DownloadFiles,
                "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.DownloadFiles, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForDirectory_GrantGroupLocalEscalator()
        {
            var filesProv = MockFilesProvider();

            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.ForDirectories.DownloadFiles, "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.ForDirectories.DownloadFiles, "G.Group100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.List, "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForDirectory_GrantGroupLocalEscalator_DenyUserExplicit()
        {
            var filesProv = MockFilesProvider();

            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"), Actions.ForDirectories.DownloadFiles,
                "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"), Actions.ForDirectories.List,
                "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.List, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForDirectory_GrantUserDirectoryEscalator()
        {
            var filesProv = MockFilesProvider();

            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.ForDirectories.DownloadFiles, "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.ForDirectories.DownloadFiles, "U.User100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(AuthChecker.CheckActionForDirectory(filesProv, "/Dir/Sub",
                Actions.ForDirectories.DownloadFiles, "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForDirectory_GrantUserDirectoryEscalator_DenyUserExplicit()
        {
            var filesProv = MockFilesProvider();

            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.ForDirectories.DownloadFiles, "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir/Sub"),
                Actions.ForDirectories.DownloadFiles, "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir/Sub",
                Actions.ForDirectories.DownloadFiles, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForDirectory_GrantUserDirectoryEscalator_RecursiveName()
        {
            var filesProv = MockFilesProvider();

            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/"),
                Actions.ForDirectories.List, "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Sub/Sub/"),
                Actions.ForDirectories.List, "U.User2", Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(AuthChecker.CheckActionForDirectory(filesProv, "/Sub/Sub/Sub", Actions.ForDirectories.List,
                "User", new string[0]), "Permission should be granted");

            Assert.IsTrue(AuthChecker.CheckActionForDirectory(filesProv, "/Sub/Sub/Sub/", Actions.ForDirectories.List,
                "User", new string[0]), "Permission should be granted");

            Assert.IsTrue(AuthChecker.CheckActionForDirectory(filesProv, "/Sub/Sub/Sub/", Actions.ForDirectories.List,
                "User2", new string[0]), "Permission should be granted");

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Sub/Sub/Sub", Actions.ForDirectories.List,
                "User2", new string[0]), "Permission should be granted");
        }

        [Test]
        public void CheckActionForDirectory_GrantUserExplicit()
        {
            var filesProv = MockFilesProvider();

            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.ForDirectories.UploadFiles, "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.ForDirectories.UploadFiles, "U.User100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.DeleteFiles, "User", new[] {"Group"}), "Permission should be denied");

            Assert.IsTrue(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.UploadFiles, "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.UploadFiles, "User2", new[] {"Group"}), "Permission should be denied");

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir2",
                Actions.ForDirectories.UploadFiles, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForDirectory_GrantUserFullControl()
        {
            var filesProv = MockFilesProvider();

            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.FullControl, "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.FullControl, "U.User100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.DeleteFiles, "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsTrue(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.UploadFiles, "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.UploadFiles, "User2", new[] {"Group"}), "Permission should be denied");

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir2",
                Actions.ForDirectories.UploadFiles, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForDirectory_GrantUserGlobalEscalator()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix,
                Actions.ForGlobals.ManageFiles, "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix,
                Actions.ForGlobals.ManageFiles, "U.User100", Value.Deny));

            var filesProv = MockFilesProvider();

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.UploadFiles, "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForDirectory_GrantUserGlobalEscalator_DenyUserExplicit()
        {
            var filesProv = MockFilesProvider();

            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix,
                Actions.ForGlobals.ManageFiles, "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"), Actions.ForDirectories.DownloadFiles,
                "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.DownloadFiles, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForDirectory_GrantUserGlobalFullControl()
        {
            var filesProv = MockFilesProvider();

            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix,
                Actions.FullControl, "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix,
                Actions.FullControl, "U.User100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.List, "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForDirectory_GrantUserGlobalFullControl_DenyUserExplicit()
        {
            var filesProv = MockFilesProvider();

            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix,
                Actions.FullControl, "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"), Actions.ForDirectories.DownloadFiles,
                "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.DownloadFiles, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForDirectory_GrantUserLocalEscalator()
        {
            var filesProv = MockFilesProvider();

            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.ForDirectories.DownloadFiles, "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"),
                Actions.ForDirectories.DownloadFiles, "U.User100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.List, "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForDirectory_GrantUserLocalEscalator_DenyUserExplicit()
        {
            var filesProv = MockFilesProvider();

            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"), Actions.ForDirectories.DownloadFiles,
                "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/Dir"), Actions.ForDirectories.List,
                "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/Dir",
                Actions.ForDirectories.List, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForDirectory_RandomTestForRootDirectory()
        {
            var filesProv = MockFilesProvider();

            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForDirectories.ResourceMasterPrefix +
                                     AuthTools.GetDirectoryName(filesProv, "/"),
                Actions.ForDirectories.List, "U.User", Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(AuthChecker.CheckActionForDirectory(filesProv, "/", Actions.ForDirectories.List,
                "User", new string[0]), "Permission should be granted");

            Assert.IsFalse(AuthChecker.CheckActionForDirectory(filesProv, "/", Actions.ForDirectories.List,
                "User2", new string[0]), "Permission should be denied");
        }

        [Test]
        public void CheckActionForGlobals_AdminBypass()
        {
            Collectors.SettingsProvider = MockProvider();
            Assert.IsTrue(AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageAccounts, "admin", new string[0]),
                "Admin account should bypass security");
        }

        [Test]
        public void CheckActionForGlobals_DenyGroupExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.ForGlobals.ManageFiles, "G.Group",
                Value.Deny));
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.ForGlobals.ManageFiles,
                "G.Group10", Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageGroups, "User", new[] {"Group"}),
                "Permission should be denied");
            Assert.IsFalse(AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageFiles, "User", new[] {"Group"}),
                "Permission should be denied");
            Assert.IsFalse(AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageFiles, "User2", new[] {"Group"}),
                "Permission should be denied");
        }

        [Test]
        public void CheckActionForGlobals_DenyGroupFullControl()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "G.Group", Value.Deny));
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "G.Group10",
                Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageGroups, "User", new[] {"Group"}),
                "Permission should be denied");
            Assert.IsFalse(AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageFiles, "User", new[] {"Group"}),
                "Permission should be denied");
            Assert.IsFalse(AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageFiles, "User2", new[] {"Group"}),
                "Permission should be denied");
        }

        [Test]
        public void CheckActionForGlobals_DenyUserExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.ForGlobals.ManageFiles, "U.User",
                Value.Deny));
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.ForGlobals.ManageFiles, "U.User10",
                Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageGroups, "User", new[] {"Group"}),
                "Permission should be denied");
            Assert.IsFalse(AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageFiles, "User", new[] {"Group"}),
                "Permission should be denied");
            Assert.IsFalse(AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageFiles, "User2", new[] {"Group"}),
                "Permission should be denied");
        }

        [Test]
        public void CheckActionForGlobals_DenyUserFullControl()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "U.User", Value.Deny));
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "U.User10",
                Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageGroups, "User", new[] {"Group"}),
                "Permission should be denied");
            Assert.IsFalse(AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageFiles, "User", new[] {"Group"}),
                "Permission should be denied");
            Assert.IsFalse(AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageFiles, "User2", new[] {"Group"}),
                "Permission should be denied");
        }

        [Test]
        public void CheckActionForGlobals_GrantGroupExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.ForGlobals.ManageFiles, "G.Group",
                Value.Grant));
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.ForGlobals.ManageFiles,
                "G.Group10", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageGroups, "User", new[] {"Group"}),
                "Permission should be denied");
            Assert.IsTrue(AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageFiles, "User", new[] {"Group"}),
                "Permission should be granted");
            Assert.IsFalse(AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageFiles, "User", new[] {"Group2"}),
                "Permission should be denied");
        }

        [Test]
        public void CheckActionForGlobals_GrantGroupFullControl()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "G.Group",
                Value.Grant));
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "G.Group10",
                Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageGroups, "User", new[] {"Group"}),
                "Permission should be granted");
            Assert.IsTrue(AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageFiles, "User", new[] {"Group"}),
                "Permission should be granted");
            Assert.IsFalse(
                AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageGroups, "User", new[] {"Group2"}),
                "Permission should be denied");
        }

        [Test]
        public void CheckActionForGlobals_GrantUserExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.ForGlobals.ManageFiles, "U.User",
                Value.Grant));
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.ForGlobals.ManageFiles, "U.User10",
                Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageGroups, "User", new[] {"Group"}),
                "Permission should be denied");
            Assert.IsTrue(AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageFiles, "User", new[] {"Group"}),
                "Permission should be granted");
            Assert.IsFalse(AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageFiles, "User2", new[] {"Group"}),
                "Permission should be denied");
        }

        [Test]
        public void CheckActionForGlobals_GrantUserFullControl()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "U.User10",
                Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageGroups, "User", new[] {"Group"}),
                "Permission should be granted");
            Assert.IsTrue(AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageFiles, "User", new[] {"Group"}),
                "Permission should be granted");
            Assert.IsFalse(
                AuthChecker.CheckActionForGlobals(Actions.ForGlobals.ManageGroups, "User2", new[] {"Group"}),
                "Permission should be denied");
        }

        [Test]
        public void CheckActionForNamespace_AdminBypass()
        {
            Collectors.SettingsProvider = MockProvider();
            Assert.IsTrue(
                AuthChecker.CheckActionForNamespace(null, Actions.ForNamespaces.CreatePages, "admin", new string[0]),
                "Admin account should bypass security");
        }

        [Test]
        public void CheckActionForNamespace_DenyGroupExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Namespace",
                Actions.ForNamespaces.ManagePages, "G.Group", Value.Deny));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Namespace",
                Actions.ForNamespaces.ManagePages, "G.Group100", Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManageCategories, "User", new[] {"Group"}), "Permission should be denied");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group"}), "Permission should be denied");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group2"}), "Permission should be denied");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace2", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForNamespace_DenyGroupFullControl()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Namespace", Actions.FullControl,
                "G.Group", Value.Deny));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Namespace", Actions.FullControl,
                "G.Group100", Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManageCategories, "User", new[] {"Group"}), "Permission should be denied");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group"}), "Permission should be denied");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group2"}), "Permission should be denied");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace2", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForNamespace_DenyUserExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Namespace",
                Actions.ForNamespaces.ManagePages, "U.User", Value.Deny));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Namespace",
                Actions.ForNamespaces.ManagePages, "U.User100", Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManageCategories, "User", new[] {"Group"}), "Permission should be denied");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group"}), "Permission should be denied");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManagePages, "User2", new[] {"Group"}), "Permission should be denied");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace2", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForNamespace_DenyUserFullControl()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Namespace", Actions.FullControl,
                "U.User", Value.Deny));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Namespace", Actions.FullControl,
                "U.User100", Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManageCategories, "User", new[] {"Group"}), "Permission should be denied");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group"}), "Permission should be denied");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManagePages, "User2", new[] {"Group"}), "Permission should be denied");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace2", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForNamespace_GrantGroupExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Namespace",
                Actions.ForNamespaces.ManagePages, "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Namespace",
                Actions.ForNamespaces.ManagePages, "G.Group100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManageCategories, "User", new[] {"Group"}), "Permission should be denied");
            Assert.IsTrue(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group"}), "Permission should be granted");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group2"}), "Permission should be denied");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace2", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForNamespace_GrantGroupFullControl()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Namespace", Actions.FullControl,
                "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Namespace", Actions.FullControl,
                "G.Group100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManageCategories, "User", new[] {"Group"}), "Permission should be granted");
            Assert.IsTrue(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group"}), "Permission should be granted");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group2"}), "Permission should be denied");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace2", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForNamespace_GrantGroupGlobalEscalator()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix,
                Actions.ForGlobals.ManagePagesAndCategories, "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix,
                Actions.ForGlobals.ManagePagesAndCategories, "G.Group100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManageCategories, "User", new[] {"Group"}), "Permission should be granted");
            Assert.IsTrue(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group"}), "Permission should be granted");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group2"}), "Permission should be denied");
            Assert.IsTrue(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace2", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForNamespace_GrantGroupGlobalEscalator_DenyUserExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.ForGlobals.ManageNamespaces,
                "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "NS",
                Actions.ForNamespaces.ModifyPages, "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("NS", null, null),
                    Actions.ForNamespaces.ModifyPages, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForNamespace_GrantGroupGlobalFullControl()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "G.Group",
                Value.Grant));
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "G.Group100",
                Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManageCategories, "User", new[] {"Group"}), "Permission should be granted");
            Assert.IsTrue(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group"}), "Permission should be granted");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group2"}), "Permission should be denied");
            Assert.IsTrue(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace2", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForNamespace_GrantGroupGlobalFullControl_DenyUserExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "G.Group",
                Value.Grant));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "NS",
                Actions.ForNamespaces.ModifyPages, "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("NS", null, null),
                    Actions.ForNamespaces.ModifyPages, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForNamespace_GrantGroupLocalEscalator()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Namespace",
                Actions.ForNamespaces.ManagePages, "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Namespace",
                Actions.ForNamespaces.ManagePages, "G.Group100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ReadPages, "User", new[] {"Group"}), "Permission should be granted");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ReadPages, "User", new[] {"Group2"}), "Permission should be denied");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace2", null, null),
                    Actions.ForNamespaces.ReadPages, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForNamespace_GrantGroupLocalEscalator_DenyUserExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "NS",
                Actions.ForNamespaces.ManagePages, "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "NS", Actions.ForNamespaces.ReadPages,
                "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("NS", null, null), Actions.ForNamespaces.ReadPages,
                    "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForNamespace_GrantGroupRootEscalator_DenyUserExplicitSub()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix,
                Actions.ForNamespaces.ManagePages, "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Sub",
                Actions.ForNamespaces.ManagePages, "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Sub", null, null),
                    Actions.ForNamespaces.ManagePages,
                    "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForNamespace_GrantUserExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Namespace",
                Actions.ForNamespaces.ManagePages, "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Namespace",
                Actions.ForNamespaces.ManagePages, "U.User100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManageCategories, "User", new[] {"Group"}), "Permission should be denied");
            Assert.IsTrue(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group"}), "Permission should be granted");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManagePages, "User2", new[] {"Group"}), "Permission should be denied");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace2", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForNamespace_GrantUserFullControl()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Namespace", Actions.FullControl,
                "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Namespace", Actions.FullControl,
                "U.User100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManageCategories, "User", new[] {"Group"}), "Permission should be granted");
            Assert.IsTrue(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group"}), "Permission should be granted");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManagePages, "User2", new[] {"Group"}), "Permission should be denied");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace2", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForNamespace_GrantUserGlobalEscalator()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix,
                Actions.ForGlobals.ManagePagesAndCategories, "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix,
                Actions.ForGlobals.ManagePagesAndCategories, "U.User100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManageCategories, "User", new[] {"Group"}), "Permission should be granted");
            Assert.IsTrue(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group"}), "Permission should be granted");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManagePages, "User2", new[] {"Group"}), "Permission should be denied");
            Assert.IsTrue(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace2", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForNamespace_GrantUserGlobalEscalator_DenyUserExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.ForGlobals.ManageNamespaces,
                "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "NS",
                Actions.ForNamespaces.ModifyPages, "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("NS", null, null),
                    Actions.ForNamespaces.ModifyPages, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForNamespace_GrantUserGlobalFullControl()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "U.User100",
                Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManageCategories, "User", new[] {"Group"}), "Permission should be granted");
            Assert.IsTrue(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group"}), "Permission should be granted");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ManagePages, "User2", new[] {"Group"}), "Permission should be denied");
            Assert.IsTrue(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace2", null, null),
                    Actions.ForNamespaces.ManagePages, "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForNamespace_GrantUserGlobalFullControl_DenyUserExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "NS",
                Actions.ForNamespaces.ModifyPages, "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("NS", null, null),
                    Actions.ForNamespaces.ModifyPages, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForNamespace_GrantUserLocalEscalator()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Namespace",
                Actions.ForNamespaces.ManagePages, "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Namespace",
                Actions.ForNamespaces.ManagePages, "U.User100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ReadPages, "User", new[] {"Group"}), "Permission should be granted");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace", null, null),
                    Actions.ForNamespaces.ReadPages, "User2", new[] {"Group"}), "Permission should be denied");
            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Namespace2", null, null),
                    Actions.ForNamespaces.ReadPages, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForNamespace_GrantUserLocalEscalator_DenyUserExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "NS",
                Actions.ForNamespaces.ManagePages, "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "NS", Actions.ForNamespaces.ReadPages,
                "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("NS", null, null), Actions.ForNamespaces.ReadPages,
                    "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForNamespace_GrantUserRootEscalator()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix,
                Actions.ForNamespaces.ManagePages, "U.User", Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Sub", null, null),
                    Actions.ForNamespaces.ManagePages,
                    "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForNamespace_GrantUserRootEscalator_DenyGroupExplicitSub()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix,
                Actions.ForNamespaces.ManagePages, "G.Group", Value.Deny));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Sub",
                Actions.ForNamespaces.ManagePages, "U.User", Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Sub", null, null),
                    Actions.ForNamespaces.ManagePages,
                    "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForNamespace_GrantUserRootEscalator_DenyUserExplicitSub()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix,
                Actions.ForNamespaces.ManagePages, "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Sub",
                Actions.ForNamespaces.ManagePages, "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("Sub", null, null),
                    Actions.ForNamespaces.ManagePages,
                    "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForNamespace_NullNamespace()
        {
            // No exceptions should be thrown
            Collectors.SettingsProvider = MockProvider();
            AuthChecker.CheckActionForNamespace(null, Actions.ForNamespaces.CreatePages, "User", new string[0]);
        }

        [Test]
        public void CheckActionForNamespace_RandomTestForRootNamespace()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix,
                Actions.ForNamespaces.ReadPages, "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix,
                Actions.ForNamespaces.ReadPages, "U.User100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(AuthChecker.CheckActionForNamespace(null, Actions.ForNamespaces.ManageCategories,
                "User", new[] {"Group"}), "Permission should be denied");

            Assert.IsTrue(AuthChecker.CheckActionForNamespace(null, Actions.ForNamespaces.ReadPages,
                "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsFalse(AuthChecker.CheckActionForNamespace(null, Actions.ForNamespaces.ReadPages,
                "User2", new[] {"Group"}), "Permission should be denied");

            Assert.IsTrue(
                AuthChecker.CheckActionForNamespace(new NamespaceInfo("NS", null, null), Actions.ForNamespaces.ReadPages,
                    "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForPage_AdminBypass()
        {
            Collectors.SettingsProvider = MockProvider();
            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ManagePage,
                    "admin", new string[0]), "Admin account should bypass security");
        }

        [Test]
        public void CheckActionForPage_DenyGroupExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage,
                "G.Group", Value.Deny));
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage,
                "G.Group100", Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now),
                    Actions.ForPages.DeleteAttachments,
                    "User", new[] {"Group"}), "Permission should be denied");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be denied");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now),
                    Actions.ForPages.DeleteAttachments,
                    "User", new[] {"Group2"}), "Permission should be denied");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Inexistent", null, DateTime.Now),
                    Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForPage_DenyGroupFullControl()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.FullControl, "G.Group",
                Value.Deny));
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.FullControl, "G.Group100",
                Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now),
                    Actions.ForPages.DeleteAttachments,
                    "User", new[] {"Group"}), "Permission should be denied");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be denied");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now),
                    Actions.ForPages.DeleteAttachments,
                    "User", new[] {"Group2"}), "Permission should be denied");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Inexistent", null, DateTime.Now),
                    Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForPage_DenyGroupFullControl_GrantGroupExplicitNamespace()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "G.Group", Value.Deny));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "NS1", Actions.FullControl, "G.Group",
                Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);
            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo(NameTools.GetFullName("NS1", "Page"), null, DateTime.Now),
                    Actions.ForPages.ModifyPage, "User", new[] {"Group"}), "Permission should be granted");
            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo(NameTools.GetFullName("NS1", "Page"), null, DateTime.Now),
                    Actions.ForPages.ReadPage, "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForPage_DenyGroupFullControl_GrantGroupNamespaceEscalator()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "G.Group", Value.Deny));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.FullControl, "G.Group",
                Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);
            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo(NameTools.GetFullName("NS1", "Page"), null, DateTime.Now),
                    Actions.ForPages.ModifyPage, "User", new[] {"Group"}), "Permission should be granted");
            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo(NameTools.GetFullName("NS1", "Page"), null, DateTime.Now),
                    Actions.ForPages.ReadPage, "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForPage_DenyGroupFullControl_GrantGroupReadPagesExplicitNamespace()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "G.Group", Value.Deny));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "NS1", Actions.ForNamespaces.ReadPages,
                "G.Group", Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);
            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo(NameTools.GetFullName("NS1", "Page"), null, DateTime.Now),
                    Actions.ForPages.ModifyPage, "User", new[] {"Group"}), "Permission should be denied");
            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo(NameTools.GetFullName("NS1", "Page"), null, DateTime.Now),
                    Actions.ForPages.ReadPage, "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForPage_DenyGroupFullControl_GrantGroupReadPagesExplicitNamespaceLocalEscalator()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "G.Group", Value.Deny));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "NS1",
                Actions.ForNamespaces.ManagePages, "G.Group", Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);
            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo(NameTools.GetFullName("NS1", "Page"), null, DateTime.Now),
                    Actions.ForPages.ModifyPage, "User", new[] {"Group"}), "Permission should be granted");
            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo(NameTools.GetFullName("NS1", "Page"), null, DateTime.Now),
                    Actions.ForPages.ReadPage, "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForPage_DenyGroupFullControl_GrantGroupReadPagesNamespaceEscalator()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "G.Group", Value.Deny));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ReadPages,
                "G.Group", Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);
            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo(NameTools.GetFullName("NS1", "Page"), null, DateTime.Now),
                    Actions.ForPages.ModifyPage, "User", new[] {"Group"}), "Permission should be denied");
            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo(NameTools.GetFullName("NS1", "Page"), null, DateTime.Now),
                    Actions.ForPages.ReadPage, "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForPage_DenyGroupFullControl_GrantGroupReadPagesNamespaceEscalatorLocalEscalator()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "G.Group", Value.Deny));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ManagePages,
                "G.Group", Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);
            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo(NameTools.GetFullName("NS1", "Page"), null, DateTime.Now),
                    Actions.ForPages.ModifyPage, "User", new[] {"Group"}), "Permission should be granted");
            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo(NameTools.GetFullName("NS1", "Page"), null, DateTime.Now),
                    Actions.ForPages.ReadPage, "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForPage_DenyUserExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage,
                "U.User", Value.Deny));
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage,
                "U.User100", Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now),
                    Actions.ForPages.DeleteAttachments,
                    "User", new[] {"Group"}), "Permission should be denied");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be denied");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now),
                    Actions.ForPages.DeleteAttachments,
                    "User2", new[] {"Group"}), "Permission should be denied");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Inexistent", null, DateTime.Now),
                    Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForPage_DenyUserFullControl()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.FullControl, "U.User",
                Value.Deny));
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.FullControl, "U.User100",
                Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now),
                    Actions.ForPages.DeleteAttachments,
                    "User", new[] {"Group"}), "Permission should be denied");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be denied");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now),
                    Actions.ForPages.DeleteAttachments,
                    "User2", new[] {"Group"}), "Permission should be denied");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Inexistent", null, DateTime.Now),
                    Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForPage_GrantGroupExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage,
                "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage,
                "G.Group100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now),
                    Actions.ForPages.DeleteAttachments,
                    "User", new[] {"Group"}), "Permission should be denied");

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now),
                    Actions.ForPages.DeleteAttachments,
                    "User", new[] {"Group2"}), "Permission should be denied");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Inexistent", null, DateTime.Now),
                    Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForPage_GrantGroupFullControl()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.FullControl, "G.Group",
                Value.Grant));
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.FullControl, "G.Group100",
                Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now),
                    Actions.ForPages.DeleteAttachments,
                    "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now),
                    Actions.ForPages.DeleteAttachments,
                    "User", new[] {"Group2"}), "Permission should be denied");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Inexistent", null, DateTime.Now),
                    Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForPage_GrantGroupFullControl_DenyGroupExplicitNamespace_ExceptReadPages()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "G.Group",
                Value.Grant));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "NS1", Actions.ForNamespaces.ReadPages,
                "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "NS1", Actions.FullControl, "G.Group",
                Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);
            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo(NameTools.GetFullName("NS1", "Page"), null, DateTime.Now),
                    Actions.ForPages.ModifyPage, "User", new[] {"Group"}), "Permission should be denied");
            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo(NameTools.GetFullName("NS1", "Page"), null, DateTime.Now),
                    Actions.ForPages.ReadPage, "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForPage_GrantGroupFullControl_DenyGroupNamespaceEscalator_ExceptReadPages()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "G.Group",
                Value.Grant));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ReadPages,
                "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.FullControl, "G.Group",
                Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);
            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo(NameTools.GetFullName("NS1", "Page"), null, DateTime.Now),
                    Actions.ForPages.ModifyPage, "User", new[] {"Group"}), "Permission should be denied");
            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo(NameTools.GetFullName("NS1", "Page"), null, DateTime.Now),
                    Actions.ForPages.ReadPage, "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForPage_GrantGroupGlobalEscalator()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix,
                Actions.ForGlobals.ManagePagesAndCategories, "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix,
                Actions.ForGlobals.ManagePagesAndCategories, "G.Group100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ReadPage,
                    "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ReadPage,
                    "User", new[] {"Group2"}), "Permission should be denied");

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page2", null, DateTime.Now), Actions.ForPages.ReadPage,
                    "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForPage_GrantGroupGlobalEscalator_DenyUserExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix,
                Actions.ForGlobals.ManagePagesAndCategories, "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage,
                "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForPage_GrantGroupGlobalFullControl()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "G.Group",
                Value.Grant));
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "G.Group100",
                Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ReadPage,
                    "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ReadPage,
                    "User", new[] {"Group2"}), "Permission should be denied");
            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page2", null, DateTime.Now), Actions.ForPages.ReadPage,
                    "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForPage_GrantGroupGlobalFullControl_DenyUserExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "G.Group",
                Value.Grant));
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage,
                "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForPage_GrantGroupLocalEscalator()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage,
                "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage,
                "G.Group100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ReadPage,
                    "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ReadPage,
                    "User", new[] {"Group2"}), "Permission should be denied");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page2", null, DateTime.Now), Actions.ForPages.ReadPage,
                    "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForPage_GrantGroupLocalEscalator_DenyUserExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage,
                "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ReadPage, "U.User",
                Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ReadPage,
                    "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForPage_GrantGroupNamespaceEscalator()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ModifyPages,
                "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ModifyPages,
                "G.Group100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ReadPage,
                    "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ReadPage,
                    "User", new[] {"Group2"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForPage_GrantGroupNamespaceEscalator_DenyUserExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ModifyPages,
                "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage,
                "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForPage_GrantGroupRootEscalator_DenyUserExplicitPage()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix,
                Actions.ForNamespaces.ModifyPages, "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + NameTools.GetFullName("Sub", "Page"),
                Actions.ForPages.ModifyPage, "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo(NameTools.GetFullName("Sub", "Page"), null, DateTime.Now),
                    Actions.ForPages.ModifyPage, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForPage_GrantGroupRootEscalator_DenyUserExplicitSub()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix,
                Actions.ForNamespaces.ModifyPages, "G.Group", Value.Grant));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Sub",
                Actions.ForNamespaces.ModifyPages, "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo(NameTools.GetFullName("Sub", "Page"), null, DateTime.Now),
                    Actions.ForPages.ModifyPage, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForPage_GrantUserExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage,
                "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage,
                "U.User100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now),
                    Actions.ForPages.DeleteAttachments,
                    "User", new[] {"Group"}), "Permission should be denied");

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now),
                    Actions.ForPages.DeleteAttachments,
                    "User2", new[] {"Group"}), "Permission should be denied");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Inexistent", null, DateTime.Now),
                    Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForPage_GrantUserFullControl()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.FullControl, "U.User",
                Value.Grant));
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.FullControl, "U.User100",
                Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now),
                    Actions.ForPages.DeleteAttachments,
                    "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now),
                    Actions.ForPages.DeleteAttachments,
                    "User2", new[] {"Group"}), "Permission should be denied");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Inexistent", null, DateTime.Now),
                    Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForPage_GrantUserGlobalEscalator()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix,
                Actions.ForGlobals.ManagePagesAndCategories, "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix,
                Actions.ForGlobals.ManagePagesAndCategories, "U.User100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ReadPage,
                    "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ReadPage,
                    "User2", new[] {"Group"}), "Permission should be denied");

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page2", null, DateTime.Now), Actions.ForPages.ReadPage,
                    "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForPage_GrantUserGlobalEscalator_DenyUserExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix,
                Actions.ForGlobals.ManagePagesAndCategories, "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage,
                "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForPage_GrantUserGlobalFullControl()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "U.User100",
                Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ReadPage,
                    "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ReadPage,
                    "User2", new[] {"Group"}), "Permission should be denied");

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page2", null, DateTime.Now), Actions.ForPages.ReadPage,
                    "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForPage_GrantUserGlobalFullControl_DenyUserExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForGlobals.ResourceMasterPrefix, Actions.FullControl, "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage,
                "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForPage_GrantUserLocalEscalator()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage,
                "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage,
                "U.User100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ReadPage,
                    "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ReadPage,
                    "User2", new[] {"Group"}), "Permission should be denied");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page2", null, DateTime.Now), Actions.ForPages.ReadPage,
                    "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForPage_GrantUserLocalEscalator_DenyUserExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage,
                "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ReadPage, "U.User",
                Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ReadPage,
                    "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForPage_GrantUserNamespaceEscalator()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ModifyPages,
                "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ModifyPages,
                "U.User100", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ReadPage,
                    "User", new[] {"Group"}), "Permission should be granted");

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ReadPage,
                    "User2", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForPage_GrantUserNamespaceEscalator_DenyUserExplicit()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix, Actions.ForNamespaces.ModifyPages,
                "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage,
                "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ModifyPage,
                    "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForPage_GrantUserRootEscalator()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix,
                Actions.ForNamespaces.ModifyPages, "U.User", Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo(NameTools.GetFullName("Sub", "Page"), null, DateTime.Now),
                    Actions.ForPages.ModifyPage, "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForPage_GrantUserRootEscalator_DenyGroupExplicitPage()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix,
                Actions.ForNamespaces.ModifyPages, "G.Group", Value.Deny));
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + NameTools.GetFullName("Sub", "Page"),
                Actions.ForPages.ModifyPage, "U.User", Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo(NameTools.GetFullName("Sub", "Page"), null, DateTime.Now),
                    Actions.ForPages.ModifyPage, "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForPage_GrantUserRootEscalator_DenyGroupExplicitSub()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix,
                Actions.ForNamespaces.ModifyPages, "G.Group", Value.Deny));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Sub",
                Actions.ForNamespaces.ModifyPages, "U.User", Value.Grant));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo(NameTools.GetFullName("Sub", "Page"), null, DateTime.Now),
                    Actions.ForPages.ModifyPage, "User", new[] {"Group"}), "Permission should be granted");
        }

        [Test]
        public void CheckActionForPage_GrantUserRootEscalator_DenyUserExplicitPage()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix,
                Actions.ForNamespaces.ModifyPages, "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + NameTools.GetFullName("Sub", "Page"),
                Actions.ForPages.ModifyPage, "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo(NameTools.GetFullName("Sub", "Page"), null, DateTime.Now),
                    Actions.ForPages.ModifyPage, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForPage_GrantUserRootEscalator_DenyUserExplicitSub()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix,
                Actions.ForNamespaces.ModifyPages, "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForNamespaces.ResourceMasterPrefix + "Sub",
                Actions.ForNamespaces.ModifyPages, "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo(NameTools.GetFullName("Sub", "Page"), null, DateTime.Now),
                    Actions.ForPages.ModifyPage, "User", new[] {"Group"}), "Permission should be denied");
        }

        [Test]
        public void CheckActionForPage_RandomTestForSubNamespace()
        {
            var entries = new List<AclEntry>();
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "P.Page", Actions.ForPages.ModifyPage,
                "U.User", Value.Grant));
            entries.Add(new AclEntry(Actions.ForPages.ResourceMasterPrefix + "Page", Actions.ForPages.ModifyPage,
                "U.User", Value.Deny));

            Collectors.SettingsProvider = MockProvider(entries);

            Assert.IsTrue(
                AuthChecker.CheckActionForPage(new PageInfo("P.Page", null, DateTime.Now), Actions.ForPages.ModifyPage,
                    "User", new string[0]), "Permission should be granted");
            Assert.IsFalse(
                AuthChecker.CheckActionForPage(new PageInfo("Page", null, DateTime.Now), Actions.ForPages.ModifyPage,
                    "User", new string[0]), "Permission should be denied");
        }
    }
}