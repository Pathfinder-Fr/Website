using System;
using System.Collections.Generic;
using System.Web;
using NUnit.Framework;
using Rhino.Mocks;
using ScrewTurn.Wiki.PluginFramework;

namespace ScrewTurn.Wiki.Tests
{
    [TestFixture]
    public class FormatterTests
    {
        [TearDown]
        public void TearDown()
        {
            mocks.VerifyAll();
        }

        private const string Input =
            @"'''bold''' ''italic'' __underlined__ --striked-- [page1]\r\n[page2|title]
* item 1
* item 2

second line";

        private const string ExpectedOutput =
            @"<b>bold</b> <i>italic</i> <u>underlined</u> <strike>striked</strike> <a class=""pagelink"" href=""page1.ashx"" title=""Page 1"">page1</a>\r\n" +
            "<a class=\"unknownlink\" href=\"page2.ashx\" title=\"page2\">title</a>\n<ul><li>item 1</li><li>item 2<br /></li></ul><br />second line\n";

        private MockRepository mocks;

        public void SetUp(bool ignoreSingleSquareBrackets = false)
        {
            mocks = new MockRepository();

            var settingsProvider = mocks.StrictMock<ISettingsStorageProviderV30>();
            Expect.Call(settingsProvider.GetSetting("ProcessSingleLineBreaks")).Return("false").Repeat.Any();
            Expect.Call(settingsProvider.GetSetting("WikiTitle")).Return("Title").Repeat.Any();
            Expect.Call(settingsProvider.GetSetting("IgnoreSingleSquareBrackets"))
                .Return(ignoreSingleSquareBrackets ? "true" : "false")
                .Repeat.Any();

            Collectors.SettingsProvider = settingsProvider;

            var pagesProvider = mocks.StrictMock<IPagesStorageProviderV30>();
            Collectors.PagesProviderCollector = new ProviderCollector<IPagesStorageProviderV30>();
            Collectors.PagesProviderCollector.AddProvider(pagesProvider);

            Expect.Call(settingsProvider.GetSetting("DefaultPagesProvider"))
                .Return(pagesProvider.GetType().FullName)
                .Repeat.Any();

            var page1 = new PageInfo("page1", pagesProvider, DateTime.Now);
            var page1Content = new PageContent(page1, "Page 1", "User", DateTime.Now, "Comment", "Content", null, null);
            Expect.Call(pagesProvider.GetPage("page1")).Return(page1).Repeat.Any();
            Expect.Call(pagesProvider.GetContent(page1)).Return(page1Content).Repeat.Any();

            Expect.Call(pagesProvider.GetPage("page2")).Return(null).Repeat.Any();

            //Pages.Instance = new Pages();

            Host.Instance = new Host();

            Expect.Call(settingsProvider.GetSetting("CacheSize")).Return("100").Repeat.Any();
            Expect.Call(settingsProvider.GetSetting("CacheCutSize")).Return("20").Repeat.Any();

            Expect.Call(settingsProvider.GetSetting("DefaultCacheProvider"))
                .Return(typeof (CacheProvider).FullName)
                .Repeat.Any();

            // Cache needs setting to init
            mocks.Replay(settingsProvider);

            ICacheProviderV30 cacheProvider = new CacheProvider();
            cacheProvider.Init(Host.Instance, "");
            Collectors.CacheProviderCollector = new ProviderCollector<ICacheProviderV30>();
            Collectors.CacheProviderCollector.AddProvider(cacheProvider);

            mocks.Replay(pagesProvider);

            Collectors.FormatterProviderCollector = new ProviderCollector<IFormatterProviderV30>();

            //System.Web.UI.HtmlTextWriter writer = new System.Web.UI.HtmlTextWriter(new System.IO.StreamWriter(new System.IO.MemoryStream()));
            //System.Web.Hosting.SimpleWorkerRequest request = new System.Web.Hosting.SimpleWorkerRequest("Default.aspx", "?Page=MainPage", writer);
            HttpContext.Current = new HttpContext(new DummyRequest());
        }

        [Test]
        [TestCase("{wikititle}", "Title\n")]
        [TestCase("@@rigatesto1\r\nriga2@@", "<pre>rigatesto1\r\nriga2</pre>\n")]
        [TestCase(Input, ExpectedOutput)]
        public void Format(string input, string output)
        {
            SetUp();

            var context = FormattingContext.PageContent;
            PageInfo currentPage = null;
            IList<string> linkedPages = null;

            var _input = Formatter.Format(input, false, context, currentPage, out linkedPages, false);

            // Ignore \r characters
            // Ignore \n characters

            Assert.AreEqual(output, _input, "Formatter output is different from expected output");
        }

        [Test]
        public void Single_Square_Regex_Are_Ignored_When_Setting_Is_Set()
        {
            SetUp(true);

            var context = FormattingContext.PageContent;
            PageInfo currentPage = null;
            IList<string> linkedPages = null;

            var formatted = Formatter.Format("[page1]", false, context, currentPage, out linkedPages, false);

            Assert.AreEqual("[page1]\n", formatted, "Single Square Brackets should be ignored when setting is set");
        }

        [Test]
        public void Single_Square_Regex_Are_Not_Ignored_By_Default()
        {
            SetUp(false);

            var context = FormattingContext.PageContent;
            PageInfo currentPage = null;
            IList<string> linkedPages = null;

            var formatted = Formatter.Format("[page1]", false, context, currentPage, out linkedPages, false);

            Assert.AreEqual("<a class=\"pagelink\" href=\"page1.ashx\" title=\"Page 1\">page1</a>\n", formatted,
                "Single Square Brackets should not be ignored by default");
        }
    }

    public class DummyRequest : HttpWorkerRequest
    {
        public override void EndOfRequest()
        {
        }

        public override void FlushResponse(bool finalFlush)
        {
        }

        public override string GetHttpVerbName()
        {
            return "GET";
        }

        public override string GetHttpVersion()
        {
            return "1.1";
        }

        public override string GetLocalAddress()
        {
            return "http://localhost/";
        }

        public override int GetLocalPort()
        {
            return 80;
        }

        public override string GetQueryString()
        {
            return "";
        }

        public override string GetRawUrl()
        {
            return "http://localhost/Default.aspx";
        }

        public override string GetRemoteAddress()
        {
            return "127.0.0.1";
        }

        public override int GetRemotePort()
        {
            return 45695;
        }

        public override string GetUriPath()
        {
            return "/";
        }

        public override void SendKnownResponseHeader(int index, string value)
        {
        }

        public override void SendResponseFromFile(IntPtr handle, long offset, long length)
        {
        }

        public override void SendResponseFromFile(string filename, long offset, long length)
        {
        }

        public override void SendResponseFromMemory(byte[] data, int length)
        {
        }

        public override void SendStatus(int statusCode, string statusDescription)
        {
        }

        public override void SendUnknownResponseHeader(string name, string value)
        {
        }
    }
}