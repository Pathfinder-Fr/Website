using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Collections.Specialized;
using System.IO;
using Sueetie.Core;

namespace Saltie.Core
{
    public class SaltieContext
    {

        private SiteSettings _siteSettings = null;
        private SiteStatistics _siteStatistics = null;
        private SaltieConfiguration _config = null;
        private HttpContext _httpContext = null;
        private SaltieUrls _SaltieUrls = null;
        private string[] _userRoles = null;



        private static SaltieContext _currentInstance = new SaltieContext();
        public static SaltieContext Current
        {
            get
            {
                Page currentPage = HttpContext.Current.Handler as Page;
                if (currentPage == null)
                {
                    return _currentInstance;
                }
                // save the SaltieContext to base classes
                return (currentPage.Items["CurrentPageContext"] ?? (currentPage.Items["CurrentPageContext"] = new SaltieContext())) as SaltieContext;
            }
        }

        public bool IsAnonymousUser
        {
            get
            {
                MembershipUser user = Membership.GetUser();
                if (user != null)
                    return false;
                else
                    return true;
            }
        }
        public SueetieUser User
        {
            get
            {
                if (!IsAnonymousUser)
                {
                    SueetieUser _user = new SueetieUser();
                    MembershipUser user = Membership.GetUser();
                    _user = SueetieUsers.GetUser(user.UserName);
                    return _user;
                }
                else
                    return SueetieUsers.GetAnonymousUser();

            }
        }

        public SueetieUser ActiveUser { get; set; }

        public SueetieUserProfile UserProfile
        {
            get
            {
                if (!IsAnonymousUser)
                {

                    int userID = SueetieUsers.GetUserID(this.User.UserName);
                    return SueetieUsers.GetSueetieUserProfile(userID);
                }
                else
                    return SueetieUsers.GetAnonymousUserProfile();
            }
        }
        public SueetieQStringHelper QueryIDs { get; set; }

        public SiteSettings SiteSettings
        {
            get
            {
                if (_siteSettings == null)
                    _siteSettings = SiteSettings.Instance;
                return _siteSettings;
            }
            set { _siteSettings = value; }
        }
        public SiteStatistics SiteStatistics
        {
            get
            {
                if (_siteStatistics == null)
                    _siteStatistics = SiteStatistics.Instance;
                return _siteStatistics;
            }
            set { _siteStatistics = value; }
        }

        public SaltieUrls Urls
        {
            get
            {
                if (_SaltieUrls == null)
                    _SaltieUrls = SaltieUrls.Instance;
                return _SaltieUrls;
            }
            set { _SaltieUrls = value; }
        }

        public SaltieConfiguration Config
        {
            get
            {
                if (_config == null)
                    _config = SaltieConfiguration.Get();

                return _config;
            }
        }

        public CoreSetting Core
        {
            get
            {
                if (_config == null)
                    _config = SaltieConfiguration.Get();
                return _config.Core;
            }
        }

        public string[] UserRoles
        {
            get
            {
                if (_userRoles == null)
                    _userRoles = SueetieRoles.GetRolesForUser(this.User.UserName);
                return _userRoles;
            }
        }

    }
}
