// -----------------------------------------------------------------------
// <copyright file="SueetieContext.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Security;
    using System.Web.UI;

    public class SueetieContext
    {
        private SiteSettings _siteSettings;
        private SiteStatistics _siteStatistics;
        private SueetieConfiguration _config;
        private HttpContext _httpContext = null;
        private SueetieUrls _sueetieUrls;
        private string[] _userRoles;


        private static SueetieContext _currentInstance = new SueetieContext();

        public static SueetieContext Current
        {
            get
            {
                var currentPage = HttpContext.Current.Handler as Page;
                if (currentPage == null)
                {
                    return _currentInstance;
                }
                // save the SueetieContext to base classes
                return (currentPage.Items["CurrentPageContext"] ?? (currentPage.Items["CurrentPageContext"] = new SueetieContext())) as SueetieContext;
            }
        }

        public bool IsAnonymousUser
        {
            get
            {
                if (HostingEnvironment.IsHosted)
                {
                    HttpContext current = HttpContext.Current;
                    if (current != null)
                    {
                        return !current.User.Identity.IsAuthenticated;
                    }
                }

                var user = Membership.GetUser();
                if (user != null)
                    return false;
                return true;
            }
        }

        public SueetieUser User
        {
            get
            {
                if (!this.IsAnonymousUser)
                {
                    var _user = new SueetieUser();
                    var user = Membership.GetUser();
                    _user = SueetieUsers.GetUser(user.UserName);
                    return _user;
                }

                return SueetieUsers.GetAnonymousUser();
            }
        }

        // Cookies a good idea if using single browser, but problematic for users with multiple browsers.  Bad idea
        //public string UserDisplayName
        //{
        //    get
        //    {
        //        HttpCookie cookie = HttpContext.Current.Request.Cookies["SueetieUserProfile"];
        //        if (cookie == null)
        //            return this.User.DisplayName;

        //        if (string.IsNullOrEmpty(cookie["DisplayName"]))
        //            return this.User.DisplayName;

        //        return cookie["DisplayName"];
        //    }
        //}

        //public string UserAvatarRoot
        //{
        //    get
        //    {
        //        HttpCookie cookie = HttpContext.Current.Request.Cookies["SueetieUserProfile"];
        //        if (cookie == null)
        //            return this.User.DisplayName;

        //        if (string.IsNullOrEmpty(cookie["AvatarRoot"]))
        //            return this.User.DisplayName;

        //        return cookie["AvatarRoot"];
        //    }
        //}

        public SueetieUser ActiveUser { get; set; }

        public SueetieUserProfile UserProfile
        {
            get
            {
                if (!this.IsAnonymousUser)
                {
                    var userID = SueetieUsers.GetUserID(this.User.UserName);
                    return SueetieUsers.GetSueetieUserProfile(userID);
                }
                return SueetieUsers.GetAnonymousUserProfile();
            }
        }

        public SueetieQStringHelper QueryIDs { get; set; }

        public SiteSettings SiteSettings
        {
            get
            {
                if (this._siteSettings == null)
                    this._siteSettings = SiteSettings.Instance;
                return this._siteSettings;
            }
            set { this._siteSettings = value; }
        }

        public SiteStatistics SiteStatistics
        {
            get
            {
                if (this._siteStatistics == null)
                    this._siteStatistics = SiteStatistics.Instance;
                return this._siteStatistics;
            }
            set { this._siteStatistics = value; }
        }

        public SueetieUrls Urls
        {
            get
            {
                if (this._sueetieUrls == null)
                    this._sueetieUrls = SueetieUrls.Instance;
                return this._sueetieUrls;
            }
            set { this._sueetieUrls = value; }
        }

        public SueetieConfiguration Config
        {
            get
            {
                if (this._config == null)
                    this._config = SueetieConfiguration.Get();

                return this._config;
            }
        }

        public CoreSetting Core
        {
            get
            {
                if (this._config == null)
                    this._config = SueetieConfiguration.Get();
                return this._config.Core;
            }
        }

        public string[] UserRoles
        {
            get
            {
                if (this._userRoles == null)
                    this._userRoles = SueetieRoles.GetRolesForUser(this.User.UserName);
                return this._userRoles;
            }
        }

        public string SiteUrl()
        {
            var hostName = this._httpContext.Request.Url.Host.Replace("www.", string.Empty);
            var applicationPath = this._httpContext.Request.ApplicationPath;

            if (applicationPath.EndsWith("/"))
                applicationPath = applicationPath.Remove(applicationPath.Length - 1, 1);

            return hostName + applicationPath;
        }

        public string UserIP
        {
            get { return this._httpContext.Request.ServerVariables["REMOTE_ADDR"]; }
        }

        public string RawUrl
        {
            get { return HttpContext.Current.Request.RawUrl; }
        }

        public string Theme
        {
            get
            {
                if (SueetieUIHelper.IsMobile && !string.IsNullOrEmpty(this.SiteSettings.MobileTheme))
                    return this.SiteSettings.MobileTheme;
                return this.SiteSettings.Theme;
            }
        }

        public bool IsMobile
        {
            get { return SueetieUIHelper.IsMobile; }
        }

        public bool IsNonApplicationPage
        {
            get
            {
                var app = SueetieApplications.Current;
                if (app != null && app.ApplicationTypeID == (int)SueetieApplicationType.Unknown)
                    return true;
                return false;
            }
        }

        public SueetieApplication Application
        {
            get { return SueetieApplications.Current; }
        }

        public SueetieContentPage ContentPage
        {
            get { return SueetieContentParts.CurrentContentPage; }
        }

        public SueetieCalendar Calendar
        {
            get { return SueetieCalendars.GetCurrentCalendar(); }
        }
    }
}