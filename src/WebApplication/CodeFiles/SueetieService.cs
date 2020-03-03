using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Sueetie.Core;
using System.Collections.Generic;
using System.Web.UI;
using Sueetie.Blog;
using System.Web.Security;
using Sueetie.Wiki;
using Sueetie.AddonPack;
using Sueetie.Analytics;


[ServiceContract(Namespace = "Sueetie.Web")]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class SueetieService
{
    #region Constants
    private const string EMPTYGUID = "00000000-0000-0000-0000-000000000000";
    #endregion

    #region Hello World

    [OperationContract]
    public string HelloWorld(string name)
    {
        return "Hello " + name + ". Time is: " + DateTime.Now.ToString();
    }

    #endregion

    #region Following and Favorites

    [OperationContract]
    public string BlogAuthorFollow(int userID, string postGuid)
    {
        SueetieBlogPost sueetieBlogPost = SueetieBlogs.GetSueetieBlogPost(postGuid);

        if (userID > 0)
        {
            if (sueetieBlogPost.SueetiePostID > 0)
            {
                string result = "You are now following " + sueetieBlogPost.DisplayName;
                SueetieFollow sueetieFollow = new SueetieFollow
                {
                    FollowerUserID = userID,
                    FollowingUserID = sueetieBlogPost.UserID,
                    ContentIDFollowed = sueetieBlogPost.SueetiePostID
                };

                if (sueetieFollow.FollowerUserID == sueetieFollow.FollowingUserID)
                    result = "Sorry, you cannot follow yourself...";
                else
                {
                    int followID = SueetieUsers.FollowUser(sueetieFollow);
                    if (followID < 0)
                        result = "You are already following " + sueetieBlogPost.DisplayName;

                    SueetieLogs.LogUserEntry(UserLogCategoryType.Following, sueetieBlogPost.UserID, userID);
                }
                return result;
            }
            else
            {
                return "Sorry, we added following after this post was written. Please use a more current post to follow this member.";
            }
        }
        else
            return "Please login or become a member to follow this person.";
    }

    [OperationContract]
    public string BlogCommenterFollow(int userID, string postGuid)
    {
        SueetieBlogComment sueetieBlogComment = SueetieBlogs.GetSueetieBlogComment(postGuid);

        if (userID > 0)
        {
            if (sueetieBlogComment.SueetieCommentID > 0)
            {
                string result = "You are now following " + sueetieBlogComment.DisplayName;
                SueetieFollow sueetieFollow = new SueetieFollow
                {
                    FollowerUserID = userID,
                    FollowingUserID = sueetieBlogComment.UserID,
                    ContentIDFollowed = sueetieBlogComment.SueetieCommentID
                };

                if (sueetieBlogComment.UserID > 0)
                {
                    if (sueetieFollow.FollowerUserID == sueetieFollow.FollowingUserID)
                        result = "Sorry, you cannot follow yourself...";
                    else
                    {
                        int followID = SueetieUsers.FollowUser(sueetieFollow);
                        if (followID < 0)
                            result = "You are already following " + sueetieBlogComment.DisplayName;
                        else
                            SueetieLogs.LogUserEntry(UserLogCategoryType.Following, sueetieBlogComment.UserID, userID);
                    }
                }
                else
                    result = "Sorry, " + sueetieBlogComment.Author + " is not a member and thus cannot be followed.";
                return result;
            }
            else
            {
                return "Sorry, we added following after this comment was posted. Please use a more current comment to follow this member.";
            }
        }
        else
            return "Please login or become a member to follow this person.";
    }

    [OperationContract]
    public string BlogFavePost(int userID, string postGuid)
    {
        SueetieBlogPost sueetieBlogPost = SueetieBlogs.GetSueetieBlogPost(postGuid);

        if (userID > 0)
        {
            if (sueetieBlogPost.SueetiePostID > 0)
            {
                string result = "You have tagged " + sueetieBlogPost.Title + " as a favorite!";
                UserContent userContent = new UserContent
                {
                    ContentID = sueetieBlogPost.ContentID,
                    UserID = userID
                };

                int favoriteID = SueetieUsers.CreateFavorite(userContent);
                if (favoriteID < 0)
                    result = "You already tagged this post as a favorite.";

                return result;
            }
            else
            {
                return "Sorry, we added favorites after this post was published. Please consider tagging more recent posts as favorites.";
            }
        }
        else
            return "Please login or become a member to tag this post as a favorite";
    }

    [OperationContract]
    public string BlogFaveComment(int userID, string postGuid)
    {
        SueetieBlogComment sueetieBlogComment = SueetieBlogs.GetSueetieBlogComment(postGuid);

        if (userID > 0)
        {
            if (sueetieBlogComment.SueetieCommentID > 0)
            {
                string result = "You tagged this comment by " + sueetieBlogComment.Author + " as a favorite!";
                UserContent userContent = new UserContent
                {
                    ContentID = sueetieBlogComment.ContentID,
                    UserID = userID
                };

                int favoriteID = SueetieUsers.CreateFavorite(userContent);
                if (favoriteID < 0)
                    result = "You already tagged this comment as a favorite.";

                return result;
            }
            else
            {
                return "Sorry, we added favorites after this comment was written. Please consider tagging more recent comments as favorites.";
            }
        }
        else
            return "Please login or become a member to tag this comment as a favorite";

    }

    [OperationContract]
    public string ForumFaveMessage(int userID, int messageID, int applicationID)
    {
        SueetieForumContent sueetieForumContent = new SueetieForumContent
        {
            MessageID = messageID,
            ContentTypeID = (int)SueetieContentType.ForumMessage,
            ApplicationID = applicationID
        };

        SueetieForumMessage sueetieForumMessage = SueetieForums.GetSueetieForumMessage(sueetieForumContent);

        if (userID > 0)
        {
            if (sueetieForumMessage.ContentID > 0)
            {
                string result = "You tagged this message by " + sueetieForumMessage.DisplayName + " as a favorite!";
                UserContent userContent = new UserContent
                {
                    ContentID = sueetieForumMessage.ContentID,
                    UserID = userID
                };

                int favoriteID = SueetieUsers.CreateFavorite(userContent);
                if (favoriteID < 0)
                    result = "You already tagged this forums message as a favorite.";

                return result;
            }
            else
            {
                return "Dang it. This forums message cannot be tagged as a favorite for some reason.  Sorry.";
            }
        }
        else
            return "Please login or become a member to tag this forums message as a favorite";
    }

    [OperationContract]
    public List<Triplet> GetFollowList(int userid, int followTypeID)
    {
        // 0 - following, 1 - followers, 2 - friends
        List<Triplet> followList = new List<Triplet>();
        if (userid == -2)
            userid = SueetieContext.Current.User.UserID;

        var q = from c in SueetieUsers.GetSueetieFollowList(userid, followTypeID)
                select new Triplet(c.UserID, c.DisplayName, c.ThumbnailFilename);
        followList.AddRange(q.ToList());
        return followList;
    }

    [OperationContract]
    public string GetDisplayName(int userid)
    {
        return SueetieUsers.GetUser(userid).DisplayName;
    }

    [OperationContract]
    public List<Pair> ProfileUserFollow(int userID, int profileUserID, bool stopFollowing)
    {

        if (userID > 0)
        {
            SueetieUser sueetieFollowingUser = SueetieUsers.GetUser(profileUserID);
            string result = "You are now following " + sueetieFollowingUser.DisplayName;
            SueetieFollow sueetieFollow = new SueetieFollow
            {
                FollowerUserID = userID,
                FollowingUserID = profileUserID,
                ContentIDFollowed = -1
            };

            if (sueetieFollow.FollowerUserID == sueetieFollow.FollowingUserID)
                result = "Sorry, you cannot follow yourself...";
            else
            {
                if (!stopFollowing)
                {
                    int followID = SueetieUsers.FollowUser(sueetieFollow);
                }
                else
                {
                    SueetieUsers.UnFollowUser(sueetieFollow);
                    result = "You are no longer following " + sueetieFollowingUser.DisplayName;
                }
            }
            List<Pair> followResult = new List<Pair>();
            followResult.Add(new Pair(stopFollowing, result));

            return followResult;
        }
        else
        {
            List<Pair> followResult = new List<Pair>();
            followResult.Add(new Pair(stopFollowing, "Please login or become a member to follow this person."));

            return followResult;
        }
    }

    [OperationContract]
    public List<FavoriteContent> GetFavoriteContent(int userID, int contentTypeID, int groupID, bool isRestricted)
    {
        int _userID = SueetieContext.Current.User.UserID;
        if (userID == -2)
            userID = _userID;

        var favorites = from f in SueetieUsers.GetFavoriteContentList(userID)
                        orderby f.Title
                        where f.ContentTypeID == contentTypeID && f.GroupID == groupID
                        select f;
        if (isRestricted)
            return favorites.Where(f => f.IsRestricted == false).ToList();
        else
            return favorites.ToList();
    }

    [OperationContract]
    public UserContent DeleteFavorite(int favoriteID)
    {
        UserContent userContent = SueetieUsers.DeleteFavorite(favoriteID);
        return userContent;
    }

    #endregion

    #region Recent Activity

    [OperationContract]
    public List<SueetieBlogComment> GetRecentComments(int numRecords, int userID, int applicationID, bool isRestricted)
    {

        ContentQuery contentQuery = new ContentQuery
        {
            NumRecords = numRecords,
            UserID = userID,
            ContentTypeID = (int)SueetieContentType.BlogComment,
            GroupID = -1,
            ApplicationID = applicationID,
            IsRestricted = isRestricted,
            TruncateText = true
        };

        List<SueetieBlogComment> _sueetieBlogComments = SueetieBlogs.GetSueetieBlogCommentList(contentQuery);
        foreach (SueetieBlogComment msg in _sueetieBlogComments)
        {
            msg.Comment = DataHelper.TruncateText(msg.Comment, SueetieConfiguration.Get().Core.TruncateTextCount);
        }
        return _sueetieBlogComments;

    }

    [OperationContract]
    public List<SueetieForumMessage> GetRecentForumMessages(int numRecords, int userID, int applicationID, bool isRestricted)
    {

        ContentQuery contentQuery = new ContentQuery
        {
            NumRecords = numRecords,
            UserID = userID,
            ContentTypeID = (int)SueetieContentType.ForumMessage,
            GroupID = -1,
            ApplicationID = applicationID,
            IsRestricted = isRestricted,
            TruncateText = true
        };

        List<SueetieForumMessage> _sueetieForumMessages = SueetieForums.GetSueetieForumMessageList(contentQuery);
        foreach (SueetieForumMessage msg in _sueetieForumMessages)
        {
            msg.Message = DataHelper.TruncateText(msg.Message, SueetieConfiguration.Get().Core.TruncateTextCount);
        }
        return _sueetieForumMessages;

    }

    #endregion

    #region SlideShows

    [OperationContract]
    public List<SueetieMediaObject> GetSlideShowImages()
    {
        List<SueetieMediaObject> _SlideshowImages = new List<SueetieMediaObject>() 
         {
             new SueetieMediaObject {
                 Permalink = "/images/dev/img1.jpg",
                 MediaObjectTitle = "Image One",
                 MediaObjectUrl = "/images/dev/img1s.jpg"
             },
             new SueetieMediaObject {
                 Permalink = "/images/dev/img2.jpg",
                  MediaObjectTitle = "Image Two",
                 MediaObjectUrl = "/images/dev/img2s.jpg"
             },
             new SueetieMediaObject {
                 Permalink = "/images/dev/img3.jpg",
                  MediaObjectTitle = "Image Three",
                 MediaObjectUrl = "/images/dev/img3s.jpg"
             },
             new SueetieMediaObject {
                 Permalink = "/images/dev/img4.jpg",
                  MediaObjectTitle = "Image Three",
                 MediaObjectUrl = "/images/dev/img4s.jpg"
             },
             new SueetieMediaObject {
                 Permalink = "/images/dev/img5.jpg",
                  MediaObjectTitle = "Image Three",
                 MediaObjectUrl = "/images/dev/img5s.jpg"
             }
         };

        return _SlideshowImages;
    }
    #endregion

    #region Blogs

    [OperationContract]
    public string CreateBlogAdmin(string _userIDs)
    {
        string[] userIDs = _userIDs.Split(',');
        string newadmins = string.Empty;
        foreach (string userID in userIDs)
        {
            if (!string.IsNullOrEmpty(userID))
            {
                SueetieUser sueetieUser = SueetieUsers.GetUser(Convert.ToInt32(userID));
                SueetieBlogs.CreateBlogAdmin(sueetieUser);
                SueetieBlogUtils.CreateProfile(sueetieUser, "blog");
                if (!sueetieUser.IsBlogAdministrator)
                    Roles.AddUserToRole(sueetieUser.UserName, "BlogAdministrator");
                newadmins += sueetieUser.DisplayName + ",";
            }
        }

        return "The following are now site blog administrators: " + DataHelper.CommaTrim(newadmins);
    }

    #endregion

    #region Wikis

    [OperationContract]
    public string CreateWikiUsers(string _userIDs)
    {
        string[] userIDs = _userIDs.Split(',');
        string newusers = string.Empty;
        foreach (string userID in userIDs)
        {
            if (!string.IsNullOrEmpty(userID))
            {
                SueetieUser sueetieUser = SueetieUsers.GetUser(Convert.ToInt32(userID));
                WikiUsers.AddUser(sueetieUser.UserName, sueetieUser.Email, null, sueetieUser.DisplayName);
                newusers += sueetieUser.DisplayName + ",";
            }
        }

        return "The following now have wiki accounts: " + DataHelper.CommaTrim(newusers);
    }

    #endregion

    #region Tags and Categories

    [OperationContract]
    public string ProcessTags(int _itemID, int _contentID, int _contentTypeID, string _tags)
    {
        if (string.IsNullOrEmpty(_tags))
            return SueetieLocalizer.GetString("no_tags_entered");
        string pipedTags = SueetieTags.PipedTags(_tags);
        SueetieTagEntry sueetieTagEntry = new SueetieTagEntry
        {
            ItemID = _itemID,
            ContentID = _contentID,
            ContentTypeID = _contentTypeID,
            UserID = SueetieContext.Current.User.UserID,
            Tags = pipedTags
        };

        if (_contentTypeID == (int)SueetieContentType.CMSPage)
            SueetieContentParts.EnterContentPageTags(sueetieTagEntry);
        else if (SueetieCommon.IsMediaObject(_contentTypeID))
            SueetieMedia.EnterMediaObjectTags(sueetieTagEntry);
        else if (SueetieCommon.IsMediaAlbum(_contentTypeID))
            SueetieMedia.EnterMediaAlbumTags(sueetieTagEntry);
        else if (_contentTypeID == (int)SueetieContentType.WikiPage)
            SueetieWikis.EnterWikiPageTags(sueetieTagEntry);
        else if (_contentTypeID == (int)SueetieContentType.ForumTopic)
            SueetieForums.EnterForumTopicTags(sueetieTagEntry);

        SueetieTags.ClearSueetieTagCache();
        return SueetieTags.TagUrls(pipedTags);
    }

    [OperationContract]
    public List<SueetieTag> GetCloudTags(int applicationTypeID)
    {

        List<SueetieTag> sueetieTags = SueetieTags.GetSueetieTagCloudList(new SueetieTagQuery { ApplicationTypeID = applicationTypeID, IsRestricted = true });

        int maxTagCount = 0;
        foreach (SueetieTag _tag in sueetieTags)
        {
            if (_tag.TagCount > maxTagCount)
                maxTagCount = _tag.TagCount;
        }

        foreach (SueetieTag _tag in sueetieTags)
        {
            _tag.TagPlus = DataHelper.PrepareTag(_tag.Tag);
            _tag.WeightedClass = SueetieTags.TagWeightClass(_tag.TagCount, maxTagCount);
        }
        return sueetieTags;

    }

    #endregion

    #region Calendars

    [OperationContract]
    public string AddCalendarEvent(string id, string title, string description, string start, string end, string allDay, string endRepeat, int calendarID, string url, int sourceContentID)
    {
        int _currentSueetieUserID = SueetieContext.Current.User.UserID;
        SueetieCalendarEvent sueetieCalendarEvent = new SueetieCalendarEvent
        {
            EventGuid = new Guid(id),
            EventTitle = title,
            EventDescription = description,
            StartDateTime = SueetieCalendars.ConvertJsonDate(start),
            EndDateTime = SueetieCalendars.ConvertJsonDate(end),
            CalendarID = calendarID,
            AllDayEvent = DataHelper.StringToBool(allDay),
            RepeatEndDate = DataHelper.SafeMinDate(endRepeat),
            SourceContentID = 0,
            CreatedBy = _currentSueetieUserID
        };
        if (string.IsNullOrEmpty(url))
        {
            sueetieCalendarEvent.SourceContentID = sourceContentID;
            sueetieCalendarEvent.Url = url;
        }

        if (!string.IsNullOrEmpty(endRepeat))
            try
            {
                Convert.ToDateTime(endRepeat);
            }
            catch { }

        int eventID = SueetieCalendars.CreateSueetieCalendarEvent(sueetieCalendarEvent);


        SueetieContent sueetieContent = new SueetieContent
        {
            SourceID = eventID,
            ContentTypeID = (int)SueetieContentType.CalendarEvent,
            Permalink = "na",
            ApplicationID = (int)SueetieApplicationType.Unknown,
            UserID = _currentSueetieUserID
        };
        int contentID = SueetieCommon.AddSueetieContent(sueetieContent);

        if (SueetieContext.Current.User.IsContentAdministrator)
        {
            SueetieLogs.LogUserEntry(UserLogCategoryType.CalendarEvent, contentID, _currentSueetieUserID);
        }

        SueetieCalendars.ClearSueetieCalendarEventListCache(calendarID);

        return "<b>NEW EVENT ITEM:</b> " + id + "\n\n<b>TITLE:</b> " + title + "\n<b>DESCRIPTION:</b> " + description + " \n<b>START DATETIME:</b> " + SueetieCalendars.ConvertJsonDate(start).ToString() +
            "\n<b>END DATETIME:</b> " + SueetieCalendars.ConvertJsonDate(end).ToString() + "\n<b>END REPEAT DATE:</b> " + endRepeat + "\n<b>ALL DAY EVENT:</b> " +
            allDay + "\n<b>CALENDAR ID:</b> " + calendarID;
    }

    [OperationContract]
    public void DeleteCalendarEvent(string id, int calendarID)
    {
        SueetieCalendars.DeleteCalendarEvent(id);
        SueetieCalendars.ClearSueetieCalendarEventListCache(calendarID);
    }

    [OperationContract]
    public string EditCalendarEvent(string id, string title, string description, string start, string end, string allDay, string endRepeat, int calendarID, int daysDiff)
    {
        SueetieCalendarEvent sueetieCalendarEvent = new SueetieCalendarEvent
        {
            EventGuid = new Guid(id),
            EventTitle = title,
            EventDescription = description,
            StartDateTime = SueetieCalendars.ConvertJsonDate(start),
            EndDateTime = SueetieCalendars.ConvertJsonDate(end),
            AllDayEvent = DataHelper.StringToBool(allDay),
            RepeatEndDate = DataHelper.SafeMinDate(endRepeat),
            CalendarID = calendarID,
            IsActive = true
        };

        if (!DataHelper.IsMinDate(sueetieCalendarEvent.RepeatEndDate))
            sueetieCalendarEvent.RepeatEndDate = sueetieCalendarEvent.RepeatEndDate.AddDays(daysDiff);


        SueetieCalendars.UpdateSueetieCalendarEvent(sueetieCalendarEvent);
        SueetieCalendars.ClearSueetieCalendarEventListCache(calendarID);

        return "<b>RESCHEDULED EVENT:</b> " + id + "\n\n<b>TITLE:</b> " + title + "\n<b>DESCRIPTION:</b> " + description + " \n<b>START DATETIME:</b> " + SueetieCalendars.ConvertJsonDate(start).ToString() +
            "\n<b>END DATETIME:</b> " + SueetieCalendars.ConvertJsonDate(end).ToString() + "\n<b>END REPEAT DATE:</b> " + endRepeat + "\n<b>ALL DAY EVENT:</b> " +
            allDay + "\n<b>CALENDAR ID:</b> " + calendarID;
    }

    [OperationContract]
    public string CreateUpdateCalendarEvent(int sourceContentID, string title, string description, string startDate, string endDate, string endRepeatDate, string startTime, string endTime, string url)
    {
        DateTime _startDate = DateTime.Parse(startDate).AddMinutes(EventTime(startTime));
        DateTime _endDate = DateTime.Parse(startDate).AddMinutes(EventTime(endTime));

        // Calendar Control uses Default Calendar Only in v2.0
        SueetieCalendarEvent _sueetieCalendarEvent = SueetieCalendars.GetSueetieCalendarEvent(sourceContentID, 1);
        if (_sueetieCalendarEvent == null)
        {
            _sueetieCalendarEvent = new SueetieCalendarEvent
            {
                EventGuid = new Guid(EMPTYGUID)
            };
        }

        int _currentSueetieUserID = SueetieContext.Current.User.UserID;

        if (!string.IsNullOrEmpty(endDate))
            _endDate = DateTime.Parse(endDate).AddMinutes(EventTime(endTime));

        SueetieCalendarEvent sueetieCalendarEvent = new SueetieCalendarEvent
        {
            EventGuid = _sueetieCalendarEvent.EventGuid,
            EventTitle = title,
            EventDescription = description,
            StartDateTime = _startDate,
            EndDateTime = _endDate,
            AllDayEvent = _startDate.Hour == 0 ? true : false,
            RepeatEndDate = DataHelper.SafeMinDate(endRepeatDate),
            CalendarID = 1,
            CreatedBy = _currentSueetieUserID,
            Url = url,
            SourceContentID = sourceContentID,
            IsActive = true
        };

        string _result = SueetieLocalizer.GetString("calendar_created_success");

        if (sueetieCalendarEvent.EventGuid != new Guid(EMPTYGUID))
        {
            SueetieCalendars.UpdateSueetieCalendarEvent(sueetieCalendarEvent);
            _result = SueetieLocalizer.GetString("calendar_updated_success");
        }
        else
        {
            sueetieCalendarEvent.EventGuid = Guid.NewGuid();
            int eventID = SueetieCalendars.CreateSueetieCalendarEvent(sueetieCalendarEvent);

            SueetieContent sueetieContent = new SueetieContent
            {
                SourceID = eventID,
                ContentTypeID = (int)SueetieContentType.CalendarEvent,
                Permalink = url,
                ApplicationID = (int)SueetieApplicationType.Unknown,
                UserID = _currentSueetieUserID
            };
            int contentID = SueetieCommon.AddSueetieContent(sueetieContent);

            if (SueetieContext.Current.User.IsContentAdministrator)
            {
                SueetieLogs.LogUserEntry(UserLogCategoryType.CalendarEvent, contentID, _currentSueetieUserID);
            }
        }

        SueetieCalendars.ClearSueetieCalendarEventListCache(1);
        return _result;

    }

    private double EventTime(string _time)
    {
        double minutes = 0;
        if (!string.IsNullOrEmpty(_time))
        {
            try
            {
                int hourMinutes = DateTime.Parse(_time).Hour * 60;
                minutes = DateTime.Parse(_time).Minute + hourMinutes;
            }
            catch
            { }
        }
        return minutes;
    }
    #endregion

    #region Registration

    [OperationContract]
    public bool IsNewUsername(string username)
    {
        if (SueetieUsers.IsNewUsername(username))
            return true;
        else
            return false;
    }

    [OperationContract]
    public bool IsNewEmailAddress(string email)
    {
        if (SueetieUsers.IsNewEmailAddress(email))
            return true;
        else
            return false;
    }

    [OperationContract]
    public bool IsNewDisplayName(string displayName)
    {
        if (!string.IsNullOrEmpty(displayName))
            return SueetieUsers.IsNewDisplayName(displayName);
        else
            return true;
    }

    #endregion

    #region AddonPack

    #region Access Control

    [OperationContract]
    public string DeleteRequestAgents(string reportLogGuid)
    {
        int recordsDeleted = AddonPackCommon.DeleteRequestAgents(reportLogGuid);
        return recordsDeleted.ToString() + " records deleted";
    }

    [OperationContract]
    public string DeleteRequestIPs(string reportLogGuid)
    {
        int recordsDeleted = AddonPackCommon.DeleteRequestIPs(reportLogGuid);
        return recordsDeleted.ToString() + " records deleted";
    }

    #endregion

    #region Forum Answers

    [OperationContract]
    public string GetAnswers(int topicID)
    {
        return ForumAnswers.GetTopicAnswerMessageIDs(topicID);
    }

    [OperationContract]
    public int RecordAnswer(int topicID, int messageID, int userID, int applicationID)
    {
        int resultID = ProcessAddAnswer(topicID, messageID, userID, applicationID, true);
        if (resultID > 0)
            return messageID;
        else
            return -1;
    }

    [OperationContract]
    public int UndoAnswer(int messageID, int userID)
    {
        ProcessRemoveAnswer(messageID, userID);
        return messageID;
    }

    private void ProcessRemoveAnswer(int messageID, int userID)
    {
        ForumAnswerKey _forumAnswerKey = new ForumAnswerKey
        {
            MessageID = messageID,
            UserID = userID
        };
        ForumAnswers.RemoveForumAnswer(_forumAnswerKey);
        ForumAnswers.ClearForumAnswerKeyListCache();
    }

    private int ProcessAddAnswer(int topicID, int messageID, int userID, int applicationID, bool isFirst)
    {
        ForumAnswerKey _forumAnswerKey = new ForumAnswerKey
        {
            IsFirst = isFirst,
            MessageID = messageID,
            TopicID = topicID,
            UserID = userID
        };

        int answerID = ForumAnswers.RecordAnswer(_forumAnswerKey);

        if (answerID > 0)
        {
            string appKey = SueetieCommon.GetSueetieApplication(applicationID).ApplicationKey;

            SueetieContent _sueetieContent = new SueetieContent
            {
                SourceID = answerID,
                ApplicationID = applicationID,
                ContentTypeID = (int)SueetieContentType.ForumAnswer,
                Permalink = "/" + appKey + "/default.aspx?g=posts&m=" + messageID + "#post" + messageID,
                UserID = userID
            };
            int contentID = SueetieCommon.AddSueetieContent(_sueetieContent);

            SueetieLogs.LogUserEntry(UserLogCategoryType.ForumAnswer, contentID, userID);
            ForumAnswers.ClearForumAnswerKeyListCache();
        }
        return answerID;
    }

    [OperationContract]
    public int ConfirmAnswer(int topicID, int messageID, int userID, int applicationID)
    {
        int resultID = ProcessAddAnswer(topicID, messageID, userID, applicationID, false);
        if (resultID > 0)
            return messageID;
        else
            return -1;
    }


    #endregion

    #region Media Sets

    [OperationContract]
    public void AddMediaSetItem(int contentID, int mediaSetID, int userID)
    {
        MediaSetObjectKey _mediaSetObjectKey = new MediaSetObjectKey
        {
            ContentID = contentID,
            MediaSetID = mediaSetID,
            MediaSetUserID = userID
        };
        MediaSets.AddMediaSetObject(_mediaSetObjectKey);
        MediaSets.ClearMediaSetObjectListCache(mediaSetID);
    }

    [OperationContract]
    public void DeleteMediaSetItem(int contentID, int mediaSetID, int userID)
    {
        MediaSetObjectKey _mediaSetObjectKey = new MediaSetObjectKey
        {
            ContentID = contentID,
            MediaSetID = mediaSetID,
            MediaSetUserID = userID
        };
        MediaSets.RemoveMediaSetObject(_mediaSetObjectKey);
        MediaSets.ClearMediaSetObjectListCache(mediaSetID);
    }

    [OperationContract]
    public List<MediaSetObject> GetMediaSet(int mediaSetID, int userID)
    {
        return MediaSets.GetMediaSetObjectList(mediaSetID);
    }

    #endregion

    #endregion


}

