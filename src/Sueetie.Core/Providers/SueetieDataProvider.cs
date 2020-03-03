// -----------------------------------------------------------------------
// <copyright file="SueetieDataProvider.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using System.Data;
    using System.Data.SqlClient;
    using System.Web.Caching;
    using System.Web.UI;

    public abstract class SueetieDataProvider : ProviderBase
    {
        private static readonly object _lock = new object();
        private static string providerKey = "SueetieSqlDataProvider";
        private static SueetieDataProvider provider;

        public static SueetieDataProvider Provider
        {
            get
            {
                LoadProvider();
                return provider;
            }
        }

        public static SueetieDataProvider LoadProvider()
        {
            provider = SueetieCache.Current[providerKey] as SueetieDataProvider;

            // Avoid claiming lock if providers are already loaded
            if (provider == null)
            {
                lock (_lock)
                {
                    // Do this again to make sure provider is still null
                    if (provider == null)
                    {
                        var sueetieConfig = SueetieConfiguration.Get();
                        var sueetieProviders = sueetieConfig.SueetieProviders;

                        var _p = sueetieProviders.Find(delegate(SueetieProvider sp) { return sp.Name == "SueetieSqlDataProvider"; });
                        provider = Activator.CreateInstance(Type.GetType(_p.ProviderType), _p.ConnectionString) as SueetieDataProvider;
                        SueetieCache.Current.InsertMax(providerKey, provider, new CacheDependency(sueetieConfig.ConfigPath));
                    }
                }
            }
            return provider;
        }

        public abstract StringDictionary GetSiteSettingsDictionary();
        public abstract void UpdateSiteSetting(SiteSetting siteSetting);

        public abstract SueetieUser GetUser(int userID);
        public abstract SueetieUser GetUser(string username);

        public abstract SueetieUser GetSueetieUserByEmail(string email);

        public abstract SueetieUser GetSueetieUserFromForumID(int userforumid);

        public abstract SueetieUserProfile GetSueetieUserProfile(int userId);

        public abstract SueetieUserProfile GetSueetieUserProfile(string username);

        public abstract int GetUserID(string username);

        public abstract int CreateSueetieUser(SueetieUser sueetieUser);

        public abstract void UpdateSueetieUser(SueetieUser sueetieUser);

        public abstract void UpdateSueetieUserBio(SueetieUser sueetieUser);

        public abstract void UpdateDisplayName(SueetieUser sueetieUser);

        public abstract SueetiePM[] GetUnreadPMs(string username);

        public abstract List<SueetieUser> GetSueetieUserList(SueetieUserType sueetieUserType);

        public abstract List<SueetieAspnetUser> GetUnapprovedUserList();

        public abstract List<SueetieAspnetUser> GetInactiveUserList();

        public abstract void DeactivateUser(string _username);

        public abstract List<SueetieBannedIP> GetSueetieBannedIPList();

        public abstract void BanIP(string ip);

        public abstract void UpdateBannedIP(int bannedID, string ip);

        public abstract void RemoveBannedIP(int bannedID);

        public abstract void RemoveBannedIP(string ip);

        public abstract void UpdateSueetieUserIP(SueetieUser sueetieUser);

        public abstract void UpdateSueetieUserProfile(Pair pair, int userID);

        public abstract List<SueetieSubscriber> GetSueetieSubscriberList();

        public abstract bool IsNewUsername(string username);

        public abstract bool IsNewEmailAddress(string email);

        public abstract List<string> GetRoles(string applicationName, string username);

        public abstract List<SueetieRole> GetSueetieRoleList();

        public abstract void CreateSueetieRole(SueetieRole sueetieRole);

        public abstract Guid GetAspnetRoleID(string rolename);

        public abstract bool DeleteSueetieRole(string rolename);

        public abstract void UpdateSueetieRole(SueetieRole sueetieRole);

        public abstract void UpdateSueetieUserAvatar(SueetieUserAvatar sueetieUserAvatar);

        public abstract void DeleteAvatar(int _userID);

        public abstract SueetieBlog GetSueetieBlog(int blogID, string applicationKey);

        public abstract List<SueetieBlog> GetSueetieBlogTitles();

        public abstract void UpdateSueetieBlog(SueetieBlog sueetieBlog);

        public abstract void CreateSueetieBlog(SueetieBlog sueetieBlog);

        public abstract void SetMostRecentContentID(SueetieBlog sueetieBlog);

        public abstract List<SueetieBlog> GetSueetieBlogList();

        public abstract int CreateUpdateSueetieBlogPost(SueetieBlogPost sueetieBlogPost);
        public abstract int CreateUpdateSueetieBlogComment(SueetieBlogComment sueetieBlogComment);
        public abstract SueetieBlogPost GetSueetieBlogPost(string postGuid);
        public abstract SueetieBlogComment GetSueetieBlogComment(string postcommentid);

        public abstract List<SueetieBlogPost> GetSueetieBlogPostList();
        public abstract List<SueetieBlog> GetSueetieBlogList(ApplicationQuery applicationQuery);
        public abstract List<SueetieBlogPost> GetSueetieBlogPostList(ContentQuery contentQuery);
        public abstract List<SueetieBlogComment> GetSueetieBlogCommentList(ContentQuery contentQuery);

        public abstract void CreateBlogAdmin(SueetieUser sueetieUser);

        public abstract void SaveSueetieBlogSpam(SueetieBlogSpam sueetieBlogSpam);

        public abstract int FollowUser(SueetieFollow sueetieFollow);
        public abstract void UnFollowUser(SueetieFollow sueetieFollow);
        public abstract int CreateFavorite(UserContent userContent);
        public abstract int GetFavoriteID(UserContent userContent);
        public abstract UserContent DeleteFavorite(int favoriteID);
        public abstract UserContent DeleteFavorite(UserContent userContent);
        public abstract int GetFollowID(SueetieFollow sueetieFollow);
        public abstract List<SueetieFollow> GetSueetieFollowList(int userID, int followTypeID);
        public abstract List<FavoriteContent> GetFavoriteContentList(int userID);

        public abstract int AddForumTopic(SueetieForumContent sueetieForumContent);
        public abstract int AddForumMessage(SueetieForumContent sueetieForumContent);
        public abstract SueetieForumTopic GetSueetieForumTopic(SueetieForumContent sueetieForumContent);
        public abstract SueetieForumMessage GetSueetieForumMessage(SueetieForumContent sueetieForumContent);
        public abstract void UpdateForumTheme(string themename);

        public abstract List<SueetieForumTopic> GetSueetieForumTopicList(ContentQuery contentQuery);
        public abstract List<SueetieForumMessage> GetSueetieForumMessageList(ContentQuery contentQuery);

        public abstract void CompleteForumSetup();

        public abstract int GetForumUserID(int boardID, string username);
        public abstract int CreateForumUser(SueetieUser sueetieUser);

        public abstract void EnterForumTopicTags(SueetieTagEntry sueetieTagEntry);

        public abstract List<SueetieMediaGallery> GetSueetieMediaGalleryList();
        public abstract void AdminUpdateSueetieMediaGallery(SueetieMediaGallery sueetieMediaGallery);
        public abstract void CreateMediaGallery(int _galleryID);

        public abstract SueetieMediaObject GetSueetieMediaPhoto(int mediaObjectID);
        public abstract List<SueetieMediaAlbum> GetSueetieAlbumUpdateList(int galleryID);
        public abstract List<SueetieMediaObject> GetSueetieMediaUpdateList();
        public abstract List<SueetieMediaObject> GetSueetieMediaUpdateList(int albumID);
        public abstract List<SueetieMediaObject> GetSueetieMediaObjectList(ContentQuery contentQuery);
        public abstract void RecordDownload(SueetieDownload sueetieDownload);
        public abstract List<ContentTypeDescription> GetAlbumContentTypeDescriptionList();
        public abstract SueetieMediaAlbum GetSueetieMediaAlbum(int albumID);
        public abstract void CreateSueetieAlbum(int albumID, string albumPath, int contentTypeID);
        public abstract void CreateSueetieMediaObject(SueetieMediaObject sueetieMediaObject);
        public abstract int GetAlbumContentTypeID(int albumid);
        public abstract void UpdateAlbumContentTypeID(SueetieMediaObject sueetieMediaObject);
        public abstract List<SueetieMediaAlbum> GetSueetieMediaAlbumList(ContentQuery contentQuery);
        public abstract void AdminUpdateSueetieMediaAlbum(SueetieMediaAlbum sueetieMediaAlbum);
        public abstract void UpdateSueetieMediaObject(SueetieMediaObject sueetieMediaObject);
        public abstract void UpdateSueetieMediaAlbum(SueetieMediaAlbum sueetieMediaAlbum);

        public abstract List<SueetieDownload> GetSueetieDownloadList(ContentQuery contentQuery);
        public abstract bool IsIncludedInDownload(int mediaobjectid);
        public abstract void SetIncludedInDownload(int mediaobjectid, bool includedInDownload);
        public abstract List<SueetieMediaAlbum> GetSueetieMediaAlbumList(int galleryID);
        public abstract List<SueetieMediaObject> GetSueetieMediaObjectList(int albumID);

        public abstract void EnterMediaObjectTags(SueetieTagEntry sueetieTagEntry);
        public abstract void EnterMediaAlbumTags(SueetieTagEntry sueetieTagEntry);

        public abstract void PopulateMediaObjectTitles();

        public abstract List<SueetieMediaDirectory> GetSueetieMediaDirectoryList();

        public abstract void UpdateSueetieAlbumPath(SueetieMediaAlbum sueetieMediaAlbum);

        public abstract SueetieWikiPage GetSueetieWikiPage(SueetieWikiPage sueetieWikiPage);
        public abstract int CreateSueetieWikiPage(SueetieWikiPage sueetieWikiPage);
        public abstract void UpdateSueetieWikiPage(SueetieWikiPage sueetieWikiPage);
        public abstract void EnterWikiPageTags(SueetieTagEntry sueetieTagEntry);
        public abstract int CreateWikiMessage(SueetieWikiMessage sueetieWikiMessage);
        public abstract List<SueetieWikiMessage> GetSueetieWikiMessageList();
        public abstract SueetieWikiMessage GetSueetieWikiMessage(string messageQueryID);
        public abstract void UpdateSueetieWikiMessage(SueetieWikiMessage sueetieWikiMessage);

        public abstract List<SueetieWikiPage> GetSueetieWikiPageList(ContentQuery contentQuery);

        public abstract int AddPhoto(ClassifiedsPhoto classifiedsPhoto);
        public abstract bool HasClassifiedsCategories();

        public abstract int AddSueetieContent(SueetieContent sueetieContent);
        public abstract void UpdateSueetieContentPermalink(SueetieContent sueetieContent);
        public abstract void UpdateSueetieContentIsRestricted(int contentID, bool isRestricted);

        public abstract List<SueetieApplication> GetSueetieApplicationList();
        public abstract List<SueetieGroup> GetSueetieGroupList();
        public abstract SueetieGroup GetSueetieGroup(int groupID);
        public abstract int CreateGroup(SueetieGroup sueetieGroup);
        public abstract void UpdateSueetieGroup(SueetieGroup sueetieGroup);

        public abstract void UpdateSueetieApplication(SueetieApplication sueetieApplication);
        public abstract int CreateSueetieApplication(SueetieApplication sueetieApplication);
        public abstract void DeleteSueetieApplication(int ApplicationID);

        public abstract SueetieContentPart GetSueetieContentPart(string contentID);
        public abstract int UpdateSueetieContentPart(SueetieContentPart contentPart);
        public abstract List<SueetieContentPart> GetSueetieContentPartList(int pageID);

        public abstract SueetieContentPage GetSueetieContentPage(int pageID);
        public abstract List<SueetieContentPage> GetSueetieContentPageList(int contentGroupID);
        public abstract List<SueetieContentPageGroup> GetSueetieContentPageGroupList();
        public abstract SueetieContentPageGroup GetSueetieContentPageGroup(int contentPageGroupID);
        public abstract void CreateContentPageGroup(SueetieContentPageGroup sueetieContentPageGroup);
        public abstract void UpdateSueetieContentPageGroup(SueetieContentPageGroup sueetieContentPageGroup);
        public abstract int CreateContentPage(SueetieContentPage sueetieContentPage);
        public abstract void UpdateSueetieContentPage(SueetieContentPage sueetieContentPage);
        public abstract List<SueetieApplication> GetCMSApplicationList();
        public abstract void UpdateCMSPermalink(SueetieContentPage sueetieContentPage);

        public abstract void DeleteContentPage(int _contentpageID);
        public abstract void EnterContentPageTags(SueetieTagEntry sueetieTagEntry);

        public abstract bool IsSiteInstalled();

        public abstract void CreateSiteLogEntry(SiteLogEntry siteLogEntry);
        public abstract List<UserLogCategory> GetUserLogCategoryList();
        public abstract void UpdateUserLogCategory(UserLogCategory userLogCategory);
        public abstract void CreateUserLogCategory(UserLogCategory userLogCategory);
        public abstract void DeleteUserLogCategory(int _userlogcategoryID);
        public abstract void CreateUserLogEntry(UserLogEntry userLogEntry);
        public abstract List<UserLogActivity> GetUserLogActivityList(ContentQuery contentQuery);
        public abstract List<UserLogActivity> GetUserLogActivityList(bool showAll);
        public abstract void DeleteUserLogActivity(int _userlogID);
        public abstract List<SiteLogEntry> GetSiteLogEntryList(SiteLogEntry siteLogEntry);
        public abstract List<SiteLogEntry> GetSiteLogTypeList();
        public abstract int GetEventLogCount();
        public abstract void ClearEventLog();

        public abstract void TestTaskEntry();

        public abstract List<SueetieTag> GetSueetieTagList();
        public abstract List<SueetieTag> GetSueetieTagCloudList(SueetieTagQuery sueetieTagQuery);
        public abstract List<SueetieTagMaster> GetSueetieTagMasterList();

        public abstract List<SueetieCalendarEvent> GetSueetieCalendarEventList(int calendarID);
        public abstract List<SueetieCalendar> GetSueetieCalendarList();
        public abstract void UpdateSueetieCalendar(SueetieCalendar sueetieCalendar);
        public abstract void CreateSueetieCalendar(SueetieCalendar sueetieCalendar);
        public abstract SueetieCalendar GetSueetieCalendar(int calendarID);
        public abstract int CreateSueetieCalendarEvent(SueetieCalendarEvent sueetieCalendarEvent);
        public abstract void UpdateSueetieCalendarEvent(SueetieCalendarEvent sueetieCalendarEvent);
        public abstract void DeleteCalendarEvent(string _calendareventGuid);

        public abstract void AddSueetieRequest(SueetieRequest sueetieRequest);
        public abstract List<string> GetFilteredUrlList();
        public abstract List<PageRule> GetPageRuleList();
        public abstract void UpdatePageRule(PageRule pageRule);
        public abstract void DeletePageRule(int pageruleId);
        public abstract void CreatePageRule(PageRule pageRule);

        public abstract List<CrawlerAgent> GetCrawlerAgentList();

        public static void PopulateSueetieAspnetUserList(IDataReader dr, SueetieAspnetUser _sueetieAspnetUser)
        {
            _sueetieAspnetUser.UserID = (int)dr["userid"];
            _sueetieAspnetUser.UserName = dr["username"] as string;
            _sueetieAspnetUser.Email = dr["email"] as string;
            _sueetieAspnetUser.CreateDate = (DateTime)dr["createdate"];
            _sueetieAspnetUser.LastLoginDate = (DateTime)dr["lastlogindate"];
            _sueetieAspnetUser.LastActivityDate = (DateTime)dr["lastactivitydate"];
            _sueetieAspnetUser.IsActive = (bool)dr["isactive"];
            _sueetieAspnetUser.IsApproved = (bool)dr["isapproved"];
        }

        public static void PopulateSueetieUserList(IDataReader dr, SueetieUser _sueetieUser)
        {
            _sueetieUser.UserID = (int)dr["userid"];
            _sueetieUser.MembershipID = DataHelper.GetGuid(dr, "membershipid");
            _sueetieUser.UserName = dr["username"] as string;
            _sueetieUser.Email = dr["email"] as string;
            _sueetieUser.DisplayName = dr["displayname"] as string;
            _sueetieUser.AvatarRoot = (int)dr["avatarroot"];
            _sueetieUser.DateJoined = DataHelper.DateOrNull(dr["datejoined"] as string);
            _sueetieUser.LastActivity = DataHelper.DateOrNull(dr["lastactivity"] as string);
            _sueetieUser.ForumUserID = (int)dr["forumuserid"];
            _sueetieUser.IsFiltered = (bool)dr["isfiltered"];
        }

        public static void PopulateSueetieUser(IDataReader dr, SueetieUser _sueetieUser)
        {
            _sueetieUser.UserID = (int)dr["userid"];
            _sueetieUser.MembershipID = DataHelper.GetGuid(dr, "membershipid");
            _sueetieUser.UserName = dr["username"] as string;
            _sueetieUser.Email = dr["email"] as string;
            _sueetieUser.AvatarImage = dr["avatarimage"] as byte[];
            _sueetieUser.AvatarImageType = dr["avatarimagetype"] as string;
            _sueetieUser.DisplayName = dr["displayname"] as string;
            _sueetieUser.AvatarFilename = dr["avatarRoot"] + ".jpg";
            _sueetieUser.AvatarThumbnailFilename = dr["avatarRoot"] + "t.jpg";
            _sueetieUser.DateJoined = (DateTime)dr["datejoined"];
            _sueetieUser.Bio = dr["bio"] as string;
            _sueetieUser.ForumUserID = (int)dr["forumuserid"];
            _sueetieUser.TimeZone = (int)dr["timezone"];
            _sueetieUser.IsActive = (bool)dr["isactive"];
            _sueetieUser.LastActivity = DataHelper.DateOrNull(dr["lastactivity"] as string);
            _sueetieUser.IP = dr["ip"] as string;
            _sueetieUser.AvatarRoot = (int)dr["avatarroot"];
            _sueetieUser.IsFiltered = (bool)dr["isfiltered"];
        }

        public static void PopulateSueetieUserProfile(IDataReader dr, SueetieUserProfile _sueetieUserProfile)
        {
            _sueetieUserProfile.DisplayName = dr["displayname"] as string;
            _sueetieUserProfile.Gender = dr["gender"] as string;
            _sueetieUserProfile.Country = dr["country"] as string;
            _sueetieUserProfile.Occupation = dr["occupation"] as string;
            _sueetieUserProfile.Website = dr["website"] as string;
            _sueetieUserProfile.TwitterName = dr["twittername"] as string;
            _sueetieUserProfile.Newsletter = DataHelper.StringToBool(dr["newsletter"] as string);
        }

        public static void PopulateSueetieFollowList(IDataReader dr, SueetieFollow _sueetieFollow)
        {
            _sueetieFollow.UserID = int.Parse(dr["userId"].ToString());
            _sueetieFollow.DisplayName = dr["displayname"] as string;
            _sueetieFollow.ThumbnailFilename = dr["thumbnailfilename"] + "t.jpg";
        }

        public static void PopulateFavoriteContentList(IDataReader dr, FavoriteContent _favoriteContent)
        {
            _favoriteContent.FavoriteID = (int)dr["favoriteid"];
            _favoriteContent.ContentID = (int)dr["contentid"];
            _favoriteContent.UserID = (int)dr["userid"];
            _favoriteContent.AuthorUserID = (int)dr["authoruserid"];
            _favoriteContent.DisplayName = dr["displayname"] as string;
            _favoriteContent.Title = dr["title"] as string;
            _favoriteContent.Description = DataHelper.TruncateText(dr["description"] as string, 180);
            _favoriteContent.Permalink = dr["permalink"] as string;
            _favoriteContent.ApplicationID = (int)dr["applicationid"];
            _favoriteContent.ContentTypeID = (int)dr["contenttypeid"];
            _favoriteContent.GroupID = (int)dr["groupid"];
            _favoriteContent.GroupName = dr["groupname"] as string;
            _favoriteContent.IsRestricted = (bool)dr["isrestricted"];
            _favoriteContent.DateTimeCreated = (DateTime)dr["datetimecreated"];
            _favoriteContent.ApplicationName = dr["applicationname"] as string;
            _favoriteContent.ContentType = dr["contenttype"] as string;
            _favoriteContent.ContentTypeAuthoredBy = DataHelper.ContentTypeAuthoredBy(_favoriteContent);
        }

        public static void PopulateSueetieBlogPost(SqlDataReader dr, SueetieBlogPost _sueetieBlogPost)
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
            _sueetieBlogPost.ApplicationKey = dr["applicationkey"] as string;
            _sueetieBlogPost.ApplicationTypeID = (int)dr["applicationtypeid"];
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

        public static void PopulateSueetieBlogComment(SqlDataReader dr, SueetieBlogComment _sueetieBlogComment)
        {
            _sueetieBlogComment.SueetieCommentID = (int)dr["sueetiecommentid"];
            _sueetieBlogComment.UserID = (int)dr["userid"];
            _sueetieBlogComment.PostCommentID = DataHelper.GetGuid(dr, "postcommentid");
            _sueetieBlogComment.PostID = DataHelper.GetGuid(dr, "postid");
            _sueetieBlogComment.CommentDate = (DateTime)dr["commentdate"];
            _sueetieBlogComment.Author = dr["author"] as string;
            _sueetieBlogComment.Email = dr["email"] as string;
            _sueetieBlogComment.Website = dr["website"] as string;
            _sueetieBlogComment.Comment = dr["comment"] as string;
            _sueetieBlogComment.Country = dr["country"] as string;
            _sueetieBlogComment.Ip = dr["ip"] as string;
            _sueetieBlogComment.IsApproved = (bool)dr["isapproved"];
            _sueetieBlogComment.ApplicationID = (int)dr["applicationid"];
            _sueetieBlogComment.ApplicationKey = dr["applicationkey"] as string;
            _sueetieBlogComment.GroupID = (int)dr["groupid"];
            _sueetieBlogComment.GroupKey = dr["groupkey"] as string;
            _sueetieBlogComment.DisplayName = dr["displayname"] as string;
            _sueetieBlogComment.ContentID = (int)dr["contentid"];
            _sueetieBlogComment.ContentTypeID = (int)dr["contenttypeid"];
            _sueetieBlogComment.Permalink = dr["permalink"] as string;
            _sueetieBlogComment.DateTimeCreated = (DateTime)dr["datetimecreated"];
            _sueetieBlogComment.IsRestricted = (bool)dr["isrestricted"];
            _sueetieBlogComment.Title = dr["title"] as string;
            _sueetieBlogComment.PostUserID = (int)dr["postuserid"];
            _sueetieBlogComment.PostDisplayName = dr["postdisplayname"] as string;
        }

        public static void PopulateSueetieForumTopic(IDataReader dr, SueetieForumTopic _sueetieForumTopic)
        {
            _sueetieForumTopic.TopicID = (int)dr["topicid"];
            _sueetieForumTopic.ForumID = (int)dr["forumid"];
            _sueetieForumTopic.UserID = (int)dr["userid"];
            _sueetieForumTopic.Topic = dr["topic"] as string;
            _sueetieForumTopic.SueetieUserID = (int)dr["sueetieuserid"];
            _sueetieForumTopic.ContentID = (int)dr["contentid"];
            _sueetieForumTopic.SourceID = (int)dr["sourceid"];
            _sueetieForumTopic.ContentTypeID = (int)dr["contenttypeid"];
            _sueetieForumTopic.ApplicationID = (int)dr["applicationid"];
            _sueetieForumTopic.Permalink = dr["permalink"] as string;
            _sueetieForumTopic.IsRestricted = (bool)dr["isrestricted"];
            _sueetieForumTopic.GroupID = (int)dr["groupid"];
            _sueetieForumTopic.DisplayName = dr["displayname"] as string;
            _sueetieForumTopic.ApplicationKey = dr["applicationkey"] as string;
            _sueetieForumTopic.GroupName = dr["groupname"] as string;
            _sueetieForumTopic.Forum = dr["forum"] as string;
            _sueetieForumTopic.DateTimeCreated = (DateTime)dr["datetimecreated"];
            _sueetieForumTopic.IsDeleted = (bool)dr["isdeleted"];
            _sueetieForumTopic.Tags = dr["tags"] as string;
            _sueetieForumTopic.SueetieUserIDs = dr["sueetieuserids"] as string;
            _sueetieForumTopic.LeadTopicMessageID = dr["leadtopicmessageid"] as string;
        }

        public static void PopulateSueetieForumMessage(IDataReader dr, SueetieForumMessage _sueetieForumMessage)
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
            _sueetieForumMessage.Edited = (DateTime)dr["edited"];
            _sueetieForumMessage.Tags = dr["tags"] as string;
        }

        public static void PopulateSueetieMediaAlbumList(IDataReader dr, SueetieMediaAlbum sueetieMediaAlbum)
        {
            sueetieMediaAlbum.AlbumID = (int)dr["albumid"];
            sueetieMediaAlbum.AlbumParentID = (int)dr["albumparentid"];
            sueetieMediaAlbum.ContentID = (int)dr["contentid"];
            sueetieMediaAlbum.SourceID = (int)dr["sourceid"];
            sueetieMediaAlbum.ContentTypeID = (int)dr["contenttypeid"];
            sueetieMediaAlbum.ApplicationID = (int)dr["applicationid"];
            sueetieMediaAlbum.ApplicationKey = dr["applicationkey"] as string;
            sueetieMediaAlbum.SueetieUserID = (int)dr["sueetieuserid"];
            sueetieMediaAlbum.Permalink = dr["permalink"] as string;
            sueetieMediaAlbum.DateTimeCreated = DataHelper.DateOrNull(dr["datetimecreated"].ToString());
            sueetieMediaAlbum.IsRestricted = (bool)dr["isrestricted"];
            sueetieMediaAlbum.AlbumTitle = dr["albumtitle"] as string;
            sueetieMediaAlbum.DirectoryName = dr["directoryname"] as string;
            sueetieMediaAlbum.SueetieAlbumPath = dr["sueetiealbumpath"] as string;
            sueetieMediaAlbum.ApplicationDescription = dr["applicationdescription"] as string;
            sueetieMediaAlbum.GroupID = (int)dr["groupid"];
            sueetieMediaAlbum.GroupKey = dr["groupkey"] as string;
            sueetieMediaAlbum.GroupName = dr["groupname"] as string;
            sueetieMediaAlbum.UserName = dr["username"] as string;
            sueetieMediaAlbum.Email = dr["email"] as string;
            sueetieMediaAlbum.DisplayName = dr["displayname"] as string;
            sueetieMediaAlbum.MediaObjectUrl = dr["mediaobjecturl"] as string;
            sueetieMediaAlbum.GalleryId = (int)dr["galleryid"];
            sueetieMediaAlbum.GalleryName = dr["galleryname"] as string;
            sueetieMediaAlbum.GalleryKey = dr["gallerykey"] as string;
            sueetieMediaAlbum.ContentTypeName = dr["contenttypename"] as string;
            sueetieMediaAlbum.IsAlbum = (bool)dr["isalbum"];
            sueetieMediaAlbum.ContentTypeDescription = dr["contenttypedescription"] as string;
            sueetieMediaAlbum.SueetieAlbumID = (int)dr["sueetiealbumid"];
            sueetieMediaAlbum.UserLogCategoryID = (int)dr["userlogcategoryid"];
            sueetieMediaAlbum.AlbumMediaCategoryID = (int)dr["albummediacategoryid"];
            sueetieMediaAlbum.ApplicationTypeID = (int)dr["applicationtypeid"];
            sueetieMediaAlbum.AlbumDescription = dr["albumdescription"] as string;
            sueetieMediaAlbum.DateLastModified = DataHelper.DateOrNull(dr["datelastmodified"].ToString());
            sueetieMediaAlbum.Tags = dr["tags"] as string;
        }

        public static void PopulateSueetieMediaGalleryList(IDataReader dr, SueetieMediaGallery _sueetieMediaGallery)
        {
            _sueetieMediaGallery.GalleryID = (int)dr["galleryid"];
            _sueetieMediaGallery.GalleryKey = dr["gallerykey"] as string;
            _sueetieMediaGallery.GalleryTitle = dr["gallerytitle"] as string;
            _sueetieMediaGallery.GalleryDescription = dr["gallerydescription"] as string;
            _sueetieMediaGallery.DateAdded = (DateTime)dr["dateadded"];
            _sueetieMediaGallery.DisplayTypeID = (int)dr["displaytypeid"];
            _sueetieMediaGallery.DisplayTypeDescription = dr["displaytypedescription"] as string;
            _sueetieMediaGallery.ApplicationID = (int)dr["applicationid"];
            _sueetieMediaGallery.ApplicationKey = dr["applicationkey"] as string;
            _sueetieMediaGallery.ApplicationDescription = dr["applicationdescription"] as string;
            _sueetieMediaGallery.IsPublic = (bool)dr["ispublic"];
            _sueetieMediaGallery.IsLogged = (bool)dr["islogged"];
            _sueetieMediaGallery.MediaObjectPath = dr["mediaobjectpath"] as string;
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
            if (_sueetieMediaObject.ContentTypeID == 6)
                _sueetieMediaObject.IsImage = true;
            else
                _sueetieMediaObject.IsImage = false;
            _sueetieMediaObject.Tags = dr["tags"] as string;
        }

        public static void PopulateSueetieApplicationList(IDataReader dr, SueetieApplication _sueetieApplication)
        {
            _sueetieApplication.ApplicationID = (int)dr["applicationid"];
            _sueetieApplication.ApplicationTypeID = (int)dr["applicationtypeid"];
            _sueetieApplication.ApplicationKey = dr["applicationkey"] as string;
            _sueetieApplication.Description = dr["description"] as string;
            _sueetieApplication.GroupID = (int)dr["groupid"];
            _sueetieApplication.IsActive = (bool)dr["isactive"];
            _sueetieApplication.GroupKey = dr["groupkey"] as string;
            _sueetieApplication.ApplicationName = dr["applicationname"] as string;
            _sueetieApplication.IsGroup = (bool)dr["isgroup"];
            _sueetieApplication.IsLocked = (bool)dr["islocked"];
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

        public static void PopulateSueetieWikiMessageList(IDataReader dr, SueetieWikiMessage _sueetieWikiMessage)
        {
            _sueetieWikiMessage.MessageID = (int)dr["messageid"];
            _sueetieWikiMessage.PageID = (int)dr["pageid"];
            _sueetieWikiMessage.MessageQueryID = dr["messagequeryid"] as string;
            _sueetieWikiMessage.Subject = dr["subject"] as string;
            _sueetieWikiMessage.Message = dr["message"] as string;
            _sueetieWikiMessage.UserID = (int)dr["userid"];
            _sueetieWikiMessage.DateTimeCreated = (DateTime)dr["datetimecreated"];
            _sueetieWikiMessage.DateTimeModified = DataHelper.DateOrNull(dr["datetimemodified"].ToString());
            _sueetieWikiMessage.IsActive = (bool)dr["isactive"];
            _sueetieWikiMessage.UserName = dr["username"] as string;
            _sueetieWikiMessage.Email = dr["email"] as string;
            _sueetieWikiMessage.DisplayName = dr["displayname"] as string;
            _sueetieWikiMessage.PageFileName = dr["pagefilename"] as string;
            _sueetieWikiMessage.PageTitle = dr["pagetitle"] as string;
            _sueetieWikiMessage.Namespace = dr["namespace"] as string;
            _sueetieWikiMessage.ContentID = (int)dr["contentid"];
            _sueetieWikiMessage.ContentTypeID = (int)dr["contenttypeid"];
            _sueetieWikiMessage.ContentTypeName = dr["contenttypename"] as string;
            _sueetieWikiMessage.SourceID = (int)dr["sourceid"];
            _sueetieWikiMessage.ApplicationID = (int)dr["applicationid"];
            _sueetieWikiMessage.ApplicationTypeID = (int)dr["applicationtypeid"];
            _sueetieWikiMessage.ApplicationKey = dr["applicationkey"] as string;
            _sueetieWikiMessage.GroupID = (int)dr["groupid"];
            _sueetieWikiMessage.Permalink = dr["permalink"] as string;
            _sueetieWikiMessage.IsRestricted = (bool)dr["isrestricted"];
            _sueetieWikiMessage.UserLogCategoryID = (int)dr["userlogcategoryid"];
        }

        public static void PopulateUserLogCategoryList(IDataReader dr, UserLogCategory _userLogCategory)
        {
            _userLogCategory.UserLogCategoryID = (int)dr["userlogcategoryid"];
            _userLogCategory.UserLogCategoryCode = dr["userlogcategorycode"] as string;
            _userLogCategory.UserLogCategoryDescription = dr["userlogcategorydescription"] as string;
            _userLogCategory.IsDisplayed = (bool)dr["isdisplayed"];
            _userLogCategory.IsSyndicated = (bool)dr["issyndicated"];
        }

        public static void PopulateContentTypeDescriptionList(IDataReader dr, ContentTypeDescription _contentTypeDescription)
        {
            _contentTypeDescription.ContentTypeID = (int)dr["contenttypeid"];
            _contentTypeDescription.ContentTypeName = dr["contenttypename"] as string;
            _contentTypeDescription.Description = dr["description"] as string;
            _contentTypeDescription.IsAlbum = (bool)dr["isalbum"];
            _contentTypeDescription.UserLogCategoryID = (int)dr["userlogcategoryid"];
            _contentTypeDescription.AlbumMediaCategoryID = (int)dr["albummediacategoryID"];
        }

        public static void PopulateUserLogActivityList(IDataReader dr, UserLogActivity _userLogActivity)
        {
            _userLogActivity.UserLogID = (int)dr["userlogid"];
            _userLogActivity.UserLogCategoryID = (int)dr["userlogcategoryid"];
            _userLogActivity.UserLogCategoryCode = dr["userlogcategorycode"] as string;
            _userLogActivity.UserID = (int)dr["userid"];
            _userLogActivity.DisplayName = dr["displayname"] as string;
            _userLogActivity.ItemID = (int)dr["itemid"];
            _userLogActivity.ContentID = (int)dr["contentid"];
            _userLogActivity.ContentTypeID = (int)dr["contenttypeid"];
            _userLogActivity.ApplicationID = (int)dr["applicationid"];
            _userLogActivity.GroupID = (int)dr["groupid"];
            _userLogActivity.Permalink = dr["permalink"] as string;
            _userLogActivity.SourceID = (int)dr["sourceid"];
            _userLogActivity.SourceDescription = dr["sourcedescription"] as string;
            _userLogActivity.SourceParentID = (int)dr["sourceparentid"];
            _userLogActivity.SourceParentDescription = dr["sourceparentdescription"] as string;
            _userLogActivity.SourceParentPermalink = dr["sourceparentpermalink"] as string;
            _userLogActivity.ToUserID = (int)dr["touserid"];
            _userLogActivity.ToUserDisplayName = dr["touserdisplayname"] as string;
            _userLogActivity.GroupPath = dr["grouppath"] as string;
            _userLogActivity.ApplicationPath = dr["applicationpath"] as string;
            _userLogActivity.ApplicationName = dr["applicationname"] as string;
            _userLogActivity.DateTimeActivity = (DateTime)dr["datetimeactivity"];
            _userLogActivity.Email = dr["email"] as string;
            _userLogActivity.IsDisplayed = (bool)dr["isdisplayed"];
            _userLogActivity.IsSyndicated = (bool)dr["issyndicated"];
            _userLogActivity.ShowHeader = false;
        }

        public static void PopulateSueetieGroupList(IDataReader dr, SueetieGroup _sueetieGroup)
        {
            _sueetieGroup.GroupID = (int)dr["groupid"];
            _sueetieGroup.GroupKey = dr["groupkey"] as string;
            _sueetieGroup.GroupName = dr["groupname"] as string;
            _sueetieGroup.GroupAdminRole = dr["groupadminrole"] as string;
            _sueetieGroup.GroupUserRole = dr["groupuserrole"] as string;
            _sueetieGroup.GroupDescription = dr["groupdescription"] as string;
            _sueetieGroup.GroupTypeID = (int)dr["grouptypeid"];
            _sueetieGroup.IsActive = (bool)dr["isactive"];
            _sueetieGroup.HasAvatar = (bool)dr["hasavatar"];
        }

        public static void PopulateSueetieRoleList(IDataReader dr, SueetieRole _sueetieRole)
        {
            _sueetieRole.SueetieRoleID = (int)dr["sueetieroleid"];
            _sueetieRole.RoleID = DataHelper.GetGuid(dr, "roleid");
            _sueetieRole.RoleName = dr["rolename"] as string;
            _sueetieRole.IsGroupAdminRole = (bool)dr["isgroupadminrole"];
            _sueetieRole.IsGroupUserRole = (bool)dr["isgroupuserrole"];
            _sueetieRole.IsBlogOwnerRole = (bool)dr["isblogownerrole"];
        }

        public static void PopulateSueetieBannedIPList(IDataReader dr, SueetieBannedIP _sueetieBannedIP)
        {
            _sueetieBannedIP.BannedID = (int)dr["bannedid"];
            _sueetieBannedIP.Mask = dr["mask"] as string;
            _sueetieBannedIP.BannedDateTime = (DateTime)dr["banneddatetime"];
        }

        public static void PopulateSueetieBlog(SqlDataReader dr, SueetieBlog _sueetieBlog)
        {
            _sueetieBlog.BlogID = (int)dr["blogid"];
            _sueetieBlog.ApplicationKey = dr["applicationkey"] as string;
            _sueetieBlog.AppDescription = dr["appdescription"] as string;
            _sueetieBlog.GroupID = (int)dr["groupid"];
            _sueetieBlog.ApplicationID = (int)dr["applicationid"];
            _sueetieBlog.CategoryID = (int)dr["categoryid"];
            _sueetieBlog.BlogOwnerRole = dr["blogownerrole"] as string;
            _sueetieBlog.BlogAccessRole = dr["blogaccessrole"] as string;
            _sueetieBlog.BlogTitle = dr["blogtitle"] as string;
            _sueetieBlog.BlogDescription = dr["blogdescription"] as string;
            _sueetieBlog.PostCount = (int)dr["postcount"];
            _sueetieBlog.CommentCount = (int)dr["commentcount"];
            _sueetieBlog.TrackbackCount = (int)dr["trackbackcount"];
            _sueetieBlog.IncludeInAggregateList = (bool)dr["includeinaggregatelist"];
            _sueetieBlog.IsActive = (bool)dr["isactive"];
            _sueetieBlog.ContentID = (int)dr["contentid"];
            _sueetieBlog.PostImageTypeID = (int)dr["postimagetypeid"];
            _sueetieBlog.RegisteredComments = (bool)dr["registeredcomments"];
            if (_sueetieBlog.ContentID > 0)
            {
                _sueetieBlog.PostAuthorID = (int)dr["postauthorid"];
                _sueetieBlog.PostTitle = dr["posttitle"] as string;
                _sueetieBlog.PostDescription = dr["postdescription"] as string;
                _sueetieBlog.PostContent = dr["postcontent"] as string;
                _sueetieBlog.PostDateCreated = (DateTime)dr["postdatecreated"];
                _sueetieBlog.PostAuthorUserName = dr["postauthorusername"] as string;
                _sueetieBlog.PostAuthorEmail = dr["postauthoremail"] as string;
                _sueetieBlog.PostAuthorDisplayName = dr["postauthordisplayname"] as string;
                _sueetieBlog.IsPublished = (bool)dr["ispublished"];
                _sueetieBlog.Permalink = dr["permalink"] as string;
            }
            _sueetieBlog.DateBlogCreated = (DateTime)dr["dateblogcreated"];
        }

        public static void PopulateSueetieBlogList(IDataReader dr, SueetieBlog _sueetieBlog)
        {
            _sueetieBlog.BlogID = (int)dr["blogid"];
            _sueetieBlog.ApplicationKey = dr["applicationkey"] as string;
            _sueetieBlog.AppDescription = dr["appdescription"] as string;
            _sueetieBlog.GroupID = (int)dr["groupid"];
            _sueetieBlog.ApplicationID = (int)dr["applicationid"];
            _sueetieBlog.CategoryID = (int)dr["categoryid"];
            _sueetieBlog.BlogOwnerRole = dr["blogownerrole"] as string;
            _sueetieBlog.BlogAccessRole = dr["blogaccessrole"] as string;
            _sueetieBlog.BlogTitle = dr["blogtitle"] as string;
            _sueetieBlog.BlogDescription = dr["blogdescription"] as string;
            _sueetieBlog.PostCount = (int)dr["postcount"];
            _sueetieBlog.CommentCount = (int)dr["commentcount"];
            _sueetieBlog.TrackbackCount = (int)dr["trackbackcount"];
            _sueetieBlog.IncludeInAggregateList = (bool)dr["includeinaggregatelist"];
            _sueetieBlog.IsActive = (bool)dr["isactive"];
            _sueetieBlog.ContentID = (int)dr["contentid"];
            _sueetieBlog.PostAuthorID = (int)dr["postauthorid"];
            _sueetieBlog.PostTitle = dr["posttitle"] as string;
            _sueetieBlog.PostDescription = dr["postdescription"] as string;
            _sueetieBlog.PostContent = dr["postcontent"] as string;
            _sueetieBlog.PostDateCreated = DataHelper.DateOrNull(dr["postdatecreated"].ToString());
            _sueetieBlog.PostAuthorUserName = dr["postauthorusername"] as string;
            _sueetieBlog.PostAuthorEmail = dr["postauthoremail"] as string;
            _sueetieBlog.PostAuthorDisplayName = dr["postauthordisplayname"] as string;
            _sueetieBlog.IsPublished = (bool)dr["ispublished"];
            _sueetieBlog.Permalink = dr["permalink"] as string;
            _sueetieBlog.DateBlogCreated = DataHelper.DateOrNull(dr["dateblogcreated"].ToString());
            _sueetieBlog.RegisteredComments = (bool)dr["registeredcomments"];
            _sueetieBlog.PostImageTypeID = (int)dr["postimagetypeid"];
            _sueetieBlog.PostImageHeight = (int)dr["postimageheight"];
            _sueetieBlog.PostImageWidth = (int)dr["postimagewidth"];
            _sueetieBlog.AnchorPositionID = (int)dr["anchorpositionid"];
            _sueetieBlog.MediaAlbumID = (int)dr["mediaalbumid"];
            _sueetieBlog.DefaultPostImage = dr["defaultpostimage"] as string;
        }

        public static void PopulateSueetieBlogTitles(SqlDataReader dr, SueetieBlog _sueetieBlog)
        {
            _sueetieBlog.BlogID = (int)dr["blogid"];
            _sueetieBlog.BlogTitle = dr["blogtitle"] as string;
        }

        public static void PopulateSiteLogEntryList(SqlDataReader dr, SiteLogEntry _siteLogEntry)
        {
            _siteLogEntry.SiteLogID = (int)dr["sitelogid"];
            _siteLogEntry.SiteLogTypeID = (int)dr["sitelogtypeid"];
            _siteLogEntry.SiteLogTypeCode = (string)dr["sitelogtypecode"];
            _siteLogEntry.SiteLogCategoryID = (int)dr["sitelogcategoryid"];
            _siteLogEntry.ApplicationID = (int)dr["applicationid"];
            _siteLogEntry.Message = DataHelper.CleanTextForJScript(dr["message"] as string);
            _siteLogEntry.LogDateTime = (DateTime)dr["logdatetime"];
            _siteLogEntry.ApplicationKey = dr["applicationkey"] as string;
            _siteLogEntry.SiteLogCategoryCode = dr["sitelogcategorycode"] as string;
        }

        public static void PopulateSiteLogTypeList(IDataReader dr, SiteLogEntry _siteLogEntry)
        {
            _siteLogEntry.SiteLogTypeID = (int)dr["sitelogtypeid"];
            _siteLogEntry.SiteLogTypeCode = dr["sitelogtypecode"] as string;
        }

        public static void PopulateSueetieDownloadList(SqlDataReader dr, SueetieDownload _sueetieDownload)
        {
            _sueetieDownload.DownloadID = (int)dr["downloadid"];
            _sueetieDownload.ContentID = (int)dr["contentid"];
            _sueetieDownload.ContentTypeID = (int)dr["contenttypeid"];
            _sueetieDownload.ContentTypeDescription = dr["contenttypedescription"] as string;
            _sueetieDownload.ApplicationID = (int)dr["applicationid"];
            _sueetieDownload.ApplicationKey = dr["applicationkey"] as string;
            _sueetieDownload.GroupID = (int)dr["groupid"];
            _sueetieDownload.GroupKey = dr["groupkey"] as string;
            _sueetieDownload.GroupName = dr["groupname"] as string;
            _sueetieDownload.MediaObjectID = (int)dr["mediaobjectid"];
            _sueetieDownload.MediaObjectTitle = dr["mediaobjecttitle"] as string;
            _sueetieDownload.AlbumID = (int)dr["albumid"];
            _sueetieDownload.AlbumTitle = dr["albumtitle"] as string;
            _sueetieDownload.GalleryID = (int)dr["galleryid"];
            _sueetieDownload.GalleryName = dr["galleryname"] as string;
            _sueetieDownload.DownloadUserID = (int)dr["downloaduserid"];
            _sueetieDownload.DownloadUserName = dr["downloadusername"] as string;
            _sueetieDownload.DownloadDisplayName = dr["downloaddisplayname"] as string;
            _sueetieDownload.DownloadEmail = dr["downloademail"] as string;
            _sueetieDownload.DownloadDateTime = DataHelper.DateOrNull(dr["downloaddatetime"].ToString());
            _sueetieDownload.InDownloadReport = (bool)dr["indownloadreport"];
            _sueetieDownload.TotalDownloads = (int)dr["totaldownloads"];
            _sueetieDownload.DateTimeLastDownload = DataHelper.DateOrNull(dr["datetimelastdownload"].ToString());
        }

        public static void PopulateSueetieSubscriberList(IDataReader dr, SueetieSubscriber _sueetieSubscriber)
        {
            _sueetieSubscriber.UserID = (int)dr["userid"];
            _sueetieSubscriber.Username = dr["username"] as string;
            _sueetieSubscriber.DisplayName = dr["displayname"] as string;
            _sueetieSubscriber.Email = dr["email"] as string;
        }

        public static void PopulateSueetieContentPartList(IDataReader dr, SueetieContentPart _sueetieContentPart)
        {
            _sueetieContentPart.ContentPartID = (int)dr["contentpartid"];
            _sueetieContentPart.ContentName = dr["contentname"] as string;
            _sueetieContentPart.PageKey = dr["pagekey"] as string;
            _sueetieContentPart.ContentPageID = (int)dr["contentpageid"];
            _sueetieContentPart.ContentPageGroupID = (int)dr["contentpagegroupid"];
            _sueetieContentPart.PageTitle = dr["pagetitle"] as string;
            _sueetieContentPart.LastUpdateDateTime = (DateTime)dr["lastupdatedatetime"];
            _sueetieContentPart.LastUpdateUserID = (int)dr["lastupdateuserid"];
            _sueetieContentPart.UserName = dr["username"] as string;
            _sueetieContentPart.Email = dr["email"] as string;
            _sueetieContentPart.DisplayName = dr["displayname"] as string;
            _sueetieContentPart.ContentText = dr["contenttext"] as string;
            _sueetieContentPart.PageSlug = dr["pageslug"] as string;
            _sueetieContentPart.Permalink = dr["permalink"] as string;
            _sueetieContentPart.ContentPageGroupTitle = dr["contentpagegrouptitle"] as string;
            _sueetieContentPart.ApplicationKey = dr["applicationkey"] as string;
            _sueetieContentPart.ApplicationID = (int)dr["applicationid"];
        }

        public static void PopulateSueetieContentPagePartList(IDataReader dr, SueetieContentPagePart _sueetieContentPagePart)
        {
            _sueetieContentPagePart.ContentPartID = (int)dr["contentpartid"];
            _sueetieContentPagePart.ContentName = dr["contentname"] as string;
            _sueetieContentPagePart.PageKey = dr["pagekey"] as string;
            _sueetieContentPagePart.ContentPageID = (int)dr["contentpageid"];
            _sueetieContentPagePart.ContentPageGroupID = (int)dr["contentpagegroupid"];
            _sueetieContentPagePart.PageTitle = dr["pagetitle"] as string;
            _sueetieContentPagePart.LastUpdateDateTime = (DateTime)dr["lastupdatedatetime"];
            _sueetieContentPagePart.LastUpdateUserID = (int)dr["lastupdateuserid"];
            _sueetieContentPagePart.UserName = dr["username"] as string;
            _sueetieContentPagePart.Email = dr["email"] as string;
            _sueetieContentPagePart.DisplayName = dr["displayname"] as string;
            _sueetieContentPagePart.ContentText = dr["contenttext"] as string;
            _sueetieContentPagePart.ContentID = (int)dr["contentid"];
            _sueetieContentPagePart.ContentTypeID = (int)dr["contenttypeid"];
            _sueetieContentPagePart.Permalink = dr["permalink"] as string;
            _sueetieContentPagePart.ApplicationKey = dr["applicationkey"] as string;
            _sueetieContentPagePart.PageSlug = dr["pageslug"] as string;
            _sueetieContentPagePart.ContentPageGroupTitle = dr["contentpagegrouptitle"] as string;
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

        public static void PopulateSueetieContentPageGroupList(IDataReader dr, SueetieContentPageGroup _sueetieContentPageGroup)
        {
            _sueetieContentPageGroup.ContentPageGroupID = (int)dr["contentpagegroupid"];
            _sueetieContentPageGroup.ApplicationID = (int)dr["applicationid"];
            _sueetieContentPageGroup.ApplicationKey = dr["applicationkey"] as string;
            _sueetieContentPageGroup.ContentPageGroupTitle = dr["contentpagegrouptitle"] as string;
            _sueetieContentPageGroup.EditorRoles = dr["editorroles"] as string;
            _sueetieContentPageGroup.IsActive = (bool)dr["isactive"];
        }

        public static void PopulateSueetieTagList(IDataReader dr, SueetieTag _sueetieTag)
        {
            _sueetieTag.TagMasterID = (int)dr["tagmasterid"];
            _sueetieTag.Tag = dr["tag"] as string;
            _sueetieTag.TagCount = (int)dr["tagcount"];
        }


        public static void PopulateSueetieCalendarEventList(IDataReader dr, SueetieCalendarEvent _sueetieCalendarEvent)
        {
            _sueetieCalendarEvent.EventID = (int)dr["eventid"];
            _sueetieCalendarEvent.EventGuid = DataHelper.GetGuid(dr, "eventguid");
            _sueetieCalendarEvent.CalendarID = (int)dr["calendarid"];
            _sueetieCalendarEvent.EventTitle = dr["eventtitle"] as string;
            _sueetieCalendarEvent.EventDescription = dr["eventdescription"] as string;
            _sueetieCalendarEvent.StartDateTime = DataHelper.DateOrNull(dr["startdatetime"].ToString());
            _sueetieCalendarEvent.EndDateTime = DataHelper.DateOrNull(dr["enddatetime"].ToString());
            _sueetieCalendarEvent.AllDayEvent = (bool)dr["alldayevent"];
            _sueetieCalendarEvent.RepeatEndDate = DataHelper.DateOrNull(dr["repeatenddate"].ToString());
            _sueetieCalendarEvent.IsActive = (bool)dr["isactive"];
            _sueetieCalendarEvent.SourceContentID = (int)dr["sourcecontentid"];
            _sueetieCalendarEvent.ContentID = (int)dr["contentid"];
            _sueetieCalendarEvent.ContentTypeID = (int)dr["contenttypeid"];
            _sueetieCalendarEvent.UserID = (int)dr["userid"];
            _sueetieCalendarEvent.UserName = dr["username"] as string;
            _sueetieCalendarEvent.DisplayName = dr["displayname"] as string;
            _sueetieCalendarEvent.CreatedDateTIme = DataHelper.DateOrNull(dr["createddatetime"].ToString());
            _sueetieCalendarEvent.CreatedBy = (int)dr["createdby"];
            _sueetieCalendarEvent.Url = dr["url"] as string;
            _sueetieCalendarEvent.CalendarTitle = dr["calendartitle"] as string;
            _sueetieCalendarEvent.CalendarDescription = dr["calendardescription"] as string;
            _sueetieCalendarEvent.CalendarUrl = dr["calendarurl"] as string;
        }

        public static void PopulateSueetieCalendarList(IDataReader dr, SueetieCalendar _sueetieCalendar)
        {
            _sueetieCalendar.CalendarID = (int)dr["calendarid"];
            _sueetieCalendar.CalendarTitle = dr["calendartitle"] as string;
            _sueetieCalendar.CalendarDescription = dr["calendardescription"] as string;
            _sueetieCalendar.CalendarUrl = dr["calendarurl"] as string;
            _sueetieCalendar.IsActive = (bool)dr["isactive"];
        }

        public static void PopulateCrawlerAgentList(IDataReader dr, CrawlerAgent _crawlerAgent)
        {
            _crawlerAgent.AgentID = (int)dr["agentid"];
            _crawlerAgent.AgentExcerpt = dr["agentexcerpt"] as string;
            _crawlerAgent.IsBlocked = (bool)dr["isblocked"];
            _crawlerAgent.DateEntered = (DateTime)dr["dateentered"];
        }

        public static void PopulateFilteredUrlList(IDataReader dr, string _string)
        {
            _string = dr["urlexcerpt"] as string;
        }

        public static void PopulatePageRuleList(IDataReader dr, PageRule pageRule)
        {
            pageRule.PageRuleID = (int)dr["pageruleid"];
            pageRule.UrlExcerpt = dr["urlexcerpt"] as string;
            pageRule.UrlFinal = dr["urlfinal"] as string;
            pageRule.PageTitle = dr["pagetitle"] as string;
            pageRule.IsEqual = (bool)dr["isequal"];
        }

        public static void PopulateSueetieTagMasterList(IDataReader dr, SueetieTagMaster sueetieTagMaster)
        {
            sueetieTagMaster.TagID = (int)dr["tagid"];
            sueetieTagMaster.TagMasterID = (int)dr["tagmasterid"];
            sueetieTagMaster.ContentID = (int)dr["contentid"];
            sueetieTagMaster.Tag = dr["tag"] as string;
            sueetieTagMaster.CreatedBy = (int)dr["createdby"];
            sueetieTagMaster.CreatedDateTime = DataHelper.DateOrNull(dr["createddatetime"].ToString());
            sueetieTagMaster.IsActive = (bool)dr["isactive"];
            sueetieTagMaster.ContentTypeID = (int)dr["contenttypeid"];
            sueetieTagMaster.ApplicationID = (int)dr["applicationid"];
        }

        public static void PopulateSueetieMediaDirectoryList(IDataReader dr, SueetieMediaDirectory sueetieMediaDirectory)
        {
            sueetieMediaDirectory.MediaObjectId = (int)dr["mediaobjectid"];
            sueetieMediaDirectory.ContentID = (int)dr["contentid"];
            sueetieMediaDirectory.GalleryId = (int)dr["galleryid"];
            sueetieMediaDirectory.AlbumId = (int)dr["albumid"];
            sueetieMediaDirectory.AlbumParentId = (int)dr["albumparentid"];
            sueetieMediaDirectory.DirectoryName = dr["directoryname"] as string;
            sueetieMediaDirectory.SueetieAlbumPath = dr["sueetiealbumpath"] as string;
            sueetieMediaDirectory.ThumbnailFilename = dr["thumbnailfilename"] as string;
            sueetieMediaDirectory.OptimizedFilename = dr["optimizedfilename"] as string;
            sueetieMediaDirectory.OriginalFilename = dr["originalfilename"] as string;
            sueetieMediaDirectory.ContentTypeID = (int)dr["contenttypeid"];
            sueetieMediaDirectory.ApplicationID = (int)dr["applicationid"];
            sueetieMediaDirectory.ApplicationKey = dr["applicationkey"] as string;
        }
    }
}