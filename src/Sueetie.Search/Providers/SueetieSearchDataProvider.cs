#region Using

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Configuration;
using System.Web;
using System.IO;
using Sueetie.Core;
using Sueetie.Core.Configuration;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Web.Caching;
using System.Data.SqlClient;
using System.Data;

#endregion

namespace Sueetie.Search
{

    public abstract partial class SueetieSearchDataProvider : ProviderBase
    {
        #region Provider model

        private static string providerKey = "SueetieSearchSqlDataProvider";
        private static SueetieSearchDataProvider _provider;
        private static object _lock = new object();

        public static SueetieSearchDataProvider Provider
        {
            get { LoadProvider(); return _provider; }
        }

        public static SueetieSearchDataProvider LoadProvider()
        {
            _provider = SueetieCache.Current[providerKey] as SueetieSearchDataProvider;
            // Avoid claiming lock if providers are already loaded
            if (_provider == null)
            {
                lock (_lock)
                {
                    // Do this again to make sure _provider is still null
                    if (_provider == null)
                    {

                        SueetieConfiguration sueetieConfig = SueetieConfiguration.Get();
                        List<SueetieProvider> sueetieProviders = sueetieConfig.SueetieProviders;

                        SueetieProvider _p = sueetieProviders.Find(delegate(SueetieProvider sp) { return sp.Name == "SueetieSearchSqlDataProvider"; });
                        _provider = Activator.CreateInstance(Type.GetType(_p.ProviderType), new object[] { _p.ConnectionString }) as SueetieSearchDataProvider;
                        SueetieCache.Current.InsertMax(providerKey, _provider, new CacheDependency(sueetieConfig.ConfigPath));

                    }
                }
            }
            return _provider;
        }

        #endregion

        #region Blogs

        public abstract List<SueetieBlogPost> GetSueetieBlogPostsToIndex(int contenttypeID, DateTime pubDate);

        #endregion

        #region Wiki Pages

        public abstract List<SueetieWikiPage> GetSueetieWikiPagesToIndex(int contenttypeID, DateTime pubDate);

        #endregion

        #region Forum Messages

        public abstract List<SueetieForumMessage> GetSueetieForumMessagesToIndex(int contenttypeID, DateTime pubDate);

        #endregion

        #region Media Albums

        public abstract List<SueetieMediaAlbum> GetSueetieMediaAlbumsToIndex(DateTime pubDate);

        #endregion

        #region Media Objects

        public abstract List<SueetieMediaObject> GetSueetieMediaObjectsToIndex(DateTime pubDate);

        #endregion

        #region Content Pages

        public abstract List<SueetieContentPage> GetSueetieContentPagesToIndex(int contenttypeID, DateTime pubDate);

        #endregion

        #region Background Task Functions

        public abstract SueetieIndexTaskItem GetSueetieIndexTaskItem(int taskID);
        public abstract void UpdateAndEndTask(SueetieIndexTaskItem sueetieIndexTaskItem);

        #endregion

        #region Populate

