using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sueetie.Core;
using Lucene.Net.Store;
using Lucene.Net.Analysis;
using Lucene.Net.Index;
using Lucene.Net.Analysis.Standard;
using System.Web;
using Lucene.Net.Documents;
using Lucene.Net.Util;
using Lucene.Net.Search;
using System.IO;
using Lucene.Net.QueryParsers;
using Lucene.Net.Highlight;

namespace Sueetie.Search
{
    public class SueetieSearch : IDisposable
    {

        #region Properties

        private readonly Lucene.Net.Store.Directory _directory;
        private readonly Analyzer _analyzer;
        private static IndexWriter _writer;
        private static SearchConfiguration _config;
        private int _bodyDisplayLength;

        private bool _disposed;

        #endregion

        #region String Constants

        private const string ContentID = "ContentID";
        private const string Title = "Title";
        private const string Body = "Body";
        private const string Tags = "Tags";
        private const string DisplayTags = "DisplayTags";
        private const string Categories = "Categories";
        private const string Pubdate = "PubDate";
        private const string AppID = "AppID";
        private const string ApplicationTypeID = "ApplicationTypeID";
        private const string GroupID = "GroupID";
        private const string ContainerName = "ContainerName";
        private const string IsRestricted = "IsRestricted";
        private const string ContentTypeID = "ContentTypeID";
        private const string Author = "Author";
        private const string ApplicationKey = "ApplicationKey";
        private const string GroupKey = "GroupKey";
        private const string PermaLink = "PermaLink";
        private const string Username = "Username";
        private const string App = "App";

        #endregion

        #region Constructor

        public SueetieSearch()
        {
            _config = SearchConfiguration.Get();
            _directory = FSDirectory.Open(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + _config.SearchSettings.IndexPath));
            _analyzer = new StandardAnalyzer(_config.SearchSettings.StopWords);
            _bodyDisplayLength = _config.SearchSettings.BodyDisplayLength;
        }

        #endregion

        #region Background Index Task Functions

        public static SueetieIndexTaskItem GetSueetieIndexTaskItem(int taskID)
        {
            SueetieSearchDataProvider _provider = SueetieSearchDataProvider.LoadProvider();
            return _provider.GetSueetieIndexTaskItem(taskID);
        }
        #endregion

        #region Indexing

        #region Build Index

