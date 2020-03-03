// -----------------------------------------------------------------------
// <copyright file="SueetieLogs.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Caching;
    using System.Xml.Linq;

    public static class SueetieLogs
    {
        //private static readonly Regex FILTEREDURL_REGEX = new Regex(GetFilteredUrlList().Aggregate((i, j) => i + "|" + j), RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static List<string> GetFilteredUrlList()
        {
            var provider = SueetieDataProvider.LoadProvider();
            var key = FilteredUrlListCacheKey();

            var filteredUrls = SueetieCache.Current[key] as List<string>;
            if (filteredUrls == null)
            {
                filteredUrls = provider.GetFilteredUrlList();
                SueetieCache.Current.Insert(key, filteredUrls);
            }

            return filteredUrls;
        }

        public static string FilteredUrlListCacheKey()
        {
            return string.Format("FilteredUrlList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static void ClearFilteredUrlListCache()
        {
            SueetieCache.Current.Remove(FilteredUrlListCacheKey());
        }

        public static bool IsFilteredUrl
        {
            get
            {
                var _isFilteredUrl = false;
                var context = HttpContext.Current;
                if (context != null)
                {
                    var request = context.Request;
                    if (!string.IsNullOrEmpty(request.RawUrl) && SueetieContext.Current.IsAnonymousUser)
                    {
                        _isFilteredUrl = IsRawUrlAFilteredUrl(SueetieUrlHelper.PrepUrlForLogging(request.RawUrl));
                    }
                }
                return _isFilteredUrl;
            }
        }

        public static bool IsRawUrlAFilteredUrl(string _url)
        {
            var isAMatch = false;
            foreach (var _filteredUrl in GetFilteredUrlList())
            {
                if (_url.ToLowerInvariant().Contains(_filteredUrl.ToLower()))
                    isAMatch = true;
            }
            return isAMatch;
        }

        private static List<CrawlerAgent> GetFilteredAgentList()
        {
            var key = FilteredAgentListCacheKey();

            var crawlerAgents = SueetieCache.Current[key] as List<CrawlerAgent>;
            if (crawlerAgents == null)
            {
                crawlerAgents = SueetieCommon.GetCrawlerAgentList().Where(c => c.IsBlocked == false).ToList();
                SueetieCache.Current.Insert(key, crawlerAgents);
            }

            return crawlerAgents;
        }

        public static string FilteredAgentListCacheKey()
        {
            return string.Format("FilteredAgentList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static void ClearFilteredAgentListCache()
        {
            SueetieCache.Current.Remove(FilteredAgentListCacheKey());
        }

        private static readonly Regex CRAWLER_REGEX = new Regex(GetFilteredAgentList().Select(query => query.AgentExcerpt).Aggregate((a, b) => a + "|" + b), RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static bool IsCrawler
        {
            get
            {
                var context = HttpContext.Current;
                if (context != null)
                {
                    var request = context.Request;

                    if (!string.IsNullOrEmpty(request.UserAgent) && CRAWLER_REGEX.IsMatch(request.UserAgent))
                        return true;
                }

                return false;
            }
        }

        public static bool IsAgentACrawler(string _userAgent)
        {
            if (CRAWLER_REGEX.IsMatch(_userAgent))
                return true;
            return false;
        }

        public static void LogSiteEntry(string message)
        {
            LogSiteEntry(SiteLogType.General, SiteLogCategoryType.GenericMessage, message, 0);
        }

        public static void LogSiteEntry(SiteLogType logType, SiteLogCategoryType categoryType, string message)
        {
            LogSiteEntry(logType, categoryType, message, 0);
        }

        public static void LogSiteEntry(SiteLogType logType, SiteLogCategoryType categoryType, string message, int applicationID)
        {
            var entry = new SiteLogEntry();
            entry.Message = message;
            entry.SiteLogTypeID = (int)logType;
            entry.SiteLogCategoryID = (int)categoryType;
            entry.ApplicationID = applicationID;
            LogSiteEntry(entry);
        }

        public static void LogSiteEntry(SiteLogEntry entry)
        {
            SueetieCommon.CreateSiteLogEntry(entry);
        }

        public static void LogException(string message)
        {
            var entry = new SiteLogEntry();
            entry.Message = message;
            entry.SiteLogTypeID = (int)SiteLogType.Exception;
            entry.SiteLogCategoryID = (int)SiteLogCategoryType.GeneralException;
            LogSiteEntry(entry);
        }

        public static void LogApplicationStopped(string message)
        {
            var entry = new SiteLogEntry();
            entry.Message = message;
            entry.SiteLogTypeID = (int)SiteLogType.General;
            entry.SiteLogCategoryID = (int)SiteLogCategoryType.AppStartStop;
            entry.ApplicationID = 0;
            LogSiteEntry(entry);
        }

        public static void LogTaskException(string message)
        {
            var entry = new SiteLogEntry();
            entry.Message = message;
            entry.SiteLogTypeID = (int)SiteLogType.Exception;
            entry.SiteLogCategoryID = (int)SiteLogCategoryType.TasksException;
            entry.ApplicationID = 0;
            LogSiteEntry(entry);
        }

        public static void LogSearchException(int docID, string docTitle, string message)
        {
            var entry = new SiteLogEntry();
            entry.Message = "ContentID and Title: " + docID + " : " + docTitle + " ERROR: " + message;
            entry.SiteLogTypeID = (int)SiteLogType.Exception;
            entry.SiteLogCategoryID = (int)SiteLogCategoryType.SearchException;
            entry.ApplicationID = 0;
            LogSiteEntry(entry);
        }

        public static void LogSearchException(string message)
        {
            var entry = new SiteLogEntry();
            entry.Message = message;
            entry.SiteLogTypeID = (int)SiteLogType.Exception;
            entry.SiteLogCategoryID = (int)SiteLogCategoryType.SearchException;
            entry.ApplicationID = 0;
            LogSiteEntry(entry);
        }

        public static void LogMarketplaceException(string message)
        {
            var entry = new SiteLogEntry();
            entry.Message = message;
            entry.SiteLogTypeID = (int)SiteLogType.Exception;
            entry.SiteLogCategoryID = (int)SiteLogCategoryType.MarketplaceException;
            entry.ApplicationID = 0;
            LogSiteEntry(entry);
        }

        public static void LogMarketplaceMessage(string message)
        {
            var entry = new SiteLogEntry();
            entry.Message = message;
            entry.SiteLogTypeID = (int)SiteLogType.General;
            entry.SiteLogCategoryID = (int)SiteLogCategoryType.MarketplaceMessage;
            entry.ApplicationID = 0;
            LogSiteEntry(entry);
        }

        public static void LogAddonPackException(string message)
        {
            var entry = new SiteLogEntry();
            entry.Message = message;
            entry.SiteLogTypeID = (int)SiteLogType.Exception;
            entry.SiteLogCategoryID = (int)SiteLogCategoryType.AddonPackException;
            entry.ApplicationID = 0;
            LogSiteEntry(entry);
        }

        public static void LogAddonPackMessage(string message)
        {
            var entry = new SiteLogEntry();
            entry.Message = message;
            entry.SiteLogTypeID = (int)SiteLogType.General;
            entry.SiteLogCategoryID = (int)SiteLogCategoryType.AddonPackMessage;
            entry.ApplicationID = 0;
            LogSiteEntry(entry);
        }


        public static void LogAnalyticsException(string message)
        {
            var entry = new SiteLogEntry();
            entry.Message = message;
            entry.SiteLogTypeID = (int)SiteLogType.Exception;
            entry.SiteLogCategoryID = (int)SiteLogCategoryType.AnalyticsException;
            entry.ApplicationID = 0;
            LogSiteEntry(entry);
        }

        public static void LogAnalyticsMessage(string message)
        {
            var entry = new SiteLogEntry();
            entry.Message = message;
            entry.SiteLogTypeID = (int)SiteLogType.General;
            entry.SiteLogCategoryID = (int)SiteLogCategoryType.AnalyticsMessage;
            entry.ApplicationID = 0;
            LogSiteEntry(entry);
        }

        public static void LogUserEntry(UserLogCategoryType categoryType, int itemID, int userID)
        {
            var entry = new UserLogEntry();
            entry.UserLogCategoryID = (int)categoryType;
            entry.ItemID = itemID;
            entry.UserID = userID;
            LogUserEntry(entry);
        }

        public static void LogUserEntry(UserLogEntry entry)
        {
            SueetieCommon.CreateUserLogEntry(entry);
        }

        public static void DeleteUserLogActivity(int UserLogID)
        {
            SueetieCommon.DeleteUserLogActivity(UserLogID);
        }

        public static List<UserLogActivity> GetUserLogActivityList()
        {
            return GetUserLogActivityList(false);
        }

        public static List<UserLogActivity> GetUserLogActivityList(bool showAll)
        {
            // Will extend to filter and retrieve all site activity rather than by group (or top level - group 0)
            var _userLogActivityList = SueetieCommon.GetUserLogActivityList(showAll);
            var dayCheck = string.Empty;
            foreach (var _userLogActivity in _userLogActivityList)
            {
                if (_userLogActivity.DateTimeActivity.ToShortDateString() != dayCheck)
                {
                    _userLogActivity.ShowHeader = true;
                    dayCheck = _userLogActivity.DateTimeActivity.ToShortDateString();
                }
                FormatUserLogRow(_userLogActivity);
            }
            return _userLogActivityList;
        }

        public static List<UserLogActivity> GetUserLogActivityList(ContentQuery contentQuery)
        {
            // Will extend to filter and retrieve all site activity rather than by group (or top level - group 0)
            var _userLogActivityList = SueetieCommon.GetUserLogActivityList(contentQuery);
            var dayCheck = string.Empty;
            foreach (var _userLogActivity in _userLogActivityList)
            {
                if (_userLogActivity.DateTimeActivity.ToShortDateString() != dayCheck)
                {
                    _userLogActivity.ShowHeader = true;
                    dayCheck = _userLogActivity.DateTimeActivity.ToShortDateString();
                }
                FormatUserLogRow(_userLogActivity);
            }
            return _userLogActivityList;
        }

        private static UserLogActivity FormatUserLogRow(UserLogActivity _u)
        {
            _u.ActivityClass = _u.ApplicationPath;

            var _profileUrl = SueetieUrls.Instance.MyProfile(_u.UserID).Url;
            var _toProfileUrl = SueetieUrls.Instance.MasterProfile(SueetieUsers.GetThinSueetieUser(_u.ToUserID).ForumUserID).Url;
            if (SueetieConfiguration.Get().Core.UseForumProfile)
            {
                var thinUser = SueetieUsers.GetThinSueetieUser(_u.UserID);
                if (thinUser != null)
                {
                    var masterProfile = SueetieUrls.Instance.MasterProfile(thinUser.ForumUserID);
                    if (masterProfile != null)
                    {
                        _profileUrl = masterProfile.Url;
                    }
                }
            }
            var username = string.Format("<a href='" + _profileUrl + "' class='ActivityUser'>{0}</a> ", _u.DisplayName);
            var toUsername = string.Format("<a href='" + _profileUrl + "' class='ActivityUser'>{0}</a> ", _u.ToUserDisplayName);

            _u.Activity = string.Empty;

            switch (_u.UserLogCategoryID)
            {
                case (int)UserLogCategoryType.Following:
                    var _toUserProfileUrl = SueetieUrls.Instance.MyProfile(_u.ToUserID).Url;
                    if (SueetieConfiguration.Get().Core.UseForumProfile)
                        _toUserProfileUrl = SueetieUrls.Instance.MasterProfile(SueetieUsers.GetThinSueetieUser(_u.ToUserID).ForumUserID).Url;
                    _u.Activity += string.Format(
                        SueetieLocalizer.GetString("activity_is_now_following"),
                        username,
                        string.Format(
                            "<a href='" + _toUserProfileUrl + "' class='ActivitySource'>{0}</a>",
                            _u.ToUserDisplayName));
                    break;
                case (int)UserLogCategoryType.BlogPost:
                    _u.Activity += string.Format(
                        SueetieLocalizer.GetString("activity_blogged"),
                        username,
                        string.Format(
                            "<a href='{0}' class='ActivitySource'>{1}</a>", _u.Permalink, _u.SourceDescription),
                        SueetieLocalizer.GetString("activity_in"),
                        string.Format(
                            "<a href='/{0}/default.aspx' class='ActivitySourceParent'>{1}</a>",
                            _u.ApplicationPath,
                            _u.ApplicationName));

                    break;
                case (int)UserLogCategoryType.BlogComment:
                    _u.Activity += string.Format(
                        SueetieLocalizer.GetString("activity_commented_on"),
                        username,
                        string.Format(
                            "<a href='{0}' class='ActivitySource'>{1}</a>", _u.Permalink, _u.SourceParentDescription),
                        SueetieLocalizer.GetString("activity_in"),
                        string.Format(
                            "<a href='/{0}/default.aspx' class='ActivitySourceParent'>{1}</a>",
                            _u.ApplicationPath,
                            _u.ApplicationName));
                    _u.ActivityClass = "blogcomment";
                    break;
                case (int)UserLogCategoryType.ForumTopic:
                    _u.Activity += string.Format(
                        SueetieLocalizer.GetString("activity_posted"),
                        username,
                        string.Format(
                            "<a href='{0}' class='ActivitySource'>{1}</a>", _u.Permalink, _u.SourceDescription),
                        SueetieLocalizer.GetString("activity_in"),
                        string.Format(
                            "<a href='/{0}/default.aspx' class='ActivitySourceParent'>{1}</a>",
                            _u.ApplicationPath,
                            _u.ApplicationName));
                    break;
                case (int)UserLogCategoryType.ForumMessage:
                    _u.Activity += string.Format(
                        SueetieLocalizer.GetString("activity_replied_to"),
                        username,
                        string.Format(
                            "<a href='{0}'>{1}</a>", _u.Permalink, _u.SourceParentDescription),
                        SueetieLocalizer.GetString("activity_in"),
                        string.Format(
                            "<a href='/{0}/default.aspx' class='ActivitySourceParent'>{1}</a>",
                            _u.ApplicationPath,
                            _u.ApplicationName));
                    break;
                case (int)UserLogCategoryType.ForumAnswer:
                    var _answerBy = SueetieLocalizer.GetString("activity_by") + " " + toUsername;
                    if (_u.UserID == _u.ToUserID)
                        _answerBy = string.Empty;
                    _u.Activity += string.Format(
                        SueetieLocalizer.GetString("activity_marked_as_answer"),
                        username,
                        string.Format(
                            "<a href='{0}'>{1}</a>", _u.Permalink, _u.SourceDescription),
                        _answerBy,
                        SueetieLocalizer.GetString("activity_as_an_answer"),
                        SueetieLocalizer.GetString("activity_in"),
                        string.Format(
                            "<a href='/{0}/default.aspx' class='ActivitySourceParent'>{1}</a>",
                            _u.ApplicationPath,
                            _u.ApplicationName));
                    // {0} marked {1} {2} {3} {4} {5} {6}
                    break;
                case (int)UserLogCategoryType.NewWikiPage:
                    _u.Activity += string.Format(
                        SueetieLocalizer.GetString("activity_created"),
                        username,
                        string.Format(
                            "<a href='{0}' class='ActivitySource'>{1}</a>", _u.Permalink, _u.SourceDescription),
                        SueetieLocalizer.GetString("activity_in_the"),
                        string.Format(
                            "<a href='/{0}/default.aspx' class='ActivitySourceParent'>{1}</a>",
                            _u.ApplicationPath,
                            _u.ApplicationName));
                    break;
                case (int)UserLogCategoryType.NewWikiMessage:
                    _u.Activity += string.Format(
                        SueetieLocalizer.GetString("activity_new_wiki_message"),
                        username,
                        string.Format(
                            "<a href='{0}' class='ActivitySource'>{1}</a>", _u.Permalink, SueetieLocalizer.GetString("activity_a_new_message")),
                        SueetieLocalizer.GetString("activity_to"),
                        string.Format(
                            "<a href='{0}' class='ActivitySourceParent'>{1}</a>",
                            _u.SourceParentPermalink,
                            _u.SourceParentDescription),
                        SueetieLocalizer.GetString("activity_in_the"),
                        string.Format("<a href='/{0}/default.aspx' class='ActivitySourceParent'>{1}</a>",
                            _u.ApplicationPath,
                            _u.ApplicationName));
                    break;
                case (int)UserLogCategoryType.WikiPageUpdated:
                    _u.Activity += string.Format(
                        SueetieLocalizer.GetString("activity_updated"),
                        username,
                        string.Format(
                            "<a href='{0}' class='ActivitySource'>{1}</a>", _u.Permalink, _u.SourceDescription),
                        SueetieLocalizer.GetString("activity_in_the"),
                        string.Format(
                            "<a href='/{0}/default.aspx' class='ActivitySourceParent'>{1}</a>",
                            _u.ApplicationPath,
                            _u.ApplicationName));

                    break;
                case (int)UserLogCategoryType.DocumentAlbum:
                case (int)UserLogCategoryType.AudioAlbum:
                case (int)UserLogCategoryType.ImageAlbum:
                case (int)UserLogCategoryType.UserMediaAlbum:
                case (int)UserLogCategoryType.MultipurposeAlbum:
                    _u.Activity += string.Format(
                        SueetieLocalizer.GetString("activity_created_a_new"),
                        username,
                        _u.SourceDescription.ToLower(),
                        SueetieLocalizer.GetString("activity_named"),
                        string.Format(
                            "<a href='{0}' class='ActivitySource'>{1}</a>", _u.Permalink, _u.SourceParentDescription),
                        SueetieLocalizer.GetString("activity_in"),
                        string.Format(
                            "<a href='{0}' class='ActivitySourceParent'>{1}</a>",
                            _u.SourceParentPermalink,
                            _u.ApplicationName));
                    break;
                case (int)UserLogCategoryType.AudioUploaded:
                case (int)UserLogCategoryType.DocumentUploaded:
                case (int)UserLogCategoryType.ImageUploaded:
                case (int)UserLogCategoryType.OtherMediaUploaded:
                case (int)UserLogCategoryType.VideoUploaded:
                    _u.Activity += string.Format(
                        SueetieLocalizer.GetString("activity_added_new"),
                        username,
                        _u.SourceDescription.ToLower(),
                        SueetieLocalizer.GetString("activity_to"),
                        string.Format(
                            "<a href='{0}' class='ActivitySource'>{1}</a>", _u.Permalink, _u.SourceParentDescription),
                        SueetieLocalizer.GetString("activity_in"),
                        string.Format(
                            "<a href='{0}' class='ActivitySourceParent'>{1}</a>",
                            _u.SourceParentPermalink,
                            _u.ApplicationName));
                    _u.ActivityClass = "mediaupload";
                    break;
                case (int)UserLogCategoryType.MarketplaceProduct:
                    _u.Activity += string.Format(
                        SueetieLocalizer.GetString("activity_added_a_new_product_item"),
                        username,
                        string.Format(
                            "<a href='{0}' class='ActivitySource'>{1}</a>", _u.Permalink, _u.SourceDescription),
                        SueetieLocalizer.GetString("activity_in_the"),
                        string.Format(
                            "<a href='/{0}/default.aspx' class='ActivitySourceParent'>{1}</a>",
                            _u.ApplicationPath,
                            _u.ApplicationName));
                    break;
                case (int)UserLogCategoryType.CMSPageUpdated:
                    _u.Activity += string.Format(
                        SueetieLocalizer.GetString("activity_updated_a_cms_page"),
                        username,
                        string.Format(
                            "<a href='{0}' class='ActivitySource'>{1}</a>", _u.Permalink, _u.SourceDescription),
                        SueetieLocalizer.GetString("activity_in_the"),
                        string.Format(
                            "<a href='/{0}/default.aspx' class='ActivitySourceParent'>{1}</a>",
                            _u.ApplicationPath,
                            _u.ApplicationName));
                    break;
                case (int)UserLogCategoryType.CMSPageCreated:
                    _u.Activity += string.Format(
                        SueetieLocalizer.GetString("activity_created_a_cms_page"),
                        username,
                        string.Format(
                            "<a href='{0}' class='ActivitySource'>{1}</a>", _u.Permalink, _u.SourceDescription),
                        SueetieLocalizer.GetString("activity_in_the"),
                        string.Format(
                            "<a href='/{0}/default.aspx' class='ActivitySourceParent'>{1}</a>",
                            _u.ApplicationPath,
                            _u.ApplicationName));
                    break;
                case (int)UserLogCategoryType.CalendarEvent:
                    if (_u.Permalink == "na")
                        _u.Activity += string.Format(
                            SueetieLocalizer.GetString("activity_new_calendar_event"),
                            username,
                            string.Format("<span) class='ActivitySpan'>{0}</span>", _u.SourceDescription),
                            SueetieLocalizer.GetString("activity_in_the"),
                            string.Format(
                                "<a href='{0}' class='ActivitySourceParent'>{1}</a>",
                                _u.SourceParentPermalink,
                                _u.SourceParentDescription));
                    else
                        _u.Activity += string.Format(
                            SueetieLocalizer.GetString("activity_new_calendar_event"),
                            username,
                            string.Format(
                                "<a href='{0}' class='ActivitySource'>{1}</a>", _u.Permalink, _u.SourceDescription),
                            SueetieLocalizer.GetString("activity_in_the"),
                            string.Format(
                                "<a href='{0}' class='ActivitySourceParent'>{1}</a>",
                                _u.SourceParentPermalink,
                                _u.SourceParentDescription));

                    _u.ActivityClass = "calendarevent";
                    break;
                case (int)UserLogCategoryType.JoinedCommunity:
                    _u.Activity = string.Format(SueetieLocalizer.GetString("activity_became_a_new_member"), username);
                    _u.ActivityClass = "newuser";
                    break;
                case (int)UserLogCategoryType.Registered:
                    _u.Activity = string.Format(SueetieLocalizer.GetString("activity_user_registered"), username);
                    _u.ActivityClass = "newuser";
                    break;
                case (int)UserLogCategoryType.LoggedIn:
                    _u.Activity = string.Format(SueetieLocalizer.GetString("activity_user_loggedin"), username);
                    _u.ActivityClass = "newuser";
                    break;
                default:
                    _u.Activity = _u.UserLogCategoryID + " - " + _u.UserLogID + " - " + _u.ApplicationName;
                    break;
            }
            return _u;
        }

        // going away...shouldn't be in use anywhere

        public static void LogNonProviderEvent(string message)
        {
            var entry = new SiteLogEntry();
            entry.Message = message;
            entry.SiteLogTypeID = (int)SiteLogType.General;
            entry.SiteLogCategoryID = (int)SiteLogCategoryType.GeneralAppEvent;
            entry.ApplicationID = 0;
            LogNonProviderEvent(entry);
        }

        public static void LogNonProviderEvent(SiteLogEntry siteLogEntry)
        {
            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SueetieConnectionString"].ConnectionString))
            {
                using (var cmd = new SqlCommand("Sueetie_SiteLog_Add", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@SiteLogTypeID", SqlDbType.Int, 4).Value = siteLogEntry.SiteLogTypeID;
                    cmd.Parameters.Add("@SiteLogCategoryID", SqlDbType.Int, 4).Value = siteLogEntry.SiteLogCategoryID;
                    cmd.Parameters.Add("@ApplicationID", SqlDbType.Int, 4).Value = siteLogEntry.ApplicationID;
                    cmd.Parameters.Add("@Message", SqlDbType.NText).Value = siteLogEntry.Message;

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }

        private static void ApplyPageRules(SueetieRequest _sueetieRequest)
        {
            var request = HttpContext.Current.Request;
            var _pageRules = GetPageRuleList();

            var _pageRulesContains = _pageRules.Where(p => p.IsEqual == false).ToList();
            foreach (var _pageRule in _pageRulesContains)
            {
                if (_sueetieRequest.Url.Contains(_pageRule.UrlExcerpt, StringComparison.OrdinalIgnoreCase))
                {
                    _sueetieRequest.Url = _pageRule.UrlFinal;
                    _sueetieRequest.PageTitle = _pageRule.PageTitle;
                }
            }

            var _pageRulesEquals = _pageRules.Where(p => p.IsEqual).ToList();
            foreach (var _pageRule in _pageRulesEquals)
            {
                if (_sueetieRequest.Url.Equals(_pageRule.UrlExcerpt, StringComparison.OrdinalIgnoreCase))
                {
                    _sueetieRequest.Url = _pageRule.UrlFinal;
                    _sueetieRequest.PageTitle = _pageRule.PageTitle;
                }
            }
        }

        public static void LogRequest(string _pageTitle, int _currentContentID, int _currentUserID)
        {
            var _loggingOn = SiteSettings.Instance.RecordAnalytics;
            var request = HttpContext.Current.Request;
            if (_loggingOn)
            {
                var _sueetieRequest = new SueetieRequest();
                _sueetieRequest.PageTitle = DataHelper.NAit(_pageTitle);
                _sueetieRequest.Url = SueetieUrlHelper.PrepUrlForLogging(request.RawUrl);
                _sueetieRequest.ApplicationID = SueetieApplications.Current.ApplicationID;
                _sueetieRequest.ContentID = _currentContentID;
                _sueetieRequest.UserID = _currentUserID;
                _sueetieRequest.RemoteIP = request.UserHostAddress;
                _sueetieRequest.UserAgent = request.UserAgent;
                _sueetieRequest.RecipientID = _currentContentID > 1 ? -1 : SueetieUrlHelper.GetRecipientID(request.RawUrl);
                _sueetieRequest.ContactTypeID = _currentContentID > 1 ? 0 : SueetieUrlHelper.GetContactTypeID(request.RawUrl);
                ApplyPageRules(_sueetieRequest);
                //_sueetieRequest.PageTitle = SueetieUrlHelper.PrepTitleForLogging(DataHelper.NAit(_pageTitle), request.RawUrl);
                //_sueetieRequest.Url = SueetieUrlHelper.PrepUrlForLogging(request.RawUrl);
                try
                {
                    if (!IsCrawler &&
                        !IsNoLogUrl &&
                        !SueetieContext.Current.User.IsFiltered &&
                        !IsFilteredUrl &&
                        !SueetieUrlHelper.IsAdminPage(request.RawUrl))
                    {
                        //SueetieDataProvider provider = SueetieDataProvider.LoadProvider();
                        //provider.AddSueetieRequest(_sueetieRequest);

                        SueetieThreads.FireAndForget(new SueetieThreads.RequestLogInsertDelegate(SueetieThreads.PerformRequestLogInsert), _sueetieRequest);
                    }
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Response.Write(ex.Message);
                }
            }
        }

        public static void CreatePageRule(string urlExcerpt, string urlFinal, string pageTitle, string isEqual)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.CreatePageRule(new PageRule { UrlExcerpt = urlExcerpt, UrlFinal = urlFinal, PageTitle = pageTitle, IsEqual = isEqual.ToBoolean() });
            SueetieCache.Current.Remove(PageRuleListCacheKey());
        }

        public static List<PageRule> GetPageRuleList()
        {
            var key = PageRuleListCacheKey();

            var pageRules = SueetieCache.Current[key] as List<PageRule>;
            if (pageRules == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                pageRules = provider.GetPageRuleList();
                SueetieCache.Current.Insert(key, pageRules);
            }

            return pageRules;
        }

        public static string PageRuleListCacheKey()
        {
            return string.Format("PageRuleList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static void ClearPageRuleListCache()
        {
            SueetieCache.Current.Remove(PageRuleListCacheKey());
        }


        public static void UpdatePageRule(int pageRuleID, string urlExcerpt, string urlFinal, string pageTitle, string isEqual)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.UpdatePageRule(new PageRule
            {
                PageRuleID = pageRuleID,
                UrlExcerpt = urlExcerpt,
                UrlFinal = urlFinal,
                PageTitle = pageTitle,
                IsEqual = isEqual.ToBoolean()
            });
            SueetieCache.Current.Remove(PageRuleListCacheKey());
        }

        public static void DeletePageRule(int pageRuleID)
        {
            var dp = SueetieDataProvider.LoadProvider();
            dp.DeletePageRule(pageRuleID);
            ClearPageRuleListCache();
        }

        public static bool IsNoLogUrl
        {
            get
            {
                var context = HttpContext.Current;
                if (context != null)
                {
                    var request = context.Request;
                    var _rawUrl = request.RawUrl;
                    foreach (var _noLogUrl in GetNoLogs())
                    {
                        if (request.RawUrl.ToLowerInvariant().Contains(_noLogUrl.UniquePathExcerpt.ToLower()))
                            return true;
                    }
                }
                return false;
            }
        }

        private static readonly string nologCacheKey = SueetieConfiguration.Get().Core.SiteUniqueName + "-SueetieNoLogUrls";
        private static readonly object nologLocker = new object();
        private static List<SueetieNoLog> _urls = new List<SueetieNoLog>();

        private static List<SueetieNoLog> GetNoLogs()
        {
            var urls = SueetieCache.Current[nologCacheKey] as List<SueetieNoLog>;
            var nologConfig = HttpContext.Current.Server.MapPath("/util/config/nolog.config");
            var dp = new CacheDependency(nologConfig);

            if (urls == null)
            {
                lock (nologLocker)
                {
                    urls = SueetieCache.Current[nologCacheKey] as List<SueetieNoLog>;
                    if (urls == null)
                    {
                        urls = LoadUrls();
                        SueetieCache.Current.InsertMax(nologCacheKey, urls, dp);
                    }
                }
            }
            return urls;
        }

        private static List<SueetieNoLog> LoadUrls()
        {
            var _context = SueetieContext.Current;
            var urlConfig = HttpContext.Current.Server.MapPath("/util/config/nolog.config");

            var cacheKey = string.Format("{0}-{1}", _context.Core.SiteUniqueName, "-SueetieNoLogConfig");

            _urls = SueetieCache.Current[cacheKey] as List<SueetieNoLog>;
            if (_urls == null)
            {
                _urls = new List<SueetieNoLog>();
                var doc = XDocument.Load(urlConfig);
                var dp = new CacheDependency(urlConfig);

                var urls = from url in doc.Descendants("url")
                           select new SueetieNoLog
                           {
                               Name = (string)url.Attribute("name"),
                               UniquePathExcerpt = (string)url.Attribute("uniquePathExcerpt")
                           };
                _urls = urls.ToList();
                SueetieCache.Current.InsertMax(cacheKey, _urls, dp);
            }

            return _urls;
        }

        public static int GetEventLogCount()
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetEventLogCount();
        }

        public static void ClearEventLog()
        {
            var dp = SueetieDataProvider.LoadProvider();
            dp.ClearEventLog();
        }
    }
}