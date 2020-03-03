using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using ScrewTurn.Wiki.PluginFramework;
using ScrewTurn.Wiki.SearchEngine;

namespace ScrewTurn.Wiki.Tests
{
    [TestFixture]
    public abstract class PagesStorageProviderTestScaffolding
    {
        [TearDown]
        public void TearDown()
        {
            try
            {
                Directory.Delete(testDir, true);
            }
            catch
            {
            }
        }

        private readonly MockRepository mocks = new MockRepository();

        private readonly string testDir = Path.Combine(Environment.GetEnvironmentVariable("TEMP"),
            Guid.NewGuid().ToString());

        private delegate string ToStringDelegate(PageInfo p, string input);

        protected IHostV30 MockHost()
        {
            if (!Directory.Exists(testDir)) Directory.CreateDirectory(testDir);

            var host = mocks.DynamicMock<IHostV30>();
            Expect.Call(host.GetSettingValue(SettingName.PublicDirectory)).Return(testDir).Repeat.AtLeastOnce();
            Expect.Call(host.PrepareContentForIndexing(null, null))
                .IgnoreArguments()
                .Do((ToStringDelegate)delegate (PageInfo p, string input) { return input; })
                .Repeat.Any();
            Expect.Call(host.PrepareTitleForIndexing(null, null))
                .IgnoreArguments()
                .Do((ToStringDelegate)delegate (PageInfo p, string input) { return input; })
                .Repeat.Any();

            mocks.Replay(host);

            return host;
        }

        public abstract IPagesStorageProviderV30 GetProvider();

