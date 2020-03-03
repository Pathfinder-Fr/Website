// -----------------------------------------------------------------------
// <copyright file="SueetieExtensions.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Web;
    using System.Web.UI.HtmlControls;

    public static class SueetieExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static bool ToBoolean(this string _boolString)
        {
            if (!string.IsNullOrEmpty(_boolString))
            {
                var b = _boolString.Trim().ToLower() == "true" ? true : false;
                return b;
            }
            return false;
        }

        public static string Match(this string _string)
        {
            return _string.ToLower().Trim();
        }

        public static int ToInteger(this string _string)
        {
            int result;
            if (!int.TryParse(_string, out result))
                return -1;
            return result;
        }

        public static void LineBreak(this HtmlLink _link)
        {
            HttpContext.Current.Response.Write("\n");
        }

        public static string Truncate(this string text, int len, bool addElipse)
        {
            if (text.Length > len)
                if (addElipse)
                    return text.Substring(0, len - 3).Trim() + "...";
                else
                    return text.Substring(0, len);
            return text;
        }

        public static string Truncate(this string text, int len)
        {
            return Truncate(text, len, false);
        }

        public static string MediaThumbnailUrl(this SueetieMediaDirectory _sueetieMediaDirectory)
        {
            return MediaUrl(_sueetieMediaDirectory, SueetieImageDisplayType.Thumbnail, true);
        }

        public static string MediaOptimizedUrl(this SueetieMediaDirectory _sueetieMediaDirectory)
        {
            return MediaUrl(_sueetieMediaDirectory, SueetieImageDisplayType.Optimized, true);
        }

        public static string MediaOriginalUrl(this SueetieMediaDirectory _sueetieMediaDirectory)
        {
            return MediaUrl(_sueetieMediaDirectory, SueetieImageDisplayType.Original, true);
        }

        public static string MediaUrl(this SueetieMediaDirectory _sueetieMediaDirectory, SueetieImageDisplayType _sueetieImageDisplayType)
        {
            return MediaUrl(_sueetieMediaDirectory, _sueetieImageDisplayType, true);
        }

        public static string MediaUrl(this SueetieMediaDirectory _sueetieMediaDirectory, SueetieImageDisplayType _sueetieImageDisplayType, bool DisplayDomain)
        {
            var _host = DisplayDomain ? HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) : string.Empty;
            var _url = string.Format("{0}/{1}/gs/mediaobjects{2}{3}",
                _host, _sueetieMediaDirectory.ApplicationKey, _sueetieMediaDirectory.SueetieAlbumPath,
                SueetieMedia.GetSueetieMediaDirectoryFileName(_sueetieImageDisplayType, _sueetieMediaDirectory));
            return _url;
        }
    }
}