        public static void PopulateSueetieBlogPostList(IDataReader dr, SueetieBlogPost _sueetieBlogPost)
        {
            _sueetieBlogPost.SueetiePostID = (int)dr["sueetiepostid"];
            _sueetieBlogPost.UserID = (int)dr["userid"];
            _sueetieBlogPost.PostID = DataHelper.GetGuid(dr, "postid");
            _sueetieBlogPost.Title = dr["title"] as string;
            _sueetieBlogPost.Description = dr["description"] as string;
            _sueetieBlogPost.PostContent = dr["postcontent"] as string;
            _sueetieBlogPost.DateCreated = (DateTime)dr["datecreated"];
            _sueetieBlogPost.DateModified = (DateTime)dr["datemodified"];
            _sueetieBlogPost.Author = dr["author"] as string;
            _sueetieBlogPost.IsPublished = (bool)dr["ispublished"];
            _sueetieBlogPost.IsCommentEnabled = (bool)dr["iscommentenabled"];
            _sueetieBlogPost.Raters = (int)dr["raters"];
            _sueetieBlogPost.Rating = DataHelper.GetFloat(dr, "rating");
            _sueetieBlogPost.Slug = dr["slug"] as string;
            _sueetieBlogPost.Email = dr["email"] as string;
            _sueetieBlogPost.DisplayName = dr["displayname"] as string;
            _sueetieBlogPost.GroupID = (int)dr["groupid"];
            _sueetieBlogPost.GroupKey = dr["groupkey"] as string;
            _sueetieBlogPost.ApplicationID = (int)dr["applicationid"];
            _sueetieBlogPost.ApplicationTypeID = (int)dr["applicationtypeid"];
            _sueetieBlogPost.ApplicationKey = dr["applicationkey"] as string;
            _sueetieBlogPost.ContentID = (int)dr["contentid"];
            _sueetieBlogPost.ContentTypeID = (int)dr["contenttypeid"];
            _sueetieBlogPost.Permalink = dr["permalink"] as string;
            _sueetieBlogPost.IsRestricted = (bool)dr["isrestricted"];
            _sueetieBlogPost.GroupName = dr["groupname"] as string;
            _sueetieBlogPost.ApplicationName = dr["applicationname"] as string;
            _sueetieBlogPost.BlogAccessRole = dr["blogaccessrole"] as string;
            _sueetieBlogPost.BlogTitle = dr["blogtitle"] as string;
            _sueetieBlogPost.IncludeInAggregateList = (bool)dr["includeinaggregatelist"];
            _sueetieBlogPost.IsActive = (bool)dr["isactive"];
            _sueetieBlogPost.Categories = dr["categories"] as string;
            _sueetieBlogPost.Tags = dr["tags"] as string;
        }
        public static void PopulateSueetieWikiPageList(IDataReader dr, SueetieWikiPage _sueetieWikiPage)
        {
            _sueetieWikiPage.PageID = (int)dr["pageid"];
            _sueetieWikiPage.PageFileName = dr["pagefilename"] as string;
            _sueetieWikiPage.PageTitle = dr["pagetitle"] as string;
            _sueetieWikiPage.Keywords = dr["keywords"] as string;
            _sueetieWikiPage.Abstract = dr["abstract"] as string;
            _sueetieWikiPage.Namespace = dr["namespace"] as string;
            _sueetieWikiPage.DateTimeCreated = (DateTime)dr["datetimecreated"];
            _sueetieWikiPage.DateTimeModified = (DateTime)dr["datetimemodified"];
            _sueetieWikiPage.UserID = (int)dr["userid"];
            _sueetieWikiPage.ContentID = (int)dr["contentid"];
            _sueetieWikiPage.Permalink = dr["permalink"] as string;
            _sueetieWikiPage.IsRestricted = (bool)dr["isrestricted"];
            _sueetieWikiPage.Email = dr["email"] as string;
            _sueetieWikiPage.DisplayName = dr["displayname"] as string;
            _sueetieWikiPage.ApplicationKey = dr["applicationkey"] as string;
            _sueetieWikiPage.ApplicationName = dr["applicationname"] as string;
            _sueetieWikiPage.GroupID = (int)dr["groupid"];
            _sueetieWikiPage.GroupKey = dr["groupkey"] as string;
            _sueetieWikiPage.GroupName = dr["groupname"] as string;
            _sueetieWikiPage.ContentTypeID = (int)dr["contenttypeid"];
            _sueetieWikiPage.Active = (bool)dr["active"];
            _sueetieWikiPage.ApplicationID = (int)dr["applicationid"];
            _sueetieWikiPage.ApplicationTypeID = (int)dr["applicationtypeid"];
            _sueetieWikiPage.Categories = dr["categories"] as string;
            _sueetieWikiPage.PageContent = dr["pagecontent"] as string;
            _sueetieWikiPage.UserName = dr["username"] as string;
            _sueetieWikiPage.Tags = dr["tags"] as string;
        }


