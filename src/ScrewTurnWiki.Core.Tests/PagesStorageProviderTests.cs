using System;
using System.IO;
using NUnit.Framework;
using Rhino.Mocks;
using ScrewTurn.Wiki.PluginFramework;

namespace ScrewTurn.Wiki.Tests
{
    public class PagesStorageProviderTests : PagesStorageProviderTestScaffolding
    {
        public override IPagesStorageProviderV30 GetProvider()
        {
            var prov = new PagesStorageProvider();
            prov.Init(MockHost(), "");
            return prov;
        }

        [Test]
        public void Init()
        {
            var prov = GetProvider();
            prov.Init(MockHost(), "");

            Assert.IsNotNull(prov.Information, "Information should not be null");
        }

        [Test]
        public void Init_Upgrade()
        {
            var testDir = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid().ToString());
            Directory.CreateDirectory(testDir);

            var mocks = new MockRepository();
            var host = mocks.DynamicMock<IHostV30>();
            Expect.Call(host.GetSettingValue(SettingName.PublicDirectory)).Return(testDir).Repeat.AtLeastOnce();

            Expect.Call(host.UpgradePageStatusToAcl(null, 'L')).IgnoreArguments().Repeat.Twice().Return(true);

            mocks.Replay(host);

            var file = Path.Combine(host.GetSettingValue(SettingName.PublicDirectory), "Pages.cs");
            var categoriesFile = Path.Combine(host.GetSettingValue(SettingName.PublicDirectory), "Categories.cs");
            var navPathsFile = Path.Combine(host.GetSettingValue(SettingName.PublicDirectory), "NavigationPaths.cs");
            var directory = Path.Combine(host.GetSettingValue(SettingName.PublicDirectory), "Pages");
            var messagesDirectory = Path.Combine(host.GetSettingValue(SettingName.PublicDirectory), "Messages");
            Directory.CreateDirectory(directory);
            Directory.CreateDirectory(messagesDirectory);

            // Structure (Keywords and Description are new in v3)
            // Page Title
            // Username|DateTime[|Comment] --- Comment is optional
            // ##PAGE##
            // Content...

            File.WriteAllText(Path.Combine(directory, "Page1.cs"),
                "Title1\r\nSYSTEM|2008/10/30 20:20:20|Comment\r\n##PAGE##\r\nContent...");
            File.WriteAllText(Path.Combine(directory, "Page2.cs"),
                "Title2\r\nSYSTEM|2008/10/30 20:20:20\r\n##PAGE\r\nContent. [[Page.3]] [Page.3|Link to update].");
            File.WriteAllText(Path.Combine(directory, "Page.3.cs"),
                "Title3\r\nSYSTEM|2008/10/30 20:20:20|Comment\r\n##PAGE\r\nContent...");

            // ID|Username|Subject|DateTime|ParentID|Body
            File.WriteAllText(Path.Combine(messagesDirectory, "Page.3.cs"),
                "0|User|Hello|2008/10/30 21:21:21|-1|Blah\r\n");

            // Structure
            // [Namespace.]PageName|PageFile|Status|DateTime
            File.WriteAllText(file,
                "Page1|Page1.cs|NORMAL|2008/10/30 20:20:20\r\nPage2|Page2.cs|PUBLIC\r\nPage.3|Page.3.cs|LOCKED");

            File.WriteAllText(categoriesFile, "Cat1|Page.3\r\nCat.2|Page1|Page2\r\n");

            File.WriteAllText(navPathsFile, "Path1|Page1|Page.3\r\nPath2|Page2\r\n");

            var prov = new PagesStorageProvider();
            prov.Init(host, "");

            var pages = prov.GetPages(null);

            Assert.AreEqual(3, pages.Count, "Wrong page count");
            Assert.AreEqual("Page1", pages[0].FullName, "Wrong name");
            Assert.AreEqual("Page2", pages[1].FullName, "Wrong name");
            Assert.AreEqual("Page_3", pages[2].FullName, "Wrong name");
            //Assert.IsFalse(prov.GetContent(pages[1]).Content.Contains("Page.3"), "Content should not contain 'Page.3'");
            //Assert.IsTrue(prov.GetContent(pages[1]).Content.Contains("Page_3"), "Content should contain 'Page_3'");

            var messages = prov.GetMessages(pages[2]);
            Assert.AreEqual(1, messages.Count, "Wrong message count");
            Assert.AreEqual("Hello", messages[0].Subject, "Wrong subject");

            var categories = prov.GetCategories(null);

            Assert.AreEqual(2, categories.Count, "Wrong category count");
            Assert.AreEqual("Cat1", categories[0].FullName, "Wrong name");
            Assert.AreEqual(1, categories[0].Pages.Count, "Wrong page count");
            Assert.AreEqual("Page_3", categories[0].Pages[0], "Wrong page");
            Assert.AreEqual("Cat_2", categories[1].FullName, "Wrong name");
            Assert.AreEqual(2, categories[1].Pages.Count, "Wrong page count");
            Assert.AreEqual("Page1", categories[1].Pages[0], "Wrong page");
            Assert.AreEqual("Page2", categories[1].Pages[1], "Wrong page");

            var navPaths = prov.GetNavigationPaths(null);

            Assert.AreEqual(2, navPaths.Count, "Wrong nav path count");
            Assert.AreEqual("Path1", navPaths[0].FullName, "Wrong name");
            Assert.AreEqual(2, navPaths[0].Pages.Length, "Wrong page count");
            Assert.AreEqual("Page1", navPaths[0].Pages[0], "Wrong page");
            Assert.AreEqual("Page_3", navPaths[0].Pages[1], "Wrong page");
            Assert.AreEqual(1, navPaths[1].Pages.Length, "Wrong page count");
            Assert.AreEqual("Page2", navPaths[1].Pages[0], "Wrong page");

            mocks.Verify(host);

            // Simulate another startup - upgrade not needed anymore

            mocks.BackToRecord(host);
            Expect.Call(host.GetSettingValue(SettingName.PublicDirectory)).Return(testDir).Repeat.AtLeastOnce();
            Expect.Call(host.UpgradePageStatusToAcl(null, 'L')).IgnoreArguments().Repeat.Times(0).Return(false);

            mocks.Replay(host);

            prov = new PagesStorageProvider();
            prov.Init(host, "");

            mocks.Verify(host);

            Directory.Delete(testDir, true);
        }
    }
}