        public void AsyncUpdateIndex(DateTime indexStartDateTime)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(delegate(object state)
             {
                 UpdateIndex(SueetieTaskType.NonTaskProcess, indexStartDateTime);
             });
        }
        public void UpdateIndex(SueetieTaskType _taskType, DateTime indexStartDateTime)
        {
            var sueetieIndexTaskItem = new SueetieIndexTaskItem
            {
                TaskTypeID = (int)_taskType,
                TaskStartDateTime = indexStartDateTime
            };
            UpdateIndex(sueetieIndexTaskItem);
        }

        public void UpdateIndex(SueetieIndexTaskItem _taskItem)
        {
            DateTime startTime = DateTime.Now;
            try
            {
                _taskItem.BlogPosts = AddSueetieSearchDocs(ProcessBlogPosts(_taskItem.TaskStartDateTime));
                _taskItem.WikiPages = AddSueetieSearchDocs(ProcessSueetieWikiPages(_taskItem.TaskStartDateTime));
                _taskItem.ForumMessages = AddSueetieSearchDocs(ProcessSueetieForumMessages(_taskItem.TaskStartDateTime));
                _taskItem.MediaAlbums = AddSueetieSearchDocs(ProcessSueetieMediaAlbums(_taskItem.TaskStartDateTime));
                _taskItem.MediaObjects = AddSueetieSearchDocs(ProcessSueetieMediaObjects(_taskItem.TaskStartDateTime));
                _taskItem.ContentPages = AddSueetieSearchDocs(ProcessSueetieContentPages(_taskItem.TaskStartDateTime));

                _taskItem.DocumentsIndexed = GetTotalIndexedCount();
                CommitAndCloseWriter();

                if (_taskItem.TaskTypeID == (int)SueetieTaskType.BuildSearchIndex)
                {
                    SueetieSearchDataProvider _provider = SueetieSearchDataProvider.LoadProvider();
                    _provider.UpdateAndEndTask(_taskItem);
                }
                DateTime endTime = DateTime.Now;
                SueetieLogs.LogSiteEntry(SiteLogType.General, SiteLogCategoryType.TasksMessage, "INDEX SUCCESSFUL: " +
                    string.Format("Blog Posts: {0}, Wiki Pages: {1}, Forum Messages: {2}, Media Albums: {3}, Media Objects: {4}, Content Pages: {5}, Indexed Documents: {6}", 
                    _taskItem.BlogPosts, _taskItem.WikiPages, _taskItem.ForumMessages, _taskItem.MediaAlbums, _taskItem.MediaObjects, _taskItem.ContentPages, _taskItem.DocumentsIndexed) +
                    " - REBUILD INDEX DURATION: " + (endTime - startTime).TotalMilliseconds + " Milliseconds.");
            }
            catch (Exception e)
            {
                DateTime endTime = DateTime.Now;
                SueetieLogs.LogSearchException("SEARCH REBUILD INDEX ERROR: " + e.Message + "\n\nSTACKTRACE: " + e.StackTrace + " - REBUILD INDEX DURATION: " + (endTime - startTime).TotalMilliseconds + " Milliseconds.");
            }
        }

        #endregion

        #region Retrieve Sueetie Content

        #region Blog Posts

        private List<SueetieSearchDoc> ProcessBlogPosts(DateTime pubDate)
        {
            List<SueetieSearchDoc> sueetieSearchDocs = new List<SueetieSearchDoc>();
            List<SueetieBlogPost> sueetieBlogPosts = GetSueetieBlogPostList((int)SueetieContentType.BlogPost, pubDate);
            foreach (SueetieBlogPost sueetieBlogPost in sueetieBlogPosts)
            {
                sueetieSearchDocs.Add(SueetieSearch.ConvertBlogPostToSearchDoc(sueetieBlogPost));
            }
            return sueetieSearchDocs;
        }
        private List<SueetieBlogPost> GetSueetieBlogPostList(int contenttypeID, DateTime pubDate)
        {
            SueetieSearchDataProvider _provider = SueetieSearchDataProvider.LoadProvider();
            return _provider.GetSueetieBlogPostsToIndex(contenttypeID, pubDate);
        }
        private static SueetieSearchDoc ConvertBlogPostToSearchDoc(SueetieBlogPost post)
        {
            string _tags = !string.IsNullOrEmpty(post.Tags) ? post.Tags.Replace('|', ' ') : string.Empty;
            string _displayTags = !string.IsNullOrEmpty(post.Tags) ? SueetieTags.TagUrls(post.Tags) : string.Empty;
            string _categories = !string.IsNullOrEmpty(post.Categories) ? post.Categories.Replace('|', ' ') : string.Empty;
            var doc = new SueetieSearchDoc()
            {
                App = "Blogs",
                AppID = post.ApplicationID,
                ContainerName = post.BlogTitle,
                GroupID = post.GroupID,
                GroupKey = string.IsNullOrEmpty(post.GroupKey) ? "na" : post.GroupKey,
                IsRestricted = post.IsRestricted,
                ContentID = post.ContentID,
                PublishDate = post.DateCreated,
                Tags = _tags,
                DisplayTags = _displayTags,
                Categories = _categories,
                Title = post.Title,
                ContentTypeID = post.ContentTypeID,
                ApplicationTypeID = post.ApplicationTypeID,
                Author = post.DisplayName,
                PermaLink = post.Permalink,
                ApplicationKey = post.ApplicationKey,
                Username = post.Author,
                Body = DataHelper.CleanSearchBodyContent(post.PostContent) + RaquoIt(post.BlogTitle) + RaquoIt(post.Author) + RaquoIt(post.Title) + RaquoIt(_tags) + RaquoIt(_categories)
            };
            return doc;
        }

        #endregion

        #region Wiki Pages

        private List<SueetieSearchDoc> ProcessSueetieWikiPages(DateTime publishedDate)
        {
            List<SueetieSearchDoc> sueetieSearchDocs = new List<SueetieSearchDoc>();
            List<SueetieWikiPage> sueetieWikiPages = GetSueetieWikiPageList((int)SueetieContentType.WikiPage, publishedDate);
            foreach (SueetieWikiPage sueetieWikiPage in sueetieWikiPages)
            {
                sueetieSearchDocs.Add(SueetieSearch.ConvertSueetieWikiPageToSearchDoc(sueetieWikiPage));
            }
            return sueetieSearchDocs;
        }
        private List<SueetieWikiPage> GetSueetieWikiPageList(int contenttypeID, DateTime pubDate)
        {
            SueetieSearchDataProvider _provider = SueetieSearchDataProvider.LoadProvider();
            return _provider.GetSueetieWikiPagesToIndex(contenttypeID, pubDate);
        }
        private static SueetieSearchDoc ConvertSueetieWikiPageToSearchDoc(SueetieWikiPage sueetieWikiPage)
        {
            string _body = sueetieWikiPage.PageContent;
            string _tags = !string.IsNullOrEmpty(sueetieWikiPage.Tags) ? sueetieWikiPage.Tags.Replace('|', ' ') : string.Empty;
            string _displayTags = !string.IsNullOrEmpty(sueetieWikiPage.Tags) ? SueetieTags.TagUrls(sueetieWikiPage.Tags) : string.Empty;
            string _keywords = !string.IsNullOrEmpty(sueetieWikiPage.Keywords) ? sueetieWikiPage.Keywords.Replace(',', ' ') : string.Empty;
            string _categories = !string.IsNullOrEmpty(sueetieWikiPage.Categories) ? sueetieWikiPage.Categories.Replace('|', ' ') : string.Empty;
            SueetieSearchDoc doc = new SueetieSearchDoc()
            {
                App = "Wikis",
                ContentID = sueetieWikiPage.ContentID,
                Title = sueetieWikiPage.PageTitle,
                Tags = _tags,
                DisplayTags = _displayTags,
                AppID = sueetieWikiPage.ApplicationID,
                IsRestricted = sueetieWikiPage.IsRestricted,
                PublishDate = sueetieWikiPage.DateTimeModified,
                ContainerName = sueetieWikiPage.ApplicationName,
                GroupID = sueetieWikiPage.GroupID,
                GroupKey = string.IsNullOrEmpty(sueetieWikiPage.GroupKey) ? "na" : sueetieWikiPage.GroupKey,
                ContentTypeID = sueetieWikiPage.ContentTypeID,
                Categories = _categories,
                ApplicationTypeID = sueetieWikiPage.ApplicationTypeID,
                ApplicationKey = sueetieWikiPage.ApplicationKey,
                PermaLink = sueetieWikiPage.Permalink,
                Author = sueetieWikiPage.DisplayName,
                Username = sueetieWikiPage.UserName,
                Body = _body + RaquoIt(sueetieWikiPage.PageTitle) + RaquoIt(_categories) + RaquoIt(_tags) + RaquoIt(_keywords) + RaquoIt(sueetieWikiPage.UserName)
            };
            return doc;
        }

        #endregion

        #region Content Pages

        private List<SueetieSearchDoc> ProcessSueetieContentPages(DateTime publishedDate)
        {
            List<SueetieSearchDoc> sueetieSearchDocs = new List<SueetieSearchDoc>();
            List<SueetieContentPage> sueetieContentPages = GetSueetieContentPageList((int)SueetieContentType.CMSPage, publishedDate);
            foreach (SueetieContentPage sueetieContentPage in sueetieContentPages)
            {
                sueetieSearchDocs.Add(SueetieSearch.ConvertSueetieContentPageToSearchDoc(sueetieContentPage));
            }
            return sueetieSearchDocs;
        }
        private List<SueetieContentPage> GetSueetieContentPageList(int contenttypeID, DateTime pubDate)
        {
            SueetieSearchDataProvider _provider = SueetieSearchDataProvider.LoadProvider();
            return _provider.GetSueetieContentPagesToIndex(contenttypeID, pubDate);
        }
        private static SueetieSearchDoc ConvertSueetieContentPageToSearchDoc(SueetieContentPage sueetieContentPage)
        {
            string _body = DataHelper.StripHtml(sueetieContentPage.SearchBody).TrimStart();
            string _tags = !string.IsNullOrEmpty(sueetieContentPage.Tags) ? sueetieContentPage.Tags.Replace('|', ' ') : string.Empty;
            string _displayTags = !string.IsNullOrEmpty(sueetieContentPage.Tags) ? SueetieTags.TagUrls(sueetieContentPage.Tags) : string.Empty;

            SueetieSearchDoc doc = new SueetieSearchDoc()
            {
                App = "CMS",
                ContentID = sueetieContentPage.ContentID,
                Title = sueetieContentPage.PageTitle,
                Tags = _tags,
                DisplayTags = _displayTags,
                AppID = sueetieContentPage.ApplicationID,
                IsRestricted = sueetieContentPage.IsRestricted,
                PublishDate = sueetieContentPage.LastUpdateDateTime,
                ContainerName = sueetieContentPage.ApplicationName,
                GroupID = 0,
                GroupKey = "na",
                ContentTypeID = sueetieContentPage.ContentTypeID,
                Categories = string.Empty,
                ApplicationTypeID = sueetieContentPage.ApplicationTypeID,
                ApplicationKey = sueetieContentPage.ApplicationKey,
                PermaLink = sueetieContentPage.Permalink,
                Author = sueetieContentPage.DisplayName,
                Username = sueetieContentPage.UserName,
                Body = _body + RaquoIt(sueetieContentPage.PageTitle) + RaquoIt(_tags) + RaquoIt(sueetieContentPage.UserName)
            };
            return doc;
        }


        #endregion

        #region Forum Messages

        private List<SueetieSearchDoc> ProcessSueetieForumMessages(DateTime publishedDate)
        {
            List<SueetieSearchDoc> sueetieSearchDocs = new List<SueetieSearchDoc>();
            List<SueetieForumMessage> sueetieForumMessages = GetSueetieForumMessageList((int)SueetieContentType.ForumMessage, publishedDate);
            foreach (SueetieForumMessage sueetieForumMessage in sueetieForumMessages)
            {
                sueetieSearchDocs.Add(SueetieSearch.ConvertSueetieForumMessageToSearchDoc(sueetieForumMessage));
            }
            return sueetieSearchDocs;
        }
        private List<SueetieForumMessage> GetSueetieForumMessageList(int contenttypeID, DateTime pubDate)
        {
            SueetieSearchDataProvider _provider = SueetieSearchDataProvider.LoadProvider();
            return _provider.GetSueetieForumMessagesToIndex(contenttypeID, pubDate);
        }
        private static SueetieSearchDoc ConvertSueetieForumMessageToSearchDoc(SueetieForumMessage sueetieForumMessage)
        {
            string _body = DataHelper.CleanSearchBodyContent(sueetieForumMessage.Message);
            string _tags = !string.IsNullOrEmpty(sueetieForumMessage.Tags) ? sueetieForumMessage.Tags.Replace('|', ' ') : string.Empty;
            string _displayTags = !string.IsNullOrEmpty(sueetieForumMessage.Tags) ? SueetieTags.TagUrls(sueetieForumMessage.Tags) : string.Empty;

            SueetieSearchDoc doc = new SueetieSearchDoc()
            {
                App = "Forums",
                ContentID = sueetieForumMessage.ContentID,
                Title = sueetieForumMessage.Topic,
                Tags = _tags,
                DisplayTags = _displayTags,
                AppID = sueetieForumMessage.ApplicationID,
                IsRestricted = sueetieForumMessage.IsRestricted,
                PublishDate = sueetieForumMessage.DateTimeCreated,
                ContainerName = sueetieForumMessage.Forum,
                GroupID = sueetieForumMessage.GroupID,
                GroupKey = string.IsNullOrEmpty(sueetieForumMessage.GroupKey) ? "na" : sueetieForumMessage.GroupKey,
                ContentTypeID = sueetieForumMessage.ContentTypeID,
                Categories = string.Empty,
                ApplicationTypeID = sueetieForumMessage.ApplicationTypeID,
                ApplicationKey = sueetieForumMessage.ApplicationKey,
                PermaLink = sueetieForumMessage.Permalink,
                Author = sueetieForumMessage.DisplayName,
                Username = sueetieForumMessage.UserName,
                Body = _body + RaquoIt(sueetieForumMessage.Topic) + RaquoIt(_tags) + RaquoIt(sueetieForumMessage.UserName)
            };
            return doc;
        }

        #endregion

        #region Media Albums

        private List<SueetieSearchDoc> ProcessSueetieMediaAlbums(DateTime publishedDate)
        {
            List<SueetieSearchDoc> sueetieSearchDocs = new List<SueetieSearchDoc>();
            List<SueetieMediaAlbum> sueetieMediaAlbums = GetSueetieMediaAlbumList(publishedDate);
            foreach (SueetieMediaAlbum sueetieMediaAlbum in sueetieMediaAlbums)
            {
                sueetieSearchDocs.Add(SueetieSearch.ConvertSueetieMediaAlbumToSearchDoc(sueetieMediaAlbum));
            }
            return sueetieSearchDocs;
        }
        private List<SueetieMediaAlbum> GetSueetieMediaAlbumList(DateTime pubDate)
        {
            SueetieSearchDataProvider _provider = SueetieSearchDataProvider.LoadProvider();
            return _provider.GetSueetieMediaAlbumsToIndex(pubDate);
        }
        private static SueetieSearchDoc ConvertSueetieMediaAlbumToSearchDoc(SueetieMediaAlbum sueetieMediaAlbum)
        {
            string _body = sueetieMediaAlbum.AlbumTitle;
            string _tags = !string.IsNullOrEmpty(sueetieMediaAlbum.Tags) ? sueetieMediaAlbum.Tags.Replace('|', ' ') : string.Empty;
            string _displayTags = !string.IsNullOrEmpty(sueetieMediaAlbum.Tags) ? SueetieTags.TagUrls(sueetieMediaAlbum.Tags) : string.Empty;

            SueetieSearchDoc doc = new SueetieSearchDoc()
            {
                App = "Albums",
                ContentID = sueetieMediaAlbum.ContentID,
                Title = sueetieMediaAlbum.AlbumTitle,
                Tags = _tags,
                DisplayTags = _displayTags,
                AppID = sueetieMediaAlbum.ApplicationID,
                IsRestricted = sueetieMediaAlbum.IsRestricted,
                PublishDate = sueetieMediaAlbum.DateTimeCreated,
                ContainerName = sueetieMediaAlbum.ApplicationDescription,
                GroupID = sueetieMediaAlbum.GroupID,
                GroupKey = string.IsNullOrEmpty(sueetieMediaAlbum.GroupKey) ? "na" : sueetieMediaAlbum.GroupKey,
                ContentTypeID = sueetieMediaAlbum.ContentTypeID,
                Categories = string.Empty,
                ApplicationTypeID = sueetieMediaAlbum.ApplicationTypeID,
                ApplicationKey = sueetieMediaAlbum.ApplicationKey,
                PermaLink = sueetieMediaAlbum.Permalink,
                Author = sueetieMediaAlbum.DisplayName,
                Username = sueetieMediaAlbum.UserName,
                Body = _body + RaquoIt(_tags) + RaquoIt(sueetieMediaAlbum.UserName)
            };
            return doc;
        }

        #endregion

        #region Media Objects

        private List<SueetieSearchDoc> ProcessSueetieMediaObjects(DateTime publishedDate)
        {
            List<SueetieSearchDoc> sueetieSearchDocs = new List<SueetieSearchDoc>();
            List<SueetieMediaObject> sueetieMediaObjects = GetSueetieMediaObjectList(publishedDate);
            foreach (SueetieMediaObject sueetieMediaObject in sueetieMediaObjects)
            {
                sueetieSearchDocs.Add(SueetieSearch.ConvertSueetieMediaObjectToSearchDoc(sueetieMediaObject));
            }
            return sueetieSearchDocs;
        }
        private List<SueetieMediaObject> GetSueetieMediaObjectList(DateTime pubDate)
        {
            SueetieSearchDataProvider _provider = SueetieSearchDataProvider.LoadProvider();
            return _provider.GetSueetieMediaObjectsToIndex(pubDate);
        }
        private static SueetieSearchDoc ConvertSueetieMediaObjectToSearchDoc(SueetieMediaObject sueetieMediaObject)
        {
            string _body = string.IsNullOrEmpty(sueetieMediaObject.MediaObjectDescription) ? sueetieMediaObject.MediaObjectTitle : sueetieMediaObject.MediaObjectDescription;
            string _tags = !string.IsNullOrEmpty(sueetieMediaObject.Tags) ? sueetieMediaObject.Tags.Replace('|', ' ') : string.Empty;
            string _displayTags = !string.IsNullOrEmpty(sueetieMediaObject.Tags) ? SueetieTags.TagUrls(sueetieMediaObject.Tags) : string.Empty;

            SueetieSearchDoc doc = new SueetieSearchDoc()
            {
                App = "Media",
                ContentID = sueetieMediaObject.ContentID,
                Title = sueetieMediaObject.MediaObjectTitle,
                Tags = _tags,
                DisplayTags = _displayTags,
                AppID = sueetieMediaObject.ApplicationID,
                IsRestricted = sueetieMediaObject.IsRestricted,
                PublishDate = sueetieMediaObject.DateTimeCreated,
                ContainerName = sueetieMediaObject.AlbumTitle,
                GroupID = sueetieMediaObject.GroupID,
                GroupKey = string.IsNullOrEmpty(sueetieMediaObject.GroupKey) ? "na" : sueetieMediaObject.GroupKey,
                ContentTypeID = sueetieMediaObject.ContentTypeID,
                Categories = string.Empty,
                ApplicationTypeID = sueetieMediaObject.ApplicationTypeID,
                ApplicationKey = sueetieMediaObject.ApplicationKey,
                PermaLink = "/" + sueetieMediaObject.ApplicationKey + "/default.aspx?moid=" + sueetieMediaObject.MediaObjectID,
                Author = sueetieMediaObject.DisplayName,
                Username = sueetieMediaObject.UserName,
                Body = _body + RaquoIt(sueetieMediaObject.MediaObjectTitle) + RaquoIt(_tags) + RaquoIt(sueetieMediaObject.UserName)
            };
            return doc;
        }

        #endregion

        #endregion

        #region Add/Create Documents

        private int AddSueetieSearchDocs(List<SueetieSearchDoc> sueetieSearchDocs)
        {
            int _docsIndexed = 0;
            foreach (SueetieSearchDoc sueetieSearchDoc in sueetieSearchDocs)
            {
                ExecuteRemoveDoc(sueetieSearchDoc.ContentID);
                try
                {
                    SueetieSearchDoc currentSueetieSearchDoc = sueetieSearchDoc;
                    DoWriterAction(writer => writer.AddDocument(CreateLuceneDocument(currentSueetieSearchDoc)));
                    _docsIndexed++;
                }
                catch (Exception ex)
                {
                    SueetieLogs.LogSearchException(sueetieSearchDoc.ContentID, sueetieSearchDoc.Title, ex.Message + "\n\nSTACKTRACE: " + ex.StackTrace);
                }
            }
            return _docsIndexed;
        }

        protected virtual Document CreateLuceneDocument(SueetieSearchDoc sueetieSearchDoc)
        {
            Document doc = new Document();

            Field contentID = new Field(ContentID,
                NumericUtils.IntToPrefixCoded(sueetieSearchDoc.ContentID),
                Field.Store.YES,
                Field.Index.NOT_ANALYZED,
                Field.TermVector.NO);

            Field title = new Field(Title,
                sueetieSearchDoc.Title,
                Field.Store.YES,
                Field.Index.ANALYZED,
                Field.TermVector.YES);
            title.SetBoost(_config.SearchSettings.TitleBoost);

            Field body = new Field(Body,
                sueetieSearchDoc.Body,
                Field.Store.YES,
                Field.Index.ANALYZED,
                Field.TermVector.YES);
            body.SetBoost(_config.SearchSettings.BodyBoost);

            Field tags = new Field(Tags,
                sueetieSearchDoc.Tags,
                Field.Store.NO,
                Field.Index.ANALYZED,
                Field.TermVector.YES);
            tags.SetBoost(_config.SearchSettings.TagsBoost);

            Field displaytags = new Field(DisplayTags,
             sueetieSearchDoc.DisplayTags,
             Field.Store.YES,
             Field.Index.NOT_ANALYZED,
             Field.TermVector.NO);

            Field categories = new Field(Categories,
                  sueetieSearchDoc.Categories,
                  Field.Store.NO,
                  Field.Index.ANALYZED,
                  Field.TermVector.YES);
            categories.SetBoost(_config.SearchSettings.CategoryBoost);

            Field appID = new Field(AppID,
                NumericUtils.IntToPrefixCoded(sueetieSearchDoc.AppID),
                Field.Store.YES,
                Field.Index.NOT_ANALYZED,
                Field.TermVector.NO);

            Field isRestricted = new Field(IsRestricted,
                sueetieSearchDoc.IsRestricted.ToString(),
                Field.Store.NO,
                Field.Index.NOT_ANALYZED,
                Field.TermVector.NO);

            Field pubDate = new Field(Pubdate,
                DateTools.DateToString(sueetieSearchDoc.PublishDate, DateTools.Resolution.MINUTE),
                Field.Store.YES,
                Field.Index.NOT_ANALYZED,
                Field.TermVector.NO);

            Field groupID = new Field(GroupID,
                NumericUtils.IntToPrefixCoded(sueetieSearchDoc.GroupID),
                Field.Store.NO,
                Field.Index.NOT_ANALYZED,
                Field.TermVector.NO);

            Field containerName = new Field(ContainerName,
                sueetieSearchDoc.ContainerName,
                Field.Store.YES,
                Field.Index.NO,
                Field.TermVector.NO);

            Field contentTypeID = new Field(ContentTypeID,
                NumericUtils.IntToPrefixCoded(sueetieSearchDoc.ContentTypeID),
                Field.Store.YES,
                Field.Index.NOT_ANALYZED,
                Field.TermVector.NO);

            Field appTypeID = new Field(ApplicationTypeID,
               NumericUtils.IntToPrefixCoded(sueetieSearchDoc.ApplicationTypeID),
                Field.Store.YES,
                Field.Index.NOT_ANALYZED,
                Field.TermVector.NO);

            //(sueetieSearchDoc.ApplicationTypeID + 10000).ToString(),
            //Field.Store.YES,
            //Field.Index.NOT_ANALYZED,
            //Field.TermVector.NO);

            Field applicationKey = new Field(ApplicationKey,
                sueetieSearchDoc.ApplicationKey,
                Field.Store.YES,
                Field.Index.NO,
                Field.TermVector.NO);

            Field permalink = new Field(PermaLink,
                sueetieSearchDoc.PermaLink,
                Field.Store.YES,
                Field.Index.NO,
                Field.TermVector.NO);

            Field author = new Field(Author,
                sueetieSearchDoc.Author,
                Field.Store.YES,
                Field.Index.NO,
                Field.TermVector.NO);

            Field username = new Field(Username,
                sueetieSearchDoc.Username,
                Field.Store.NO,
                Field.Index.ANALYZED,
                Field.TermVector.NO);
            username.SetBoost(_config.SearchSettings.UsernameBoost);

            Field app = new Field(App,
                sueetieSearchDoc.App,
                Field.Store.NO,
                Field.Index.ANALYZED,
                Field.TermVector.NO);

            Field groupKey = new Field(GroupKey,
                sueetieSearchDoc.GroupKey,
                Field.Store.NO,
                Field.Index.NOT_ANALYZED,
                Field.TermVector.NO);

            doc.Add(contentID);
            doc.Add(title);
            doc.Add(body);
            doc.Add(tags);
            doc.Add(displaytags);
            doc.Add(categories);
            doc.Add(appID);
            doc.Add(isRestricted);
            doc.Add(pubDate);
            doc.Add(groupID);
            doc.Add(containerName);
            doc.Add(contentTypeID);
            doc.Add(applicationKey);
            doc.Add(groupKey);
            doc.Add(permalink);
            doc.Add(author);
            doc.Add(username);
            doc.Add(appTypeID);
            doc.Add(app);

            return doc;
        }

        #endregion

        #region Writer Actions

        private static readonly Object WriterLock = new Object();

        private IndexSearcher Searcher
        {
            get { return DoWriterAction(writer => new IndexSearcher(writer.GetReader())); }
        }

        private void DoWriterAction(Action<IndexWriter> action)
        {
            lock (WriterLock)
            {
                EnsureIndexWriter();
            }
            action(_writer);
        }

        private T DoWriterAction<T>(Func<IndexWriter, T> action)
        {
            lock (WriterLock)
            {
                EnsureIndexWriter();
            }
            return action(_writer);
        }

        void EnsureIndexWriter()
        {
            if (_writer == null)
            {
                if (IndexWriter.IsLocked(_directory))
                {
                    SueetieLogs.LogSiteEntry("SEARCH INDEX WRITER MESSAGE: Deleting Lock");
                    IndexWriter.Unlock(_directory);
                    SueetieLogs.LogSiteEntry("SEARCH INDEX WRITER MESSAGE: Lock Deleted");
                }
                _writer = new IndexWriter(_directory, _analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
                _writer.SetMergePolicy(new LogDocMergePolicy(_writer));
                _writer.SetMergeFactor(5);
            }
        }

        private void CommitAndCloseWriter()
        {
            DoWriterAction(writer =>
            {
                writer.Commit();
                writer.Optimize();
            });

            Dispose();
        }

        public void Dispose()
        {

            lock (WriterLock)
            {
                if (!_disposed)
                {
                    var writer = _writer;

                    if (writer != null)
                    {
                        try
                        {
                            writer.Close();
                        }
                        catch (ObjectDisposedException e)
                        {
                            SueetieLogs.LogSiteEntry(SiteLogType.Exception, SiteLogCategoryType.SearchException, "EXCEPTION CLOSING INDEX WRITER: " + e.Message);
                        }
                        _writer = null;
                    }

                    var directory = _directory;
                    if (directory != null)
                    {
                        try
                        {
                            directory.Close();
                        }
                        catch (ObjectDisposedException e)
                        {
                            SueetieLogs.LogSiteEntry(SiteLogType.Exception, SiteLogCategoryType.SearchException, "EXCEPTION CLOSING SEARCH DIRECTORY: " + e.Message);
                        }
                    }

                    _disposed = true;
                }
            }
            GC.SuppressFinalize(this);
        }
        private void ExecuteRemoveDoc(int contentID)
        {
            Query searchQuery = GetContentIDSearchQuery(contentID);
            DoWriterAction(writer => writer.DeleteDocuments(searchQuery));
        }

        #endregion

        #endregion

        #region Search

        protected virtual SueetieSearchResult CreateSearchResult(Document doc, float score)
        {
            SueetieSearchResult result = new SueetieSearchResult
               {
                   ContainerName = doc.Get(ContainerName),
                   ContentID = NumericUtils.PrefixCodedToInt(doc.Get(ContentID)),
                   PublishDate = DateTools.StringToDate(doc.Get(Pubdate)),
                   Title = doc.Get(Title),
                   Score = score,
                   ContentTypeID = NumericUtils.PrefixCodedToInt(doc.Get(ContentTypeID)),
                   ApplicationTypeID = NumericUtils.PrefixCodedToInt(doc.Get(ApplicationTypeID)),
                   PermaLink = doc.Get(PermaLink),
                   Author = doc.Get(Author),
                   ApplicationKey = doc.Get(ApplicationKey),
                   DisplayTags = doc.Get(DisplayTags)
               };

            return result;
        }

        public List<SueetieSearchResult> Search(string queryString, int max, bool isRestricted)
        {
            var list = new List<SueetieSearchResult>();
            if (String.IsNullOrEmpty(queryString)) return list;
            QueryParser parser = BuildQueryParser(Body);
            Query query = parser.Parse(queryString);
            return PerformQuery(list, query, max, isRestricted);
        }


        private List<SueetieSearchResult> PerformQuery(List<SueetieSearchResult> list, Query queryBody, int max, bool isRestricted)
        {
            Query isRestrictedQuery = new TermQuery(new Term(IsRestricted, isRestricted.ToString()));

            var query = new BooleanQuery();
            query.Add(queryBody, BooleanClause.Occur.MUST);
            if (isRestricted)
                query.Add(isRestrictedQuery, BooleanClause.Occur.MUST_NOT);
            IndexSearcher searcher = Searcher;
            TopDocs hits = searcher.Search(query, max);
            int length = hits.scoreDocs.Length;
            int resultsAdded = 0;
            float minScore = _config.SearchSettings.MinimumScore;
            float scoreNorm = 100.0f / hits.GetMaxScore();
            for (int i = 0; i < length && resultsAdded < max; i++)
            {
                float score = hits.scoreDocs[i].score * scoreNorm;
                SueetieSearchResult result = CreateSearchResult(searcher.Doc(hits.scoreDocs[i].doc), score);
                if (result.Score > minScore)
                {
                    string _content = HighlightContents(query, searcher.Doc(hits.scoreDocs[i].doc).GetField(Body).StringValue());
                    if (!string.IsNullOrEmpty(_content))
                    {
                        if (string.IsNullOrEmpty(_content) || _content.Contains("&raquo;"))
                            _content = DataHelper.TruncateText(searcher.Doc(hits.scoreDocs[i].doc).GetField(Body).StringValue(), _bodyDisplayLength);
                    }
                    else
                    {
                        _content = DataHelper.TruncateText(searcher.Doc(hits.scoreDocs[i].doc).GetField(Body).StringValue(), _bodyDisplayLength);
                    }
                    if (_content.Contains("&raquo;"))
                        _content = DataHelper.TruncateTextNoElipse(_content.Replace("...", string.Empty), _content.IndexOf("&raquo;") + 2);
                    result.HighlightedContent = _content;
                    list.Add(result);
                    resultsAdded++;
                }

            }
            return list;
        }


        private string HighlightContents(Query q, string text)
        {
            string highlightStartTag = "<span class='highlight'>";
            string highlightEndTag = "</span>";
            int fragmentLength = 150;
            QueryScorer scorer = new QueryScorer(q, Body);
            Formatter formatter = new SimpleHTMLFormatter(highlightStartTag, highlightEndTag);
            Highlighter highlighter = new Highlighter(formatter, scorer);
            highlighter.SetTextFragmenter(new SimpleFragmenter(fragmentLength));
            TokenStream stream = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29).TokenStream(Body, new StringReader(text));
            return highlighter.GetBestFragments(stream, text, 3, "...");
        }

        private QueryParser BuildQueryParser(string field)
        {
            QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, field, _analyzer);
            parser.SetDefaultOperator(QueryParser.Operator.AND);
            return parser;
        }

        #endregion

        #region Support Utilities

        public void RemovePost(int contentID)
        {
            ExecuteRemoveDoc(contentID);
            DoWriterAction(writer => writer.Commit());
        }

        public int GetIndexedCountForApplicationTypeID(int appTypeID)
        {
            Query query = GetApplicationTypeIDSearchQuery(appTypeID);
            TopDocs hits = Searcher.Search(query, 1);
            return hits.totalHits;
        }

        public int GetTotalIndexedCount()
        {
            return DoWriterAction(writer => writer.GetReader().NumDocs());
        }

        private static Query GetContentIDSearchQuery(int contentID)
        {
            return new TermQuery(new Term(ContentID, NumericUtils.IntToPrefixCoded(contentID)));
        }
        private static Query GetApplicationTypeIDSearchQuery(int id)
        {
            return new TermQuery(new Term(ApplicationTypeID, NumericUtils.IntToPrefixCoded(id)));
        }

        private static string RaquoIt(string field)
        {
            if (string.IsNullOrEmpty(field))
                return string.Empty;
            else
                return " &raquo; " + field;
        }

        #endregion


    }
}