        public static void PopulateSueetieForumMessageList(IDataReader dr, SueetieForumMessage _sueetieForumMessage)
        {
            _sueetieForumMessage.MessageID = (int)dr["messageid"];
            _sueetieForumMessage.TopicID = (int)dr["topicid"];
            _sueetieForumMessage.UserID = (int)dr["userid"];
            _sueetieForumMessage.Message = dr["message"] as string;
            _sueetieForumMessage.SueetieUserID = (int)dr["sueetieuserid"];
            _sueetieForumMessage.ContentID = (int)dr["contentid"];
            _sueetieForumMessage.ContentTypeID = (int)dr["contenttypeid"];
            _sueetieForumMessage.ApplicationID = (int)dr["applicationid"];
            _sueetieForumMessage.IsRestricted = (bool)dr["isrestricted"];
            _sueetieForumMessage.Permalink = dr["permalink"] as string;
            _sueetieForumMessage.DateTimeCreated = (DateTime)dr["datetimecreated"];
            _sueetieForumMessage.SourceID = (int)dr["sourceid"];
            _sueetieForumMessage.Topic = dr["topic"] as string;
            _sueetieForumMessage.DisplayName = dr["displayname"] as string;
            _sueetieForumMessage.Email = dr["email"] as string;
            _sueetieForumMessage.ApplicationDescription = dr["applicationdescription"] as string;
            _sueetieForumMessage.GroupID = (int)dr["groupid"];
            _sueetieForumMessage.GroupName = dr["groupname"] as string;
            _sueetieForumMessage.TopicSueetieUserID = (int)dr["topicsueetieuserid"];
            _sueetieForumMessage.TopicDisplayName = dr["topicdisplayname"] as string;
            _sueetieForumMessage.UserName = dr["username"] as string;
            _sueetieForumMessage.ApplicationTypeID = (int)dr["applicationtypeid"];
            _sueetieForumMessage.ApplicationKey = dr["applicationkey"] as string;
            _sueetieForumMessage.Forum = dr["forum"] as string;
            _sueetieForumMessage.ForumID = (int)dr["forumid"];
            _sueetieForumMessage.GroupKey = dr["groupkey"] as string;
            _sueetieForumMessage.Edited =  (DateTime)DataHelper.DateOrNull(dr["edited"].ToString());
            _sueetieForumMessage.Tags = dr["tags"] as string;
        }

	

        public static void PopulateSueetieMediaAlbumList(IDataReader dr, SueetieMediaAlbum _sueetieMediaAlbum)
        {
            _sueetieMediaAlbum.AlbumID = (int)dr["albumid"];
            _sueetieMediaAlbum.ContentID = (int)dr["contentid"];
            _sueetieMediaAlbum.SourceID = (int)dr["sourceid"];
            _sueetieMediaAlbum.ContentTypeID = (int)dr["contenttypeid"];
            _sueetieMediaAlbum.ApplicationID = (int)dr["applicationid"];
            _sueetieMediaAlbum.ApplicationKey = dr["applicationkey"] as string;
            _sueetieMediaAlbum.SueetieUserID = (int)dr["sueetieuserid"];
            _sueetieMediaAlbum.Permalink = dr["permalink"] as string;
            _sueetieMediaAlbum.DateTimeCreated = (DateTime)DataHelper.DateOrNull(dr["datetimecreated"].ToString());
            _sueetieMediaAlbum.IsRestricted = (bool)dr["isrestricted"];
            _sueetieMediaAlbum.AlbumTitle = dr["albumtitle"] as string;
            _sueetieMediaAlbum.ApplicationDescription = dr["applicationdescription"] as string;
            _sueetieMediaAlbum.GroupID = (int)dr["groupid"];
            _sueetieMediaAlbum.GroupKey = dr["groupkey"] as string;
            _sueetieMediaAlbum.GroupName = dr["groupname"] as string;
            _sueetieMediaAlbum.UserName = dr["username"] as string;
            _sueetieMediaAlbum.Email = dr["email"] as string;
            _sueetieMediaAlbum.DisplayName = dr["displayname"] as string;
            _sueetieMediaAlbum.MediaObjectUrl = dr["mediaobjecturl"] as string;
            _sueetieMediaAlbum.GalleryId = (int)dr["galleryid"];
            _sueetieMediaAlbum.GalleryName = dr["galleryname"] as string;
            _sueetieMediaAlbum.ContentTypeName = dr["contenttypename"] as string;
            _sueetieMediaAlbum.IsAlbum = (bool)dr["isalbum"];
            _sueetieMediaAlbum.ContentTypeDescription = dr["contenttypedescription"] as string;
            _sueetieMediaAlbum.SueetieAlbumID = (int)dr["sueetiealbumid"];
            _sueetieMediaAlbum.UserLogCategoryID = (int)dr["userlogcategoryid"];
            _sueetieMediaAlbum.AlbumMediaCategoryID = (int)dr["albummediacategoryid"];
            _sueetieMediaAlbum.ApplicationTypeID = (int)dr["applicationtypeid"];
            _sueetieMediaAlbum.AlbumDescription = dr["albumdescription"] as string;
            _sueetieMediaAlbum.DateLastModified = (DateTime)DataHelper.DateOrNull(dr["datelastmodified"].ToString());
            _sueetieMediaAlbum.Tags = dr["tags"] as string;
        }
	