        private void AssertNamespaceInfosAreEqual(NamespaceInfo expected, NamespaceInfo actual, bool checkProvider)
        {
            Assert.AreEqual(expected.Name, actual.Name, "Wrong name");
            if (expected.DefaultPage == null) Assert.IsNull(actual.DefaultPage, "DefaultPage should be null");
            else AssertPageInfosAreEqual(expected.DefaultPage, actual.DefaultPage, true);
            if (checkProvider) Assert.AreSame(expected.Provider, actual.Provider);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void GetNamespace_InvalidName(string n)
        {
            var prov = GetProvider();

            prov.GetNamespace(n);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void AddNamespace_InvalidName(string n)
        {
            var prov = GetProvider();
            prov.AddNamespace(n);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void RenameNamespace_InvalidNewName(string n)
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("Sub");

            prov.RenameNamespace(ns, n);
        }

        private void AssertCategoryInfosAreEqual(CategoryInfo expected, CategoryInfo actual, bool checkProvider)
        {
            Assert.AreEqual(expected.FullName, actual.FullName, "Wrong full name");
            Assert.AreEqual(expected.Pages.Count, actual.Pages.Count, "Wrong page count");
            for (var i = 0; i < expected.Pages.Count; i++)
            {
                Assert.AreEqual(expected.Pages[i], actual.Pages[i], "Wrong page at position " + i);
            }
            if (checkProvider) Assert.AreSame(expected.Provider, actual.Provider, "Different provider instances");
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void GetCategory_InvalidName(string n)
        {
            var prov = GetProvider();
            prov.GetCategory(n);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void AddCategory_InvalidCategory(string c)
        {
            var prov = GetProvider();
            prov.AddCategory(null, c);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void RenameCategory_InvalidNewName(string n)
        {
            var prov = GetProvider();
            var c1 = prov.AddCategory(null, "Category1");
            prov.RenameCategory(c1, n);
        }

        private void AssertPageInfosAreEqual(PageInfo expected, PageInfo actual, bool checkProvider)
        {
            Assert.AreEqual(expected.FullName, actual.FullName, "Wrong name");
            Assert.AreEqual(expected.NonCached, actual.NonCached, "Wrong non-cached flag");
            Tools.AssertDateTimesAreEqual(expected.CreationDateTime, actual.CreationDateTime, true);
            if (checkProvider) Assert.AreSame(expected.Provider, actual.Provider, "Different provider instances");
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void GetPage_InvalidName(string n)
        {
            var prov = GetProvider();

            prov.GetPage(n);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void AddPage_InvalidName(string n)
        {
            var prov = GetProvider();

            prov.AddPage(null, n, DateTime.Now);
        }

        private void AssertPageContentsAreEqual(PageContent expected, PageContent actual)
        {
            AssertPageInfosAreEqual(expected.PageInfo, actual.PageInfo, true);
            Assert.AreEqual(expected.Title, actual.Title, "Wrong title");
            Assert.AreEqual(expected.User, actual.User, "Wrong user");
            Tools.AssertDateTimesAreEqual(expected.LastModified, actual.LastModified, true);
            Assert.AreEqual(expected.Comment, actual.Comment, "Wrong comment");
            Assert.AreEqual(expected.Content, actual.Content, "Wrong content");

            if (expected.LinkedPages != null)
            {
                Assert.IsNotNull(actual.LinkedPages, "LinkedPages is null");
                Assert.AreEqual(expected.LinkedPages.Count, actual.LinkedPages.Count, "Wrong linked page count");
                for (var i = 0; i < expected.LinkedPages.Count; i++)
                {
                    Assert.AreEqual(expected.LinkedPages[i], actual.LinkedPages[i], "Wrong linked page");
                }
            }

            if (expected.Keywords != null)
            {
                Assert.IsNotNull(actual.Keywords);
                Assert.AreEqual(expected.Keywords.Count, actual.Keywords.Count, "Wrong keyword count");
                for (var i = 0; i < expected.Keywords.Count; i++)
                {
                    Assert.AreEqual(expected.Keywords[i], actual.Keywords[i], "Wrong keyword");
                }
            }

            Assert.AreEqual(expected.Description, actual.Description, "Wrong description");
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void ModifyPage_InvalidTitle(string t)
        {
            var prov = GetProvider();
            var p = prov.AddPage(null, "Page", DateTime.Now);
            prov.ModifyPage(p, t, "NUnit", DateTime.Now, "Comment", "Content", null, null, SaveMode.Backup);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void ModifyPage_InvalidUsername(string u)
        {
            var prov = GetProvider();
            var p = prov.AddPage(null, "Page", DateTime.Now);
            prov.ModifyPage(p, "Title", u, DateTime.Now, "Comment", "Content", null, null, SaveMode.Backup);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void RenamePage_InvalidName(string n)
        {
            var prov = GetProvider();

            var p = prov.AddPage(null, "Page", DateTime.Now);
            prov.ModifyPage(p, "Title", "NUnit", DateTime.Now, "Comment", "Content", null, null, SaveMode.Normal);

            prov.RenamePage(p, n);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void RebindPage_InvalidCategoryElement(string e)
        {
            var prov = GetProvider();

            var cat1 = prov.AddCategory(null, "Cat1");
            var cat2 = prov.AddCategory(null, "Cat2");

            var page = prov.AddPage(null, "Page", DateTime.Now);

            prov.RebindPage(page, new[] { "Cat1", e });
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void AddMessage_InvalidUsername(string u)
        {
            var prov = GetProvider();

            var page = prov.AddPage(null, "Page", DateTime.Now);

            prov.AddMessage(page, u, "Subject", DateTime.Now, "Body", -1);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void AddMessage_InvalidSubject(string s)
        {
            var prov = GetProvider();

            var page = prov.AddPage(null, "Page", DateTime.Now);

            prov.AddMessage(page, "NUnit", s, DateTime.Now, "Body", -1);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void ModifyMessage_InvalidUsername(string u)
        {
            var prov = GetProvider();

            var page = prov.AddPage(null, "Page", DateTime.Now);
            prov.AddMessage(page, "NUnit", "Subject", DateTime.Now, "Body", -1);

            Assert.IsFalse(prov.ModifyMessage(page, prov.GetMessages(page)[0].ID, u, "Subject", DateTime.Now, "Body"),
                "ModifyMessage should return false");
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void ModifyMessage_InvalidSubject(string s)
        {
            var prov = GetProvider();

            var page = prov.AddPage(null, "Page", DateTime.Now);
            prov.AddMessage(page, "NUnit", "Subject", DateTime.Now, "Body", -1);

            Assert.IsFalse(prov.ModifyMessage(page, prov.GetMessages(page)[0].ID, "NUnit", s, DateTime.Now, "Body"),
                "ModifyMessage should return false");
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void AddNavigationPath_InvalidName(string n)
        {
            var prov = GetProvider();

            var page1 = prov.AddPage(null, "Page1", DateTime.Now);
            var page2 = prov.AddPage(null, "Page2", DateTime.Now);

            prov.AddNavigationPath(null, n, new[] { page1, page2 });
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void AddSnippet_InvalidName(string n)
        {
            var prov = GetProvider();

            prov.AddSnippet(n, "Content");
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void ModifySnippet_InvalidName(string n)
        {
            var prov = GetProvider();

            prov.ModifySnippet(n, "Content");
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void RemoveSnippet_InvalidName(string n)
        {
            var prov = GetProvider();

            prov.RemoveSnippet(n);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void AddContentTemplate_InvalidName(string n)
        {
            var prov = GetProvider();

            prov.AddContentTemplate(n, "Content");
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void ModifyContentTemplate_InvalidName(string n)
        {
            var prov = GetProvider();

            prov.ModifyContentTemplate(n, "Content");
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void RemoveContentTemplate_InvalidName(string n)
        {
            var prov = GetProvider();

            prov.RemoveContentTemplate(n);
        }

        private void DoChecksFor_RebuildIndex_ManyPages(IPagesStorageProviderV30 prov)
        {
            int docCount, wordCount, matchCount;
            long size;
            prov.GetIndexStats(out docCount, out wordCount, out matchCount, out size);
            Assert.AreEqual(PagesContent.Length, docCount, "Wrong document count");
            Assert.IsTrue(wordCount > 0, "Wrong word count");
            Assert.IsTrue(matchCount > 0, "Wrong match count");
            Assert.IsTrue(size > 0, "Wrong size");

            var results = prov.PerformSearch(new SearchParameters("lorem"));
            Assert.IsTrue(results.Count > 0, "No results returned");
        }

        private static readonly string[] PagesContent =
        {
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis non massa eu erat imperdiet porta nec eu ipsum. Nulla ullamcorper massa et dui tincidunt eget volutpat velit pellentesque.",
            "Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Nunc venenatis vestibulum velit, at molestie dui blandit eget.",
            "Praesent bibendum accumsan nulla quis convallis. Quisque eget est metus. Praesent cursus mauris at diam aliquam luctus sit amet sed odio.",
            "Cras dignissim eros quis risus vehicula quis ultrices ipsum mattis. Praesent hendrerit sodales volutpat.",
            "Sed laoreet, quam at tempus rhoncus, ipsum ipsum tempus libero, in sodales justo nisl a tortor. Mauris non enim libero, ac rutrum tortor. Quisque at lacus mauris. Aenean non tortor a eros fermentum fringilla. Mauris tincidunt scelerisque mattis.",
            "Ut ut augue ut sapien dapibus ullamcorper a imperdiet lorem. Maecenas rhoncus nibh nec purus ullamcorper dapibus. Sed blandit, dui eget aliquet adipiscing, nunc orci semper leo, eu fringilla ipsum neque ut augue.",
            "Vestibulum cursus lectus dolor, eget lobortis libero. Sed nulla lacus, vulputate at vestibulum sit amet, faucibus id sapien. Nunc egestas semper laoreet.",
            "Nunc tempus molestie velit, eu imperdiet ante luctus ut. Praesent diam sapien, mattis nec feugiat a, gravida sit amet quam.",
            "Sed eu erat sed nulla vulputate molestie vel ut justo. Cras vestibulum ultrices mauris in consectetur. In bibendum enim neque, id tempus erat.",
            "Aenean blandit, justo et tempus dignissim, arcu odio vestibulum erat, sed venenatis odio turpis sed nulla. Aenean venenatis rhoncus sem, sed tincidunt est cursus id. Nam ut sem id dui varius porta.",
            "Suspendisse potenti. Duis non dui ac nulla cursus varius. Morbi auctor diam quis urna lobortis sit amet laoreet leo egestas. Integer velit ante, dictum id faucibus quis, pulvinar vitae ipsum. Ut sed lorem lacus. Morbi a enim purus, quis tincidunt risus.",
            "Maecenas ac odio quis magna vehicula faucibus. Ut arcu est, volutpat fringilla gravida non, mattis a diam. Nullam dapibus, arcu eget sagittis mattis, tortor leo consectetur tortor, eget mollis libero elit vel metus.",
            "Vivamus faucibus ante at urna adipiscing pulvinar. Pellentesque ligula ante, sollicitudin a iaculis sed, dictum quis leo. Quisque augue ipsum, ultrices vitae pretium vel, vulputate vel arcu.",
            "Nullam semper luctus dui. Morbi gravida tortor odio, et condimentum velit. Integer semper dapibus turpis, ac suscipit mauris eleifend et. Cras a quam tortor. Mauris lorem mauris, ultricies sed tristique ac, sollicitudin sit amet nibh.",
            "Praesent scelerisque convallis risus, a tincidunt turpis porta nec. Aenean tristique malesuada diam, ut fringilla tortor congue vel. Duis id feugiat sapien. In hendrerit, nisl id porttitor convallis, sem est pretium sem, a tincidunt lorem ligula eget massa.",
            "Aliquam neque quam, cursus eu iaculis non, laoreet et eros. Maecenas a lacus arcu. Mauris et placerat erat. Pellentesque ut felis est, sit amet sollicitudin turpis. Etiam non odio orci.",
            "Nulla purus orci, elementum nec convallis in, feugiat sit amet lectus. Aenean eu elit sem. Quisque sit amet ante nibh, sed elementum magna. Quisque non est odio.",
            "Morbi porta metus at mi vehicula sit amet scelerisque nulla vehicula. Sed eleifend venenatis velit. Nulla augue mauris, dignissim sed rhoncus non, luctus nec nulla. In hac habitasse platea dictumst.",
            "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Lorem ipsum dolor sit amet, consectetur adipiscing elit. In nunc est, euismod et ullamcorper vitae, aliquam quis nulla.",
            "Morbi porttitor vehicula placerat. Nunc et lorem mauris. Morbi in nunc lorem. Integer aliquet sem vel magna scelerisque lobortis. Integer nec suscipit libero. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nam posuere cursus enim. Sed auctor semper vehicula. Phasellus volutpat est et sem ultricies ornare."
        };

        [Test]
        public void AddCategory_GetCategories_Root()
        {
            var prov = GetProvider();

            Assert.AreEqual(0, prov.GetCategories(null).Count, "Wrong initial category count");

            var c1 = prov.AddCategory(null, "Category1");
            var c2 = prov.AddCategory(null, "Category2");

            Assert.IsNull(prov.AddCategory(null, "Category1"), "AddCategory should return null");

            AssertCategoryInfosAreEqual(new CategoryInfo("Category1", prov), c1, true);
            AssertCategoryInfosAreEqual(new CategoryInfo("Category2", prov), c2, true);

            var categories = prov.GetCategories(null).OrderBy(c => c.FullName).ToList();

            AssertCategoryInfosAreEqual(new CategoryInfo("Category1", prov), categories[0], true);
            AssertCategoryInfosAreEqual(new CategoryInfo("Category2", prov), categories[1], true);
        }

        [Test]
        public void AddCategory_GetCategories_Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("Namespace");

            Assert.AreEqual(0, prov.GetCategories(ns).Count, "Wrong initial category count");

            var c1 = prov.AddCategory(ns.Name, "Category1");
            var c2 = prov.AddCategory(ns.Name, "Category2");

            Assert.IsNull(prov.AddCategory(ns.Name, "Category1"), "AddCategory should return null");

            AssertCategoryInfosAreEqual(new CategoryInfo(NameTools.GetFullName(ns.Name, "Category1"), prov), c1, true);
            AssertCategoryInfosAreEqual(new CategoryInfo(NameTools.GetFullName(ns.Name, "Category2"), prov), c2, true);

            var categories = prov.GetCategories(ns).OrderBy(c => c.FullName).ToList();

            AssertCategoryInfosAreEqual(new CategoryInfo(NameTools.GetFullName(ns.Name, "Category1"), prov),
                categories[0], true);
            AssertCategoryInfosAreEqual(new CategoryInfo(NameTools.GetFullName(ns.Name, "Category2"), prov),
                categories[1], true);
        }

        [Test]
        public void AddCategory_GetCategory_Root()
        {
            var prov = GetProvider();

            Assert.IsNull(prov.GetCategory("Category1"), "GetCategory should return null");

            var c1 = prov.AddCategory(null, "Category1");
            var c2 = prov.AddCategory(null, "Category2");

            Assert.IsNull(prov.GetCategory("Category3"), "GetCategory should return null");

            var c1Out = prov.GetCategory("Category1");
            var c2Out = prov.GetCategory("Category2");

            AssertCategoryInfosAreEqual(c1, c1Out, true);
            AssertCategoryInfosAreEqual(c2, c2Out, true);
        }

        [Test]
        public void AddCategory_GetCategory_Sub()
        {
            var prov = GetProvider();

            prov.AddNamespace("NS");

            Assert.IsNull(prov.GetCategory("NS.Category1"), "GetCategory should return null");

            var c1 = prov.AddCategory("NS", "Category1");
            var c2 = prov.AddCategory("NS", "Category2");

            Assert.IsNull(prov.GetCategory("NS.Category3"), "GetCategory should return null");

            var c1Out = prov.GetCategory("NS.Category1");
            var c2Out = prov.GetCategory("NS.Category2");

            AssertCategoryInfosAreEqual(c1, c1Out, true);
            AssertCategoryInfosAreEqual(c2, c2Out, true);
        }

        [Test]
        public void AddContentTemplate_GetContentTemplates()
        {
            var prov = GetProvider();

            var temp1 = prov.AddContentTemplate("T1", "Template1");
            Assert.AreEqual("T1", temp1.Name, "Wrong name");
            Assert.AreEqual("Template1", temp1.Content, "Wrong content");

            Assert.IsNull(prov.AddContentTemplate("T1", "Blah"), "AddContentTemplate should return null");

            var temp2 = prov.AddContentTemplate("T2", "Template2");
            Assert.AreEqual("T2", temp2.Name, "Wrong name");
            Assert.AreEqual("Template2", temp2.Content, "Wrong content");

            var templates = prov.GetContentTemplates();
            Assert.AreEqual(2, templates.Length, "Wrong template count");

            Array.Sort(templates, (x, y) => { return x.Name.CompareTo(y.Name); });

            Assert.AreEqual("T1", templates[0].Name, "Wrong name");
            Assert.AreEqual("Template1", templates[0].Content, "Wrong content");
            Assert.AreEqual("T2", templates[1].Name, "Wrong name");
            Assert.AreEqual("Template2", templates[1].Content, "Wrong content");
        }

        [Test]
        public void AddMessage_GetMessages_Root()
        {
            var prov = GetProvider();

            var page = prov.AddPage(null, "Page", DateTime.Now);

            Assert.AreEqual(-1, prov.GetMessageCount(new PageInfo("Inexistent", prov, DateTime.Now)),
                "GetMessageCount should return -1");
            Assert.IsNull(prov.GetMessages(new PageInfo("Inexistent", prov, DateTime.Now)),
                "GetMessages should return null");

            Assert.AreEqual(0, prov.GetMessageCount(page), "Wrong initial message count");
            Assert.AreEqual(0, prov.GetMessages(page).Count, "Wrong initial message count");

            Assert.IsFalse(
                prov.AddMessage(new PageInfo("Inexistent", prov, DateTime.Now), "NUnit", "Subject", DateTime.Now, "Body",
                    -1), "AddMessage should return false");

            var dt = DateTime.Now;

            Assert.IsTrue(prov.AddMessage(page, "NUnit", "Subject", dt, "Body", -1), "AddMessage should return true");
            Assert.AreEqual(1, prov.GetMessageCount(page), "Wrong message count");
            Assert.IsTrue(
                prov.AddMessage(page, "NUnit1", "Subject1", dt.AddDays(1), "Body1", prov.GetMessages(page)[0].ID),
                "AddMessage should return true");
            Assert.AreEqual(2, prov.GetMessageCount(page), "Wrong message count");

            var messages = prov.GetMessages(page);

            Assert.AreEqual(1, messages.Count, "Wrong message count");
            Assert.AreEqual("NUnit", messages[0].Username, "Wrong username");
            Assert.AreEqual("Subject", messages[0].Subject, "Wrong subject");
            Tools.AssertDateTimesAreEqual(dt, messages[0].DateTime);
            Assert.AreEqual("Body", messages[0].Body, "Wrong body");

            messages = messages[0].Replies;
            Assert.AreEqual(1, messages.Count, "Wrong reply count");
            Assert.AreEqual(0, messages[0].Replies.Length, "Wrong reply count");

            Assert.AreEqual("NUnit1", messages[0].Username, "Wrong username");
            Assert.AreEqual("Subject1", messages[0].Subject, "Wrong subject");
            Tools.AssertDateTimesAreEqual(dt.AddDays(1), messages[0].DateTime);
            Assert.AreEqual("Body1", messages[0].Body, "Wrong body");
        }

        [Test]
        public void AddMessage_GetMessages_Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("NS");

            var page = prov.AddPage(ns.Name, "Page", DateTime.Now);

            Assert.AreEqual(-1, prov.GetMessageCount(new PageInfo(NameTools.GetFullName(ns.Name, "Inexistent"),
                prov, DateTime.Now)), "GetMessageCount should return -1");

            Assert.IsNull(prov.GetMessages(new PageInfo(NameTools.GetFullName(ns.Name, "Inexistent"),
                prov, DateTime.Now)), "GetMessages should return null");

            Assert.AreEqual(0, prov.GetMessageCount(page), "Wrong initial message count");
            Assert.AreEqual(0, prov.GetMessages(page).Count, "Wrong initial message count");

            Assert.IsFalse(prov.AddMessage(new PageInfo(NameTools.GetFullName(ns.Name, "Inexistent"),
                prov, DateTime.Now), "NUnit", "Subject", DateTime.Now, "Body", -1), "AddMessage should return false");

            var dt = DateTime.Now;

            Assert.IsTrue(prov.AddMessage(page, "NUnit", "Subject", dt, "Body", -1), "AddMessage should return true");
            Assert.AreEqual(1, prov.GetMessageCount(page), "Wrong message count");
            Assert.IsTrue(
                prov.AddMessage(page, "NUnit1", "Subject1", dt.AddDays(1), "Body1", prov.GetMessages(page)[0].ID),
                "AddMessage should return true");
            Assert.AreEqual(2, prov.GetMessageCount(page), "Wrong message count");

            var messages = prov.GetMessages(page);

            Assert.AreEqual(1, messages.Count, "Wrong message count");
            Assert.AreEqual("NUnit", messages[0].Username, "Wrong username");
            Assert.AreEqual("Subject", messages[0].Subject, "Wrong subject");
            Tools.AssertDateTimesAreEqual(dt, messages[0].DateTime);
            Assert.AreEqual("Body", messages[0].Body, "Wrong body");

            messages = messages[0].Replies;
            Assert.AreEqual(1, messages.Count, "Wrong reply count");
            Assert.AreEqual(0, messages[0].Replies.Length, "Wrong reply count");

            Assert.AreEqual("NUnit1", messages[0].Username, "Wrong username");
            Assert.AreEqual("Subject1", messages[0].Subject, "Wrong subject");
            Tools.AssertDateTimesAreEqual(dt.AddDays(1), messages[0].DateTime);
            Assert.AreEqual("Body1", messages[0].Body, "Wrong body");
        }

        [Test]
        public void AddMessage_InexistentParent()
        {
            var prov = GetProvider();

            var page = prov.AddPage(null, "Page", DateTime.Now);

            Assert.IsFalse(prov.AddMessage(page, "NUnit", "Subject", DateTime.Now, "Body", 5),
                "AddMessage should return false");
        }

        [Test]
        public void AddMessage_PerformSearch()
        {
            var prov = GetProvider();

            var page = prov.AddPage(null, "Page", DateTime.Now);

            prov.AddMessage(page, "NUnit", "Message", DateTime.Now, "Blah, Test.", -1);
            prov.AddMessage(page, "NUnit", "Re: Message2", DateTime.Now, "Dummy.", prov.GetMessages(page)[0].ID);

            var result = prov.PerformSearch(new SearchParameters("dummy message"));
            Assert.AreEqual(2, result.Count, "Wrong result count");

            bool found1 = false, found2 = false;
            foreach (var res in result)
            {
                Assert.AreEqual(MessageDocument.StandardTypeTag, res.Document.TypeTag, "Wrong type tag");
                if (res.Matches[0].Text == "dummy") found1 = true;
                if (res.Matches[0].Text == "message") found2 = true;
            }

            Assert.IsTrue(found1, "First word not found");
            Assert.IsTrue(found2, "Second word not found");
        }

        [Test]
        public void AddNamespace_GetNamespace()
        {
            var prov = GetProvider();

            Assert.IsNull(prov.GetNamespace("Sub1"), "GetNamespace should return null");

            var ns1 = prov.AddNamespace("Sub1");
            var ns2 = prov.AddNamespace("Sub2");

            Assert.IsNull(prov.GetNamespace("Sub3"), "GetNamespace should return null");

            var ns1Out = prov.GetNamespace("Sub1");
            var ns2Out = prov.GetNamespace("Sub2");

            AssertNamespaceInfosAreEqual(ns1, ns1Out, true);
            AssertNamespaceInfosAreEqual(ns2, ns2Out, true);
        }

        [Test]
        public void AddNamespace_GetNamespaces()
        {
            var prov = GetProvider();

            Assert.AreEqual(0, prov.GetNamespaces().Count, "Wrong initial namespace count");

            var ns1 = prov.AddNamespace("Sub1");
            var ns2 = prov.AddNamespace("Sub2");
            var ns3 = prov.AddNamespace("Spaced Namespace");
            Assert.IsNull(prov.AddNamespace("Sub1"), "AddNamespace should return null");

            AssertNamespaceInfosAreEqual(new NamespaceInfo("Sub1", prov, null), ns1, true);
            AssertNamespaceInfosAreEqual(new NamespaceInfo("Sub2", prov, null), ns2, true);
            AssertNamespaceInfosAreEqual(new NamespaceInfo("Spaced Namespace", prov, null), ns3, true);

            var allNS = prov.GetNamespaces().OrderBy(n => n.Name).ToList();
            Assert.AreEqual(3, allNS.Count, "Wrong namespace count");

            AssertNamespaceInfosAreEqual(ns3, allNS[0], true);
            AssertNamespaceInfosAreEqual(ns1, allNS[1], true);
            AssertNamespaceInfosAreEqual(ns2, allNS[2], true);
        }

        [Test]
        public void AddNamespace_GetNamespaces_WithDefaultPages()
        {
            var prov = GetProvider();

            Assert.AreEqual(0, prov.GetNamespaces().Count, "Wrong initial namespace count");

            var ns1 = prov.AddNamespace("Sub1");
            var ns2 = prov.AddNamespace("Sub2");
            Assert.IsNull(prov.AddNamespace("Sub1"), "AddNamespace should return null");

            var dp1 = prov.AddPage(ns1.Name, "MainPage", DateTime.Now);
            ns1 = prov.SetNamespaceDefaultPage(ns1, dp1);

            var dp2 = prov.AddPage(ns2.Name, "MainPage", DateTime.Now);
            ns2 = prov.SetNamespaceDefaultPage(ns2, dp2);

            AssertNamespaceInfosAreEqual(new NamespaceInfo("Sub1", prov, dp1), ns1, true);
            AssertNamespaceInfosAreEqual(new NamespaceInfo("Sub2", prov, dp2), ns2, true);

            var allNS = prov.GetNamespaces().OrderBy(n => n.Name).ToList();
            Assert.AreEqual(2, allNS.Count, "Wrong namespace count");

            AssertNamespaceInfosAreEqual(ns1, allNS[0], true);
            AssertNamespaceInfosAreEqual(ns2, allNS[1], true);
        }

        [Test]
        public void AddNavigationPath_GetNavigationPaths_Root()
        {
            var prov = GetProvider();

            var page1 = prov.AddPage(null, "Page1", DateTime.Now);
            var page2 = prov.AddPage(null, "Page2", DateTime.Now);

            Assert.AreEqual(0, prov.GetNavigationPaths(null).Count, "Wrong initial navigation path count");

            var path1 = prov.AddNavigationPath(null, "Path1", new[] { page1, page2 });
            Assert.IsNotNull(path1, "AddNavigationPath should return something");
            Assert.AreEqual("Path1", path1.FullName, "Wrong name");
            Assert.AreEqual(2, path1.Pages.Length, "Wrong page count");
            Assert.AreEqual(page1.FullName, path1.Pages[0], "Wrong page at position 0");
            Assert.AreEqual(page2.FullName, path1.Pages[1], "Wrong page at position 1");

            var path2 = prov.AddNavigationPath(null, "Path2", new[] { page1 });
            Assert.IsNotNull(path2, "AddNavigationPath should return something");
            Assert.AreEqual("Path2", path2.FullName, "Wrong name");
            Assert.AreEqual(1, path2.Pages.Length, "Wrong page count");
            Assert.AreEqual(page1.FullName, path2.Pages[0], "Wrong page at position 0");

            Assert.IsNull(prov.AddNavigationPath(null, "Path1", new[] { page2, page1 }),
                "AddNavigationPath should return null");

            var paths = prov.GetNavigationPaths(null);
            Assert.AreEqual(2, paths.Count, "Wrong navigation path count");
            Assert.AreEqual("Path1", paths[0].FullName, "Wrong name");
            Assert.AreEqual(2, paths[0].Pages.Length, "Wrong page count");
            Assert.AreEqual(page1.FullName, paths[0].Pages[0], "Wrong page at position 0");
            Assert.AreEqual(page2.FullName, paths[0].Pages[1], "Wrong page at position 1");
            Assert.AreEqual("Path2", paths[1].FullName, "Wrong name");
            Assert.AreEqual(1, paths[1].Pages.Length, "Wrong page count");
            Assert.AreEqual(page1.FullName, paths[1].Pages[0], "Wrong page at position 0");
        }

        [Test]
        public void AddNavigationPath_GetNavigationPaths_Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("NS");

            var page1 = prov.AddPage(ns.Name, "Page1", DateTime.Now);
            var page2 = prov.AddPage(ns.Name, "Page2", DateTime.Now);

            Assert.AreEqual(0, prov.GetNavigationPaths(ns).Count, "Wrong initial navigation path count");

            var path1 = prov.AddNavigationPath(ns.Name, "Path1", new[] { page1, page2 });
            Assert.IsNotNull(path1, "AddNavigationPath should return something");
            Assert.AreEqual(NameTools.GetFullName(ns.Name, "Path1"), path1.FullName, "Wrong name");
            Assert.AreEqual(2, path1.Pages.Length, "Wrong page count");
            Assert.AreEqual(page1.FullName, path1.Pages[0], "Wrong page at position 0");
            Assert.AreEqual(page2.FullName, path1.Pages[1], "Wrong page at position 1");

            var path2 = prov.AddNavigationPath(ns.Name, "Path2", new[] { page1 });
            Assert.IsNotNull(path2, "AddNavigationPath should return something");
            Assert.AreEqual(NameTools.GetFullName(ns.Name, "Path2"), path2.FullName, "Wrong name");
            Assert.AreEqual(1, path2.Pages.Length, "Wrong page count");
            Assert.AreEqual(page1.FullName, path2.Pages[0], "Wrong page at position 0");

            Assert.IsNull(prov.AddNavigationPath(ns.Name, "Path1", new[] { page2, page1 }),
                "AddNavigationPath should return null");

            var paths = prov.GetNavigationPaths(ns);
            Assert.AreEqual(2, paths.Count, "Wrong navigation path count");
            Assert.AreEqual(NameTools.GetFullName(ns.Name, "Path1"), paths[0].FullName, "Wrong name");
            Assert.AreEqual(2, paths[0].Pages.Length, "Wrong page count");
            Assert.AreEqual(page1.FullName, paths[0].Pages[0], "Wrong page at position 0");
            Assert.AreEqual(page2.FullName, paths[0].Pages[1], "Wrong page at position 1");
            Assert.AreEqual(NameTools.GetFullName(ns.Name, "Path2"), paths[1].FullName, "Wrong name");
            Assert.AreEqual(1, paths[1].Pages.Length, "Wrong page count");
            Assert.AreEqual(page1.FullName, paths[1].Pages[0], "Wrong page at position 0");
        }

        [Test]
        public void AddPage_GetPage_Root()
        {
            var prov = GetProvider();

            Assert.IsNull(prov.GetPage("Page1"), "GetPage should return null");

            var p1 = prov.AddPage(null, "Page1", DateTime.Now);
            var p2 = prov.AddPage(null, "Page2", DateTime.Now);

            Assert.IsNull(prov.GetPage("Page3"), "GetPage should return null");

            var p1Out = prov.GetPage("Page1");
            var p2Out = prov.GetPage("Page2");

            AssertPageInfosAreEqual(p1, p1Out, true);
            AssertPageInfosAreEqual(p2, p2Out, true);
        }

        [Test]
        public void AddPage_GetPage_Sub()
        {
            var prov = GetProvider();

            prov.AddNamespace("NS");

            Assert.IsNull(prov.GetPage("NS.Page1"), "GetPage should return null");

            var p1 = prov.AddPage("NS", "Page1", DateTime.Now);
            var p2 = prov.AddPage("NS", "Page2", DateTime.Now);

            Assert.IsNull(prov.GetPage("NS.Page3"), "GetPage should return null");

            var p1Out = prov.GetPage("NS.Page1");
            var p2Out = prov.GetPage("NS.Page2");

            AssertPageInfosAreEqual(p1, p1Out, true);
            AssertPageInfosAreEqual(p2, p2Out, true);
        }

        [Test]
        public void AddPage_GetPage_SubSpaced()
        {
            var prov = GetProvider();

            prov.AddNamespace("Spaced Namespace");

            Assert.IsNull(prov.GetPage("Spaced Namespace.Page1"), "GetPage should return null");

            var p1 = prov.AddPage("Spaced Namespace", "Page1", DateTime.Now);
            var p2 = prov.AddPage("Spaced Namespace", "Page2", DateTime.Now);

            Assert.IsNull(prov.GetPage("Spaced Namespace.Page3"), "GetPage should return null");

            var p1Out = prov.GetPage("Spaced Namespace.Page1");
            var p2Out = prov.GetPage("Spaced Namespace.Page2");

            AssertPageInfosAreEqual(p1, p1Out, true);
            AssertPageInfosAreEqual(p2, p2Out, true);
        }

        [Test]
        public void AddPage_GetPages_Root()
        {
            var prov = GetProvider();

            Assert.AreEqual(0, prov.GetPages(null).Count, "Wrong initial page count");

            var p1 = prov.AddPage(null, "Page1", DateTime.Now);
            var p2 = prov.AddPage(null, "Page2", DateTime.Now);

            Assert.IsNull(prov.AddPage(null, "Page1", DateTime.Now.AddDays(-1)), "AddPage should return null");

            AssertPageInfosAreEqual(new PageInfo("Page1", prov, p1.CreationDateTime), p1, true);
            AssertPageInfosAreEqual(new PageInfo("Page2", prov, p2.CreationDateTime), p2, true);

            var pages = prov.GetPages(null).OrderBy(c => c.FullName).ToList();

            Assert.AreEqual(2, pages.Count, "Wrong page count");
            AssertPageInfosAreEqual(new PageInfo("Page1", prov, p1.CreationDateTime), pages[0], true);
            AssertPageInfosAreEqual(new PageInfo("Page2", prov, p2.CreationDateTime), pages[1], true);
        }

        [Test]
        public void AddPage_GetPages_Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("Namespace");

            Assert.AreEqual(0, prov.GetPages(ns).Count, "Wrong initial page count");

            var p1 = prov.AddPage(ns.Name, "Page1", DateTime.Now);
            var p2 = prov.AddPage(ns.Name, "Page2", DateTime.Now);

            Assert.IsNull(prov.AddPage(ns.Name, "Page1", DateTime.Now.AddDays(-1)), "AddPage should return null");

            AssertPageInfosAreEqual(new PageInfo(NameTools.GetFullName(ns.Name, "Page1"), prov, p1.CreationDateTime), p1,
                true);
            AssertPageInfosAreEqual(new PageInfo(NameTools.GetFullName(ns.Name, "Page2"), prov, p2.CreationDateTime), p2,
                true);

            var pages = prov.GetPages(ns).OrderBy(c => c.FullName).ToList();

            Assert.AreEqual(2, pages.Count, "Wrong page count");
            AssertPageInfosAreEqual(new PageInfo(NameTools.GetFullName(ns.Name, "Page1"), prov, p1.CreationDateTime),
                pages[0], true);
            AssertPageInfosAreEqual(new PageInfo(NameTools.GetFullName(ns.Name, "Page2"), prov, p2.CreationDateTime),
                pages[1], true);
        }

        [Test]
        public void AddSnippet_GetSnippets()
        {
            var prov = GetProvider();

            Assert.AreEqual(0, prov.GetSnippets().Count, "Wrong snippet count");

            var snippet1 = prov.AddSnippet("Snippet1", "Content1");
            var snippet2 = prov.AddSnippet("Snippet2", "Content2");

            Assert.IsNull(prov.AddSnippet("Snippet1", "Content"), "AddSnippet should return null");

            Assert.AreEqual("Snippet1", snippet1.Name, "Wrong name");
            Assert.AreEqual("Content1", snippet1.Content, "Wrong content");
            Assert.AreEqual("Snippet2", snippet2.Name, "Wrong name");
            Assert.AreEqual("Content2", snippet2.Content, "Wrong content");

            var snippets = prov.GetSnippets().OrderBy(s => s.Name).ToList();
            Assert.AreEqual(2, snippets.Count, "Wrong snippet count");

            Assert.AreEqual("Snippet1", snippets[0].Name, "Wrong name");
            Assert.AreEqual("Content1", snippets[0].Content, "Wrong content");
            Assert.AreEqual("Snippet2", snippets[1].Name, "Wrong name");
            Assert.AreEqual("Content2", snippets[1].Content, "Wrong content");
        }

        [Test]
        public void BulkStoreMessages_DuplicateID()
        {
            var prov = GetProvider();

            var page = prov.AddPage(null, "Page", DateTime.Now);

            var dt = DateTime.Now;

            var newMessages = new List<Message>();
            newMessages.Add(new Message(1, "NUnit", "New1", dt, "Body1"));
            newMessages[0].Replies = new[] { new Message(1, "NUnit", "New11", dt.AddDays(2), "Body11") };
            newMessages.Add(new Message(3, "NUnit", "New2", dt.AddDays(1), "Body2"));

            Assert.IsFalse(prov.BulkStoreMessages(page, newMessages.ToArray()), "BulkStoreMessages should return false");
        }

        [Test]
        public void BulkStoreMessages_PerformSearch()
        {
            var prov = GetProvider();

            var page = prov.AddPage(null, "Page", DateTime.Now);

            prov.AddMessage(page, "NUnit", "Blah", DateTime.Now, "Blah", -1);

            var newMessages = new List<Message>();
            newMessages.Add(new Message(1, "NUnit", "New1", DateTime.Now, "Body1"));
            newMessages[0].Replies = new[] { new Message(2, "NUnit", "New11", DateTime.Now, "Body11") };
            newMessages.Add(new Message(3, "NUnit", "New2", DateTime.Now, "Body2"));

            prov.BulkStoreMessages(page, newMessages.ToArray());

            var result = prov.PerformSearch(new SearchParameters("new1 new11 new2 blah"));
            Assert.AreEqual(3, result.Count, "Wrong result count");
            foreach (var res in result)
            {
                foreach (var info in res.Matches)
                {
                    Assert.AreNotEqual("blah", info.Text, "Invalid search macth");
                }
            }
        }

        [Test]
        public void BulkStoreMessages_Root()
        {
            var prov = GetProvider();

            Assert.IsFalse(prov.BulkStoreMessages(new PageInfo("Inexistent", prov, DateTime.Now), new Message[0]),
                "BulkStoreMessages should return false");

            var page = prov.AddPage(null, "Page", DateTime.Now);

            prov.AddMessage(page, "NUnit", "Test", DateTime.Now, "Message", -1);
            prov.AddMessage(page, "NUnit", "Test-1", DateTime.Now, "Message-1", prov.GetMessages(page)[0].ID);
            prov.AddMessage(page, "NUnit", "Test400", DateTime.Now, "Message400", -1);
            prov.AddMessage(page, "NUnit", "Test500", DateTime.Now, "Message500", -1);

            var dt = DateTime.Now;

            var newMessages = new List<Message>();
            newMessages.Add(new Message(1, "NUnit", "New1", dt, "Body1"));
            newMessages[0].Replies = new[] { new Message(2, "NUnit", "New11", dt.AddDays(2), "Body11") };
            newMessages.Add(new Message(3, "NUnit", "New2", dt.AddDays(1), "Body2"));

            Assert.IsTrue(prov.BulkStoreMessages(page, newMessages.ToArray()), "BulkStoreMessages should return true");

            var result = new List<Message>(prov.GetMessages(page));

            Assert.AreEqual(2, result.Count, "Wrong root message count");
            Assert.AreEqual(1, result[0].Replies.Length, "Wrong reply count");

            Assert.AreEqual(1, result[0].ID, "Wrong ID");
            Assert.AreEqual("NUnit", result[0].Username, "Wrong username");
            Assert.AreEqual("New1", result[0].Subject, "Wrong subject");
            Tools.AssertDateTimesAreEqual(dt, result[0].DateTime);
            Assert.AreEqual("Body1", result[0].Body, "Wrong body");

            Assert.AreEqual(2, result[0].Replies[0].ID, "Wrong ID");
            Assert.AreEqual("NUnit", result[0].Replies[0].Username, "Wrong username");
            Assert.AreEqual("New11", result[0].Replies[0].Subject, "Wrong subject");
            Tools.AssertDateTimesAreEqual(dt.AddDays(2), result[0].Replies[0].DateTime);
            Assert.AreEqual("Body11", result[0].Replies[0].Body, "Wrong body");

            Assert.AreEqual(3, result[1].ID, "Wrong ID");
            Assert.AreEqual("NUnit", result[1].Username, "Wrong username");
            Assert.AreEqual("New2", result[1].Subject, "Wrong subject");
            Tools.AssertDateTimesAreEqual(dt.AddDays(1), result[1].DateTime);
            Assert.AreEqual("Body2", result[1].Body, "Wrong body");
        }

        [Test]
        public void BulkStoreMessages_Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("NS");

            Assert.IsFalse(prov.BulkStoreMessages(new PageInfo(NameTools.GetFullName(ns.Name, "Inexistent"),
                prov, DateTime.Now), new Message[0]), "BulkStoreMessages should return false");

            var page = prov.AddPage(ns.Name, "Page", DateTime.Now);

            prov.AddMessage(page, "NUnit", "Test", DateTime.Now, "Message", -1);
            prov.AddMessage(page, "NUnit", "Test-1", DateTime.Now, "Message-1", prov.GetMessages(page)[0].ID);
            prov.AddMessage(page, "NUnit", "Test400", DateTime.Now, "Message400", -1);
            prov.AddMessage(page, "NUnit", "Test500", DateTime.Now, "Message500", -1);

            var dt = DateTime.Now;

            var newMessages = new List<Message>();
            newMessages.Add(new Message(1, "NUnit", "New1", dt, "Body1"));
            newMessages[0].Replies = new[] { new Message(2, "NUnit", "New11", dt.AddDays(2), "Body11") };
            newMessages.Add(new Message(3, "NUnit", "New2", dt.AddDays(1), "Body2"));

            Assert.IsTrue(prov.BulkStoreMessages(page, newMessages.ToArray()), "BulkStoreMessages should return true");

            var result = new List<Message>(prov.GetMessages(page));

            Assert.AreEqual(2, result.Count, "Wrong root message count");
            Assert.AreEqual(1, result[0].Replies.Length, "Wrong reply count");

            Assert.AreEqual(1, result[0].ID, "Wrong ID");
            Assert.AreEqual("NUnit", result[0].Username, "Wrong username");
            Assert.AreEqual("New1", result[0].Subject, "Wrong subject");
            Tools.AssertDateTimesAreEqual(dt, result[0].DateTime);
            Assert.AreEqual("Body1", result[0].Body, "Wrong body");

            Assert.AreEqual(2, result[0].Replies[0].ID, "Wrong ID");
            Assert.AreEqual("NUnit", result[0].Replies[0].Username, "Wrong username");
            Assert.AreEqual("New11", result[0].Replies[0].Subject, "Wrong subject");
            Tools.AssertDateTimesAreEqual(dt.AddDays(2), result[0].Replies[0].DateTime);
            Assert.AreEqual("Body11", result[0].Replies[0].Body, "Wrong body");

            Assert.AreEqual(3, result[1].ID, "Wrong ID");
            Assert.AreEqual("NUnit", result[1].Username, "Wrong username");
            Assert.AreEqual("New2", result[1].Subject, "Wrong subject");
            Tools.AssertDateTimesAreEqual(dt.AddDays(1), result[1].DateTime);
            Assert.AreEqual("Body2", result[1].Body, "Wrong body");
        }

        [Test]
        public void DeleteBackups_Root()
        {
            var prov = GetProvider();

            var p = prov.AddPage(null, "Page", DateTime.Now);
            prov.ModifyPage(p, "Title", "NUnit", DateTime.Now, "Comment", "Content", null, null, SaveMode.Normal);
            prov.ModifyPage(p, "Title1", "NUnit1", DateTime.Now, "Comment1", "Content1", null, null, SaveMode.Backup);
            prov.ModifyPage(p, "Title2", "NUnit2", DateTime.Now, "Comment2", "Content2", new[] { "k1", "k2" }, "Descr",
                SaveMode.Backup);

            var content = prov.GetContent(p);

            Assert.IsFalse(prov.DeleteBackups(new PageInfo("Inexistent", prov, DateTime.Now), -1),
                "DeleteBackups should return false");

            Assert.IsTrue(prov.DeleteBackups(p, 5), "DeleteBackups should return true");
            Assert.AreEqual(2, prov.GetBackups(p).Length, "Wrong backup count");
            AssertPageContentsAreEqual(content, prov.GetContent(p));

            Assert.IsTrue(prov.DeleteBackups(p, 0), "DeleteBackups should return true");
            Assert.AreEqual(1, prov.GetBackups(p).Length, "Wrong backup count");
            AssertPageContentsAreEqual(content, prov.GetContent(p));

            Assert.IsTrue(prov.DeleteBackups(p, -1), "DeleteBackups should return true");
            Assert.AreEqual(0, prov.GetBackups(p).Length, "Wrong backup count");
            AssertPageContentsAreEqual(content, prov.GetContent(p));
        }

        [Test]
        public void DeleteBackups_Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("NS");

            var p = prov.AddPage(ns.Name, "Page", DateTime.Now);
            prov.ModifyPage(p, "Title", "NUnit", DateTime.Now, "Comment", "Content", null, null, SaveMode.Normal);
            prov.ModifyPage(p, "Title1", "NUnit1", DateTime.Now, "Comment1", "Content1", null, null, SaveMode.Backup);
            prov.ModifyPage(p, "Title2", "NUnit2", DateTime.Now, "Comment2", "Content2", new[] { "k1", "k2" }, "Descr",
                SaveMode.Backup);

            var content = prov.GetContent(p);

            Assert.IsFalse(prov.DeleteBackups(new PageInfo(NameTools.GetFullName(ns.Name, "Inexistent"),
                prov, DateTime.Now), -1), "DeleteBackups should return false");

            Assert.IsTrue(prov.DeleteBackups(p, 5), "DeleteBackups should return true");
            Assert.AreEqual(2, prov.GetBackups(p).Length, "Wrong backup count");
            AssertPageContentsAreEqual(content, prov.GetContent(p));

            Assert.IsTrue(prov.DeleteBackups(p, 0), "DeleteBackups should return true");
            Assert.AreEqual(1, prov.GetBackups(p).Length, "Wrong backup count");
            AssertPageContentsAreEqual(content, prov.GetContent(p));

            Assert.IsTrue(prov.DeleteBackups(p, -1), "DeleteBackups should return true");
            Assert.AreEqual(0, prov.GetBackups(p).Length, "Wrong backup count");
            AssertPageContentsAreEqual(content, prov.GetContent(p));
        }

        [Test]
        public void DeleteDraft_Inexistent()
        {
            var prov = GetProvider();

            Assert.IsFalse(prov.DeleteDraft(new PageInfo("Page", null, DateTime.Now)), "DeleteDraft should return false");

            var page = prov.AddPage(null, "Page", DateTime.Now);
            Assert.IsFalse(prov.DeleteDraft(page), "DeleteDraft should return false");

            prov.ModifyPage(page, "Title", "NUnit", DateTime.Now, "", "Content", new string[0], "", SaveMode.Normal);
            Assert.IsFalse(prov.DeleteDraft(page), "DeleteDraft should return false");

            var dt = DateTime.Now;
            prov.ModifyPage(page, "Title2", "NUnit2", dt, "", "Content2", new string[0], "", SaveMode.Backup);
            Assert.IsFalse(prov.DeleteDraft(page), "DeleteDraft should return false");

            AssertPageContentsAreEqual(
                new PageContent(page, "Title2", "NUnit2", dt, "", "Content2", new string[0], null),
                prov.GetContent(page));
        }

        [Test]
        public void GetBackupContent_InexistentPage_Root()
        {
            var prov = GetProvider();
            Assert.IsNull(prov.GetBackupContent(new PageInfo("P", prov, DateTime.Now), 0),
                "GetBackupContent should return null");
        }

        [Test]
        public void GetBackupContent_InexistentPage_Sub()
        {
            var prov = GetProvider();
            var ns = prov.AddNamespace("Namespace");
            Assert.IsNull(
                prov.GetBackupContent(new PageInfo(NameTools.GetFullName(ns.Name, "P"), prov, DateTime.Now), 0),
                "GetBackupContent should return null");
        }

        [Test]
        public void GetBackupContent_InexistentRevision_Root()
        {
            var prov = GetProvider();

            var p = prov.AddPage(null, "Page", DateTime.Now);
            prov.ModifyPage(p, "Title", "NUnit", DateTime.Now, "Comment", "Content", null, null, SaveMode.Normal);
            prov.ModifyPage(p, "Title1", "NUnit1", DateTime.Now, "Comment1", "Content1", null, null, SaveMode.Backup);

            Assert.IsNull(prov.GetBackupContent(p, 1), "GetBackupContent should return null");
        }

        [Test]
        public void GetBackupContent_InexistentRevision_Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("NS");

            var p = prov.AddPage(ns.Name, "Page", DateTime.Now);
            prov.ModifyPage(p, "Title", "NUnit", DateTime.Now, "Comment", "Content", null, null, SaveMode.Normal);
            prov.ModifyPage(p, "Title1", "NUnit1", DateTime.Now, "Comment1", "Content1", null, null, SaveMode.Backup);

            Assert.IsNull(prov.GetBackupContent(p, 1), "GetBackupContent should return null");
        }


        [Test]
        public void GetBackups_InexistentPage_Root()
        {
            var prov = GetProvider();
            Assert.IsNull(prov.GetBackups(new PageInfo("PPP", prov, DateTime.Now)), "GetBackups should return null");
        }

        [Test]
        public void GetBackups_InexistentPage_Sub()
        {
            var prov = GetProvider();
            var ns = prov.AddNamespace("Namespace");
            Assert.IsNull(prov.GetBackups(new PageInfo(NameTools.GetFullName(ns.Name, "PPP"), prov, DateTime.Now)),
                "GetBackups should return null");
        }

        [Test]
        public void GetCategoriesForPage_Root()
        {
            var prov = GetProvider();

            var c1 = prov.AddCategory(null, "Category1");
            var c2 = prov.AddCategory(null, "Category2");
            var c3 = prov.AddCategory(null, "Category3");

            var page = prov.AddPage(null, "Page", DateTime.Now);
            var page2 = prov.AddPage(null, "Page2", DateTime.Now);

            prov.RebindPage(page, new[] { c1.FullName, c3.FullName });

            Assert.AreEqual(0, prov.GetCategoriesForPage(page2).Count, "Wrong category count");
            var categories = prov.GetCategoriesForPage(page).OrderBy(c => c.FullName).ToList();
            Assert.AreEqual(2, categories.Count, "Wrong category count");

            var cat1 = new CategoryInfo("Category1", prov);
            var cat3 = new CategoryInfo("Category3", prov);
            cat1.Pages = new[] { page.FullName };
            cat3.Pages = new[] { page.FullName };
            AssertCategoryInfosAreEqual(cat1, categories[0], true);
            AssertCategoryInfosAreEqual(cat3, categories[1], true);
        }

        [Test]
        public void GetCategoriesForPage_Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("Namespace");

            var c1 = prov.AddCategory(ns.Name, "Category1");
            var c2 = prov.AddCategory(ns.Name, "Category2");
            var c3 = prov.AddCategory(ns.Name, "Category3");

            var page = prov.AddPage(ns.Name, "Page", DateTime.Now);
            var page2 = prov.AddPage(ns.Name, "Page2", DateTime.Now);

            prov.RebindPage(page, new[] { c1.FullName, c3.FullName });

            Assert.AreEqual(0, prov.GetCategoriesForPage(page2).Count, "Wrong category count");
            var categories = prov.GetCategoriesForPage(page).OrderBy(c => c.FullName).ToList();
            Assert.AreEqual(2, categories.Count, "Wrong category count");

            var cat1 = new CategoryInfo(NameTools.GetFullName(ns.Name, "Category1"), prov);
            var cat3 = new CategoryInfo(NameTools.GetFullName(ns.Name, "Category3"), prov);
            cat1.Pages = new[] { page.FullName };
            cat3.Pages = new[] { page.FullName };
            AssertCategoryInfosAreEqual(cat1, categories[0], true);
            AssertCategoryInfosAreEqual(cat3, categories[1], true);
        }

        [Test]
        public void GetDraft_Inexistent()
        {
            var prov = GetProvider();

            Assert.IsNull(prov.GetDraft(new PageInfo("Page", null, DateTime.Now)), "GetDraft should return null");

            var page = prov.AddPage(null, "Page", DateTime.Now);
            Assert.IsNull(prov.GetDraft(page), "GetDraft should return null");

            prov.ModifyPage(page, "Title", "NUnit", DateTime.Now, "", "Content", new string[0], "", SaveMode.Normal);
            Assert.IsNull(prov.GetDraft(page), "GetDraft should return null");

            prov.ModifyPage(page, "Title", "NUnit", DateTime.Now, "", "Content", new string[0], "", SaveMode.Backup);
            Assert.IsNull(prov.GetDraft(page), "GetDraft should return null");
        }

        [Test]
        public void GetUncategorizedPages_Root()
        {
            var prov = GetProvider();

            var c1 = prov.AddCategory(null, "Category1");
            var c2 = prov.AddCategory(null, "Category2");
            var c3 = prov.AddCategory(null, "Category3");

            var page = prov.AddPage(null, "Page", DateTime.Now);
            var page2 = prov.AddPage(null, "Page2", DateTime.Now);
            var page3 = prov.AddPage(null, "Page3", DateTime.Now);

            prov.RebindPage(page, new[] { c1.FullName, c3.FullName });

            var pages = prov.GetUncategorizedPages(null);
            Assert.AreEqual(2, pages.Count, "Wrong page count");

            AssertPageInfosAreEqual(page2, pages[0], true);
            AssertPageInfosAreEqual(page3, pages[1], true);
        }

        [Test]
        public void GetUncategorizedPages_Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("Namespace");

            var c1 = prov.AddCategory(ns.Name, "Category1");
            var c2 = prov.AddCategory(ns.Name, "Category2");
            var c3 = prov.AddCategory(ns.Name, "Category3");

            var page = prov.AddPage(ns.Name, "Page", DateTime.Now);
            var page2 = prov.AddPage(ns.Name, "Page2", DateTime.Now);
            var page3 = prov.AddPage(ns.Name, "Page3", DateTime.Now);

            prov.RebindPage(page, new[] { c1.FullName, c3.FullName });

            var pages = prov.GetUncategorizedPages(ns);
            Assert.AreEqual(2, pages.Count, "Wrong page count");

            AssertPageInfosAreEqual(page2, pages[0], true);
            AssertPageInfosAreEqual(page3, pages[1], true);
        }

        [Test]
        public void MergeCategories_DifferentNamespaces()
        {
            var prov = GetProvider();

            var ns1 = prov.AddNamespace("Namespace1");
            var ns2 = prov.AddNamespace("Namespace2");

            var cat1 = prov.AddCategory(ns1.Name, "Cat1");
            var cat2 = prov.AddCategory(ns2.Name, "Cat2");
            var cat3 = prov.AddCategory(null, "Cat3");

            // Sub 2 Sub
            Assert.IsNull(prov.MergeCategories(cat1, cat2), "MergeCategories should return null");
            // Sub to Root
            Assert.IsNull(prov.MergeCategories(cat2, cat3), "MergeCategories should return null");
            // Root to Sub
            Assert.IsNull(prov.MergeCategories(cat3, cat1), "MergeCategories should return null");
        }

        [Test]
        public void MergeCategories_Root()
        {
            var prov = GetProvider();

            var cat1 = prov.AddCategory(null, "Cat1");
            var cat2 = prov.AddCategory(null, "Cat2");
            var cat3 = prov.AddCategory(null, "Cat3");

            var page1 = prov.AddPage(null, "Page1", DateTime.Now);
            var page2 = prov.AddPage(null, "Page2", DateTime.Now);

            prov.RebindPage(page1, new[] { "Cat1", "Cat2" });
            prov.RebindPage(page2, new[] { "Cat3" });

            Assert.IsNull(prov.MergeCategories(new CategoryInfo("Inexistent", prov), cat1),
                "MergeCategories should return null");
            Assert.IsNull(prov.MergeCategories(cat1, new CategoryInfo("Inexistent", prov)),
                "MergeCategories should return null");

            var merged = prov.MergeCategories(cat1, cat3);
            Assert.IsNotNull(merged, "MergeCategories should return something");
            Assert.AreEqual("Cat3", merged.FullName, "Wrong name");

            var categories = prov.GetCategories(null).OrderBy(x => x.FullName).ToList();

            Assert.AreEqual(2, categories.Count, "Wrong category count");

            Assert.AreEqual(1, categories[0].Pages.Count, "Wrong page count");
            Assert.AreEqual("Page1", categories[0].Pages[0], "Wrong page at position 0");

            Assert.AreEqual(2, categories[1].Pages.Count, "Wrong page count");
            var pages = categories[1].Pages.OrderBy(s => s).ToList();
            Assert.AreEqual("Page1", pages[0], "Wrong page at position 0");
            Assert.AreEqual("Page2", pages[1], "Wrong page at position 1");
        }

        [Test]
        public void MergeCategories_Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("Namespace");

            var cat1 = prov.AddCategory(ns.Name, "Cat1");
            var cat2 = prov.AddCategory(ns.Name, "Cat2");
            var cat3 = prov.AddCategory(ns.Name, "Cat3");

            var page1 = prov.AddPage(ns.Name, "Page1", DateTime.Now);
            var page2 = prov.AddPage(ns.Name, "Page2", DateTime.Now);

            prov.RebindPage(page1,
                new[] { NameTools.GetFullName(ns.Name, "Cat1"), NameTools.GetFullName(ns.Name, "Cat2") });
            prov.RebindPage(page2, new[] { NameTools.GetFullName(ns.Name, "Cat3") });

            Assert.IsNull(
                prov.MergeCategories(new CategoryInfo(NameTools.GetFullName(ns.Name, "Inexistent"), prov), cat1),
                "MergeCategories should return null");
            Assert.IsNull(
                prov.MergeCategories(cat1, new CategoryInfo(NameTools.GetFullName(ns.Name, "Inexistent"), prov)),
                "MergeCategories should return null");

            var merged = prov.MergeCategories(cat1, cat3);
            Assert.IsNotNull(merged, "MergeCategories should return something");
            Assert.AreEqual(NameTools.GetFullName(ns.Name, "Cat3"), merged.FullName, "Wrong name");

            var categories = prov.GetCategories(ns).OrderBy(c => c.FullName).ToList();

            Assert.AreEqual(2, categories.Count, "Wrong category count");

            Assert.AreEqual(1, categories[0].Pages.Count, "Wrong page count");
            Assert.AreEqual(NameTools.GetFullName(ns.Name, "Page1"), categories[0].Pages[0], "Wrong page at position 0");

            Assert.AreEqual(2, categories[1].Pages.Count, "Wrong page count");
            var pages = categories[1].Pages.OrderBy(s => s).ToList();
            Assert.AreEqual(NameTools.GetFullName(ns.Name, "Page1"), pages[0], "Wrong page at position 0");
            Assert.AreEqual(NameTools.GetFullName(ns.Name, "Page2"), pages[1], "Wrong page at position 1");
        }

        [Test]
        public void ModifyContentTemplate()
        {
            var prov = GetProvider();

            Assert.IsNull(prov.ModifyContentTemplate("T", "Content"), "ModifyContentTemplate should return null");

            prov.AddContentTemplate("T", "Content");
            prov.AddContentTemplate("T2", "Blah");

            var temp = prov.ModifyContentTemplate("T", "Mod");
            Assert.AreEqual("T", temp.Name, "Wrong name");
            Assert.AreEqual("Mod", temp.Content, "Wrong content");

            var templates = prov.GetContentTemplates();
            Assert.AreEqual(2, templates.Length, "Wrong template count");

            Array.Sort(templates, (x, y) => { return x.Name.CompareTo(y.Name); });

            Assert.AreEqual("T", templates[0].Name, "Wrong name");
            Assert.AreEqual("Mod", templates[0].Content, "Wrong content");
            Assert.AreEqual("T2", templates[1].Name, "Wrong name");
            Assert.AreEqual("Blah", templates[1].Content, "Wrong content");
        }

        [Test]
        public void ModifyMessage_PerformSearch()
        {
            var prov = GetProvider();

            var page = prov.AddPage(null, "Page", DateTime.Now);

            prov.AddMessage(page, "NUnit", "Message", DateTime.Now, "Blah, Test.", -1);
            prov.ModifyMessage(page, prov.GetMessages(page)[0].ID, "NUnit", "MessageMod", DateTime.Now, "Modified");

            var result = prov.PerformSearch(new SearchParameters("message modified"));
            Assert.AreEqual(1, result.Count, "Wrong result count");

            Assert.AreEqual(1, result[0].Matches.Count, "Wrong match count");
            Assert.AreEqual("modified", result[0].Matches[0].Text, "Wrong match");
        }

        [Test]
        public void ModifyMessage_Root()
        {
            var prov = GetProvider();

            var page = prov.AddPage(null, "Page", DateTime.Now);

            var dt = DateTime.Now;

            prov.AddMessage(page, "NUnit", "Subject", dt, "Body", -1);
            prov.AddMessage(page, "NUnit", "Subject", dt, "Body", -1);

            Assert.IsFalse(prov.ModifyMessage(new PageInfo("Inexistent", prov, DateTime.Now), 1,
                "NUnit", "Subject", DateTime.Now, "Body"), "ModifyMessage should return false");

            Assert.IsFalse(prov.ModifyMessage(page, 5, "NUnit", "Subject", DateTime.Now, "Body"),
                "ModifyMessage should return false");

            Assert.IsTrue(
                prov.ModifyMessage(page, prov.GetMessages(page)[0].ID, "NUnit1", "Subject1", dt.AddDays(1), "Body1"),
                "ModifyMessage should return true");

            Assert.AreEqual(2, prov.GetMessageCount(page), "Wrong message count");

            var messages = prov.GetMessages(page);

            Assert.AreEqual("NUnit", messages[0].Username, "Wrong username");
            Assert.AreEqual("Subject", messages[0].Subject, "Wrong subject");
            Tools.AssertDateTimesAreEqual(dt, messages[0].DateTime);
            Assert.AreEqual("Body", messages[0].Body, "Wrong body");

            Assert.AreEqual("NUnit1", messages[1].Username, "Wrong username");
            Assert.AreEqual("Subject1", messages[1].Subject, "Wrong subject");
            Tools.AssertDateTimesAreEqual(dt.AddDays(1), messages[1].DateTime);
            Assert.AreEqual("Body1", messages[1].Body, "Wrong body");
        }

        [Test]
        public void ModifyMessage_Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("NS");

            var page = prov.AddPage(ns.Name, "Page", DateTime.Now);

            var dt = DateTime.Now;

            prov.AddMessage(page, "NUnit", "Subject", dt, "Body", -1);
            prov.AddMessage(page, "NUnit", "Subject", dt, "Body", -1);

            Assert.IsFalse(prov.ModifyMessage(new PageInfo(NameTools.GetFullName(ns.Name, "Inexistent"),
                prov, DateTime.Now), 1, "NUnit", "Subject", DateTime.Now, "Body"), "ModifyMessage should return false");

            Assert.IsFalse(prov.ModifyMessage(page, 5, "NUnit", "Subject", DateTime.Now, "Body"),
                "ModifyMessage should return false");

            Assert.IsTrue(
                prov.ModifyMessage(page, prov.GetMessages(page)[0].ID, "NUnit1", "Subject1", dt.AddDays(1), "Body1"),
                "ModifyMessage should return true");

            Assert.AreEqual(2, prov.GetMessageCount(page), "Wrong message count");

            var messages = prov.GetMessages(page);

            Assert.AreEqual("NUnit", messages[0].Username, "Wrong username");
            Assert.AreEqual("Subject", messages[0].Subject, "Wrong subject");
            Tools.AssertDateTimesAreEqual(dt, messages[0].DateTime);
            Assert.AreEqual("Body", messages[0].Body, "Wrong body");

            Assert.AreEqual("NUnit1", messages[1].Username, "Wrong username");
            Assert.AreEqual("Subject1", messages[1].Subject, "Wrong subject");
            Tools.AssertDateTimesAreEqual(dt.AddDays(1), messages[1].DateTime);
            Assert.AreEqual("Body1", messages[1].Body, "Wrong body");
        }

        [Test]
        public void ModifyNavigationPath_Root()
        {
            var prov = GetProvider();

            var page1 = prov.AddPage(null, "Page1", DateTime.Now);
            var page2 = prov.AddPage(null, "Page2", DateTime.Now);
            var page3 = prov.AddPage(null, "Page3", DateTime.Now);

            var path = prov.AddNavigationPath(null, "Path", new[] { page1, page2 });

            Assert.IsNull(prov.ModifyNavigationPath(new NavigationPath("Inexistent", prov), new[] { page1, page3 }),
                "ModifyNavigationPath should return null");

            var output = prov.ModifyNavigationPath(path, new[] { page1, page3 });

            Assert.IsNotNull(output, "ModifyNavigationPath should return something");
            Assert.AreEqual("Path", path.FullName, "Wrong name");
            Assert.AreEqual(page1.FullName, output.Pages[0], "Wrong page at position 0");
            Assert.AreEqual(page3.FullName, output.Pages[1], "Wrong page at position 1");

            var paths = prov.GetNavigationPaths(null);
            Assert.AreEqual(1, paths.Count, "Wrong navigation path count");
            Assert.AreEqual("Path", paths[0].FullName, "Wrong name");
            var pages = paths[0].Pages.OrderBy(s => s).ToList();
            Assert.AreEqual(page1.FullName, pages[0], "Wrong page at position 0");
            Assert.AreEqual(page3.FullName, pages[1], "Wrong page at position 1");
        }

        [Test]
        public void ModifyNavigationPath_Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("NS");

            var page1 = prov.AddPage(ns.Name, "Page1", DateTime.Now);
            var page2 = prov.AddPage(ns.Name, "Page2", DateTime.Now);
            var page3 = prov.AddPage(ns.Name, "Page3", DateTime.Now);

            var path = prov.AddNavigationPath(ns.Name, "Path", new[] { page1, page2 });

            Assert.IsNull(
                prov.ModifyNavigationPath(new NavigationPath(NameTools.GetFullName(ns.Name, "Inexistent"), prov),
                    new[] { page1, page3 }), "ModifyNavigationPath should return null");

            var output = prov.ModifyNavigationPath(path, new[] { page1, page3 });

            Assert.IsNotNull(output, "ModifyNavigationPath should return something");
            Assert.AreEqual(NameTools.GetFullName(ns.Name, "Path"), path.FullName, "Wrong name");
            Assert.AreEqual(page1.FullName, output.Pages[0], "Wrong page at position 0");
            Assert.AreEqual(page3.FullName, output.Pages[1], "Wrong page at position 1");

            var paths = prov.GetNavigationPaths(ns);
            Assert.AreEqual(1, paths.Count, "Wrong navigation path count");
            Assert.AreEqual(NameTools.GetFullName(ns.Name, "Path"), paths[0].FullName, "Wrong name");
            Assert.AreEqual(page1.FullName, paths[0].Pages[0], "Wrong page at position 0");
            Assert.AreEqual(page3.FullName, paths[0].Pages[1], "Wrong page at position 1");
        }

        [Test]
        public void ModifyPage_Draft_GetDraft_DeleteDraft()
        {
            var prov = GetProvider();

            var dt = DateTime.Now;
            var page = prov.AddPage(null, "Page", DateTime.Now);
            prov.ModifyPage(page, "TitleOld", "NUnitOld", DateTime.Now, "CommentOld", "ContentOld", new string[0],
                "Blah", SaveMode.Normal);
            Assert.IsTrue(
                prov.ModifyPage(page, "Title", "NUnit", DateTime.Now, "Comment", "Content", new string[0], "",
                    SaveMode.Draft), "ModifyPage should return true");
            Assert.IsTrue(
                prov.ModifyPage(page, "Title2", "NUnit", dt, "", "Content2", new string[0], "Descr", SaveMode.Draft),
                "ModifyPage should return true");

            var content = prov.GetDraft(page);
            AssertPageContentsAreEqual(
                new PageContent(page, "Title2", "NUnit", dt, "", "Content2", new string[0], "Descr"), content);

            Assert.IsTrue(prov.DeleteDraft(page), "DeleteDraft should return true");

            Assert.IsNull(prov.GetDraft(page), "GetDraft should return null");
        }

        [Test]
        public void ModifyPage_Draft_MovePage_Root2Sub()
        {
            var prov = GetProvider();

            var dt = DateTime.Now;
            var page = prov.AddPage(null, "Page", dt);
            prov.ModifyPage(page, "Title", "NUnit", dt, "", "", new string[0], "", SaveMode.Normal);
            prov.ModifyPage(page, "Title", "NUnit", dt, "Comment", "Content", new string[0], "", SaveMode.Draft);

            var ns2 = prov.AddNamespace("NS2");

            var page2 = prov.MovePage(page, ns2, false);

            Assert.IsNull(prov.GetDraft(page), "GetDraft should return null");
            var content = prov.GetDraft(page2);

            AssertPageContentsAreEqual(
                new PageContent(page2, "Title", "NUnit", dt, "Comment", "Content", new string[0], null), content);
        }

        [Test]
        public void ModifyPage_Draft_MovePage_Sub2Root()
        {
            var prov = GetProvider();

            var dt = DateTime.Now;
            var ns = prov.AddNamespace("NS");
            var page = prov.AddPage("NS", "Page", dt);
            prov.ModifyPage(page, "Title", "NUnit", dt, "", "", new string[0], "", SaveMode.Normal);
            prov.ModifyPage(page, "Title", "NUnit", dt, "Comment", "Content", new string[0], "", SaveMode.Draft);

            var page2 = prov.MovePage(page, null, false);

            Assert.IsNull(prov.GetDraft(page), "GetDraft should return null");
            var content = prov.GetDraft(page2);

            AssertPageContentsAreEqual(
                new PageContent(page2, "Title", "NUnit", dt, "Comment", "Content", new string[0], null), content);
        }

        [Test]
        public void ModifyPage_Draft_MovePage_Sub2Sub()
        {
            var prov = GetProvider();

            var dt = DateTime.Now;
            var ns = prov.AddNamespace("NS");
            var page = prov.AddPage("NS", "Page", dt);
            prov.ModifyPage(page, "Title", "NUnit", dt, "", "", new string[0], "", SaveMode.Normal);
            prov.ModifyPage(page, "Title", "NUnit", dt, "Comment", "Content", new string[0], "", SaveMode.Draft);

            var ns2 = prov.AddNamespace("NS2");
            var page2 = prov.MovePage(page, ns2, false);

            Assert.IsNull(prov.GetDraft(page), "GetDraft should return null");
            var content = prov.GetDraft(page2);

            AssertPageContentsAreEqual(
                new PageContent(page2, "Title", "NUnit", dt, "Comment", "Content", new string[0], null), content);
        }

        [Test]
        public void ModifyPage_Draft_RemoveNamespace()
        {
            var prov = GetProvider();

            var dt = DateTime.Now;
            var ns = prov.AddNamespace("NS");
            var page = prov.AddPage("NS", "Page", dt);
            prov.ModifyPage(page, "Title", "NUnit", dt, "", "", new string[0], "", SaveMode.Normal);
            prov.ModifyPage(page, "Title", "NUnit", dt, "Comment", "Content", new string[0], "", SaveMode.Draft);

            prov.RemoveNamespace(ns);

            Assert.IsNull(prov.GetDraft(page), "GetDraft should return null");
        }

        [Test]
        public void ModifyPage_Draft_RemovePage()
        {
            var prov = GetProvider();

            var dt = DateTime.Now;
            var page = prov.AddPage(null, "Page", DateTime.Now);
            prov.ModifyPage(page, "Title", "NUnit", dt, "", "", new string[0], "", SaveMode.Normal);
            prov.ModifyPage(page, "Title", "NUnit", dt, "Comment", "Content", new string[0], "", SaveMode.Draft);

            prov.RemovePage(page);

            Assert.IsNull(prov.GetDraft(page), "GetDraft should return null");
        }

        [Test]
        public void ModifyPage_Draft_RenameNamespace()
        {
            var prov = GetProvider();

            var dt = DateTime.Now;
            var ns = prov.AddNamespace("NS");
            var page = prov.AddPage("NS", "Page", dt);
            prov.ModifyPage(page, "Title", "NUnit", dt, "", "", new string[0], "", SaveMode.Normal);
            prov.ModifyPage(page, "Title", "NUnit", dt, "Comment", "Content", new string[0], "", SaveMode.Draft);

            var ns2 = prov.RenameNamespace(ns, "NS2");

            var page2 = new PageInfo(NameTools.GetFullName(ns2.Name, "Page"), prov, dt);

            Assert.IsNull(prov.GetDraft(page), "GetDraft should return null");
            var content = prov.GetDraft(page2);

            AssertPageContentsAreEqual(
                new PageContent(page2, "Title", "NUnit", dt, "Comment", "Content", new string[0], null), content);
        }

        [Test]
        public void ModifyPage_Draft_RenamePage_Root()
        {
            var prov = GetProvider();

            var dt = DateTime.Now;
            var page = prov.AddPage(null, "Page", DateTime.Now);
            prov.ModifyPage(page, "Title", "NUnit", dt, "", "", new string[0], "", SaveMode.Normal);
            prov.ModifyPage(page, "Title", "NUnit", dt, "Comment", "Content", new string[0], "", SaveMode.Draft);

            var newPage = prov.RenamePage(page, "NewName");

            var content = prov.GetDraft(newPage);

            AssertPageContentsAreEqual(
                new PageContent(newPage, "Title", "NUnit", dt, "Comment", "Content", new string[0], null), content);
        }

        [Test]
        public void ModifyPage_Draft_RenamePage_Sub()
        {
            var prov = GetProvider();

            var dt = DateTime.Now;
            prov.AddNamespace("NS");
            var page = prov.AddPage("NS", "Page", DateTime.Now);
            prov.ModifyPage(page, "Title", "NUnit", dt, "", "", new string[0], "", SaveMode.Normal);
            prov.ModifyPage(page, "Title", "NUnit", dt, "Comment", "Content", new string[0], "", SaveMode.Draft);

            var newPage = prov.RenamePage(page, "NewName");

            var content = prov.GetDraft(newPage);

            AssertPageContentsAreEqual(
                new PageContent(newPage, "Title", "NUnit", dt, "Comment", "Content", new string[0], null), content);
        }

        [Test]
        public void ModifyPage_GetContent_GetBackups_GetBackupContent_Root()
        {
            var prov = GetProvider();

            var p = prov.AddPage(null, "Page", DateTime.Now);

            var dt = DateTime.Now;

            Assert.IsFalse(prov.ModifyPage(new PageInfo("Inexistent", prov, DateTime.Now),
                "Title", "NUnit", dt, "Comment", "Content", null, null, SaveMode.Normal),
                "ModifyPage should return false");

            Assert.IsTrue(prov.ModifyPage(p, "Title", "NUnit", dt, null, "Content",
                new[] { "keyword1", "keyword2" }, "Description", SaveMode.Normal), "ModifyPage should return true");

            // Test null/inexistent page
            Assert.IsNull(prov.GetContent(null), "GetContent should return null");
            Assert.IsNull(prov.GetContent(new PageInfo("PPP", prov, DateTime.Now)), "GetContent should return null");

            var c = prov.GetContent(p);
            Assert.IsNotNull(c, "GetContent should return something");

            AssertPageContentsAreEqual(new PageContent(p, "Title", "NUnit", dt, "", "Content",
                new[] { "keyword1", "keyword2" }, "Description"), c);

            Assert.IsTrue(
                prov.ModifyPage(p, "Title1", "NUnit1", dt.AddDays(1), "Comment1", "Content1", null, null,
                    SaveMode.Backup), "ModifyPage should return true");

            var baks = prov.GetBackups(p);
            Assert.AreEqual(1, baks.Length, "Wrong backup content");

            var backup = prov.GetBackupContent(p, baks[0]);
            Assert.IsNotNull(backup, "GetBackupContent should return something");
            AssertPageContentsAreEqual(c, backup);
        }

        [Test]
        public void ModifyPage_GetContent_GetBackups_GetBackupContent_Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("Namespace");

            var p = prov.AddPage(ns.Name, "Page", DateTime.Now);

            var dt = DateTime.Now;

            Assert.IsFalse(
                prov.ModifyPage(new PageInfo(NameTools.GetFullName(ns.Name, "Inexistent"), prov, DateTime.Now),
                    "Title", "NUnit", dt, "Comment", "Content", null, null, SaveMode.Normal),
                "ModifyPage should return false");

            Assert.IsTrue(prov.ModifyPage(p, "Title", "NUnit", dt, null, "Content",
                new[] { "keyword1", "keyword2" }, "Description", SaveMode.Normal), "ModifyPage should return true");

            // Test null/inexistent page
            Assert.IsNull(prov.GetContent(null), "GetContent should return null");
            Assert.IsNull(prov.GetContent(new PageInfo(NameTools.GetFullName(ns.Name, "PPP"), prov, DateTime.Now)),
                "GetContent should return null");

            var c = prov.GetContent(p);
            Assert.IsNotNull(c, "GetContent should return something");

            AssertPageContentsAreEqual(new PageContent(p, "Title", "NUnit", dt, "", "Content",
                new[] { "keyword1", "keyword2" }, "Description"), c);

            Assert.IsTrue(
                prov.ModifyPage(p, "Title1", "NUnit1", dt.AddDays(1), "Comment1", "Content1", null, null,
                    SaveMode.Backup), "ModifyPage should return true");

            var baks = prov.GetBackups(p);
            Assert.AreEqual(1, baks.Length, "Wrong backup content");

            var backup = prov.GetBackupContent(p, baks[0]);
            Assert.IsNotNull(backup, "GetBackupContent should return something");
            AssertPageContentsAreEqual(c, backup);
        }

        [Test]
        public void ModifyPage_OddCombinationsOfKeywordsAndDescription()
        {
            var prov = GetProvider();

            var page = prov.AddPage(null, "Page", DateTime.Now);

            var dt = DateTime.Now;

            Assert.IsTrue(prov.ModifyPage(page, "Title", "NUnit", dt, "", "Content",
                null, null, SaveMode.Normal), "ModifyPage should return true");
            var content = prov.GetContent(page);
            AssertPageContentsAreEqual(new PageContent(page, "Title", "NUnit", dt, "", "Content",
                new string[0], null), content);

            Assert.IsTrue(prov.ModifyPage(page, "Title", "NUnit", dt, "", "Content",
                new string[0], null, SaveMode.Normal), "ModifyPage should return true");
            content = prov.GetContent(page);
            AssertPageContentsAreEqual(new PageContent(page, "Title", "NUnit", dt, "", "Content",
                new string[0], null), content);

            Assert.IsTrue(prov.ModifyPage(page, "Title", "NUnit", dt, "", "Content",
                new[] { "Blah" }, null, SaveMode.Normal), "ModifyPage should return true");
            content = prov.GetContent(page);
            AssertPageContentsAreEqual(new PageContent(page, "Title", "NUnit", dt, "", "Content",
                new[] { "Blah" }, null), content);

            Assert.IsTrue(prov.ModifyPage(page, "Title", "NUnit", dt, "", "Content",
                null, "", SaveMode.Normal), "ModifyPage should return true");
            content = prov.GetContent(page);
            AssertPageContentsAreEqual(new PageContent(page, "Title", "NUnit", dt, "", "Content",
                new string[0], null), content);

            Assert.IsTrue(prov.ModifyPage(page, "Title", "NUnit", dt, "", "Content",
                null, "Descr", SaveMode.Normal), "ModifyPage should return true");
            content = prov.GetContent(page);
            AssertPageContentsAreEqual(new PageContent(page, "Title", "NUnit", dt, "", "Content",
                new string[0], "Descr"), content);

            Assert.IsTrue(prov.ModifyPage(page, "Title", "NUnit", dt, "", "Content",
                new[] { "Blah" }, "Descr", SaveMode.Normal), "ModifyPage should return true");
            content = prov.GetContent(page);
            AssertPageContentsAreEqual(new PageContent(page, "Title", "NUnit", dt, "", "Content",
                new[] { "Blah" }, "Descr"), content);
        }

        [Test]
        public void ModifyPage_PerformSearch()
        {
            var prov = GetProvider();

            // Added to check that pages inserted in reverse alphabetical order work with the search engine
            var p0 = prov.AddPage(null, "PagZ", DateTime.Now);
            prov.ModifyPage(p0, "ZZZ", "NUnit", DateTime.Now, "", "", null, "", SaveMode.Normal);

            var p = prov.AddPage(null, "Page", DateTime.Now);

            var dt = DateTime.Now;

            Assert.IsTrue(prov.ModifyPage(p, "TitleOld", "NUnitOld", dt, "CommentOld", "ContentOld",
                new[] { "keyword3", "keyword4" }, null, SaveMode.Normal), "ModifyPage should return true");
            Assert.IsTrue(prov.ModifyPage(p, "Title", "NUnit", dt, "Comment", "Content",
                new[] { "keyword1", "keyword2" }, null, SaveMode.Normal), "ModifyPage should return true");

            var result = prov.PerformSearch(new SearchParameters("content"));
            Assert.AreEqual(1, result.Count, "Wrong search result count");
            Assert.AreEqual(PageDocument.StandardTypeTag, result[0].Document.TypeTag, "Wrong type tag");
            Assert.AreEqual(1, result[0].Matches.Count, "Wrong match count");
            Assert.AreEqual(WordLocation.Content, result[0].Matches[0].Location, "Wrong word location");

            result = prov.PerformSearch(new SearchParameters("title"));
            Assert.AreEqual(1, result.Count, "Wrong search result count");
            Assert.AreEqual(PageDocument.StandardTypeTag, result[0].Document.TypeTag, "Wrong type tag");
            Assert.AreEqual(1, result[0].Matches.Count, "Wrong match count");
            Assert.AreEqual(WordLocation.Title, result[0].Matches[0].Location, "Wrong word location");

            result = prov.PerformSearch(new SearchParameters("keyword1"));
            Assert.AreEqual(1, result.Count, "Wrong search count");
            Assert.AreEqual(PageDocument.StandardTypeTag, result[0].Document.TypeTag, "Wrong type tag");
            Assert.AreEqual(1, result[0].Matches.Count, "Wrong match count");
            Assert.AreEqual(WordLocation.Keywords, result[0].Matches[0].Location, "Wrong word location");
        }

        [Test]
        public void ModifySnippet()
        {
            var prov = GetProvider();

            Assert.IsNull(prov.ModifySnippet("Inexistent", "Content"), "ModifySnippet should return null");

            prov.AddSnippet("Snippet1", "Content");
            prov.AddSnippet("Snippet2", "Content2");

            var output = prov.ModifySnippet("Snippet1", "Content1");
            Assert.IsNotNull(output, "ModifySnippet should return something");

            Assert.AreEqual("Snippet1", output.Name, "Wrong name");
            Assert.AreEqual("Content1", output.Content, "Wrong content");

            var snippets = prov.GetSnippets().OrderBy(s => s.Name).ToList();
            Assert.AreEqual(2, snippets.Count, "Wrong snippet count");

            Assert.AreEqual("Snippet1", snippets[0].Name, "Wrong name");
            Assert.AreEqual("Content1", snippets[0].Content, "Wrong content");
            Assert.AreEqual("Snippet2", snippets[1].Name, "Wrong name");
            Assert.AreEqual("Content2", snippets[1].Content, "Wrong content");
        }

        [Test]
        public void MovePage_DefaultPage()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("NS");

            var page = prov.AddPage("NS", "MainPage", DateTime.Now);

            prov.SetNamespaceDefaultPage(ns, page);

            Assert.IsNull(prov.MovePage(page, null, false), "Cannot move the default page");
        }

        [Test]
        public void MovePage_ExistentPage_Root2Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("Namespace");

            var page = prov.AddPage(null, "Page", DateTime.Now);
            var existing = prov.AddPage(ns.Name, "Page", DateTime.Now);

            Assert.IsNull(prov.MovePage(page, ns, false), "MovePage should return null");
        }

        [Test]
        public void MovePage_ExistentPage_Sub2Root()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("Namespace");

            var page = prov.AddPage(ns.Name, "Page", DateTime.Now);
            var existing = prov.AddPage(null, "Page", DateTime.Now);

            Assert.IsNull(prov.MovePage(page, null, false), "MovePage should return null");
        }

        [Test]
        public void MovePage_InexistentNamespace()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("Namespace");
            var page = prov.AddPage(ns.Name, "Page", DateTime.Now);

            Assert.IsNull(prov.MovePage(page, new NamespaceInfo("Inexistent", prov, null), false),
                "MovePage should return null");
        }

        [Test]
        public void MovePage_InexistentPage()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("Namespace");

            Assert.IsNull(prov.MovePage(new PageInfo("Page", prov, DateTime.Now),
                ns, false), "MovePage should return null");

            Assert.IsNull(prov.MovePage(new PageInfo(NameTools.GetFullName(ns.Name, "Page"), prov, DateTime.Now),
                null, false), "MovePage should return null");
        }

        [Test]
        public void MovePage_Root2Sub_NoCategories()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("Namespace");
            var cat1 = prov.AddCategory(null, "Category1");
            var cat2 = prov.AddCategory(null, "Category2");
            var cat3 = prov.AddCategory(ns.Name, "Category3");

            var page = prov.AddPage(null, "Page", DateTime.Now);
            prov.ModifyPage(page, "Title", "NUnit", DateTime.Now, "Comment", "Content", null, null, SaveMode.Normal);
            prov.ModifyPage(page, "Title0", "NUnit0", DateTime.Now, "Comment0", "Content0", null, null, SaveMode.Backup);
            prov.ModifyPage(page, "Title1", "NUnit1", DateTime.Now, "Comment1", "Content1", null, null, SaveMode.Backup);
            prov.RebindPage(page, new[] { cat1.FullName });
            prov.AddMessage(page, "NUnit", "Test", DateTime.Now, "Body", -1);

            var moved = prov.MovePage(page, ns, false);

            var expected = new PageInfo(NameTools.GetFullName(ns.Name, NameTools.GetLocalName(page.FullName)), prov,
                page.CreationDateTime);

            AssertPageInfosAreEqual(expected, moved, true);

            Assert.AreEqual(0, prov.GetPages(null).Count, "Wrong page count");

            var allPages = prov.GetPages(ns);
            Assert.AreEqual(1, allPages.Count, "Wrong page count");
            AssertPageInfosAreEqual(expected, allPages[0], true);
            Assert.AreEqual(1, prov.GetMessages(expected).Count, "Wrong message count");

            Assert.AreEqual(2, prov.GetCategories(null).Count, "Wrong category count");
            Assert.AreEqual(1, prov.GetCategories(ns).Count, "Wrong category count");

            Assert.AreEqual(2, prov.GetBackups(expected).Length, "Wrong backup count");
            Assert.AreEqual("Content1", prov.GetContent(expected).Content, "Wrong content");
        }

        [Test]
        public void MovePage_Root2Sub_PerformSearch()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("NS");

            var page = prov.AddPage(null, "Page", DateTime.Now);
            prov.ModifyPage(page, "Title", "NUnit", DateTime.Now, "Comment", "Content", new string[0], "Descr",
                SaveMode.Backup);

            prov.AddMessage(page, "NUnit", "Test1", DateTime.Now, "Body1", -1);
            prov.AddMessage(page, "NUnit", "Test2", DateTime.Now, "Body2", prov.GetMessages(page)[0].ID);

            var movedPage = prov.MovePage(page, ns, false);

            var result = prov.PerformSearch(new SearchParameters("content"));
            Assert.AreEqual(1, result.Count, "Wrong result count");
            Assert.AreEqual(movedPage.FullName, PageDocument.GetPageName(result[0].Document.Name), "Wrong document name");

            result = prov.PerformSearch(new SearchParameters("test1 test2 body1 body2"));
            Assert.AreEqual(2, result.Count, "Wrong result count");

            string pageName;
            int id;

            MessageDocument.GetMessageDetails(result[0].Document.Name, out pageName, out id);
            Assert.AreEqual(ns.Name, NameTools.GetNamespace(pageName), "Wrong document name");

            MessageDocument.GetMessageDetails(result[1].Document.Name, out pageName, out id);
            Assert.AreEqual(ns.Name, NameTools.GetNamespace(pageName), "Wrong document name");
        }

        [Test]
        public void MovePage_Root2Sub_WithCategories()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("Namespace");
            var cat1 = prov.AddCategory(null, "Category1");
            var cat2 = prov.AddCategory(null, "Category2");
            var cat1ns = prov.AddCategory(ns.Name, "Category1");
            var cat3 = prov.AddCategory(ns.Name, "Category3");

            var page = prov.AddPage(null, "Page", DateTime.Now);
            prov.ModifyPage(page, "Title", "NUnit", DateTime.Now, "Comment", "Content", null, null, SaveMode.Normal);
            prov.ModifyPage(page, "Title0", "NUnit0", DateTime.Now, "Comment0", "Content0", null, null, SaveMode.Backup);
            prov.ModifyPage(page, "Title1", "NUnit1", DateTime.Now, "Comment1", "Content1", null, null, SaveMode.Backup);
            prov.RebindPage(page, new[] { cat1.FullName });
            prov.AddMessage(page, "NUnit", "Test", DateTime.Now, "Body", -1);

            var moved = prov.MovePage(page, ns, true);

            var expected = new PageInfo(NameTools.GetFullName(ns.Name, NameTools.GetLocalName(page.FullName)), prov,
                page.CreationDateTime);

            AssertPageInfosAreEqual(expected, moved, true);

            Assert.AreEqual(0, prov.GetPages(null).Count, "Wrong page count");

            var allPages = prov.GetPages(ns);
            Assert.AreEqual(1, allPages.Count, "Wrong page count");
            AssertPageInfosAreEqual(expected, allPages[0], true);
            Assert.AreEqual(1, prov.GetMessages(expected).Count, "Wrong message count");

            Assert.AreEqual(2, prov.GetCategories(null).Count, "Wrong category count");
            Assert.AreEqual(2, prov.GetCategories(ns).Count, "Wrong category count");

            CategoryInfo[] expectedCategories =
            {
                new CategoryInfo(NameTools.GetFullName(ns.Name, NameTools.GetLocalName(cat1.FullName)), prov),
                new CategoryInfo(NameTools.GetFullName(ns.Name, NameTools.GetLocalName(cat3.FullName)), prov)
            };
            expectedCategories[0].Pages = new[] { NameTools.GetFullName(ns.Name, "Page") };

            var actualCategories = prov.GetCategories(ns).OrderBy(c => c.FullName).ToList();
            Assert.AreEqual(expectedCategories.Length, actualCategories.Count, "Wrong category count");
            AssertCategoryInfosAreEqual(expectedCategories[0], actualCategories[0], true);
            AssertCategoryInfosAreEqual(expectedCategories[1], actualCategories[1], true);

            Assert.AreEqual(2, prov.GetBackups(expected).Length, "Wrong backup count");
            Assert.AreEqual("Content1", prov.GetContent(expected).Content, "Wrong content");
        }

        [Test]
        public void MovePage_SameNamespace_Root2Root()
        {
            var prov = GetProvider();

            var page = prov.AddPage(null, "Page", DateTime.Now);

            Assert.IsNull(prov.MovePage(page, null, false), "MovePage should return null");
        }

        [Test]
        public void MovePage_SameNamespace_Sub2Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("Namespace");

            var page = prov.AddPage("Namespace", "Page", DateTime.Now);

            Assert.IsNull(prov.MovePage(page, ns, false), "MovePage should return null");
        }

