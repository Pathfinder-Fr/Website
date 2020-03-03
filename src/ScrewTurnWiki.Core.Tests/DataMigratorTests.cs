using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using ScrewTurn.Wiki.AclEngine;
using ScrewTurn.Wiki.PluginFramework;
using RMC = Rhino.Mocks.Constraints;

namespace ScrewTurn.Wiki.Tests
{
    [TestFixture]
    public class DataMigratorTests
    {
        private delegate bool RetrieveFile(string file, Stream stream, bool count);

        private delegate bool RetrieveAttachment(PageInfo page, string file, Stream stream, bool count);

        private delegate bool StoreFile(string file, Stream stream, bool overwrite);

        private delegate bool StoreAttachment(PageInfo page, string file, Stream stream, bool overwrite);

        [Test]
        public void CopySettingsStorageProviderData()
        {
            var mocks = new MockRepository();

            var source = mocks.StrictMock<ISettingsStorageProviderV30>();
            var destination = mocks.StrictMock<ISettingsStorageProviderV30>();
            var sourceAclManager = mocks.StrictMock<IAclManager>();
            var destinationAclManager = mocks.StrictMock<IAclManager>();

            // Setup SOURCE ---------------------

            // Settings
            var settings = new Dictionary<string, string>
            {
                {"Set1", "Value1"},
                {"Set2", "Value2"}
            };
            Expect.Call(source.GetAllSettings()).Return(settings);

            // Meta-data (global)
            Expect.Call(source.GetMetaDataItem(MetaDataItem.AccountActivationMessage, null)).Return("AAM");
            Expect.Call(source.GetMetaDataItem(MetaDataItem.PasswordResetProcedureMessage, null)).Return("PRM");
            Expect.Call(source.GetMetaDataItem(MetaDataItem.LoginNotice, null)).Return("");
            Expect.Call(source.GetMetaDataItem(MetaDataItem.PageChangeMessage, null)).Return("PCM");
            Expect.Call(source.GetMetaDataItem(MetaDataItem.DiscussionChangeMessage, null)).Return("DCM");

            // Meta-data (root)
            Expect.Call(source.GetMetaDataItem(MetaDataItem.EditNotice, null)).Return("");
            Expect.Call(source.GetMetaDataItem(MetaDataItem.Footer, null)).Return("FOOT");
            Expect.Call(source.GetMetaDataItem(MetaDataItem.Header, null)).Return("HEADER");
            Expect.Call(source.GetMetaDataItem(MetaDataItem.HtmlHead, null)).Return("HTML");
            Expect.Call(source.GetMetaDataItem(MetaDataItem.PageFooter, null)).Return("P_FOOT");
            Expect.Call(source.GetMetaDataItem(MetaDataItem.PageHeader, null)).Return("P_HEADER");
            Expect.Call(source.GetMetaDataItem(MetaDataItem.Sidebar, null)).Return("SIDEBAR");

            // Meta-data ("NS" namespace)
            Expect.Call(source.GetMetaDataItem(MetaDataItem.EditNotice, "NS")).Return("NS_EDIT");
            Expect.Call(source.GetMetaDataItem(MetaDataItem.Footer, "NS")).Return("NS_FOOT");
            Expect.Call(source.GetMetaDataItem(MetaDataItem.Header, "NS")).Return("NS_HEADER");
            Expect.Call(source.GetMetaDataItem(MetaDataItem.HtmlHead, "NS")).Return("NS_HTML");
            Expect.Call(source.GetMetaDataItem(MetaDataItem.PageFooter, "NS")).Return("NS_P_FOOT");
            Expect.Call(source.GetMetaDataItem(MetaDataItem.PageHeader, "NS")).Return("NS_P_HEADER");
            Expect.Call(source.GetMetaDataItem(MetaDataItem.Sidebar, "NS")).Return("NS_SIDEBAR");

            // Plugin assemblies
            byte[] asm1 = { 1, 2, 3, 4, 5 };
            byte[] asm2 = { 6, 7, 8, 9, 10, 11, 12 };
            Expect.Call(source.ListPluginAssemblies()).Return(new[] { "Plugins1.dll", "Plugins2.dll" });
            Expect.Call(source.RetrievePluginAssembly("Plugins1.dll")).Return(asm1);
            Expect.Call(source.RetrievePluginAssembly("Plugins2.dll")).Return(asm2);

            // Plugin status
            Expect.Call(source.GetPluginStatus("Test1.Plugin1")).Return(true);
            Expect.Call(source.GetPluginStatus("Test2.Plugin2")).Return(false);

            // Plugin config
            Expect.Call(source.GetPluginConfiguration("Test1.Plugin1")).Return("Config1");
            Expect.Call(source.GetPluginConfiguration("Test2.Plugin2")).Return("");

            // Outgoing links
            var outgoingLinks = new Dictionary<string, string[]>
            {
                {"Page1", new[] {"Page2", "Page3"}},
                {"Page2", new[] {"Page3"}},
                {"Page3", new[] {"Page4", "Page3"}}
            };
            Expect.Call(source.GetAllOutgoingLinks()).Return(outgoingLinks);

            // ACLs
            Expect.Call(source.AclManager).Return(sourceAclManager);
            AclEntry[] entries =
            {
                new AclEntry("Res1", "Act1", "Subj1", Value.Grant),
                new AclEntry("Res2", "Act2", "Subj2", Value.Deny)
            };
            Expect.Call(sourceAclManager.RetrieveAllEntries()).Return(entries);

            // Setup DESTINATION -----------------

            // Settings
            destination.BeginBulkUpdate();
            LastCall.On(destination).Repeat.Once();
            foreach (var pair in settings)
            {
                Expect.Call(destination.SetSetting(pair.Key, pair.Value)).Return(true);
            }
            destination.EndBulkUpdate();
            LastCall.On(destination).Repeat.Once();

            // Meta-data (global)
            Expect.Call(destination.SetMetaDataItem(MetaDataItem.AccountActivationMessage, null, "AAM")).Return(true);
            Expect.Call(destination.SetMetaDataItem(MetaDataItem.PasswordResetProcedureMessage, null, "PRM"))
                .Return(true);
            Expect.Call(destination.SetMetaDataItem(MetaDataItem.LoginNotice, null, "")).Return(true);
            Expect.Call(destination.SetMetaDataItem(MetaDataItem.PageChangeMessage, null, "PCM")).Return(true);
            Expect.Call(destination.SetMetaDataItem(MetaDataItem.DiscussionChangeMessage, null, "DCM")).Return(true);

            // Meta-data (root)
            Expect.Call(destination.SetMetaDataItem(MetaDataItem.EditNotice, null, "")).Return(true);
            Expect.Call(destination.SetMetaDataItem(MetaDataItem.Footer, null, "FOOT")).Return(true);
            Expect.Call(destination.SetMetaDataItem(MetaDataItem.Header, null, "HEADER")).Return(true);
            Expect.Call(destination.SetMetaDataItem(MetaDataItem.HtmlHead, null, "HTML")).Return(true);
            Expect.Call(destination.SetMetaDataItem(MetaDataItem.PageFooter, null, "P_FOOT")).Return(true);
            Expect.Call(destination.SetMetaDataItem(MetaDataItem.PageHeader, null, "P_HEADER")).Return(true);
            Expect.Call(destination.SetMetaDataItem(MetaDataItem.Sidebar, null, "SIDEBAR")).Return(true);

            // Meta-data ("NS" namespace)
            Expect.Call(destination.SetMetaDataItem(MetaDataItem.EditNotice, "NS", "NS_EDIT")).Return(true);
            Expect.Call(destination.SetMetaDataItem(MetaDataItem.Footer, "NS", "NS_FOOT")).Return(true);
            Expect.Call(destination.SetMetaDataItem(MetaDataItem.Header, "NS", "NS_HEADER")).Return(true);
            Expect.Call(destination.SetMetaDataItem(MetaDataItem.HtmlHead, "NS", "NS_HTML")).Return(true);
            Expect.Call(destination.SetMetaDataItem(MetaDataItem.PageFooter, "NS", "NS_P_FOOT")).Return(true);
            Expect.Call(destination.SetMetaDataItem(MetaDataItem.PageHeader, "NS", "NS_P_HEADER")).Return(true);
            Expect.Call(destination.SetMetaDataItem(MetaDataItem.Sidebar, "NS", "NS_SIDEBAR")).Return(true);

            // Plugin assemblies
            Expect.Call(destination.StorePluginAssembly("Plugins1.dll", asm1)).Return(true);
            Expect.Call(destination.StorePluginAssembly("Plugins2.dll", asm2)).Return(true);

            // Plugin status
            Expect.Call(destination.SetPluginStatus("Test1.Plugin1", true)).Return(true);
            Expect.Call(destination.SetPluginStatus("Test2.Plugin2", false)).Return(true);

            // Plugin config
            Expect.Call(destination.SetPluginConfiguration("Test1.Plugin1", "Config1")).Return(true);
            Expect.Call(destination.SetPluginConfiguration("Test2.Plugin2", "")).Return(true);

            // Outgoing links
            foreach (var pair in outgoingLinks)
            {
                Expect.Call(destination.StoreOutgoingLinks(pair.Key, pair.Value)).Return(true);
            }

            // ACLs
            Expect.Call(destination.AclManager).Return(destinationAclManager).Repeat.Any();
            foreach (var e in entries)
            {
                Expect.Call(destinationAclManager.StoreEntry(e.Resource, e.Action, e.Subject, e.Value)).Return(true);
            }

            mocks.ReplayAll();

            DataMigrator.CopySettingsStorageProviderData(source, destination,
                new[] { "NS" }, new[] { "Test1.Plugin1", "Test2.Plugin2" });

            mocks.VerifyAll();
        }