        public static void PopulateSueetieMediaObjectList(IDataReader dr, SueetieMediaObject _sueetieMediaObject)
        {
            _sueetieMediaObject.MediaObjectID = (int)dr["mediaobjectid"];
            _sueetieMediaObject.AlbumID = (int)dr["albumid"];
            _sueetieMediaObject.ContentID = (int)dr["contentid"];
            _sueetieMediaObject.SourceID = (int)dr["sourceid"];
            _sueetieMediaObject.ContentTypeID = (int)dr["contenttypeid"];
            _sueetieMediaObject.ContentTypeDescription = dr["contenttypedescription"] as string;
            _sueetieMediaObject.SueetieUserID = (int)dr["sueetieuserid"];
            _sueetieMediaObject.Permalink = dr["permalink"] as string;
            _sueetieMediaObject.DateTimeCreated = (DateTime)dr["datetimecreated"];
            _sueetieMediaObject.IsRestricted = (bool)dr["isrestricted"];
            _sueetieMediaObject.ApplicationID = (int)dr["applicationid"];
            _sueetieMediaObject.ApplicationKey = dr["applicationkey"] as string;
            _sueetieMediaObject.ApplicationDescription = dr["applicationdescription"] as string;
            _sueetieMediaObject.MediaObjectTitle = dr["mediaobjecttitle"] as string;
            _sueetieMediaObject.AlbumTitle = dr["albumtitle"] as string;
            _sueetieMediaObject.GroupID = (int)dr["groupid"];
            _sueetieMediaObject.GroupKey = dr["groupkey"] as string;
            _sueetieMediaObject.GroupName = dr["groupname"] as string;
            _sueetieMediaObject.UserName = dr["username"] as string;
            _sueetieMediaObject.Email = dr["email"] as string;
            _sueetieMediaObject.DisplayName = dr["displayname"] as string;
            _sueetieMediaObject.MediaObjectUrl = dr["mediaobjecturl"] as string;
            _sueetieMediaObject.GalleryID = (int)dr["galleryid"];
            _sueetieMediaObject.GalleryName = dr["galleryname"] as string;
            _sueetieMediaObject.MediaObjectDescription = dr["mediaobjectdescription"] as string;
            _sueetieMediaObject.InDownloadReport = (bool)dr["indownloadreport"];
            _sueetieMediaObject.ApplicationTypeID = (int)dr["applicationtypeid"];
            _sueetieMediaObject.Abstract = dr["abstract"] as string;
            _sueetieMediaObject.Authors = dr["authors"] as string;
            _sueetieMediaObject.Location = dr["location"] as string;
            _sueetieMediaObject.Series = dr["series"] as string;
            _sueetieMediaObject.DocumentType = dr["documenttype"] as string;
            _sueetieMediaObject.Keywords = dr["keywords"] as string;
            _sueetieMediaObject.Misc = dr["misc"] as string;
            _sueetieMediaObject.Number = dr["number"] as string;
            _sueetieMediaObject.Version = dr["version"] as string;
            _sueetieMediaObject.Organization = dr["organization"] as string;
            _sueetieMediaObject.Conference = dr["conference"] as string;
            _sueetieMediaObject.ISxN = dr["isxn"] as string;
            _sueetieMediaObject.PublicationDate = dr["publicationdate"] as string;
            _sueetieMediaObject.Publisher = dr["publisher"] as string;
            _sueetieMediaObject.IsAlbum = (bool)dr["isalbum"];
            _sueetieMediaObject.OriginalFilename = dr["originalfilename"] as string;
            _sueetieMediaObject.CreatedBy = dr["createdby"] as string;
            _sueetieMediaObject.DateAdded = (DateTime)dr["dateadded"];
            _sueetieMediaObject.LastModifiedBy = dr["lastmodifiedby"] as string;
            _sueetieMediaObject.DateLastModified = (DateTime)dr["datelastmodified"];
            _sueetieMediaObject.ThumbnailWidth = (int)dr["thumbnailwidth"];
            _sueetieMediaObject.ThumbnailHeight = (int)dr["thumbnailheight"];
            _sueetieMediaObject.IsImage = (bool)dr["isimage"];
            _sueetieMediaObject.Tags = dr["tags"] as string;
        }