        [Test]
        public void MovePage_Sub2Root_NoCategories()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("Namespace");
            var cat1 = prov.AddCategory(null, "Category1");
            var cat2 = prov.AddCategory(ns.Name, "Category2");
            var cat3 = prov.AddCategory(ns.Name, "Category3");

            var page = prov.AddPage(ns.Name, "Page", DateTime.Now);
            prov.ModifyPage(page, "Title", "NUnit", DateTime.Now, "Comment", "Content", null, null, SaveMode.Normal);
            prov.ModifyPage(page, "Title0", "NUnit0", DateTime.Now, "Comment0", "Content0", null, null, SaveMode.Backup);
            prov.ModifyPage(page, "Title1", "NUnit1", DateTime.Now, "Comment1", "Content1", null, null, SaveMode.Backup);
            prov.RebindPage(page, new[] { cat2.FullName });
            prov.AddMessage(page, "NUnit", "Test", DateTime.Now, "Body", -1);

            var moved = prov.MovePage(page, null, false);

            var expected = new PageInfo(NameTools.GetLocalName(page.FullName), prov, page.CreationDateTime);

            AssertPageInfosAreEqual(expected, moved, true);

            Assert.AreEqual(0, prov.GetPages(ns).Count, "Wrong page count");

            var allPages = prov.GetPages(null);
            Assert.AreEqual(1, allPages.Count, "Wrong page count");
            AssertPageInfosAreEqual(expected, allPages[0], true);
            Assert.AreEqual(1, prov.GetMessages(expected).Count, "Wrong message count");

