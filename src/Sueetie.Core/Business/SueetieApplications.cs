// -----------------------------------------------------------------------
// <copyright file="SueetieApplications.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    [Serializable]
    public class SueetieGroup
    {
        public int GroupID { get; set; }
        public string GroupKey { get; set; }
        public string GroupName { get; set; }
        public string GroupAdminRole { get; set; }
        public string GroupUserRole { get; set; }
        public string GroupDescription { get; set; }
        public int GroupTypeID { get; set; }
        public bool IsActive { get; set; }
        public bool HasAvatar { get; set; }
    }

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_vw_application.
    /// </summary>
    [Serializable]
    public class SueetieApplication
    {
        public int ApplicationID { get; set; }
        public int ApplicationTypeID { get; set; }
        public string ApplicationKey { get; set; }
        public int ParentID { get; set; }
        public string Description { get; set; }
        public int GroupID { get; set; }
        public bool IsActive { get; set; }
        public bool IsGroup { get; set; }
        public string GroupKey { get; set; }
        public string ApplicationName { get; set; }
        public bool IsLocked { get; set; }
        public SueetieGroup Group { get; set; }
    }

    public class SueetieApplications
    {
        private static readonly string appKey = "SueetieApplications";
        private static readonly object appLocker = new object();

        private enum SueetieNativeApplications
        {
            RootWeb = 0,
            Blog = 1,
            Forum = 2,
            Wiki = 3,
            MediaGallery = 4,
            Marketplace = 5,
            Classifieds = 6,
            CMS = 7
        }

        public List<SueetieApplication> All { get; set; }
        public List<SueetieGroup> Groups { get; set; }

        public SueetieApplication Forum
        {
            get
            {
                var _sueetieApplication = new SueetieApplication { ApplicationID = -1 };
                foreach (var app in this.All)
                {
                    if (app.ApplicationID == (int)SueetieNativeApplications.Forum)
                        _sueetieApplication = app;
                }
                return _sueetieApplication;
            }
        }

        public SueetieApplication Blog
        {
            get
            {
                var _sueetieApplication = new SueetieApplication { ApplicationID = -1 };
                foreach (var app in this.All)
                {
                    if (app.ApplicationID == (int)SueetieNativeApplications.Blog)
                        _sueetieApplication = app;
                }
                return _sueetieApplication;
            }
        }

        public SueetieApplication Wiki
        {
            get
            {
                var _sueetieApplication = new SueetieApplication { ApplicationID = -1 };
                foreach (var app in this.All)
                {
                    if (app.ApplicationID == (int)SueetieNativeApplications.Wiki)
                        _sueetieApplication = app;
                }
                return _sueetieApplication;
            }
        }

        public SueetieApplication MediaGallery
        {
            get
            {
                var _sueetieApplication = new SueetieApplication { ApplicationID = -1 };
                foreach (var app in this.All)
                {
                    if (app.ApplicationID == (int)SueetieNativeApplications.MediaGallery)
                        _sueetieApplication = app;
                }
                return _sueetieApplication;
            }
        }

        public SueetieApplication Marketplace
        {
            get
            {
                var _sueetieApplication = new SueetieApplication { ApplicationID = -1 };
                foreach (var app in this.All)
                {
                    if (app.ApplicationID == (int)SueetieNativeApplications.Marketplace)
                        _sueetieApplication = app;
                }
                return _sueetieApplication;
            }
        }

        public SueetieApplication Classifieds
        {
            get
            {
                var _sueetieApplication = new SueetieApplication { ApplicationID = -1 };
                foreach (var app in this.All)
                {
                    if (app.ApplicationID == (int)SueetieNativeApplications.Classifieds)
                        _sueetieApplication = app;
                }
                return _sueetieApplication;
            }
        }

        public SueetieApplication CMS
        {
            get
            {
                var _sueetieApplication = new SueetieApplication { ApplicationID = -1 };
                foreach (var app in this.All)
                {
                    if (app.ApplicationID == (int)SueetieNativeApplications.CMS)
                        _sueetieApplication = app;
                }
                return _sueetieApplication;
            }
        }

        public SueetieApplications()
        {
            this.Load();
        }

        public static SueetieApplication Prior
        {
            get
            {
                if (HttpContext.Current.Request.UrlReferrer != null)
                    return GetApplication(HttpContext.Current.Request.UrlReferrer);
                return GetDefaultApplication();
            }
        }

        public static SueetieApplication Current
        {
            get
            {
                var httpContext = HttpContext.Current;

                if (httpContext == null || httpContext.Request == null)
                {
                    return GetDefaultApplication();
                }

                var fullQualifiedUrl = SueetieUrlHelper.GetFullyQualifiedUrl(httpContext.Request.Url.ToString(), httpContext.Request.RawUrl);

                if (fullQualifiedUrl == null)
                {
                    return GetDefaultApplication();
                }

                var appUrl = new Uri(fullQualifiedUrl);

                return GetApplication(appUrl) ?? GetDefaultApplication();
            }
        }


        private static SueetieApplication GetApplication(Uri uri)
        {
            SueetieApplication sueetieApplication = null;

            //   uri = new Uri(HttpContext.Current.Request.RawUrl);

            // if (uri.Segments.Length > 2)
            if (uri.AbsolutePath.LastIndexOf("/") > 0)
            {
                var keyElement = uri.Segments.ElementAt(1).Replace("/", string.Empty);
                var groupFolder = SiteSettings.Instance.GroupsFolderName;
                if (keyElement == groupFolder && uri.Segments.Length > 3)
                {
                    var groupName = uri.Segments.ElementAt(2).Replace("/", string.Empty);
                    var groupAppKey = uri.Segments.ElementAt(3).Replace("/", string.Empty);
                    var a = from app in Get().All
                        where app.GroupKey == groupName && app.ApplicationKey == groupAppKey
                        select app;
                    try
                    {
                        sueetieApplication = a.Single();
                    }
                    catch
                    {
                        sueetieApplication = GetDefaultApplication();
                    }
                }
                else
                {
                    var b = from app in Get().All
                        where app.ApplicationKey.ToLower() == keyElement.ToLower() && app.GroupKey == null
                        select app;
                    try
                    {
                        var _sueetieApplication = b.FirstOrDefault();

                        if (_sueetieApplication != null)
                        {
                            var g = from grp in Get().Groups
                                where grp.GroupID == 0
                                select grp;
                            _sueetieApplication.Group = g.Single();

                            sueetieApplication = _sueetieApplication;
                        }
                    }
                    catch
                    {
                        sueetieApplication = GetDefaultApplication();
                    }
                }
            }
            else
            {
                sueetieApplication = GetDefaultApplication();
            }

            return sueetieApplication;
        }

        private static SueetieApplication GetDefaultApplication()
        {
            var a = from app in Get().All
                where app.ApplicationKey == "na" && app.GroupID == 0
                select app;
            var _application = a.Single();

            var g = from grp in Get().Groups
                where grp.GroupID == 0
                select grp;
            _application.Group = g.Single();
            return _application;
        }

        public static SueetieApplications Get()
        {
            var apps = SueetieCache.Current[appKey] as SueetieApplications;
            if (apps == null)
            {
                lock (appLocker)
                {
                    apps = SueetieCache.Current[appKey] as SueetieApplications;
                    if (apps == null)
                    {
                        apps = new SueetieApplications();
                        SueetieCache.Current.InsertMax(appKey, apps);
                    }
                }
            }
            return apps;
        }

        private void Load()
        {
            this.All = SueetieCommon.GetSueetieApplicationsList();
            this.Groups = SueetieCommon.GetSueetieGroupList();
        }
    }
}