        [Test]
        public void MigrateFilesStorageProviderData()
        {
            var mocks = new MockRepository();

            var source = mocks.StrictMock<IFilesStorageProviderV30>();
            var destination = mocks.StrictMock<IFilesStorageProviderV30>();
            var settingsProvider = mocks.StrictMock<ISettingsStorageProviderV30>();
            var aclManager = mocks.StrictMock<IAclManager>();
            Expect.Call(settingsProvider.AclManager).Return(aclManager).Repeat.Any();

            // Setup SOURCE -----------------

            // Directories
            Expect.Call(source.ListDirectories("/")).Return(new[] { "/Dir1/", "/Dir2/" });
            Expect.Call(source.ListDirectories("/Dir1/")).Return(new[] { "/Dir1/Sub/" });
            Expect.Call(source.ListDirectories("/Dir2/")).Return(new string[0]);
            Expect.Call(source.ListDirectories("/Dir1/Sub/")).Return(new string[0]);

            // Settings (permissions)
            Expect.Call(aclManager.RenameResource(
                Actions.ForDirectories.ResourceMasterPrefix + AuthTools.GetDirectoryName(source, "/"),
                Actions.ForDirectories.ResourceMasterPrefix + AuthTools.GetDirectoryName(destination, "/")))
                .Return(true);
            Expect.Call(aclManager.RenameResource(
                Actions.ForDirectories.ResourceMasterPrefix + AuthTools.GetDirectoryName(source, "/Dir1/"),
                Actions.ForDirectories.ResourceMasterPrefix + AuthTools.GetDirectoryName(destination, "/Dir1/")))
                .Return(true);
            Expect.Call(aclManager.RenameResource(
                Actions.ForDirectories.ResourceMasterPrefix + AuthTools.GetDirectoryName(source, "/Dir2/"),
                Actions.ForDirectories.ResourceMasterPrefix + AuthTools.GetDirectoryName(destination, "/Dir2/")))
                .Return(true);
            Expect.Call(aclManager.RenameResource(
                Actions.ForDirectories.ResourceMasterPrefix + AuthTools.GetDirectoryName(source, "/Dir1/Sub/"),
                Actions.ForDirectories.ResourceMasterPrefix + AuthTools.GetDirectoryName(destination, "/Dir1/Sub/")))
                .Return(true);

            // Filenames
            Expect.Call(source.ListFiles("/")).Return(new[] { "/File1.txt", "/File2.txt" });
            Expect.Call(source.ListFiles("/Dir1/")).Return(new[] { "/Dir1/File.txt" });
            Expect.Call(source.ListFiles("/Dir2/")).Return(new string[0]);
            Expect.Call(source.ListFiles("/Dir1/Sub/")).Return(new[] { "/Dir1/Sub/File.txt" });

            // File content
            Expect.Call(source.RetrieveFile("/File1.txt", null, false)).Constraints(
                RMC.Is.Equal("/File1.txt"), RMC.Is.TypeOf<Stream>(), RMC.Is.Equal(false)).Do(
                    new RetrieveFile(
                        delegate (string file, Stream stream, bool count)
                        {
                            var stuff = Encoding.Unicode.GetBytes("content1");
                            stream.Write(stuff, 0, stuff.Length);
                            return true;
                        }));

            Expect.Call(source.RetrieveFile("/File2.txt", null, false)).Constraints(
                RMC.Is.Equal("/File2.txt"), RMC.Is.TypeOf<Stream>(), RMC.Is.Equal(false)).Do(
                    new RetrieveFile(
                        delegate (string file, Stream stream, bool count)
                        {
                            var stuff = Encoding.Unicode.GetBytes("content2");
                            stream.Write(stuff, 0, stuff.Length);
                            return true;
                        }));

            Expect.Call(source.RetrieveFile("/Dir1/File.txt", null, false)).Constraints(
                RMC.Is.Equal("/Dir1/File.txt"), RMC.Is.TypeOf<Stream>(), RMC.Is.Equal(false)).Do(
                    new RetrieveFile(
                        delegate (string file, Stream stream, bool count)
                        {
                            var stuff = Encoding.Unicode.GetBytes("content3");
                            stream.Write(stuff, 0, stuff.Length);
                            return true;
                        }));

            Expect.Call(source.RetrieveFile("/Dir1/Sub/File.txt", null, false)).Constraints(
                RMC.Is.Equal("/Dir1/Sub/File.txt"), RMC.Is.TypeOf<Stream>(), RMC.Is.Equal(false)).Do(
                    new RetrieveFile(
                        delegate (string file, Stream stream, bool count)
                        {
                            var stuff = Encoding.Unicode.GetBytes("content4");
                            stream.Write(stuff, 0, stuff.Length);
                            return true;
                        }));

            // File details
            Expect.Call(source.GetFileDetails("/File1.txt")).Return(new FileDetails(8, DateTime.Now, 52));
            Expect.Call(source.GetFileDetails("/File2.txt")).Return(new FileDetails(8, DateTime.Now, 0));
            Expect.Call(source.GetFileDetails("/Dir1/File.txt")).Return(new FileDetails(8, DateTime.Now, 21));
            Expect.Call(source.GetFileDetails("/Dir1/Sub/File.txt")).Return(new FileDetails(8, DateTime.Now, 123));

            // Page attachments
            Expect.Call(source.GetPagesWithAttachments()).Return(new[] { "MainPage", "Sub.Page", "Sub.Another" });
            Expect.Call(source.ListPageAttachments(null))
                .Constraints(RMC.Is.Matching(delegate (PageInfo p) { return p.FullName == "MainPage"; }))
                .Return(new[] { "Attachment.txt" });
            Expect.Call(source.ListPageAttachments(null))
                .Constraints(RMC.Is.Matching(delegate (PageInfo p) { return p.FullName == "Sub.Page"; }))
                .Return(new[] { "Attachment2.txt" });
            Expect.Call(source.ListPageAttachments(null))
                .Constraints(RMC.Is.Matching(delegate (PageInfo p) { return p.FullName == "Sub.Another"; }))
                .Return(new string[0]);

            // Page attachment content
            Expect.Call(source.RetrievePageAttachment(null, "Attachment.txt", null, false)).Constraints(
                RMC.Is.Matching(delegate (PageInfo p) { return p.FullName == "MainPage"; }),
                RMC.Is.Equal("Attachment.txt"), RMC.Is.TypeOf<Stream>(), RMC.Is.Equal(false)).Do(
                    new RetrieveAttachment(
                        delegate (PageInfo page, string name, Stream stream, bool count)
                        {
                            var stuff = Encoding.Unicode.GetBytes("content5");
                            stream.Write(stuff, 0, stuff.Length);
                            return true;
                        }));

            Expect.Call(source.RetrievePageAttachment(null, "Attachment2.txt", null, false)).Constraints(
                RMC.Is.Matching(delegate (PageInfo p) { return p.FullName == "Sub.Page"; }),
                RMC.Is.Equal("Attachment2.txt"), RMC.Is.TypeOf<Stream>(), RMC.Is.Equal(false)).Do(
                    new RetrieveAttachment(
                        delegate (PageInfo page, string name, Stream stream, bool count)
                        {
                            var stuff = Encoding.Unicode.GetBytes("content6");
                            stream.Write(stuff, 0, stuff.Length);
                            return true;
                        }));

            // Attachment details
            Expect.Call(source.GetPageAttachmentDetails(null, "Attachment.txt")).Constraints(
                RMC.Is.Matching(delegate (PageInfo p) { return p.FullName == "MainPage"; }),
                RMC.Is.Equal("Attachment.txt")).Return(new FileDetails(8, DateTime.Now, 8));
            Expect.Call(source.GetPageAttachmentDetails(null, "Attachment2.txt")).Constraints(
                RMC.Is.Matching(delegate (PageInfo p) { return p.FullName == "Sub.Page"; }),
                RMC.Is.Equal("Attachment2.txt")).Return(new FileDetails(8, DateTime.Now, 29));

            // Setup DESTINATION ------------------------

            // Directories
            Expect.Call(destination.CreateDirectory("/", "Dir1")).Return(true);
            Expect.Call(destination.CreateDirectory("/", "Dir2")).Return(true);
            Expect.Call(destination.CreateDirectory("/Dir1/", "Sub")).Return(true);

            // Files
            Expect.Call(destination.StoreFile("/File1.txt", null, false)).Constraints(
                RMC.Is.Equal("/File1.txt"), RMC.Is.TypeOf<Stream>(), RMC.Is.Equal(false)).Do(new StoreFile(
                    delegate (string name, Stream stream, bool overwrite)
                    {
                        var buff = new byte[512];
                        var read = stream.Read(buff, 0, (int)stream.Length);
                        Assert.AreEqual("content1", Encoding.Unicode.GetString(buff, 0, read), "Wrong data");
                        return true;
                    }));

            Expect.Call(destination.StoreFile("/File2.txt", null, false)).Constraints(
                RMC.Is.Equal("/File2.txt"), RMC.Is.TypeOf<Stream>(), RMC.Is.Equal(false)).Do(new StoreFile(
                    delegate (string name, Stream stream, bool overwrite)
                    {
                        var buff = new byte[512];
                        var read = stream.Read(buff, 0, (int)stream.Length);
                        Assert.AreEqual("content2", Encoding.Unicode.GetString(buff, 0, read), "Wrong data");
                        return true;
                    }));

            Expect.Call(destination.StoreFile("/Dir1/File.txt", null, false)).Constraints(
                RMC.Is.Equal("/Dir1/File.txt"), RMC.Is.TypeOf<Stream>(), RMC.Is.Equal(false)).Do(new StoreFile(
                    delegate (string name, Stream stream, bool overwrite)
                    {
                        var buff = new byte[512];
                        var read = stream.Read(buff, 0, (int)stream.Length);
                        Assert.AreEqual("content3", Encoding.Unicode.GetString(buff, 0, read), "Wrong data");
                        return true;
                    }));

            Expect.Call(destination.StoreFile("/Dir1/Sub/File.txt", null, false)).Constraints(
                RMC.Is.Equal("/Dir1/Sub/File.txt"), RMC.Is.TypeOf<Stream>(), RMC.Is.Equal(false)).Do(new StoreFile(
                    delegate (string name, Stream stream, bool overwrite)
                    {
                        var buff = new byte[512];
                        var read = stream.Read(buff, 0, (int)stream.Length);
                        Assert.AreEqual("content4", Encoding.Unicode.GetString(buff, 0, read), "Wrong data");
                        return true;
                    }));

            // File retrieval count
            destination.SetFileRetrievalCount("/File1.txt", 52);
            LastCall.On(destination).Repeat.Once();
            destination.SetFileRetrievalCount("/File2.txt", 0);
            LastCall.On(destination).Repeat.Once();
            destination.SetFileRetrievalCount("/Dir1/File.txt", 21);
            LastCall.On(destination).Repeat.Once();
            destination.SetFileRetrievalCount("/Dir1/Sub/File.txt", 123);
            LastCall.On(destination).Repeat.Once();

            // Page attachments
            Expect.Call(destination.StorePageAttachment(null, "Attachment.txt", null, false)).Constraints(
                RMC.Is.Matching(delegate (PageInfo p) { return p.FullName == "MainPage"; }),
                RMC.Is.Equal("Attachment.txt"), RMC.Is.TypeOf<Stream>(), RMC.Is.Equal(false)).Do(new StoreAttachment(
                    delegate (PageInfo page, string name, Stream stream, bool overwrite)
                    {
                        var buff = new byte[512];
                        var read = stream.Read(buff, 0, (int)stream.Length);
                        Assert.AreEqual("content5", Encoding.Unicode.GetString(buff, 0, read), "Wrong data");
                        return true;
                    }));

            Expect.Call(destination.StorePageAttachment(null, "Attachment2.txt", null, false)).Constraints(
                RMC.Is.Matching(delegate (PageInfo p) { return p.FullName == "Sub.Page"; }),
                RMC.Is.Equal("Attachment2.txt"), RMC.Is.TypeOf<Stream>(), RMC.Is.Equal(false)).Do(new StoreAttachment(
                    delegate (PageInfo page, string name, Stream stream, bool overwrite)
                    {
                        var buff = new byte[512];
                        var read = stream.Read(buff, 0, (int)stream.Length);
                        Assert.AreEqual("content6", Encoding.Unicode.GetString(buff, 0, read), "Wrong data");
                        return true;
                    }));

            // Attachment retrieval count
            destination.SetPageAttachmentRetrievalCount(null, "Attachment.txt", 8);
            LastCall.On(destination)
                .Constraints(RMC.Is.Matching(delegate (PageInfo p) { return p.FullName == "MainPage"; }),
                    RMC.Is.Equal("Attachment.txt"), RMC.Is.Equal(8))
                .Repeat.Once();
            destination.SetPageAttachmentRetrievalCount(null, "Attachment2.txt", 29);
            LastCall.On(destination)
                .Constraints(RMC.Is.Matching(delegate (PageInfo p) { return p.FullName == "Sub.Page"; }),
                    RMC.Is.Equal("Attachment2.txt"), RMC.Is.Equal(29))
                .Repeat.Once();

            // Delete source content
            Expect.Call(source.DeleteFile("/File1.txt")).Return(true);
            Expect.Call(source.DeleteFile("/File2.txt")).Return(true);
            Expect.Call(source.DeleteDirectory("/Dir1/")).Return(true);
            Expect.Call(source.DeleteDirectory("/Dir2/")).Return(true);

            Expect.Call(source.DeletePageAttachment(null, "Attachment.aspx"))
                .Constraints(RMC.Is.Matching(delegate (PageInfo p) { return p.FullName == "MainPage"; }),
                    RMC.Is.Equal("Attachment.txt"))
                .Return(true);
            Expect.Call(source.DeletePageAttachment(null, "Attachment2.aspx"))
                .Constraints(RMC.Is.Matching(delegate (PageInfo p) { return p.FullName == "Sub.Page"; }),
                    RMC.Is.Equal("Attachment2.txt"))
                .Return(true);

            mocks.Replay(source);
            mocks.Replay(destination);
            mocks.Replay(settingsProvider);
            mocks.Replay(aclManager);

            DataMigrator.MigrateFilesStorageProviderData(source, destination, settingsProvider);

            mocks.Verify(source);
            mocks.Verify(destination);
            mocks.Verify(settingsProvider);
            mocks.Verify(aclManager);
        }

