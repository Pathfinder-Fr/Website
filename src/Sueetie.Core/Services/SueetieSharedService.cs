using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using Sueetie.Core;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.Security;
using System.Web;

namespace Sueetie.Core
{
    [ServiceContract(Namespace = "Sueetie.Core")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SueetieSharedService
    {
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

        #region Tags and Categories

        [OperationContract]
        public string ProcessTags(int _itemID, int _contentID, int _contentTypeID, string _tags)
        {
            if (string.IsNullOrEmpty(_tags))
                return SueetieLocalizer.GetString("no_tags_entered");
            string pipedTags = SueetieTags.PipedTags(_tags);
            if (_contentTypeID == (int)SueetieContentType.CMSPage)
            {
                SueetieTagEntry sueetieTagEntry = new SueetieTagEntry
                {
                    ItemID = _itemID,
                    ContentID = _contentID,
                    ContentTypeID = _contentTypeID,
                    UserID = SueetieContext.Current.User.UserID,
                    Tags = pipedTags
                };
                SueetieContentParts.EnterContentPageTags(sueetieTagEntry);
            }

            return SueetieTags.TagUrls(pipedTags);
        }

        #endregion
    }

}
