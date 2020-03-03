// -----------------------------------------------------------------------
// <copyright file="SueetieUrlIHelper.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Web;

    public static class SueetieUrlHelper
    {
        public static bool IsPageRequest(HttpContext context)
        {
            if (context.Request.CurrentExecutionFilePathExtension.ToLower() == ".aspx" ||
                context.Request.CurrentExecutionFilePathExtension.ToLower() == ".ashx")
                return true;
            return false;
        }

        public static string ReverseUrlCheck(string url)
        {
            var placemarkerUrl = string.Empty;
            foreach (var _sueetieUrl in SueetieUrls.Instance.All)
            {
                if (!string.IsNullOrEmpty(_sueetieUrl.RewrittenUrl) && url.ToLower() == string.Format(_sueetieUrl.RewrittenUrl, SueetieContext.Current.Theme).ToLower())
                    placemarkerUrl = _sueetieUrl.Url;
            }
            if (!string.IsNullOrEmpty(placemarkerUrl))
                return placemarkerUrl;
            return url;
        }

        public static string GetFullyQualifiedUrl(string url, string rawUrl)
        {
            return string.Concat(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority), rawUrl);
        }

        public static string[] QueryStringValues(HttpContext context)
        {
            string[] values;
            var keynum = context.Request.QueryString.AllKeys.Length;
            values = new string[keynum];
            for (var i = 0; i < keynum; i++)
            {
                values[i] = context.Request.QueryString.GetValues(i)[0];
            }
            return values;
        }

        public static string[] PopulateRewrittenUrl(HttpContext context)
        {
            string[] values;
            var keynum = context.Request.QueryString.AllKeys.Length;
            values = new string[keynum + 1];
            values[0] = SueetieContext.Current.Theme;
            for (var i = 0; i < keynum; i++)
            {
                values[i + 1] = context.Request.QueryString.GetValues(i)[0];
            }
            return values;
        }

        public static string HandleRootUrl(string _url)
        {
            var _urlOut = _url;
            if (_url == "/")
                _urlOut = "/default.aspx";
            return _urlOut;
        }

        public static string PrepUrlForLogging(string _url)
        {
            var _urlOut = _url;
            if (_url.EndsWith("/"))
                _urlOut = _url + "default.aspx";
            if (_url.Contains("/?"))
                _urlOut = _url.Replace("/?", "/default.aspx?");
            if (_url.Equals("/media/default.aspx?aid=1", StringComparison.OrdinalIgnoreCase))
                _urlOut = "/media/default.aspx";
            if ((_url.Contains("wiki/default.aspx?page=", StringComparison.OrdinalIgnoreCase) ||
                 _url.Contains("wiki/history.aspx?page=", StringComparison.OrdinalIgnoreCase) ||
                 _url.Contains("wiki/edit.aspx?page=", StringComparison.OrdinalIgnoreCase) ||
                 _url.Contains("wiki/diff.aspx?page=", StringComparison.OrdinalIgnoreCase)) && (_url.IndexOf("&") > 0))
                _urlOut = _url.Substring(0, _url.IndexOf("&")).ToLower();
            if (_url.Contains("aspx?hl=", StringComparison.OrdinalIgnoreCase))
                _urlOut = _url.Substring(0, _url.IndexOf("?")).ToLower();
            return _urlOut;
        }

        public static bool IsAdminPage(string _url)
        {
            var isAdminPage = false;

            if (_url.StartsWith("/admin/", StringComparison.OrdinalIgnoreCase) && !SiteSettings.Instance.LogAdminActivity)
                isAdminPage = true;
            return isAdminPage;
        }

        public static int GetRecipientID(string _url)
        {
            var _recipientID = -1;
            if (_url.IndexOf("yaf_profile") != -1 ||
                _url.IndexOf("yaf_im_email") != -1 ||
                _url.IndexOf("yaf_pmessage") != -1)
            {
                _recipientID = DataHelper.IntOrDefault(DataHelper.ExtractNumbers(_url), -1);
            }
            return _recipientID;
        }

        public static int GetContactTypeID(string _url)
        {
            var _contactTypeID = 0;
            if (_url.IndexOf("yaf_profile") != -1)
            {
                _contactTypeID = (int)SueetieContactType.ViewProfile;
            }
            if (_url.IndexOf("yaf_im_email") != -1)
            {
                _contactTypeID = (int)SueetieContactType.Email;
            }
            if (_url.IndexOf("yaf_pmessage") != -1)
            {
                _contactTypeID = (int)SueetieContactType.PersonalMessage;
            }
            return _contactTypeID;
        }
    }
}