        [Test]
        public void MigratePagesStorageProviderData()
        {
            var mocks = new MockRepository();

            var source = mocks.StrictMock<IPagesStorageProviderV30>();
            var destination = mocks.StrictMock<IPagesStorageProviderV30>();

            // Setup SOURCE -------------------------

            // Setup snippets
            var s1 = new Snippet("S1", "Blah1", source);
            var s2 = new Snippet("S2", "Blah2", source);
            Expect.Call(source.GetSnippets()).Return(new[] { s1, s2 });

            // Setup content templates
            var ct1 = new ContentTemplate("CT1", "Template 1", source);
            var ct2 = new ContentTemplate("CT2", "Template 2", source);
            Expect.Call(source.GetContentTemplates()).Return(new[] { ct1, ct2 });

            // Setup namespaces
            var ns1 = new NamespaceInfo("NS1", source, null);
            var ns2 = new NamespaceInfo("NS2", source, null);
            Expect.Call(source.GetNamespaces()).Return(new[] { ns1, ns2 });

            // Setup pages
            var p1 = new PageInfo("Page", source, DateTime.Now);
            var p2 = new PageInfo(NameTools.GetFullName(ns1.Name, "Page"), source, DateTime.Now);
            var p3 = new PageInfo(NameTools.GetFullName(ns1.Name, "Page1"), source, DateTime.Now);
            Expect.Call(source.GetPages(null)).Return(new[] { p1 });
            Expect.Call(source.GetPages(ns1)).Return(new[] { p2, p3 });
            Expect.Call(source.GetPages(ns2)).Return(new PageInfo[0]);

            // Set default page for NS1
            ns1.DefaultPage = p2;

            // Setup categories/bindings
            var c1 = new CategoryInfo("Cat", source);
            c1.Pages = new[] { p1.FullName };
            var c2 = new CategoryInfo(NameTools.GetFullName(ns1.Name, "Cat"), source);
            c2.Pages = new[] { p2.FullName };
            var c3 = new CategoryInfo(NameTools.GetFullName(ns1.Name, "Cat1"), source);
            c3.Pages = new string[0];
            Expect.Call(source.GetCategories(null)).Return(new[] { c1 });
            Expect.Call(source.GetCategories(ns1)).Return(new[] { c2, c3 });
            Expect.Call(source.GetCategories(ns2)).Return(new CategoryInfo[0]);

            // Setup drafts
            var d1 = new PageContent(p1, "Draft", "NUnit", DateTime.Now, "Comm", "Cont", new[] { "k1", "k2" }, "Descr");
            Expect.Call(source.GetDraft(p1)).Return(d1);
            Expect.Call(source.GetDraft(p2)).Return(null);
            Expect.Call(source.GetDraft(p3)).Return(null);

            // Setup content
            var ctn1 = new PageContent(p1, "Title1", "User1", DateTime.Now, "Comm1", "Cont1", null, "Descr1");
            var ctn2 = new PageContent(p2, "Title2", "User2", DateTime.Now, "Comm2", "Cont2", null, "Descr2");
            var ctn3 = new PageContent(p3, "Title3", "User3", DateTime.Now, "Comm3", "Cont3", null, "Descr3");
            Expect.Call(source.GetContent(p1)).Return(ctn1);
            Expect.Call(source.GetContent(p2)).Return(ctn2);
            Expect.Call(source.GetContent(p3)).Return(ctn3);

            // Setup backups
            Expect.Call(source.GetBackups(p1)).Return(new[] { 0, 1 });
            Expect.Call(source.GetBackups(p2)).Return(new[] { 0 });
            Expect.Call(source.GetBackups(p3)).Return(new int[0]);
            var bak1_0 = new PageContent(p1, "K1_0", "U1_0", DateTime.Now, "", "Cont", null, null);
            var bak1_1 = new PageContent(p1, "K1_1", "U1_1", DateTime.Now, "", "Cont", null, null);
            var bak2_0 = new PageContent(p2, "K2_0", "U2_0", DateTime.Now, "", "Cont", null, null);
            Expect.Call(source.GetBackupContent(p1, 0)).Return(bak1_0);
            Expect.Call(source.GetBackupContent(p1, 1)).Return(bak1_1);
            Expect.Call(source.GetBackupContent(p2, 0)).Return(bak2_0);

            // Messages
            var m1 = new Message(1, "User1", "Subject1", DateTime.Now, "Body1");
            m1.Replies = new[] { new Message(2, "User2", "Subject2", DateTime.Now, "Body2") };
            Message[] p1m = { m1 };
            var p2m = new Message[0];
            var p3m = new Message[0];
            Expect.Call(source.GetMessages(p1)).Return(p1m);
            Expect.Call(source.GetMessages(p2)).Return(p2m);
            Expect.Call(source.GetMessages(p3)).Return(p3m);

            // Setup navigation paths
            var n1 = new NavigationPath("N1", source);
            n1.Pages = new[] { p1.FullName };
            var n2 = new NavigationPath(NameTools.GetFullName(ns1.Name, "N1"), source);
            n2.Pages = new[] { p2.FullName, p3.FullName };
            Expect.Call(source.GetNavigationPaths(null)).Return(new[] { n1 });
            Expect.Call(source.GetNavigationPaths(ns1)).Return(new[] { n2 });
            Expect.Call(source.GetNavigationPaths(ns2)).Return(new NavigationPath[0]);

            // Setup DESTINATION --------------------------

            // Snippets
            Expect.Call(destination.AddSnippet(s1.Name, s1.Content))
                .Return(new Snippet(s1.Name, s1.Content, destination));
            Expect.Call(source.RemoveSnippet(s1.Name)).Return(true);
            Expect.Call(destination.AddSnippet(s2.Name, s2.Content))
                .Return(new Snippet(s2.Name, s2.Content, destination));
            Expect.Call(source.RemoveSnippet(s2.Name)).Return(true);

            // Content templates
            Expect.Call(destination.AddContentTemplate(ct1.Name, ct1.Content))
                .Return(new ContentTemplate(ct1.Name, ct1.Name, destination));
            Expect.Call(source.RemoveContentTemplate(ct1.Name)).Return(true);
            Expect.Call(destination.AddContentTemplate(ct2.Name, ct2.Content))
                .Return(new ContentTemplate(ct2.Name, ct2.Name, destination));
            Expect.Call(source.RemoveContentTemplate(ct2.Name)).Return(true);

            // Namespaces
            var ns1Out = new NamespaceInfo(ns1.Name, destination, null);
            var ns2Out = new NamespaceInfo(ns2.Name, destination, null);
            Expect.Call(destination.AddNamespace(ns1.Name)).Return(ns1Out);
            Expect.Call(source.RemoveNamespace(ns1)).Return(true);
            Expect.Call(destination.AddNamespace(ns2.Name)).Return(ns2Out);
            Expect.Call(source.RemoveNamespace(ns2)).Return(true);

            // Pages/drafts/content/backups/messages
            var p1Out = new PageInfo(p1.FullName, destination, p1.CreationDateTime);
            Expect.Call(destination.AddPage(null, p1.FullName, p1.CreationDateTime)).Return(p1Out);
            Expect.Call(destination.ModifyPage(p1Out, ctn1.Title, ctn1.User, ctn1.LastModified, ctn1.Comment,
                ctn1.Content, ctn1.Keywords, ctn1.Description, SaveMode.Normal)).Return(true);
            Expect.Call(destination.ModifyPage(p1Out, d1.Title, d1.User, d1.LastModified, d1.Comment, d1.Content,
                d1.Keywords, d1.Description, SaveMode.Draft)).Return(true);
            Expect.Call(destination.SetBackupContent(bak1_0, 0)).Return(true);
            Expect.Call(destination.SetBackupContent(bak1_1, 1)).Return(true);
            Expect.Call(destination.BulkStoreMessages(p1Out, p1m)).Return(true);
            Expect.Call(source.RemovePage(p1)).Return(true);

            var p2Out = new PageInfo(p2.FullName, destination, p2.CreationDateTime);
            Expect.Call(destination.AddPage(p2.Namespace, p2.Name, p2.CreationDateTime)).Return(p2Out);
            Expect.Call(destination.ModifyPage(p2Out, ctn2.Title, ctn2.User, ctn2.LastModified, ctn2.Comment,
                ctn2.Content, ctn2.Keywords, ctn2.Description, SaveMode.Normal)).Return(true);
            Expect.Call(destination.SetBackupContent(bak2_0, 0)).Return(true);
            Expect.Call(destination.BulkStoreMessages(p2Out, p2m)).Return(true);
            Expect.Call(source.RemovePage(p2)).Return(true);

            var p3Out = new PageInfo(p3.FullName, destination, p3.CreationDateTime);
            Expect.Call(destination.AddPage(p3.Namespace, p3.Name, p3.CreationDateTime)).Return(p3Out);
            Expect.Call(destination.ModifyPage(p3Out, ctn3.Title, ctn3.User, ctn3.LastModified, ctn3.Comment,
                ctn3.Content, ctn3.Keywords, ctn3.Description, SaveMode.Normal)).Return(true);
            Expect.Call(destination.BulkStoreMessages(p3Out, p3m)).Return(true);
            Expect.Call(source.RemovePage(p3)).Return(true);

            // Categories/bindings
            var c1Out = new CategoryInfo(c1.FullName, destination);
            var c2Out = new CategoryInfo(c2.FullName, destination);
            var c3Out = new CategoryInfo(c3.FullName, destination);
            Expect.Call(destination.AddCategory(null, c1.FullName)).Return(c1Out);
            Expect.Call(destination.AddCategory(NameTools.GetNamespace(c2.FullName), NameTools.GetLocalName(c2.FullName)))
                .Return(c2Out);
            Expect.Call(destination.AddCategory(NameTools.GetNamespace(c3.FullName), NameTools.GetLocalName(c3.FullName)))
                .Return(c3Out);
            Expect.Call(destination.RebindPage(p1Out, new[] { c1.FullName })).Return(true);
            Expect.Call(destination.RebindPage(p2Out, new[] { c2.FullName })).Return(true);
            Expect.Call(destination.RebindPage(p3Out, new string[0])).Return(true);
            Expect.Call(source.RemoveCategory(c1)).Return(true);
            Expect.Call(source.RemoveCategory(c2)).Return(true);
            Expect.Call(source.RemoveCategory(c3)).Return(true);

            // Navigation paths
            var n1Out = new NavigationPath(n1.FullName, destination);
            n1Out.Pages = n1.Pages;
            var n2Out = new NavigationPath(n2.FullName, destination);
            n2Out.Pages = n2.Pages;

            Expect.Call(destination.AddNavigationPath(null, n1.FullName, new[] { p1 })).Return(n1Out).Constraints(
                RMC.Is.Null(), RMC.Is.Equal(n1.FullName),
                RMC.Is.Matching(delegate (PageInfo[] array) { return array[0].FullName == p1.FullName; }));

            Expect.Call(destination.AddNavigationPath(NameTools.GetNamespace(n2.FullName),
                NameTools.GetLocalName(n2.FullName), new[] { p2, p3 })).Return(n2Out).Constraints(
                    RMC.Is.Equal(NameTools.GetNamespace(n2.FullName)), RMC.Is.Equal(NameTools.GetLocalName(n2.FullName)),
                    RMC.Is.Matching(
                        delegate (PageInfo[] array)
                        {
                            return array[0].FullName == p2.FullName && array[1].FullName == p3.FullName;
                        }));

            Expect.Call(source.RemoveNavigationPath(n1)).Return(true);
            Expect.Call(source.RemoveNavigationPath(n2)).Return(true);

            Expect.Call(destination.SetNamespaceDefaultPage(ns1Out, p2Out)).Return(ns1Out);
            Expect.Call(destination.SetNamespaceDefaultPage(ns2Out, null)).Return(ns2Out);

            // Used for navigation paths
            Expect.Call(destination.GetPages(null)).Return(new[] { p1Out });
            Expect.Call(destination.GetPages(ns1Out)).Return(new[] { p2Out, p3Out });
            Expect.Call(destination.GetPages(ns2Out)).Return(new PageInfo[0]);

            mocks.Replay(source);
            mocks.Replay(destination);

            DataMigrator.MigratePagesStorageProviderData(source, destination);

            mocks.Verify(source);
            mocks.Verify(destination);
        }