            Assert.AreEqual(2, prov.GetCategories(ns).Count, "Wrong category count");
            Assert.AreEqual(1, prov.GetCategories(null).Count, "Wrong category count");

            Assert.AreEqual(2, prov.GetBackups(expected).Length, "Wrong backup count");
            Assert.AreEqual("Content1", prov.GetContent(expected).Content, "Wrong content");
        }

        [Test]
        public void MovePage_Sub2Root_PerformSearch()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("NS");

            var page = prov.AddPage(ns.Name, "Page", DateTime.Now);
            prov.ModifyPage(page, "Title", "NUnit", DateTime.Now, "Comment", "Content", new string[0], "Descr",
                SaveMode.Backup);

            prov.AddMessage(page, "NUnit", "Test1", DateTime.Now, "Body1", -1);
            prov.AddMessage(page, "NUnit", "Test2", DateTime.Now, "Body2", prov.GetMessages(page)[0].ID);

            var movedPage = prov.MovePage(page, null, false);

            var result = prov.PerformSearch(new SearchParameters("content"));
            Assert.AreEqual(1, result.Count, "Wrong result count");
            Assert.AreEqual(movedPage.FullName, PageDocument.GetPageName(result[0].Document.Name), "Wrong document name");

            result = prov.PerformSearch(new SearchParameters("test1 test2 body1 body2"));
            Assert.AreEqual(2, result.Count, "Wrong result count");

