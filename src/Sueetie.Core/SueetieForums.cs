// -----------------------------------------------------------------------
// <copyright file="SueetieForums.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System.Collections.Generic;
    using System.Linq;

    public static class SueetieForums
    {
        public static int AddForumTopic(SueetieForumContent sueetieForumContent)
        {
            var provider = SueetieDataProvider.LoadProvider();
            var contentId = provider.AddForumTopic(sueetieForumContent);
            ClearForumTopicListCache(sueetieForumContent);
            return contentId;
        }

        public static int AddForumMessage(SueetieForumContent sueetieForumContent)
        {
            var provider = SueetieDataProvider.LoadProvider();
            var contentId = provider.AddForumMessage(sueetieForumContent);
            ClearForumMessageListCache(sueetieForumContent);
            return contentId;
        }

        public static SueetieForumTopic GetSueetieForumTopic(SueetieForumContent sueetieForumContent)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetSueetieForumTopic(sueetieForumContent);
        }

        public static SueetieForumMessage GetSueetieForumMessage(SueetieForumContent sueetieForumContent)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetSueetieForumMessage(sueetieForumContent);
        }

        public static SueetieForumTopic GetSueetieForumTopic(int topicId)
        {
            var content = new SueetieForumContent
            {
                TopicID = topicId,
                ApplicationID = 2,
                ContentTypeID = (int)SueetieContentType.ForumTopic
            };
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetSueetieForumTopic(content);
        }

        public static List<SueetieForumTopic> GetSueetieForumTopicList(ContentQuery contentQuery)
        {
            var provider = SueetieDataProvider.LoadProvider();

            var key = ForumTopicListCacheKey(contentQuery);

            var cachedSueetieForumTopics = SueetieCache.Current[key] as List<SueetieForumTopic>;
            if (cachedSueetieForumTopics != null)
            {
                if (contentQuery.NumRecords > 0)
                    return cachedSueetieForumTopics.Take(contentQuery.NumRecords).ToList();
                return cachedSueetieForumTopics.ToList();
            }

            var sueetieForumTopics = from p in provider.GetSueetieForumTopicList(contentQuery)
                select p;

            if (contentQuery.GroupID > -1)
                sueetieForumTopics = from g in sueetieForumTopics where g.GroupID == contentQuery.GroupID select g;

            if (contentQuery.ApplicationID > 0)
                sueetieForumTopics = from a in sueetieForumTopics where a.ApplicationID == contentQuery.ApplicationID select a;

            if (contentQuery.UserID > 0)
                sueetieForumTopics = from u in sueetieForumTopics where u.UserID == contentQuery.UserID select u;

            if (contentQuery.IsRestricted)
                sueetieForumTopics = from r in sueetieForumTopics where r.IsRestricted == false select r;

            if (contentQuery.CacheMinutes > 0)
                SueetieCache.Current.InsertMinutes(key, sueetieForumTopics.ToList(), contentQuery.CacheMinutes);
            else
                SueetieCache.Current.InsertMax(key, sueetieForumTopics.ToList());

            return contentQuery.NumRecords > 0 ? sueetieForumTopics.Take(contentQuery.NumRecords).ToList() : sueetieForumTopics.ToList();
        }

        public static List<SueetieForumMessage> GetSueetieForumMessageList(ContentQuery contentQuery)
        {
            var provider = SueetieDataProvider.LoadProvider();

            var key = ForumMessageListCacheKey(contentQuery);

            var cachedSueetieForumMessages = SueetieCache.Current[key] as List<SueetieForumMessage>;
            if (cachedSueetieForumMessages != null)
            {
                if (contentQuery.NumRecords > 0)
                    return cachedSueetieForumMessages.Take(contentQuery.NumRecords).ToList();
                return cachedSueetieForumMessages.ToList();
            }

            var sueetieForumMessages = from p in provider.GetSueetieForumMessageList(contentQuery)
                select p;

            if (contentQuery.GroupID > -1)
                sueetieForumMessages = from g in sueetieForumMessages where g.GroupID == contentQuery.GroupID select g;

            if (contentQuery.ApplicationID > 0)
                sueetieForumMessages = from a in sueetieForumMessages where a.ApplicationID == contentQuery.ApplicationID select a;

            if (contentQuery.UserID > 0)
                sueetieForumMessages = from u in sueetieForumMessages where u.SueetieUserID == contentQuery.UserID select u;

            if (contentQuery.IsRestricted)
                sueetieForumMessages = from r in sueetieForumMessages where r.IsRestricted == false select r;

            if (contentQuery.CacheMinutes > 0)
                SueetieCache.Current.InsertMinutes(key, sueetieForumMessages.ToList(), contentQuery.CacheMinutes);
            else
                SueetieCache.Current.InsertMax(key, sueetieForumMessages.ToList());

            return contentQuery.NumRecords > 0 ? sueetieForumMessages.Take(contentQuery.NumRecords).ToList() : sueetieForumMessages.ToList();
        }

        public static string ForumTopicListCacheKey(ContentQuery contentQuery)
        {
            return string.Format("ForumTopicList-{0}-{1}-{2}", contentQuery.GroupID, contentQuery.UserID, contentQuery.IsRestricted);
        }

        public static void ClearForumTopicListCache(ContentQuery contentQuery)
        {
            // Clearing cached ForumTopic list for the topic author and for the all user ForumTopic listview

            SueetieCache.Current.Remove(ForumTopicListCacheKey(contentQuery));
            contentQuery.UserID = (int)SueetieUserType.AllUsers;
            SueetieCache.Current.Remove(ForumTopicListCacheKey(contentQuery));
        }

        private static void ClearForumTopicListCache(SueetieForumContent sueetieForumContent)
        {
            var contentQuery = new ContentQuery
            {
                GroupID = sueetieForumContent.GroupID,
                UserID = sueetieForumContent.SueetieUserID,
                IsRestricted = sueetieForumContent.IsRestricted
            };
            ClearForumTopicListCache(contentQuery);
        }

        public static string ForumMessageListCacheKey(ContentQuery contentQuery)
        {
            return string.Format("ForumMessageList-{0}-{1}-{2}", contentQuery.GroupID, contentQuery.UserID, contentQuery.IsRestricted);
        }

        public static void ClearForumMessageListCache(ContentQuery contentQuery)
        {
            // Clearing cached ForumMessage list for the topic author and for the all user ForumMessage listview
            SueetieCache.Current.Remove(ForumMessageListCacheKey(contentQuery));
            contentQuery.UserID = (int)SueetieUserType.AllUsers;
            SueetieCache.Current.Remove(ForumMessageListCacheKey(contentQuery));
        }

        private static void ClearForumMessageListCache(SueetieForumContent sueetieForumContent)
        {
            var contentQuery = new ContentQuery
            {
                GroupID = sueetieForumContent.GroupID,
                UserID = sueetieForumContent.SueetieUserID,
                IsRestricted = sueetieForumContent.IsRestricted
            };
            ClearForumMessageListCache(contentQuery);
        }

        public static void CompleteForumSetup()
        {
            var dp = SueetieDataProvider.LoadProvider();
            dp.CompleteForumSetup();
        }

        public static void UpdateForumTheme(string themename)
        {
            var dp = SueetieDataProvider.LoadProvider();
            dp.UpdateForumTheme(themename);
        }

        public static int GetForumUserId(int boardId, string username)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetForumUserID(boardId, username);
        }

        public static SueetieUser GetSueetieUserFromForumId(int forumUserId)
        {
            var sueetieUser = SueetieUsers.GetSueetieUserList(SueetieUserType.RegisteredUser).Find(u => u.ForumUserID == forumUserId);
            if (sueetieUser == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                sueetieUser = provider.GetSueetieUserFromForumID(forumUserId);
            }
            return sueetieUser;
        }

        public static int CreateForumUser(SueetieUser sueetieUser)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.CreateForumUser(sueetieUser);
        }

        public static void EnterForumTopicTags(SueetieTagEntry sueetieTagEntry)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.EnterForumTopicTags(sueetieTagEntry);
        }
    }
}