// -----------------------------------------------------------------------
// <copyright file="SueetieCommon.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;

    public static class SueetieCommon
    {
        public static StringDictionary GetSiteSettingsDictionary()
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetSiteSettingsDictionary();
        }

        public static void ClearSiteSettingsCache()
        {
            SueetieCache.Current.Remove("SueetieSiteSettings");
        }

        public static void UpdateSiteSetting(SiteSetting siteSetting)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.UpdateSiteSetting(siteSetting);
        }

        public static int AddSueetieContent(SueetieContent sueetieContent)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.AddSueetieContent(sueetieContent);
        }

        public static void UpdateSueetieContentPermalink(SueetieContent sueetieContent)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.UpdateSueetieContentPermalink(sueetieContent);
        }

        public static bool IsMediaObject(int contentTypeId)
        {
            return contentTypeId == (int)SueetieContentType.MediaAudioFile ||
                contentTypeId == (int)SueetieContentType.MediaDocument ||
                contentTypeId == (int)SueetieContentType.MediaImage ||
                contentTypeId == (int)SueetieContentType.MediaOther ||
                contentTypeId == (int)SueetieContentType.MediaVideo;
        }

        public static bool IsMediaAlbum(int contentTypeId)
        {
            return contentTypeId == (int)SueetieContentType.AudioMediaAlbum ||
                contentTypeId == (int)SueetieContentType.DocumentMediaAlbum ||
                contentTypeId == (int)SueetieContentType.ImageMediaAlbum ||
                contentTypeId == (int)SueetieContentType.MultipurposeMediaAlbum ||
                contentTypeId == (int)SueetieContentType.OtherAlbum ||
                contentTypeId == (int)SueetieContentType.UserMediaAlbum ||
                contentTypeId == (int)SueetieContentType.VideoMediaAlbum;
        }

        public static void UpdateSueetieContentIsRestricted(int contentId, bool isRestricted)
        {
            var dp = SueetieDataProvider.LoadProvider();
            dp.UpdateSueetieContentIsRestricted(contentId, isRestricted);
        }

        public static bool IsSiteInstalled()
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.IsSiteInstalled();
        }

        public static List<SueetieApplication> GetSueetieApplicationsList()
        {
            var key = SueetieApplicationListCacheKey();

            var sueetieApplications = SueetieCache.Current[key] as List<SueetieApplication>;
            if (sueetieApplications == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                sueetieApplications = provider.GetSueetieApplicationList();
                SueetieCache.Current.InsertMax(key, sueetieApplications);
            }

            return sueetieApplications;
        }

        public static string SueetieApplicationListCacheKey()
        {
            return string.Format("SueetieApplicationList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static void ClearApplicationListCache()
        {
            SueetieCache.Current.Remove(SueetieApplicationListCacheKey());
        }

        public static void UpdateSueetieApplication(string appKey, string appDescription, bool isActive, int groupId, int appTypeId, int applicationId)
        {
            var sueetieApplication = new SueetieApplication
            {
                ApplicationID = applicationId,
                ApplicationKey = appKey,
                IsActive = isActive,
                GroupID = groupId,
                ApplicationTypeID = appTypeId,
                Description = appDescription
            };
            var provider = SueetieDataProvider.LoadProvider();
            provider.UpdateSueetieApplication(sueetieApplication);

            ClearApplicationListCache();
        }

        public static int CreateSueetieApplication(int applicationId, string appKey, string appDescription, int groupId, int appTypeId)
        {
            var sueetieApplication = new SueetieApplication
            {
                ApplicationID = applicationId,
                ApplicationKey = appKey,
                GroupID = groupId,
                ApplicationTypeID = appTypeId,
                Description = appDescription
            };

            var provider = SueetieDataProvider.LoadProvider();
            provider.CreateSueetieApplication(sueetieApplication);
            if (sueetieApplication.ApplicationTypeID == (int)SueetieApplicationType.Blog && sueetieApplication.GroupID == 0)
            {
                var sueetieBlog = new SueetieBlog
                {
                    ApplicationID = applicationId,
                    BlogTitle = sueetieApplication.Description
                };
                SueetieBlogs.CreateSueetieBlog(sueetieBlog);
            }

            ClearApplicationListCache();
            return applicationId;
        }

        public static void DeleteSueetieApplication(int applicationId)
        {
            var dp = SueetieDataProvider.LoadProvider();
            dp.DeleteSueetieApplication(applicationId);
            ClearApplicationListCache();
        }

        public static SueetieApplication GetSueetieApplication(int applicationId)
        {
            return GetSueetieApplicationsList().Find(a => a.ApplicationID == applicationId);
        }

        public static SueetieApplication GetSueetieApplication(string applicationKey)
        {
            return GetSueetieApplicationsList().Find(a => a.ApplicationKey == applicationKey);
        }

        public static List<SueetieGroup> GetSueetieGroupList()
        {
            var key = SueetieGroupListCacheKey();

            var sueetieGroups = SueetieCache.Current[key] as List<SueetieGroup>;
            if (sueetieGroups == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                sueetieGroups = provider.GetSueetieGroupList();
                SueetieCache.Current.InsertMax(key, sueetieGroups);
            }

            return sueetieGroups;
        }

        public static string SueetieGroupListCacheKey()
        {
            return string.Format("SueetieGroupList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static void ClearGroupListCache()
        {
            SueetieCache.Current.Remove(SueetieGroupListCacheKey());
        }

        public static SueetieGroup GetSueetieGroup(int groupId)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetSueetieGroup(groupId);
        }

        public static int CreateGroup(SueetieGroup sueetieGroup)
        {
            var provider = SueetieDataProvider.LoadProvider();
            var groupId = provider.CreateGroup(sueetieGroup);
            ClearGroupListCache();
            return groupId;
        }

        public static void UpdateSueetieGroup(SueetieGroup sueetieGroup)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.UpdateSueetieGroup(sueetieGroup);
        }

        public static void CreateSiteLogEntry(SiteLogEntry siteLogEntry)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.CreateSiteLogEntry(siteLogEntry);
        }

        public static List<SiteLogEntry> GetSiteLogEntryList(SiteLogEntry siteLogEntry)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetSiteLogEntryList(siteLogEntry);
        }

        public static List<SiteLogEntry> GetSiteLogTypeList()
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetSiteLogTypeList();
        }

        public static List<UserLogCategory> GetUserLogCategoryList()
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetUserLogCategoryList();
        }

        public static void UpdateUserLogCategory(bool isDisplayed, bool isSyndicated, int userLogCategoryId)
        {
            var userLogCategory = new UserLogCategory
            {
                UserLogCategoryID = userLogCategoryId,
                IsDisplayed = isDisplayed,
                IsSyndicated = isSyndicated
            };
            var provider = SueetieDataProvider.LoadProvider();
            provider.UpdateUserLogCategory(userLogCategory);
        }

        public static void CreateUserLogCategory(int catId, string catCode, string catDesc, bool isDisplayed, bool isSyndicated)
        {
            var userLogCategory = new UserLogCategory
            {
                UserLogCategoryID = catId,
                UserLogCategoryCode = catCode,
                UserLogCategoryDescription = catDesc,
                IsDisplayed = isDisplayed,
                IsSyndicated = isSyndicated
            };
            var provider = SueetieDataProvider.LoadProvider();
            provider.CreateUserLogCategory(userLogCategory);
        }

        public static void DeleteUserLogCategory(int userLogCategoryId)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.DeleteUserLogCategory(userLogCategoryId);
        }

        public static void CreateUserLogEntry(UserLogEntry userLogEntry)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.CreateUserLogEntry(userLogEntry);
        }

        public static List<UserLogActivity> GetUserLogActivityList(bool showAll)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetUserLogActivityList(showAll);
        }

        public static List<UserLogActivity> GetUserLogActivityList(ContentQuery contentQuery)
        {
            var key = UserLogActivityListCacheKey(contentQuery);

            var userLogActivities = SueetieCache.Current[key] as List<UserLogActivity>;
            if (userLogActivities == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                userLogActivities = provider.GetUserLogActivityList(contentQuery);
                SueetieCache.Current.InsertMinutes(key, userLogActivities, contentQuery.CacheMinutes);
            }

            if (contentQuery.NumRecords > 0)
                return userLogActivities.Take(contentQuery.NumRecords).ToList();
            return userLogActivities;
        }

        public static void DeleteUserLogActivity(int userLogId)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.DeleteUserLogActivity(userLogId);
            ClearUserLogActivityListCache((int)SueetieContentViewType.Unassigned);
            ClearUserLogActivityListCache((int)SueetieContentViewType.SyndicatedUserLogActivityList);
        }

        public static void ClearUserLogActivityListCache(int contentViewTypeId)
        {
            SueetieCache.Current.Remove(UserLogActivityListCacheKey(new ContentQuery { GroupID = 0, SueetieContentViewTypeID = contentViewTypeId }));
        }

        public static string UserLogActivityListCacheKey(ContentQuery contentQuery)
        {
            return string.Format("UserLogActivityList-{0}-{1}-{2}", contentQuery.GroupID, contentQuery.SueetieContentViewTypeID, SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static void TestTaskEntry()
        {
            var dp = SueetieDataProvider.LoadProvider();
            dp.TestTaskEntry();
        }

        public static List<CrawlerAgent> GetCrawlerAgentList()
        {
            var key = CrawlerAgentListCacheKey();

            var crawlerAgents = SueetieCache.Current[key] as List<CrawlerAgent>;
            if (crawlerAgents == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                crawlerAgents = provider.GetCrawlerAgentList();
                SueetieCache.Current.Insert(key, crawlerAgents);
            }

            return crawlerAgents;
        }

        public static string CrawlerAgentListCacheKey()
        {
            return string.Format("CrawlerAgentList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static void ClearCrawlerAgentListCache()
        {
            SueetieCache.Current.Remove(CrawlerAgentListCacheKey());
        }
    }
}