            string pageName;
            int id;

            MessageDocument.GetMessageDetails(result[0].Document.Name, out pageName, out id);
            Assert.AreEqual(null, NameTools.GetNamespace(pageName), "Wrong document name");

            MessageDocument.GetMessageDetails(result[1].Document.Name, out pageName, out id);
            Assert.AreEqual(null, NameTools.GetNamespace(pageName), "Wrong document name");
        }

        [Test]
        public void MovePage_Sub2Root_WithCategories()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("Namespace");
            var cat1 = prov.AddCategory(null, "Category1");
            var cat2 = prov.AddCategory(null, "Category2");
            var cat2ns = prov.AddCategory(ns.Name, "Category2");
            var cat3 = prov.AddCategory(ns.Name, "Category3");

            var page = prov.AddPage(ns.Name, "Page", DateTime.Now);
            prov.ModifyPage(page, "Title", "NUnit", DateTime.Now, "Comment", "Content", null, null, SaveMode.Normal);
            prov.ModifyPage(page, "Title0", "NUnit0", DateTime.Now, "Comment0", "Content0", null, null, SaveMode.Backup);
            prov.ModifyPage(page, "Title1", "NUnit1", DateTime.Now, "Comment1", "Content1", null, null, SaveMode.Backup);
            prov.RebindPage(page, new[] { cat2ns.FullName });
            prov.AddMessage(page, "NUnit", "Test", DateTime.Now, "Body", -1);

            var moved = prov.MovePage(page, null, true);

            var expected = new PageInfo(NameTools.GetLocalName(page.FullName), prov, page.CreationDateTime);

            AssertPageInfosAreEqual(expected, moved, true);

            Assert.AreEqual(0, prov.GetPages(ns).Count, "Wrong page count");

            var allPages = prov.GetPages(null);
            Assert.AreEqual(1, allPages.Count, "Wrong page count");
            AssertPageInfosAreEqual(expected, allPages[0], true);
            Assert.AreEqual(1, prov.GetMessages(expected).Count, "Wrong message count");

            Assert.AreEqual(2, prov.GetCategories(null).Count, "Wrong category count");
            Assert.AreEqual(2, prov.GetCategories(ns).Count, "Wrong category count");

            CategoryInfo[] expectedCategories =
            {
                new CategoryInfo(NameTools.GetLocalName(cat1.FullName), prov),
                new CategoryInfo(NameTools.GetLocalName(cat2.FullName), prov)
            };
            expectedCategories[1].Pages = new[] { "Page" };

            var actualCategories = prov.GetCategories(null).OrderBy(c => c.FullName).ToList();
            Assert.AreEqual(expectedCategories.Length, actualCategories.Count, "Wrong category count");
            AssertCategoryInfosAreEqual(expectedCategories[0], actualCategories[0], true);
            AssertCategoryInfosAreEqual(expectedCategories[1], actualCategories[1], true);

            Assert.AreEqual(2, prov.GetBackups(expected).Length, "Wrong backup count");
            Assert.AreEqual("Content1", prov.GetContent(expected).Content, "Wrong content");
        }

        [Test]
        public void MovePage_Sub2Sub_NoCategories()
        {
            var prov = GetProvider();

            var ns1 = prov.AddNamespace("Namespace1");
            var ns2 = prov.AddNamespace("Namespace2");
            var cat1 = prov.AddCategory(ns1.Name, "Category1");
            var cat2 = prov.AddCategory(ns1.Name, "Category2");
            var cat3 = prov.AddCategory(ns2.Name, "Category3");

            var page = prov.AddPage(ns1.Name, "Page", DateTime.Now);
            prov.ModifyPage(page, "Title", "NUnit", DateTime.Now, "Comment", "Content", null, null, SaveMode.Normal);
            prov.ModifyPage(page, "Title0", "NUnit0", DateTime.Now, "Comment0", "Content0", null, null, SaveMode.Backup);
            prov.ModifyPage(page, "Title1", "NUnit1", DateTime.Now, "Comment1", "Content1", null, null, SaveMode.Backup);
            prov.RebindPage(page, new[] { cat2.FullName });
            prov.AddMessage(page, "NUnit", "Test", DateTime.Now, "Body", -1);

            var moved = prov.MovePage(page, ns2, false);

            var expected = new PageInfo(NameTools.GetFullName(ns2.Name, NameTools.GetLocalName(page.FullName)), prov,
                page.CreationDateTime);

            AssertPageInfosAreEqual(expected, moved, true);

            Assert.AreEqual(0, prov.GetPages(ns1).Count, "Wrong page count");

            var allPages = prov.GetPages(ns2);
            Assert.AreEqual(1, allPages.Count, "Wrong page count");
            AssertPageInfosAreEqual(expected, allPages[0], true);
            Assert.AreEqual(1, prov.GetMessages(expected).Count, "Wrong message count");

            Assert.AreEqual(2, prov.GetCategories(ns1).Count, "Wrong category count");
            Assert.AreEqual(1, prov.GetCategories(ns2).Count, "Wrong category count");

            Assert.AreEqual(2, prov.GetBackups(expected).Length, "Wrong backup count");
            Assert.AreEqual("Content1", prov.GetContent(expected).Content, "Wrong content");
        }

        [Test]
        public void MovePage_Sub2Sub_PerformSearch()
        {
            var prov = GetProvider();

            var ns1 = prov.AddNamespace("NS1");
            var ns2 = prov.AddNamespace("NS2");

            var page = prov.AddPage(ns1.Name, "Page", DateTime.Now);
            prov.ModifyPage(page, "Title", "NUnit", DateTime.Now, "Comment", "Content", new string[0], "Descr",
                SaveMode.Backup);

            prov.AddMessage(page, "NUnit", "Test1", DateTime.Now, "Body1", -1);
            prov.AddMessage(page, "NUnit", "Test2", DateTime.Now, "Body2", prov.GetMessages(page)[0].ID);

            var movedPage = prov.MovePage(page, ns2, false);

            var result = prov.PerformSearch(new SearchParameters("content"));
            Assert.AreEqual(1, result.Count, "Wrong result count");
            Assert.AreEqual(movedPage.FullName, PageDocument.GetPageName(result[0].Document.Name), "Wrong document name");

            result = prov.PerformSearch(new SearchParameters("test1 test2 body1 body2"));
            Assert.AreEqual(2, result.Count, "Wrong result count");

            string pageName;
            int id;

            MessageDocument.GetMessageDetails(result[0].Document.Name, out pageName, out id);
            Assert.AreEqual(ns2.Name, NameTools.GetNamespace(pageName), "Wrong document name");

            MessageDocument.GetMessageDetails(result[1].Document.Name, out pageName, out id);
            Assert.AreEqual(ns2.Name, NameTools.GetNamespace(pageName), "Wrong document name");
        }

        [Test]
        public void MovePage_Sub2Sub_WithCategories()
        {
            var prov = GetProvider();

            var ns1 = prov.AddNamespace("Namespace1");
            var ns2 = prov.AddNamespace("Namespace2");
            var cat1 = prov.AddCategory(ns1.Name, "Category1");
            var cat2 = prov.AddCategory(ns1.Name, "Category2");
            var cat2ns2 = prov.AddCategory(ns2.Name, "Category2");
            var cat3 = prov.AddCategory(ns2.Name, "Category3");

            var page = prov.AddPage(ns1.Name, "Page", DateTime.Now);
            prov.ModifyPage(page, "Title", "NUnit", DateTime.Now, "Comment", "Content", null, null, SaveMode.Normal);
            prov.ModifyPage(page, "Title0", "NUnit0", DateTime.Now, "Comment0", "Content0", null, null, SaveMode.Backup);
            prov.ModifyPage(page, "Title1", "NUnit1", DateTime.Now, "Comment1", "Content1", null, null, SaveMode.Backup);
            prov.RebindPage(page, new[] { cat2.FullName });
            prov.AddMessage(page, "NUnit", "Test", DateTime.Now, "Body", -1);

            var moved = prov.MovePage(page, ns2, true);

            var expected = new PageInfo(NameTools.GetFullName(ns2.Name, NameTools.GetLocalName(page.FullName)), prov,
                page.CreationDateTime);

            AssertPageInfosAreEqual(expected, moved, true);

            Assert.AreEqual(0, prov.GetPages(ns1).Count, "Wrong page count");

            var allPages = prov.GetPages(ns2);
            Assert.AreEqual(1, allPages.Count, "Wrong page count");
            AssertPageInfosAreEqual(expected, allPages[0], true);
            Assert.AreEqual(1, prov.GetMessages(expected).Count, "Wrong message count");

            Assert.AreEqual(2, prov.GetCategories(ns2).Count, "Wrong category count");
            Assert.AreEqual(2, prov.GetCategories(ns1).Count, "Wrong category count");

            CategoryInfo[] expectedCategories =
            {
                new CategoryInfo(NameTools.GetFullName(ns2.Name, NameTools.GetLocalName(cat2.FullName)), prov),
                new CategoryInfo(NameTools.GetFullName(ns2.Name, NameTools.GetLocalName(cat3.FullName)), prov)
            };
            expectedCategories[0].Pages = new[] { moved.FullName };

            var actualCategories = prov.GetCategories(ns2).OrderBy(x => x.FullName).ToList();
            Assert.AreEqual(expectedCategories.Length, actualCategories.Count, "Wrong category count");
            AssertCategoryInfosAreEqual(expectedCategories[0], actualCategories[0], true);
            AssertCategoryInfosAreEqual(expectedCategories[1], actualCategories[1], true);

            Assert.AreEqual(2, prov.GetBackups(expected).Length, "Wrong backup count");
            Assert.AreEqual("Content1", prov.GetContent(expected).Content, "Wrong content");
        }

        [Test]
        public void PerformSearch()
        {
            var prov = GetProvider();

            var page = prov.AddPage(null, "Page", DateTime.Now);
            prov.ModifyPage(page, "Title", "NUnit", DateTime.Now, "Comment", "Content", null, null, SaveMode.Backup);

            Assert.AreEqual(1, prov.PerformSearch(new SearchParameters("content")).Count, "Wrong result count");
        }

        [Test]
        public void RebindPage_InexistentCategoryElement_Root()
        {
            var prov = GetProvider();

            var cat1 = prov.AddCategory(null, "Cat1");
            var cat2 = prov.AddCategory(null, "Cat2");

            var page = prov.AddPage(null, "Page", DateTime.Now);

            Assert.IsFalse(prov.RebindPage(page, new[] { "Cat1", "Cat222" }), "Rebind should return false");
        }

        [Test]
        public void RebindPage_InexistentCategoryElement_Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("NS");

            var cat1 = prov.AddCategory(ns.Name, "Cat1");
            var cat2 = prov.AddCategory(ns.Name, "Cat2");

            var page = prov.AddPage(ns.Name, "Page", DateTime.Now);

            Assert.IsFalse(prov.RebindPage(page, new[] { "Cat1", "Cat222" }), "Rebind should return false");
        }

        [Test]
        public void RebindPage_Root()
        {
            var prov = GetProvider();

            var cat1 = prov.AddCategory(null, "Cat1");
            var cat2 = prov.AddCategory(null, "Cat2");

            var page = prov.AddPage(null, "Page", DateTime.Now);

            Assert.IsFalse(prov.RebindPage(new PageInfo("Inexistent", prov, DateTime.Now), new[] { "Cat1" }),
                "Rebind should return false");

            Assert.IsTrue(prov.RebindPage(page, new[] { "Cat1" }), "Rebind should return true");

            var categories = prov.GetCategories(null).OrderBy(x => x.FullName).ToList();
            Assert.AreEqual(1, categories[0].Pages.Count, "Wrong page count");
            Assert.AreEqual("Page", categories[0].Pages[0], "Wrong page name");

            Assert.IsTrue(prov.RebindPage(page, new string[0]), "Rebind should return true");

            categories = prov.GetCategories(null).OrderBy(c => c.FullName).ToList();
            Assert.AreEqual(0, categories[0].Pages.Count, "Wrong page count");
        }

        [Test]
        public void RebindPage_SameNames()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("NS");

            var cat1 = prov.AddCategory(null, "Category");
            var cat2 = prov.AddCategory(ns.Name, "Category");

            var page1 = prov.AddPage(null, "Page", DateTime.Now);
            var page2 = prov.AddPage(ns.Name, "Page", DateTime.Now);

            var done1 = prov.RebindPage(page1, new[] { cat1.FullName });
            var done2 = prov.RebindPage(page2, new[] { cat2.FullName });

            var categories1 = prov.GetCategories(null);
            Assert.AreEqual(1, categories1.Count, "Wrong category count");
            Assert.AreEqual(cat1.FullName, categories1[0].FullName, "Wrong category");
            Assert.AreEqual(1, categories1[0].Pages.Count, "Wrong page count");
            Assert.AreEqual(page1.FullName, categories1[0].Pages[0], "Wrong page");

            var categories2 = prov.GetCategories(ns);
            Assert.AreEqual(1, categories2.Count, "Wrong length");
            Assert.AreEqual(cat2.FullName, categories2[0].FullName, "Wrong category");
            Assert.AreEqual(1, categories2[0].Pages.Count, "Wrong page count");
            Assert.AreEqual(page2.FullName, categories2[0].Pages[0], "Wrong page");
        }

        [Test]
        public void RebindPage_Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("NS");

            var cat1 = prov.AddCategory(ns.Name, "Cat1");
            var cat2 = prov.AddCategory(ns.Name, "Cat2");

            var page = prov.AddPage(ns.Name, "Page", DateTime.Now);

            Assert.IsFalse(
                prov.RebindPage(new PageInfo(NameTools.GetFullName(ns.Name, "Inexistent"), prov, DateTime.Now),
                    new[] { "Cat1" }), "Rebind should return false");

            Assert.IsTrue(prov.RebindPage(page, new[] { cat1.FullName }), "Rebind should return true");

            var categories = prov.GetCategories(ns).OrderBy(c => c.FullName).ToList();
            Assert.AreEqual(1, categories[0].Pages.Count, "Wrong page count");
            Assert.AreEqual(page.FullName, categories[0].Pages[0], "Wrong page name");

            Assert.IsTrue(prov.RebindPage(page, new string[0]), "Rebind should return true");

            categories = prov.GetCategories(ns).OrderBy(c => c.FullName).ToList();
            Assert.AreEqual(0, categories[0].Pages.Count, "Wrong page count");
        }

        [Test]
        public void RebuildIndex_ManyPages()
        {
            var prov = GetProvider();

            for (var i = 0; i < PagesContent.Length; i++)
            {
                var page = prov.AddPage(null, "The Longest Page Name Ever Seen In The Whole Universe (Maybe) - " + i,
                    DateTime.Now);
                Assert.IsNotNull(page, "AddPage should return something");

                var done = prov.ModifyPage(page, "Page " + i, "NUnit", DateTime.Now, "Comment " + i,
                    PagesContent[i], null, "Test Page " + i, SaveMode.Normal);
                Assert.IsTrue(done, "ModifyPage should return true");
            }

            DoChecksFor_RebuildIndex_ManyPages(prov);

            prov.RebuildIndex();

            DoChecksFor_RebuildIndex_ManyPages(prov);
        }

        [Test]
        public void RemoveCategory_Root()
        {
            var prov = GetProvider();

            var c1 = prov.AddCategory(null, "Category1");
            var c2 = prov.AddCategory(null, "Category2");

            Assert.IsFalse(prov.RemoveCategory(new CategoryInfo("Inexistent", prov)),
                "RemoveCategory should return false");

            Assert.IsTrue(prov.RemoveCategory(c1), "RemoveCategory should return true");

            var categories = prov.GetCategories(null);
            Assert.AreEqual(1, categories.Count, "Wrong category count");
            AssertCategoryInfosAreEqual(new CategoryInfo("Category2", prov), categories[0], true);
        }

        [Test]
        public void RemoveCategory_Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("Namespace");

            var c1 = prov.AddCategory(ns.Name, "Category1");
            var c2 = prov.AddCategory(ns.Name, "Category2");

            Assert.IsFalse(prov.RemoveCategory(new CategoryInfo(NameTools.GetFullName(ns.Name, "Inexistent"), prov)),
                "RemoveCategory should return false");

            Assert.IsTrue(prov.RemoveCategory(c1), "RemoveCategory should return true");

            var categories = prov.GetCategories(ns);
            Assert.AreEqual(1, categories.Count, "Wrong category count");
            AssertCategoryInfosAreEqual(new CategoryInfo(NameTools.GetFullName(ns.Name, "Category2"), prov),
                categories[0], true);
        }

        [Test]
        public void RemoveContentTemplate()
        {
            var prov = GetProvider();

            Assert.IsFalse(prov.RemoveContentTemplate("T"), "RemoveContentTemplate should return false");

            prov.AddContentTemplate("T", "Content");
            prov.AddContentTemplate("T2", "Blah");

            Assert.IsTrue(prov.RemoveContentTemplate("T"), "RemoveContentTemplate should return true");

            var templates = prov.GetContentTemplates();
            Assert.AreEqual(1, templates.Length, "Wrong template count");

            Assert.AreEqual("T2", templates[0].Name, "Wrong name");
            Assert.AreEqual("Blah", templates[0].Content, "Wrong content");
        }

        [Test]
        public void RemoveMessage_KeepReplies_PerformSearch()
        {
            var prov = GetProvider();

            var page = prov.AddPage(null, "Page", DateTime.Now);
            prov.AddMessage(page, "NUnit", "Test", DateTime.Now, "Blah", -1);
            prov.AddMessage(page, "NUnit", "RE: Test2", DateTime.Now, "Blah2", prov.GetMessages(page)[0].ID);

            prov.RemoveMessage(page, prov.GetMessages(page)[0].ID, false);

            var result = prov.PerformSearch(new SearchParameters("test blah test2 blah2"));
            Assert.AreEqual(1, result.Count, "Wrong result count");
            Assert.AreEqual(2, result[0].Matches.Count, "Wrong match count");

            bool found1 = false, found2 = false;
            foreach (var info in result[0].Matches)
            {
                if (info.Text == "test2") found1 = true;
                if (info.Text == "blah2") found2 = true;
            }

            Assert.IsTrue(found1, "First word not found");
            Assert.IsTrue(found2, "Second word not found");
        }

        [Test]
        public void RemoveMessage_RemoveReplies_PerformSearch()
        {
            var prov = GetProvider();

            var page = prov.AddPage(null, "Page", DateTime.Now);
            prov.AddMessage(page, "NUnit", "Test", DateTime.Now, "Blah", -1);
            prov.AddMessage(page, "NUnit", "RE: Test2", DateTime.Now, "Blah2", prov.GetMessages(page)[0].ID);

            prov.RemoveMessage(page, prov.GetMessages(page)[0].ID, true);

            var result = prov.PerformSearch(new SearchParameters("test blah test2 blah2"));
            Assert.AreEqual(0, result.Count, "Wrong result count");
        }

        [Test]
        public void RemoveMessage_Root()
        {
            var prov = GetProvider();

            var page = prov.AddPage(null, "Page", DateTime.Now);

            // Subject0
            //    Subject00
            // Subject1
            //    Subject11

            prov.AddMessage(page, "NUnit0", "Subject0", DateTime.Now, "Body0", -1);
            prov.AddMessage(page, "NUnit00", "Subject00", DateTime.Now.AddHours(1), "Body00",
                prov.GetMessages(page)[0].ID);
            prov.AddMessage(page, "NUnit1", "Subject1", DateTime.Now.AddHours(2), "Body1", -1);
            prov.AddMessage(page, "NUnit11", "Subject11", DateTime.Now.AddHours(3), "Body11",
                prov.GetMessages(page)[1].ID);

            var messages = prov.GetMessages(page);

            Assert.IsFalse(prov.RemoveMessage(page, 5, true), "RemoveMessage should return false");

            Assert.IsFalse(prov.RemoveMessage(new PageInfo("Inexistent", prov, DateTime.Now), 1, true),
                "RemoveMessage should return false");

            Assert.IsTrue(prov.RemoveMessage(page, messages[0].ID, false), "RemoveMessage should return true");

            Assert.AreEqual(3, prov.GetMessageCount(page), "Wrong message count");
            Assert.AreEqual("Subject00", prov.GetMessages(page)[0].Subject, "Wrong message");
            Assert.AreEqual("Subject1", prov.GetMessages(page)[1].Subject, "Wrong message");

            Assert.IsTrue(prov.RemoveMessage(page, messages[1].ID, true), "RemoveMessages should return true");
            Assert.AreEqual(1, prov.GetMessageCount(page), "Wrong message count");
            Assert.AreEqual("Subject00", prov.GetMessages(page)[0].Subject, "Wrong message");
        }

        [Test]
        public void RemoveMessage_Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("NS");

            var page = prov.AddPage(ns.Name, "Page", DateTime.Now);

            // Subject0
            //    Subject00
            // Subject1
            //    Subject11

            prov.AddMessage(page, "NUnit0", "Subject0", DateTime.Now, "Body0", -1);
            prov.AddMessage(page, "NUnit00", "Subject00", DateTime.Now.AddHours(1), "Body00",
                prov.GetMessages(page)[0].ID);
            prov.AddMessage(page, "NUnit1", "Subject1", DateTime.Now.AddHours(2), "Body1", -1);
            prov.AddMessage(page, "NUnit11", "Subject11", DateTime.Now.AddHours(3), "Body11",
                prov.GetMessages(page)[1].ID);

            var messages = prov.GetMessages(page);

            Assert.IsFalse(prov.RemoveMessage(page, 5, true), "RemoveMessage should return false");
            Assert.IsFalse(prov.RemoveMessage(new PageInfo(NameTools.GetFullName(ns.Name, "Inexistent"),
                prov, DateTime.Now), 1, true), "RemoveMessage should return false");

            Assert.IsTrue(prov.RemoveMessage(page, messages[0].ID, false), "RemoveMessage should return true");

            Assert.AreEqual(3, prov.GetMessageCount(page), "Wrong message count");
            Assert.AreEqual("Subject00", prov.GetMessages(page)[0].Subject, "Wrong message");
            Assert.AreEqual("Subject1", prov.GetMessages(page)[1].Subject, "Wrong message");

            Assert.IsTrue(prov.RemoveMessage(page, messages[1].ID, true), "RemoveMessages should return true");
            Assert.AreEqual(1, prov.GetMessageCount(page), "Wrong message count");
            Assert.AreEqual("Subject00", prov.GetMessages(page)[0].Subject, "Wrong message");
        }

        [Test]
        public void RemoveNamespace()
        {
            var prov = GetProvider();

            Assert.IsFalse(prov.RemoveNamespace(new NamespaceInfo("Inexistent", prov, null)),
                "RemoveNamespace should return alse");

            prov.AddNamespace("Sub");
            prov.AddNamespace("Sub2");

            Assert.IsTrue(prov.RemoveNamespace(new NamespaceInfo("Sub2", prov, null)),
                "RemoveNamespace should return true");

            var allNS = prov.GetNamespaces();
            Assert.AreEqual(1, allNS.Count, "Wrong namespace count");

            AssertNamespaceInfosAreEqual(new NamespaceInfo("Sub", prov, null), allNS[0], true);
        }

        [Test]
        public void RemoveNamespace_PerformSearch()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("NS");

            var page1 = prov.AddPage(ns.Name, "Page1", DateTime.Now);
            prov.ModifyPage(page1, "Title1", "NUnit", DateTime.Now, "Comment1", "Content1", new string[0], "Descr1",
                SaveMode.Normal);

            prov.AddMessage(page1, "NUnit", "Test1", DateTime.Now, "Body1", -1);
            prov.AddMessage(page1, "NUnit", "Test2", DateTime.Now, "Body2", prov.GetMessages(page1)[0].ID);

            var page2 = prov.AddPage(ns.Name, "Page2", DateTime.Now);
            prov.ModifyPage(page2, "Title2", "NUnit", DateTime.Now, "Comment2", "Content2", new string[0], "Descr2",
                SaveMode.Normal);

            prov.RemoveNamespace(ns);

            Assert.AreEqual(0, prov.PerformSearch(new SearchParameters("content1 content2")).Count, "Wrong result count");

            Assert.AreEqual(0, prov.PerformSearch(new SearchParameters("test1 test2 comment1 comment2")).Count,
                "Wrong result count");
        }

        [Test]
        public void RemoveNavigationPath_Root()
        {
            var prov = GetProvider();

            var page1 = prov.AddPage(null, "Page1", DateTime.Now);
            var page2 = prov.AddPage(null, "Page2", DateTime.Now);

            var path1 = prov.AddNavigationPath(null, "Path1", new[] { page1, page2 });
            var path2 = prov.AddNavigationPath(null, "Path2", new[] { page2, page1 });

            Assert.IsFalse(prov.RemoveNavigationPath(new NavigationPath("Inexistent", prov)),
                "RemoveNavigationPath should return false");

            Assert.IsTrue(prov.RemoveNavigationPath(path2), "RemoveNavigationPath should return true");

            var paths = prov.GetNavigationPaths(null);
            Assert.AreEqual(1, paths.Count, "Wrong navigation path count");
            Assert.AreEqual("Path1", paths[0].FullName, "Wrong name");
            Assert.AreEqual(page1.FullName, paths[0].Pages[0], "Wrong page at position 0");
            Assert.AreEqual(page2.FullName, paths[0].Pages[1], "Wrong page at position 1");
        }

        [Test]
        public void RemoveNavigationPath_Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("NS");

            var page1 = prov.AddPage(ns.Name, "Page1", DateTime.Now);
            var page2 = prov.AddPage(ns.Name, "Page2", DateTime.Now);

            var path1 = prov.AddNavigationPath(ns.Name, "Path1", new[] { page1, page2 });
            var path2 = prov.AddNavigationPath(ns.Name, "Path2", new[] { page2, page1 });

            Assert.IsFalse(
                prov.RemoveNavigationPath(new NavigationPath(NameTools.GetFullName(ns.Name, "Inexistent"), prov)),
                "RemoveNavigationPath should return false");

            Assert.IsTrue(prov.RemoveNavigationPath(path2), "RemoveNavigationPath should return true");

            var paths = prov.GetNavigationPaths(ns);
            Assert.AreEqual(1, paths.Count, "Wrong navigation path count");
            Assert.AreEqual(NameTools.GetFullName(ns.Name, "Path1"), paths[0].FullName, "Wrong name");
            Assert.AreEqual(page1.FullName, paths[0].Pages[0], "Wrong page at position 0");
            Assert.AreEqual(page2.FullName, paths[0].Pages[1], "Wrong page at position 1");
        }

        [Test]
        public void RemovePage_DefaultPage()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("NS");

            var page = prov.AddPage("NS", "MainPage", DateTime.Now);

            prov.SetNamespaceDefaultPage(ns, page);

            Assert.IsFalse(prov.RemovePage(page), "Cannot remove default page");
        }

        [Test]
        public void RemovePage_PerformSearch()
        {
            var prov = GetProvider();

            var p = prov.AddPage(null, "Page", DateTime.Now);

            prov.AddMessage(p, "NUnit", "Test1", DateTime.Now, "Body1", -1);
            prov.AddMessage(p, "NUnit", "Test2", DateTime.Now, "Body2", prov.GetMessages(p)[0].ID);

            var dt = DateTime.Now;

            Assert.IsTrue(prov.ModifyPage(p, "Title", "NUnit", dt, "Comment", "Content", null, null, SaveMode.Normal),
                "ModifyPage should return true");

            prov.RemovePage(p);

            Assert.AreEqual(0, prov.PerformSearch(new SearchParameters("content")).Count, "Wrong search result count");
            Assert.AreEqual(0, prov.PerformSearch(new SearchParameters("title")).Count, "Wrong search result count");

            Assert.AreEqual(0, prov.PerformSearch(new SearchParameters("test1 test2 body1 body2")).Count,
                "Wrong result count");
        }

        [Test]
        public void RemovePage_Root()
        {
            var prov = GetProvider();

            var p = prov.AddPage(null, "Page", DateTime.Now);
            prov.ModifyPage(p, "Title", "NUnit", DateTime.Now, "Comment", "Content", null, null, SaveMode.Normal);

            Assert.IsFalse(prov.RemovePage(new PageInfo("Inexistent", prov, DateTime.Now)),
                "RemovePage should return false");

            Assert.IsTrue(prov.RemovePage(p), "RemovePage should return true");

            Assert.AreEqual(0, prov.GetPages(null).Count, "Wrong page count");
            Assert.IsNull(prov.GetContent(p), "GetContent should return null");
            Assert.IsNull(prov.GetBackups(p), "GetBackups should return null");
        }

        [Test]
        public void RemovePage_Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("NS");

            var p = prov.AddPage(ns.Name, "Page", DateTime.Now);
            prov.ModifyPage(p, "Title", "NUnit", DateTime.Now, "Comment", "Content", null, null, SaveMode.Normal);

            Assert.IsFalse(prov.RemovePage(new PageInfo(NameTools.GetFullName(ns.Name, "Inexistent"),
                prov, DateTime.Now)), "RemovePage should return false");

            Assert.IsTrue(prov.RemovePage(p), "RemovePage should return true");

            Assert.AreEqual(0, prov.GetPages(ns).Count, "Wrong page count");
            Assert.IsNull(prov.GetContent(p), "GetContent should return null");
            Assert.IsNull(prov.GetBackups(p), "GetBackups should return null");
        }

        [Test]
        public void RemoveSnippet()
        {
            var prov = GetProvider();

            Assert.IsFalse(prov.RemoveSnippet("Inexistent"), "RemoveSnippet should return false");

            prov.AddSnippet("Snippet1", "Content1");
            prov.AddSnippet("Snippet2", "Content2");

            Assert.IsTrue(prov.RemoveSnippet("Snippet2"), "RemoveSnippet should return true");

            var snippets = prov.GetSnippets();
            Assert.AreEqual(1, snippets.Count, "Wrong snippet count");

            Assert.AreEqual("Snippet1", snippets[0].Name, "Wrong name");
            Assert.AreEqual("Content1", snippets[0].Content, "Wrong content");
        }

        [Test]
        public void RenameCategory_Root()
        {
            var prov = GetProvider();

            var c1 = prov.AddCategory(null, "Category1");
            var c2 = prov.AddCategory(null, "Category2");

            var page = prov.AddPage(null, "Page", DateTime.Now);
            prov.RebindPage(page, new[] { c2.FullName });

            Assert.IsNull(prov.RenameCategory(new CategoryInfo("Inexistent", prov), "NewName"),
                "RenameCategory should return null");
            Assert.IsNull(prov.RenameCategory(c2, "Category1"), "RenameCategory should return null");

            var c3 = new CategoryInfo("Category3", prov);
            c3.Pages = new[] { page.FullName };
            AssertCategoryInfosAreEqual(c3, prov.RenameCategory(c2, "Category3"), true);

            var categories = prov.GetCategories(null).OrderBy(c => c.FullName).ToList();
            Assert.AreEqual(2, categories.Count, "Wrong category count");

            AssertCategoryInfosAreEqual(new CategoryInfo("Category1", prov), categories[0], true);
            AssertCategoryInfosAreEqual(c3, categories[1], true);
        }

        [Test]
        public void RenameCategory_Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("Namespace");

            var c1 = prov.AddCategory(ns.Name, "Category1");
            var c2 = prov.AddCategory(ns.Name, "Category2");

            var page = prov.AddPage(ns.Name, "Page", DateTime.Now);
            prov.RebindPage(page, new[] { c2.FullName });

            Assert.IsNull(
                prov.RenameCategory(new CategoryInfo(NameTools.GetFullName(ns.Name, "Inexistent"), prov), "NewName"),
                "RenameCategory should return null");
            Assert.IsNull(prov.RenameCategory(c2, "Category1"), "RenameCategory should return null");

            var c3 = new CategoryInfo(NameTools.GetFullName(ns.Name, "Category3"), prov);
            c3.Pages = new[] { page.FullName };
            AssertCategoryInfosAreEqual(c3, prov.RenameCategory(c2, "Category3"), true);

            var categories = prov.GetCategories(ns).OrderBy(x => x.FullName).ToList();
            Assert.AreEqual(2, categories.Count, "Wrong category count");

            AssertCategoryInfosAreEqual(new CategoryInfo(NameTools.GetFullName(ns.Name, "Category1"), prov),
                categories[0], true);
            AssertCategoryInfosAreEqual(c3, categories[1], true);
        }

        [Test]
        public void RenameNamespace()
        {
            var prov = GetProvider();

            var sub = prov.AddNamespace("Sub");
            prov.AddNamespace("Sub2");

            var cat = prov.AddCategory(sub.Name, "Cat");

            var page = prov.AddPage("Sub", "Page", DateTime.Now);
            prov.AddMessage(page, "NUnit", "Test", DateTime.Now, "Body", -1);

            prov.SetNamespaceDefaultPage(sub, page);

            prov.RebindPage(page, new[] { cat.FullName });

            Assert.IsNull(prov.RenameNamespace(new NamespaceInfo("Inexistent", prov, null), "NewName"),
                "RenameNamespace should return null");

            var ns = prov.RenameNamespace(new NamespaceInfo("Sub", prov, null), "Sub1");

            AssertNamespaceInfosAreEqual(
                new NamespaceInfo("Sub1", prov, new PageInfo("Sub1.Page", prov, page.CreationDateTime)), ns, true);

            var allNS = prov.GetNamespaces();
            Assert.AreEqual(2, allNS.Count, "Wrong namespace count");

            allNS = allNS.OrderBy(x => x.Name).ToList();
            AssertNamespaceInfosAreEqual(
                new NamespaceInfo("Sub1", prov, new PageInfo("Sub1.Page", prov, page.CreationDateTime)), allNS[0], true);
            AssertNamespaceInfosAreEqual(new NamespaceInfo("Sub2", prov, null), allNS[1], true);

            Assert.AreEqual(1,
                prov.GetMessages(new PageInfo(NameTools.GetFullName("Sub1", "Page"), prov, page.CreationDateTime))
                    .Count, "Wrong message count");

            var categories = prov.GetCategories(ns);
            Assert.AreEqual(1, categories.Count, "Wrong category count");
            Assert.AreEqual(NameTools.GetFullName(ns.Name, NameTools.GetLocalName(cat.FullName)), categories[0].FullName,
                "Wrong category name");
            Assert.AreEqual(1, categories[0].Pages.Count, "Wrong page count");
            Assert.AreEqual(NameTools.GetFullName(ns.Name, NameTools.GetLocalName(page.FullName)),
                categories[0].Pages[0], "Wrong page");
        }

        [Test]
        public void RenameNamespace_PerformSearch()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("NS");

            var page1 = prov.AddPage(ns.Name, "Page1", DateTime.Now);
            prov.ModifyPage(page1, "Title1", "NUnit", DateTime.Now, "Comment1", "Content1", new string[0], "Descr1",
                SaveMode.Normal);

            var page2 = prov.AddPage(ns.Name, "Page2", DateTime.Now);
            prov.ModifyPage(page2, "Title2", "NUnit", DateTime.Now, "Comment2", "Content2", new string[0], "Descr2",
                SaveMode.Normal);

            prov.AddMessage(page1, "NUnit", "Test1", DateTime.Now, "Body1", -1);
            prov.AddMessage(page1, "NUnit", "Test2", DateTime.Now, "Body2", prov.GetMessages(page1)[0].ID);

            var renamedNamespace = prov.RenameNamespace(ns, "NS_Ren");

            var result = prov.PerformSearch(new SearchParameters("content1 content2"));
            Assert.AreEqual(2, result.Count, "Wrong result count");
            Assert.AreEqual(renamedNamespace.Name,
                NameTools.GetNamespace(PageDocument.GetPageName(result[0].Document.Name)), "Wrong document name");
            Assert.AreEqual(renamedNamespace.Name,
                NameTools.GetNamespace(PageDocument.GetPageName(result[1].Document.Name)), "Wrong document name");

            result = prov.PerformSearch(new SearchParameters("test1 test2"));
            Assert.AreEqual(2, result.Count, "Wrong result count");
            Assert.AreEqual(1, result[0].Matches.Count, "Wrong match count");
            Assert.AreEqual(1, result[1].Matches.Count, "Wrong match count");

            string page;
            int id;

            MessageDocument.GetMessageDetails(result[0].Document.Name, out page, out id);
            Assert.AreEqual(renamedNamespace.Name, NameTools.GetNamespace(page), "Wrong document name");

            MessageDocument.GetMessageDetails(result[1].Document.Name, out page, out id);
            Assert.AreEqual(renamedNamespace.Name, NameTools.GetNamespace(page), "Wrong document name");
        }

        [Test]
        public void RenamePage_DefaultPage()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("NS");

            var page = prov.AddPage("NS", "MainPage", DateTime.Now);

            prov.SetNamespaceDefaultPage(ns, page);

            Assert.IsNull(prov.RenamePage(page, "NewName"), "Cannot rename the default page");
        }

        [Test]
        public void RenamePage_PerformSearch()
        {
            var prov = GetProvider();

            var p = prov.AddPage(null, "Page", DateTime.Now);
            prov.AddMessage(p, "NUnit", "Message1", DateTime.Now, "Body1", -1);
            prov.AddMessage(p, "NUnit", "Message2", DateTime.Now, "Body2", prov.GetMessages(p)[0].ID);

            var dt = DateTime.Now;

            Assert.IsTrue(
                prov.ModifyPage(p, "TitleOld", "NUnitOld", dt, "CommentOld", "ContentOld", null, null, SaveMode.Normal),
                "ModifyPage should return true");
            Assert.IsTrue(prov.ModifyPage(p, "Title", "NUnit", dt, "Comment", "Content", null, null, SaveMode.Backup),
                "ModifyPage should return true");
            prov.RenamePage(p, "Page2");

            var results = prov.PerformSearch(new SearchParameters("content"));

            Assert.AreEqual(1, results.Count, "Wrong search result count");
            Assert.AreEqual("Page2", PageDocument.GetPageName(results[0].Document.Name), "Wrong document name");

            results = prov.PerformSearch(new SearchParameters("title"));

            Assert.AreEqual(1, results.Count, "Wrong search result count");
            Assert.AreEqual("Page2", PageDocument.GetPageName(results[0].Document.Name), "Wrong document name");

            results = prov.PerformSearch(new SearchParameters("message1 body1 message2 body2"));
            Assert.AreEqual(2, results.Count, "Wrong result count");
            Assert.AreEqual(2, results[0].Matches.Count, "Wrong match count");
            Assert.AreEqual(2, results[1].Matches.Count, "Wrong match count");

            string page;
            int id;

            MessageDocument.GetMessageDetails(results[0].Document.Name, out page, out id);
            Assert.AreEqual("Page2", page, "Wrong document name");

            MessageDocument.GetMessageDetails(results[1].Document.Name, out page, out id);
            Assert.AreEqual("Page2", page, "Wrong document name");
        }

        [Test]
        public void RenamePage_Root()
        {
            var prov = GetProvider();

            var p = prov.AddPage(null, "Page", DateTime.Now);
            prov.AddPage(null, "Page2", DateTime.Now);
            prov.ModifyPage(p, "Title", "NUnit", DateTime.Now, "Comment", "Content", null, null, SaveMode.Normal);
            prov.ModifyPage(p, "Title1", "NUnit1", DateTime.Now, "Comment1", "Content1", null, null, SaveMode.Backup);
            var cat1 = prov.AddCategory(null, "Cat1");
            var cat2 = prov.AddCategory(null, "Cat2");
            var cat3 = prov.AddCategory(null, "Cat3");
            prov.RebindPage(p, new[] { cat1.FullName, cat3.FullName });
            var content = prov.GetContent(p);

            Assert.IsTrue(prov.AddMessage(p, "NUnit", "Test", DateTime.Now, "Test message.", -1),
                "AddMessage should return true");

            Assert.IsNull(prov.RenamePage(new PageInfo("Inexistent", prov, DateTime.Now), "RenamedPage"),
                "RenamePage should return null");
            Assert.IsNull(prov.RenamePage(p, "Page2"), "RenamePage should return null");

            var renamed = prov.RenamePage(p, "Renamed");
            Assert.IsNotNull(renamed, "RenamePage should return something");
            AssertPageInfosAreEqual(new PageInfo("Renamed", prov, p.CreationDateTime), renamed, true);

            Assert.IsNull(prov.GetContent(p), "GetContent should return null");

            AssertPageContentsAreEqual(new PageContent(renamed, content.Title, content.User, content.LastModified,
                content.Comment, content.Content, null, null),
                prov.GetContent(renamed));

            Assert.IsNull(prov.GetBackups(p), "GetBackups should return null");
            Assert.AreEqual(1, prov.GetBackups(renamed).Length, "Wrong backup count");

            var categories = prov.GetCategories(null);
            Assert.AreEqual(3, categories.Count, "Wrong category count");
            categories = categories.OrderBy(c => c.FullName).ToList();

            Assert.AreEqual(1, categories[0].Pages.Count, "Wrong page count");
            Assert.AreEqual(renamed.FullName, categories[0].Pages[0], "Wrong page");
            Assert.AreEqual(0, categories[1].Pages.Count, "Wrong page count");
            Assert.AreEqual(1, categories[2].Pages.Count, "Wrong page count");
            Assert.AreEqual(renamed.FullName, categories[2].Pages[0], "Wrong page");

            Assert.IsNull(prov.GetMessages(p), "GetMessages should return null");
            var messages = prov.GetMessages(renamed);
            Assert.AreEqual(1, messages.Count, "Wrong message count");
            Assert.AreEqual("Test", messages[0].Subject, "Wrong message subject");
        }

        [Test]
        public void RenamePage_Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("NS");

            var p = prov.AddPage(ns.Name, "Page", DateTime.Now);
            prov.AddPage(ns.Name, "Page2", DateTime.Now);
            prov.ModifyPage(p, "Title", "NUnit", DateTime.Now, "Comment", "Content", null, null, SaveMode.Normal);
            prov.ModifyPage(p, "Title1", "NUnit1", DateTime.Now, "Comment1", "Content1", null, null, SaveMode.Backup);
            var cat1 = prov.AddCategory(ns.Name, "Cat1");
            var cat2 = prov.AddCategory(ns.Name, "Cat2");
            var cat3 = prov.AddCategory(ns.Name, "Cat3");
            prov.RebindPage(p, new[] { cat1.FullName, cat3.FullName });
            var content = prov.GetContent(p);

            Assert.IsTrue(prov.AddMessage(p, "NUnit", "Test", DateTime.Now, "Test message.", -1),
                "AddMessage should return true");

            Assert.IsNull(
                prov.RenamePage(new PageInfo(NameTools.GetFullName(ns.Name, "Inexistent"), prov, DateTime.Now),
                    "RenamedPage"), "RenamePage should return null");
            Assert.IsNull(prov.RenamePage(p, "Page2"), "RenamePage should return null");

            var renamed = prov.RenamePage(p, "Renamed");
            Assert.IsNotNull(renamed, "RenamePage should return something");
            AssertPageInfosAreEqual(new PageInfo(NameTools.GetFullName(ns.Name, "Renamed"), prov, p.CreationDateTime),
                renamed, true);

            Assert.IsNull(prov.GetContent(p), "GetContent should return null");

            AssertPageContentsAreEqual(
                new PageContent(renamed, content.Title, content.User, content.LastModified, content.Comment,
                    content.Content, null, null),
                prov.GetContent(renamed));

            Assert.IsNull(prov.GetBackups(p), "GetBackups should return null");
            Assert.AreEqual(1, prov.GetBackups(renamed).Length, "Wrong backup count");

            var categories = prov.GetCategories(ns).OrderBy(x => x.FullName).ToList();
            Assert.AreEqual(3, categories.Count, "Wrong category count");

            Assert.AreEqual(1, categories[0].Pages.Count, "Wrong page count");
            Assert.AreEqual(renamed.FullName, categories[0].Pages[0], "Wrong page");
            Assert.AreEqual(0, categories[1].Pages.Count, "Wrong page count");
            Assert.AreEqual(1, categories[2].Pages.Count, "Wrong page count");
            Assert.AreEqual(renamed.FullName, categories[2].Pages[0], "Wrong page");

            Assert.IsNull(prov.GetMessages(p), "GetMessages should return null");
            var messages = prov.GetMessages(renamed);
            Assert.AreEqual(1, messages.Count, "Wrong message count");
            Assert.AreEqual("Test", messages[0].Subject, "Wrong message subject");
        }

        [Test]
        public void RollbackPage_PerformSearch()
        {
            var prov = GetProvider();

            var p = prov.AddPage(null, "Page", DateTime.Now);

            var dt = DateTime.Now;

            Assert.IsTrue(
                prov.ModifyPage(p, "Title", "NUnit", dt, "Comment", "Content", new[] { "k1" }, "descr", SaveMode.Backup),
                "ModifyPage should return true");
            Assert.IsTrue(
                prov.ModifyPage(p, "TitleMod", "NUnit", dt, "Comment", "ContentMod", new[] { "k2" }, "descr2",
                    SaveMode.Backup), "ModifyPage should return true");
            // Depending on the implementation, providers might start backups numbers from 0 or 1, or even don't perform a backup if the page has no content (as in this case)
            var baks = prov.GetBackups(p);
            prov.RollbackPage(p, baks[baks.Length - 1]);

            Assert.AreEqual(0, prov.PerformSearch(new SearchParameters("contentmod")).Count, "Wrong search result count");
            Assert.AreEqual(1, prov.PerformSearch(new SearchParameters("content")).Count, "Wrong search result count");

            Assert.AreEqual(0, prov.PerformSearch(new SearchParameters("k2")).Count, "Wrong search result count");
            Assert.AreEqual(1, prov.PerformSearch(new SearchParameters("k1")).Count, "Wrong search result count");

            Assert.AreEqual(0, prov.PerformSearch(new SearchParameters("titlemod")).Count, "Wrong search result count");
            Assert.AreEqual(1, prov.PerformSearch(new SearchParameters("title")).Count, "Wrong search result count");
        }

        [Test]
        public void RollbackPage_Root()
        {
            var prov = GetProvider();

            var p = prov.AddPage(null, "Page", DateTime.Now);
            prov.ModifyPage(p, "Title", "NUnit", DateTime.Now, "Comment", "Content", new[] { "kold", "k2old" }, "DescrOld",
                SaveMode.Normal);
            prov.ModifyPage(p, "Title1", "NUnit1", DateTime.Now, "Comment1", "Content1", new[] { "k1", "k2" }, "Descr",
                SaveMode.Backup);

            var content = prov.GetContent(p);

            prov.ModifyPage(p, "Title2", "NUnit2", DateTime.Now, "Comment2", "Content2", new[] { "k4", "k5" }, "DescrNew",
                SaveMode.Backup);

            Assert.AreEqual(2, prov.GetBackups(p).Length, "Wrong backup count");

            Assert.IsFalse(prov.RollbackPage(new PageInfo("Inexistent", prov, DateTime.Now), 0),
                "RollbackPage should return false");
            Assert.IsFalse(prov.RollbackPage(p, 5), "RollbackPage should return false");

            Assert.IsTrue(prov.RollbackPage(p, 1), "RollbackPage should return true");

            Assert.AreEqual(3, prov.GetBackups(p).Length, "Wrong backup count");

            AssertPageContentsAreEqual(content, prov.GetContent(p));
        }

        [Test]
        public void RollbackPage_Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("NS");

            var p = prov.AddPage(ns.Name, "Page", DateTime.Now);
            prov.ModifyPage(p, "Title", "NUnit", DateTime.Now, "Comment", "Content", new[] { "kold", "k2old" }, "DescrOld",
                SaveMode.Normal);
            prov.ModifyPage(p, "Title1", "NUnit1", DateTime.Now, "Comment1", "Content1", new[] { "k1", "k2" }, "Descr",
                SaveMode.Backup);

            var content = prov.GetContent(p);

            prov.ModifyPage(p, "Title2", "NUnit2", DateTime.Now, "Comment2", "Content2", new[] { "k4", "k5" }, "DescrNew",
                SaveMode.Backup);

            Assert.AreEqual(2, prov.GetBackups(p).Length, "Wrong backup count");

            Assert.IsFalse(
                prov.RollbackPage(new PageInfo(NameTools.GetFullName(ns.Name, "Inexistent"), prov, DateTime.Now), 0),
                "RollbackPage should return false");

            Assert.IsFalse(prov.RollbackPage(p, 5), "RollbackPage should return false");

            Assert.IsTrue(prov.RollbackPage(p, 1), "RollbackPage should return true");

            Assert.AreEqual(3, prov.GetBackups(p).Length, "Wrong backup count");

            AssertPageContentsAreEqual(content, prov.GetContent(p));
        }

        [Test]
        public void SetBackupContent_GetBackupContent_Root()
        {
            var prov = GetProvider();

            var p = prov.AddPage(null, "Page", DateTime.Now);
            prov.ModifyPage(p, "Title", "NUnit", DateTime.Now, "Comment", "Content", null, null, SaveMode.Normal);
            prov.ModifyPage(p, "Title1", "NUnit1", DateTime.Now, "Comment1", "Content1", null, null, SaveMode.Backup);

            var content = new PageContent(p, "Title100", "NUnit100", DateTime.Now.AddDays(-1), "Comment100",
                "Content100", null, null);
            var contentInexistent = new PageContent(new PageInfo("PPP", prov, DateTime.Now),
                "Title100", "NUnit100", DateTime.Now.AddDays(-1), "Comment100", "Content100", null, null);

            Assert.IsFalse(prov.SetBackupContent(contentInexistent, 0), "SetBackupContent should return ");

            Assert.IsTrue(prov.SetBackupContent(content, 0), "SetBackupContent should return true");

            var contentOutput = prov.GetBackupContent(p, 0);

            AssertPageContentsAreEqual(content, contentOutput);
        }

        [Test]
        public void SetBackupContent_GetBackupContent_Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("Namespace");

            var p = prov.AddPage(ns.Name, "Page", DateTime.Now);
            prov.ModifyPage(p, "Title", "NUnit", DateTime.Now, "Comment", "Content", null, null, SaveMode.Normal);
            prov.ModifyPage(p, "Title1", "NUnit1", DateTime.Now, "Comment1", "Content1", null, null, SaveMode.Backup);

            var content = new PageContent(p, "Title100", "NUnit100", DateTime.Now.AddDays(-1), "Comment100",
                "Content100", null, null);
            var contentInexistent =
                new PageContent(new PageInfo(NameTools.GetFullName(ns.Name, "PPP"), prov, DateTime.Now),
                    "Title100", "NUnit100", DateTime.Now.AddDays(-1), "Comment100", "Content100", null, null);

            Assert.IsFalse(prov.SetBackupContent(contentInexistent, 0), "SetBackupContent should return ");

            Assert.IsTrue(prov.SetBackupContent(content, 0), "SetBackupContent should return true");

            var contentOutput = prov.GetBackupContent(p, 0);

            AssertPageContentsAreEqual(content, contentOutput);
        }

        [Test]
        public void SetBackupContent_InexistentRevision_Root()
        {
            var prov = GetProvider();

            var p = prov.AddPage(null, "Page", DateTime.Now);
            prov.ModifyPage(p, "Title", "NUnit", DateTime.Now, "Comment", "Content", null, null, SaveMode.Normal);
            prov.ModifyPage(p, "Title1", "NUnit1", DateTime.Now, "Comment1", "Content1", null, null, SaveMode.Backup);

            var testContent = new PageContent(p, "Title100", "NUnit100", DateTime.Now, "Comment100", "Content100", null,
                null);

            prov.SetBackupContent(testContent, 5);

            var backup = prov.GetBackupContent(p, 5);

            AssertPageContentsAreEqual(testContent, backup);

            var baks = prov.GetBackups(p);
            Assert.AreEqual(2, baks.Length, "Wrong backup count");
            Assert.AreEqual(0, baks[0], "Wrong backup number");
            Assert.AreEqual(5, baks[1], "Wrong backup number");
        }

        [Test]
        public void SetBackupContent_InexistentRevision_Sub()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("NS");

            var p = prov.AddPage(ns.Name, "Page", DateTime.Now);
            prov.ModifyPage(p, "Title", "NUnit", DateTime.Now, "Comment", "Content", null, null, SaveMode.Normal);
            prov.ModifyPage(p, "Title1", "NUnit1", DateTime.Now, "Comment1", "Content1", null, null, SaveMode.Backup);

            var testContent = new PageContent(p, "Title100", "NUnit100", DateTime.Now, "Comment100", "Content100", null,
                null);

            prov.SetBackupContent(testContent, 5);

            var backup = prov.GetBackupContent(p, 5);

            AssertPageContentsAreEqual(testContent, backup);

            var baks = prov.GetBackups(p);
            Assert.AreEqual(2, baks.Length, "Wrong backup count");
            Assert.AreEqual(0, baks[0], "Wrong backup number");
            Assert.AreEqual(5, baks[1], "Wrong backup number");
        }

        [Test]
        public void SetNamespaceDefaultPage()
        {
            var prov = GetProvider();

            var ns = prov.AddNamespace("NS");

            var page = prov.AddPage(ns.Name, "Page", DateTime.Now);

            Assert.IsNull(prov.SetNamespaceDefaultPage(new NamespaceInfo("Inexistent", prov, null),
                new PageInfo(NameTools.GetFullName("Inexistent", "Page"), prov, DateTime.Now)),
                "SetNamespaceDefaultPage should return null when the namespace does not exist");

            Assert.IsNull(
                prov.SetNamespaceDefaultPage(ns,
                    new PageInfo(NameTools.GetFullName(ns.Name, "Inexistent"), prov, DateTime.Now)),
                "SetNamespaceDefaultPage should return null when the page does not exist");

            var result = prov.SetNamespaceDefaultPage(ns, page);

            AssertNamespaceInfosAreEqual(new NamespaceInfo(ns.Name, prov, page), result, true);

            result = prov.SetNamespaceDefaultPage(ns, null);

            AssertNamespaceInfosAreEqual(new NamespaceInfo(ns.Name, prov, null), result, true);
        }
    }
}