        public static void PopulateSueetieContentPageList(IDataReader dr, SueetieContentPage _sueetieContentPage)
        {
            _sueetieContentPage.ContentPageID = (int)dr["contentpageid"];
            _sueetieContentPage.ContentPageGroupID = (int)dr["contentpagegroupid"];
            _sueetieContentPage.PageSlug = dr["pageslug"] as string;
            _sueetieContentPage.PageTitle = dr["pagetitle"] as string;
            _sueetieContentPage.PageDescription = dr["pagedescription"] as string;
            _sueetieContentPage.ReaderRoles = dr["readerroles"] as string;
            _sueetieContentPage.LastUpdateDateTime = (DateTime)dr["lastupdatedatetime"];
            _sueetieContentPage.LastUpdateUserID = (int)dr["lastupdateuserid"];
            _sueetieContentPage.PageKey = dr["pagekey"] as string;
            _sueetieContentPage.EditorRoles = dr["editorroles"] as string;
            _sueetieContentPage.IsPublished = (bool)dr["ispublished"];
            _sueetieContentPage.DisplayOrder = (int)dr["displayorder"];
            _sueetieContentPage.ContentTypeID = (int)dr["contenttypeid"];
            _sueetieContentPage.Permalink = dr["permalink"] as string;
            _sueetieContentPage.DateTimeCreated = (DateTime)dr["datetimecreated"];
            _sueetieContentPage.IsRestricted = (bool)dr["isrestricted"];
            _sueetieContentPage.ContentID = (int)dr["contentid"];
            _sueetieContentPage.ApplicationID = (int)dr["applicationid"];
            _sueetieContentPage.ApplicationKey = dr["applicationkey"] as string;
            _sueetieContentPage.ContentPageGroupTitle = dr["contentpagegrouptitle"] as string;
            _sueetieContentPage.Tags = dr["tags"] as string;
            _sueetieContentPage.SearchBody = dr["searchbody"] as string;
            _sueetieContentPage.ApplicationName = dr["applicationname"] as string;
            _sueetieContentPage.ApplicationTypeID = (int)dr["applicationtypeid"];
            _sueetieContentPage.UserName = dr["username"] as string;
            _sueetieContentPage.DisplayName = dr["displayname"] as string;
        }
	
        #endregion
    }

}