        [Test]
        public void MigrateUsersStorageProviderData()
        {
            var mocks = new MockRepository();

            var source = mocks.StrictMock<IUsersStorageProviderV30>();
            var destination = mocks.StrictMock<IUsersStorageProviderV30>();

            // Setup SOURCE --------------------

            // User groups
            var g1 = new UserGroup("G1", "G1", source);
            var g2 = new UserGroup("G2", "G2", source);
            Expect.Call(source.GetUserGroups()).Return(new[] { g1, g2 });

            // Users
            var u1 = new UserInfo("U1", "U1", "u1@users.com", true, DateTime.Now, source);
            var u2 = new UserInfo("U2", "U2", "u2@users.com", true, DateTime.Now, source);
            var u3 = new UserInfo("U3", "U3", "u3@users.com", true, DateTime.Now, source);
            Expect.Call(source.GetUsers()).Return(new[] { u1, u2, u3 });

            // Membership
            g1.Users = new[] { u1.Username, u2.Username };
            g2.Users = new[] { u2.Username, u3.Username };
            u1.Groups = new[] { g1.Name };
            u2.Groups = new[] { g1.Name, g2.Name };
            u3.Groups = new[] { g2.Name };

            // User data
            IDictionary<string, string> u1Data = new Dictionary<string, string>
            {
                {"Key1", "Value1"},
                {"Key2", "Value2"}
            };
            Expect.Call(source.RetrieveAllUserData(u1)).Return(u1Data);
            Expect.Call(source.RetrieveAllUserData(u2)).Return(new Dictionary<string, string>());
            Expect.Call(source.RetrieveAllUserData(u3)).Return(new Dictionary<string, string>());

            // Setup DESTINATION ------------------

            // User groups
            var g1Out = new UserGroup(g1.Name, g1.Description, destination);
            var g2Out = new UserGroup(g2.Name, g2.Description, destination);
            Expect.Call(destination.AddUserGroup(g1.Name, g1.Description)).Return(g1Out);
            Expect.Call(destination.AddUserGroup(g2.Name, g2.Description)).Return(g2Out);

            // Users
            var u1Out = new UserInfo(u1.Username, u1.DisplayName, u1.Email, u1.Active, u1.DateTime, destination);
            var u2Out = new UserInfo(u2.Username, u2.DisplayName, u2.Email, u2.Active, u2.DateTime, destination);
            var u3Out = new UserInfo(u3.Username, u3.DisplayName, u3.Email, u3.Active, u3.DateTime, destination);
            Expect.Call(destination.AddUser(u1.Username, u1.DisplayName, null, u1.Email, u1.Active, u1.DateTime))
                .Return(u1Out)
                .Constraints(
                    RMC.Is.Equal(u1.Username), RMC.Is.Equal(u1.DisplayName), RMC.Is.Anything(), RMC.Is.Equal(u1.Email),
                    RMC.Is.Equal(u1.Active), RMC.Is.Equal(u1.DateTime));
            Expect.Call(destination.AddUser(u2.Username, u2.DisplayName, null, u2.Email, u2.Active, u2.DateTime))
                .Return(u2Out)
                .Constraints(
                    RMC.Is.Equal(u2.Username), RMC.Is.Equal(u2.DisplayName), RMC.Is.Anything(), RMC.Is.Equal(u2.Email),
                    RMC.Is.Equal(u2.Active), RMC.Is.Equal(u2.DateTime));
            Expect.Call(destination.AddUser(u3.Username, u3.DisplayName, null, u3.Email, u3.Active, u3.DateTime))
                .Return(u3Out)
                .Constraints(
                    RMC.Is.Equal(u3.Username), RMC.Is.Equal(u3.DisplayName), RMC.Is.Anything(), RMC.Is.Equal(u3.Email),
                    RMC.Is.Equal(u3.Active), RMC.Is.Equal(u3.DateTime));

            // Membership
            Expect.Call(destination.SetUserMembership(u1Out, u1.Groups)).Return(u1Out);
            Expect.Call(destination.SetUserMembership(u2Out, u2.Groups)).Return(u2Out);
            Expect.Call(destination.SetUserMembership(u3Out, u3.Groups)).Return(u3Out);

            // User data
            Expect.Call(destination.StoreUserData(u1Out, "Key1", "Value1")).Return(true);
            Expect.Call(destination.StoreUserData(u1Out, "Key2", "Value2")).Return(true);

            // Delete source data
            Expect.Call(source.RemoveUser(u1)).Return(true);
            Expect.Call(source.RemoveUser(u2)).Return(true);
            Expect.Call(source.RemoveUser(u3)).Return(true);
            Expect.Call(source.RemoveUserGroup(g1)).Return(true);
            Expect.Call(source.RemoveUserGroup(g2)).Return(true);

            mocks.Replay(source);
            mocks.Replay(destination);

            DataMigrator.MigrateUsersStorageProviderData(source, destination, false);

            mocks.Verify(source);
            mocks.Verify(destination);
        }
    }
}