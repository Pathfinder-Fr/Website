using System;
using System.Threading;
using NUnit.Framework;
using Rhino.Mocks;
using ScrewTurn.Wiki.PluginFramework;

namespace ScrewTurn.Wiki.Tests
{
    [TestFixture]
    public abstract class CacheProviderTestScaffolding
    {
        private readonly MockRepository mocks = new MockRepository();

        protected IHostV30 MockHost()
        {
            var host = mocks.DynamicMock<IHostV30>();

            Expect.Call(host.GetSettingValue(SettingName.CacheSize)).Return("20").Repeat.Any();
            Expect.Call(host.GetSettingValue(SettingName.EditingSessionTimeout)).Return("1").Repeat.Any();

            mocks.Replay(host);

            return host;
        }

        protected IPagesStorageProviderV30 MockPagesProvider()
        {
            var prov = mocks.DynamicMock<IPagesStorageProviderV30>();

            mocks.Replay(prov);

            return prov;
        }

        public abstract ICacheProviderV30 GetProvider();

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void GetPseudoCacheValue_InvalidName(string n)
        {
            var prov = GetProvider();
            prov.GetPseudoCacheValue(n);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void SetPseudoCacheValue_InvalidName(string n)
        {
            var prov = GetProvider();
            prov.SetPseudoCacheValue(n, "Value");
        }

        [Test]
        public void GetPageContent_NullPage()
        {
            var prov = GetProvider();
            Assert.Throws<ArgumentNullException>(() =>
            {
                prov.GetPageContent(null);
            });
        }

        [TestCase(-1, ExpectedException = typeof(ArgumentOutOfRangeException))]
        [TestCase(0, ExpectedException = typeof(ArgumentOutOfRangeException))]
        public void CutCache_InvalidSize(int s)
        {
            var prov = GetProvider();
            prov.CutCache(s);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void RenewEditingSession_InvalidPage(string p)
        {
            var prov = GetProvider();
            prov.RenewEditingSession(p, "User");
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void RenewEditingSession_InvalidUser(string u)
        {
            var prov = GetProvider();
            prov.RenewEditingSession("Page", u);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void IsPageBeingEdited_InvalidPage(string p)
        {
            var prov = GetProvider();
            prov.IsPageBeingEdited(p, "User");
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void IsPageBeingEdited_InvalidUser(string u)
        {
            var prov = GetProvider();
            prov.IsPageBeingEdited("Page", u);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void CancelEditingSession_InvalidPage(string p)
        {
            var prov = GetProvider();
            prov.CancelEditingSession(p, "User");
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void CancelEditingSession_InvalidUser(string u)
        {
            var prov = GetProvider();
            prov.CancelEditingSession("Page", u);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void WhosEditing_InvalidPage(string p)
        {
            var prov = GetProvider();
            prov.WhosEditing(p);
        }

        [TestCase(null, "destination", ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", "destination", ExpectedException = typeof(ArgumentException))]
        [TestCase("source", null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("source", "", ExpectedException = typeof(ArgumentException))]
        public void AddRedirection_InvalidParameters(string src, string dest)
        {
            var prov = GetProvider();
            prov.AddRedirection(src, new PageRedirection { FullName = dest });
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void GetRedirectionDestination_InvalidSource(string src)
        {
            var prov = GetProvider();
            prov.GetRedirectionDestination(src);
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentException))]
        public void RemovePageFromRedirections_InvalidName(string name)
        {
            var prov = GetProvider();
            prov.RemovePageFromRedirections(name);
        }

        [Test]
        public void AddRedirection_GetDestination_RemovePageFromRedirections_Clear()
        {
            var prov = GetProvider();

            Assert.IsNull(prov.GetRedirectionDestination("Page"), "No redirection should be in cache");

            prov.AddRedirection("Page", new PageRedirection { FullName = "NS.OtherPage" });
            prov.AddRedirection("NS.OtherPage", new PageRedirection { FullName = "Page3" });
            prov.AddRedirection("ThirdPage", new PageRedirection { FullName = "Page" });

            Assert.AreEqual("NS.OtherPage", prov.GetRedirectionDestination("Page"), "Wrong destination");
            Assert.AreEqual("Page3", prov.GetRedirectionDestination("NS.OtherPage"), "Wrong destination");
            Assert.AreEqual("Page", prov.GetRedirectionDestination("ThirdPage"), "Wrong destination");

            prov.RemovePageFromRedirections("Page");

            Assert.IsNull(prov.GetRedirectionDestination("Page"), "No redirection should be in cache for Page");
            Assert.AreEqual("Page3", prov.GetRedirectionDestination("NS.OtherPage"), "Wrong destination");
            Assert.IsNull(prov.GetRedirectionDestination("Page"), "No redirection should be in cache for ThirdPage");

            prov.ClearRedirections();

            Assert.IsNull(prov.GetRedirectionDestination("Page"), "No redirection should be in cache");
            Assert.IsNull(prov.GetRedirectionDestination("NS.OtherPage"), "No redirection should be in cache");
            Assert.IsNull(prov.GetRedirectionDestination("Page"), "No redirection should be in cache");
        }

        [Test]
        public void CancelEditingSession_IsPageBeingEdited()
        {
            var prov = GetProvider();

            prov.RenewEditingSession("Page", "User");

            Assert.IsFalse(prov.IsPageBeingEdited("Page", "User"), "IsPageBeingEditing should return false");
            Assert.IsTrue(prov.IsPageBeingEdited("Page", "User2"), "IsPageBeingEditing should return true");

            prov.CancelEditingSession("Page", "User");

            Assert.IsFalse(prov.IsPageBeingEdited("Page", "User"), "IsPageBeingEditing should return false");
            Assert.IsFalse(prov.IsPageBeingEdited("Page", "User2"), "IsPageBeingEditing should return false");

            prov.RenewEditingSession("Page", "User1");
            prov.RenewEditingSession("Page", "User2");

            prov.CancelEditingSession("Page", "User1");

            Assert.IsTrue(prov.IsPageBeingEdited("Page", "User1"), "IsPageBeingEditing should return true");
            Assert.IsFalse(prov.IsPageBeingEdited("Page", "User2"), "IsPageBeingEditing should return false");

            prov.CancelEditingSession("Page", "User2");

            Assert.IsFalse(prov.IsPageBeingEdited("Page", "User2"), "IsPageBeingEditing should return false");
        }

        [Test]
        public void ClearPageContentCache()
        {
            var prov = GetProvider();

            var p1 = new PageInfo("Page1", MockPagesProvider(), DateTime.Now);
            var p2 = new PageInfo("Page2", MockPagesProvider(), DateTime.Now);
            var c1 = new PageContent(p1, "Page 1", "admin", DateTime.Now, "Comment", "Content", null, null);
            var c2 = new PageContent(p2, "Page 2", "user", DateTime.Now, "", "Blah", null, null);

            Assert.AreEqual(0, prov.PageCacheUsage, "Wrong cache usage");

            prov.SetPageContent(p1, c1);
            prov.SetPageContent(p2, c2);
            prov.SetFormattedPageContent(p1, "Content 1");
            prov.SetFormattedPageContent(p2, "Content 2");

            Assert.AreEqual(2, prov.PageCacheUsage, "Wrong cache usage");

            prov.ClearPageContentCache();

            Assert.AreEqual(0, prov.PageCacheUsage, "Wrong cache usage");

            Assert.IsNull(prov.GetPageContent(p1), "GetPageContent should return null");
            Assert.IsNull(prov.GetPageContent(p2), "GetPageContent should return null");
            Assert.IsNull(prov.GetFormattedPageContent(p1), "GetFormattedPageContent should return null");
            Assert.IsNull(prov.GetFormattedPageContent(p2), "GetFormattedPageContent should return null");
        }

        [Test]
        public void ClearPseudoCache()
        {
            var prov = GetProvider();

            prov.SetPseudoCacheValue("Test", "Value");
            prov.SetPseudoCacheValue("222", "VVV");

            prov.ClearPseudoCache();

            Assert.IsNull(prov.GetPseudoCacheValue("Test"), "GetPseudoCacheValue should return null");
            Assert.IsNull(prov.GetPseudoCacheValue("222"), "GetPseudoCacheValue should return null");
        }

        [Test]
        public void CutCache()
        {
            var prov = GetProvider();

            var p1 = new PageInfo("Page1", MockPagesProvider(), DateTime.Now);
            var p2 = new PageInfo("Page2", MockPagesProvider(), DateTime.Now);
            var p3 = new PageInfo("Page3", MockPagesProvider(), DateTime.Now);
            var c1 = new PageContent(p1, "Page 1", "admin", DateTime.Now, "Comment", "Content", null, null);
            var c2 = new PageContent(p2, "Page 2", "user", DateTime.Now, "", "Blah", null, null);
            var c3 = new PageContent(p3, "Page 3", "admin", DateTime.Now, "", "Content", null, null);

            Assert.AreEqual(0, prov.PageCacheUsage, "Wrong cache usage");

            prov.SetPageContent(p1, c1);
            prov.SetPageContent(p2, c2);
            prov.SetPageContent(p3, c3);
            prov.SetFormattedPageContent(p1, "Content 1");
            prov.SetFormattedPageContent(p3, "Content 2");

            prov.GetPageContent(p3);

            Assert.AreEqual(3, prov.PageCacheUsage, "Wrong cache usage");

            prov.CutCache(2);

            Assert.AreEqual(1, prov.PageCacheUsage, "Wrong cache usage");

            Assert.IsNotNull(prov.GetPageContent(p3), "GetPageContent should not return null");
            Assert.IsNull(prov.GetPageContent(p2), "GetPageContent should not null");
            Assert.IsNull(prov.GetPageContent(p1), "GetPageContent should not null");

            Assert.IsNotNull(prov.GetFormattedPageContent(p3), "GetFormattedPageContent should not return null");
            Assert.IsNull(prov.GetFormattedPageContent(p2), "GetFormattedPageContent should not null");
            Assert.IsNull(prov.GetFormattedPageContent(p1), "GetFormattedPageContent should not null");
        }

        [Test]
        public void GetFormattedPageContent_NullPage()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var prov = GetProvider();
                prov.GetFormattedPageContent(null);
            });
        }

        [Test]
        public void Init()
        {
            var prov = GetProvider();
            prov.Init(MockHost(), "");

            Assert.IsNotNull(prov.Information, "Information should not be null");
        }

        [Test]
        public void Init_NullConfig()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var prov = GetProvider();
                prov.Init(MockHost(), null);
            });
        }

        [Test]
        public void Init_NullHost()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var prov = GetProvider();

                prov.Init(null, "");
            });
        }

        [Test]
        public void RemovePageContent()
        {
            var prov = GetProvider();

            var p1 = new PageInfo("Page1", MockPagesProvider(), DateTime.Now);
            var p2 = new PageInfo("Page2", MockPagesProvider(), DateTime.Now);
            var c1 = new PageContent(p1, "Page 1", "admin", DateTime.Now, "Comment", "Content", null, null);
            var c2 = new PageContent(p2, "Page 2", "user", DateTime.Now, "", "Blah", null, null);

            Assert.AreEqual(0, prov.PageCacheUsage, "Wrong cache usage");

            prov.SetPageContent(p1, c1);
            prov.SetPageContent(p2, c2);
            prov.SetFormattedPageContent(p1, "Content 1");
            prov.SetFormattedPageContent(p2, "Content 2");

            Assert.AreEqual(2, prov.PageCacheUsage, "Wrong cache usage");

            prov.RemovePage(p2);

            Assert.IsNotNull(prov.GetFormattedPageContent(p1), "GetFormattedPageContent should not return null");
            Assert.IsNotNull(prov.GetPageContent(p1), "GetPageContent should not return null");

            Assert.IsNull(prov.GetFormattedPageContent(p2), "GetFormattedPageContent should return null");
            Assert.IsNull(prov.GetPageContent(p2), "GetPageContent should return null");
        }

        [Test]
        public void RemovePageContent_NullPage()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var prov = GetProvider();

                prov.RemovePage(null);
            });
        }

        [Test]
        public void RenewEditingSession_IsPageBeingEdited()
        {
            var prov = GetProvider();

            prov.RenewEditingSession("Page", "User");

            Assert.IsFalse(prov.IsPageBeingEdited("Page", "User"), "IsPageBeingEditing should return false");
            Assert.IsTrue(prov.IsPageBeingEdited("Page", "User2"), "IsPageBeingEditing should return true");
            Assert.IsFalse(prov.IsPageBeingEdited("Page2", "User"), "IsPageBeingEditing should return false");
            Assert.IsFalse(prov.IsPageBeingEdited("Page2", "User2"), "IsPageBeingEditing should return false");

            // Wait for timeout to expire
            Thread.Sleep(6500);
            Assert.IsFalse(prov.IsPageBeingEdited("Page", "User2"), "IsPageBeingEdited should return false");
        }

        [Test]
        public void SetFormattedPageContent_GetFormattedPageContent()
        {
            var prov = GetProvider();

            var p1 = new PageInfo("Page1", MockPagesProvider(), DateTime.Now);
            var p2 = new PageInfo("Page2", MockPagesProvider(), DateTime.Now);

            Assert.AreEqual(0, prov.PageCacheUsage, "Wrong cache usage");

            prov.SetFormattedPageContent(p1, "Content 1");
            prov.SetFormattedPageContent(p2, "Content 2");
            prov.SetFormattedPageContent(p1, "Content 1 mod");

            Assert.AreEqual(0, prov.PageCacheUsage, "Wrong cache usage");

            Assert.AreEqual("Content 1 mod", prov.GetFormattedPageContent(p1), "Wrong content");
            Assert.AreEqual("Content 2", prov.GetFormattedPageContent(p2), "Wrong content");

            Assert.IsNull(prov.GetFormattedPageContent(new PageInfo("Blah", MockPagesProvider(), DateTime.Now)),
                "GetFormattedPageContent should return null");
        }

        [Test]
        public void SetFormattedPageContent_NullContent()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var prov = GetProvider();

                var p1 = new PageInfo("Page1", MockPagesProvider(), DateTime.Now);

                prov.SetFormattedPageContent(p1, null);
            });
        }

        [Test]
        public void SetFormattedPageContent_NullPage()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var prov = GetProvider();
                prov.SetFormattedPageContent(null, "Content");
            });
        }

        [Test]
        public void SetOnlineUsers_GetOnlineUsers()
        {
            var prov = GetProvider();

            prov.OnlineUsers = 100;
            Assert.AreEqual(100, prov.OnlineUsers, "Wrong online users count");

            prov.OnlineUsers++;
            Assert.AreEqual(101, prov.OnlineUsers, "Wrong online users count");

            prov.OnlineUsers--;
            Assert.AreEqual(100, prov.OnlineUsers, "Wrong online users count");
        }

        [Test]
        public void SetPageContent_GetPageContent()
        {
            var prov = GetProvider();

            var p1 = new PageInfo("Page1", MockPagesProvider(), DateTime.Now);
            var p2 = new PageInfo("Page2", MockPagesProvider(), DateTime.Now);
            var c1 = new PageContent(p1, "Page 1", "admin", DateTime.Now, "Comment", "Content", new[] { "test", "page" },
                null);
            var c2 = new PageContent(p2, "Page 2", "user", DateTime.Now, "", "Blah", null, null);
            var c3 = new PageContent(p2, "Page 5", "john", DateTime.Now, "Comm.", "Blah 222", null, "Description");

            Assert.AreEqual(0, prov.PageCacheUsage, "Wrong cache usage");

            prov.SetPageContent(p1, c1);
            prov.SetPageContent(p2, c2);
            prov.SetPageContent(p2, c3);

            Assert.AreEqual(2, prov.PageCacheUsage, "Wrong cache usage");

            var res = prov.GetPageContent(p1);
            Assert.AreEqual(c1.PageInfo, res.PageInfo, "Wrong page info");
            Assert.AreEqual(c1.Title, res.Title, "Wrong title");
            Assert.AreEqual(c1.User, res.User, "Wrong user");
            Assert.AreEqual(c1.LastModified, res.LastModified, "Wrong date/time");
            Assert.AreEqual(c1.Comment, res.Comment, "Wrong comment");
            Assert.AreEqual(c1.Content, res.Content, "Wrong content");
            Assert.AreEqual(2, c1.Keywords.Count, "Wrong keyword count");
            Assert.AreEqual("test", c1.Keywords[0], "Wrong keyword");
            Assert.AreEqual("page", c1.Keywords[1], "Wrong keyword");
            Assert.IsNull(c1.Description, "Description should be null");

            res = prov.GetPageContent(p2);
            Assert.AreEqual(c3.PageInfo, res.PageInfo, "Wrong page info");
            Assert.AreEqual(c3.Title, res.Title, "Wrong title");
            Assert.AreEqual(c3.User, res.User, "Wrong user");
            Assert.AreEqual(c3.LastModified, res.LastModified, "Wrong date/time");
            Assert.AreEqual(c3.Comment, res.Comment, "Wrong comment");
            Assert.AreEqual(c3.Content, res.Content, "Wrong content");
            Assert.AreEqual(0, c3.Keywords.Count, "Keywords should be empty");
            Assert.AreEqual("Description", c3.Description, "Wrong description");

            Assert.IsNull(prov.GetPageContent(new PageInfo("Blah", MockPagesProvider(), DateTime.Now)),
                "GetPageContent should return null");
        }

        [Test]
        public void SetPageContent_NullContent()
        {
            var prov = GetProvider();

            var p1 = new PageInfo("Page1", MockPagesProvider(), DateTime.Now);
            var c1 = new PageContent(p1, "Page 1", "admin", DateTime.Now, "Comment", "Content", null, null);

            Assert.Throws<ArgumentNullException>(() =>
            {
                prov.SetPageContent(p1, null);
            });
        }

        [Test]
        public void SetPageContent_NullPage()
        {
            var prov = GetProvider();

            var p1 = new PageInfo("Page1", MockPagesProvider(), DateTime.Now);
            var c1 = new PageContent(p1, "Page 1", "admin", DateTime.Now, "Comment", "Content", null, null);

            Assert.Throws<ArgumentNullException>(() =>
            {
                prov.SetPageContent(null, c1);
            });
        }

        [Test]
        public void SetPseudoCacheValue_GetPseudoCacheValue()
        {
            var prov = GetProvider();

            prov.SetPseudoCacheValue("Name", "Value");
            prov.SetPseudoCacheValue("Test", "Blah");

            Assert.AreEqual("Value", prov.GetPseudoCacheValue("Name"), "Wrong pseudo-cache value");
            Assert.AreEqual("Blah", prov.GetPseudoCacheValue("Test"), "Wrong pseudo-cache value");
            Assert.IsNull(prov.GetPseudoCacheValue("Inexistent"), "Pseudo-cache value should be null");

            prov.SetPseudoCacheValue("Name", null);
            prov.SetPseudoCacheValue("Test", "");

            Assert.IsNull(prov.GetPseudoCacheValue("Name"), "Pseudo-cache value should be null");
            Assert.AreEqual("", prov.GetPseudoCacheValue("Test"), "Wrong pseudo-cache value");
        }

        [Test]
        public void WhosEditing()
        {
            var prov = GetProvider();

            prov.RenewEditingSession("Page", "User1");
            prov.RenewEditingSession("Page", "User2");

            Assert.AreEqual("", prov.WhosEditing("Inexistent"), "Wrong result (should be empty)");

            Assert.AreEqual("User1", prov.WhosEditing("Page"), "Wrong user");

            prov.CancelEditingSession("Page", "User1");

            Assert.AreEqual("User2", prov.WhosEditing("Page"), "Wrong user");

            prov.CancelEditingSession("Page", "User2");

            Assert.AreEqual("", prov.WhosEditing("Page"), "Wrong user");
        }
    }
}