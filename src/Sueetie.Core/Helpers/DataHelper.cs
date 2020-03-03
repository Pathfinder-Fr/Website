// -----------------------------------------------------------------------
// <copyright file="DataHelper.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    public static class DataHelper
    {
        private const string EmptyGuid = "00000000-0000-0000-0000-000000000000";

        public static int GetIntFromQueryString(string key, int defaultValue)
        {
            string _value = null;

            var QueryString = HttpContext.Current.Request.QueryString;
            if (QueryString[key] != null)
                _value = QueryString[key];

            if (_value == null)
                return defaultValue;
            try
            {
                defaultValue = Convert.ToInt32(_value);
            }
            catch
            {
            }

            return defaultValue;
        }

        public static bool GetBoolFromQueryString(string key, bool defaultValue)
        {
            string _value = null;

            var QueryString = HttpContext.Current.Request.QueryString;
            if (QueryString[key] != null)
                _value = QueryString[key];

            if (_value == null)
                return defaultValue;
            try
            {
                defaultValue = Convert.ToBoolean(_value);
            }
            catch
            {
            }

            return defaultValue;
        }

        public static Guid GetGuid(IDataRecord dr, string _fieldname)
        {
            var _guid = new Guid(EmptyGuid);
            try
            {
                _guid = dr.GetGuid(dr.GetOrdinal(_fieldname));
            }
            catch
            {
            }
            return _guid;
        }

        public static float GetFloat(IDataRecord dr, string _fieldname)
        {
            return dr.GetFloat(dr.GetOrdinal(_fieldname));
        }

        public static object StringOrNull(string text)
        {
            if (string.IsNullOrEmpty(text))
                return DBNull.Value;

            return text;
        }

        public static object StringOrEmpty(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            return text;
        }

        public static string StringOrDefault(string _text, string _default)
        {
            if (string.IsNullOrEmpty(_text))
                return _default;

            return _text;
        }

        public static string StringOrDash(string _text)
        {
            if (string.IsNullOrEmpty(_text))
                return "-";

            return _text;
        }

        public static int IntOrDefault(string text, int _default)
        {
            if (string.IsNullOrEmpty(text))
                return _default;

            return Convert.ToInt16(text);
        }

        public static int BoolToBit(bool _boolValue)
        {
            var i = _boolValue ? 1 : 0;
            return i;
        }

        public static string YesNo(bool _boolValue)
        {
            var s = _boolValue ? "Yes" : "No";
            return s;
        }

        public static bool BitToBool(string _boolString)
        {
            if (!string.IsNullOrEmpty(_boolString))
            {
                var b = _boolString.Trim() == "1" ? true : false;
                return b;
            }
            return false;
        }

        public static bool StringToBool(string _boolString)
        {
            if (!string.IsNullOrEmpty(_boolString))
            {
                var b = _boolString.Trim().ToLower() == "true" ? true : false;
                return b;
            }
            return false;
        }

        public static DateTime DateOrNull(string text)
        {
            if (string.IsNullOrEmpty(text))
                return Convert.ToDateTime("6/9/1969");

            return Convert.ToDateTime(text);
        }

        public static string NAit(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "NA";
            return text;
        }

        public static string NAIntIt(int _int)
        {
            if (_int < 0)
                return "NA";
            return _int.ToString();
        }

        public static string NAit(DateTime _date)
        {
            if (_date.Year == 1969)
                return "NA";
            return _date.ToShortDateString();
        }

        public static string DashIt(DateTime _date)
        {
            if (_date.Year == 1969)
                return "-";
            return _date.ToShortDateString();
        }

        public static string NotAvailableIt(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "Not Available";
            return text;
        }

        public static string AllCategoriesIt(string text)
        {
            if (string.IsNullOrEmpty(text) || text.ToLower() == "na")
                return "All Categories";
            return text;
        }

        public static bool hasHTTP(string text)
        {
            var start = text.ToLower().IndexOf("http://");
            if (start > -1)
                return true;
            return false;
        }

        public static string UrlIt(string text, string defaultOut)
        {
            var url = string.Empty;
            if (string.IsNullOrEmpty(text))
                return defaultOut;
            if (hasHTTP(text))
                url = "<a href=\"" + text + "\">" + text + "</a>";
            else
                url = "<a href=\"http://" + text + "\">http://" + text + "</a>";
            return url;
        }

        public static string TwitterIt(string text, string defaultOut)
        {
            var url = string.Empty;
            if (string.IsNullOrEmpty(text))
                return defaultOut;
            if (text.ToLower().IndexOf("twitter") > 0)
            {
                if (hasHTTP(text))
                    url = "<a href=\"" + text + "\">" + text + "</a>";
                else
                    url = "<a href=\"http://" + text + "\">http://" + text + "</a>";
            }
            else
            {
                text = text.Replace("@", string.Empty);
                url = "<a href=\"http://twitter.com/" + text + "\">http://twitter.com/" + text + "</a>";
            }
            return url;
        }

        public static string DefaultTextIt(string text, string defaultOut)
        {
            if (string.IsNullOrEmpty(text))
                return defaultOut;
            return text;
        }

        public static string TruncateText(string text, int len)
        {
            if (text.Length > len)
                return text.Substring(0, len - 3).Trim() + "...";
            return text;
        }

        public static string TruncateTextNoElipse(string text, int len)
        {
            if (text.Length > len)
                return text.Substring(0, len - 3).Trim();
            return text;
        }

        public static string CommaTrim(string text)
        {
            var trimOut = text.Trim();
            if (trimOut.EndsWith(","))
                return trimOut.Substring(0, trimOut.Length - 1);
            return trimOut;
        }

        public static string GetSafeUri(Uri _uri)
        {
            if (_uri != null)
                return _uri.ToString();
            return null;
        }

        public static UserContent UserContentIDs(int userid, int contentid)
        {
            var _userContent = new UserContent
            {
                UserID = userid,
                ContentID = contentid
            };
            return _userContent;
        }

        public static string ContentTypeAuthoredBy(FavoriteContent favoriteContent)
        {
            var contentTypeAuthoredBy = string.Empty;
            switch (favoriteContent.ContentTypeID)
            {
                case (int)SueetieContentType.BlogPost:
                    contentTypeAuthoredBy = "Blog Post by <a href=\"/members/profile.aspx?u=" +
                                            favoriteContent.AuthorUserID + "\">" +
                                            favoriteContent.DisplayName + "</a> in the " + favoriteContent.ApplicationName;
                    break;
                case (int)SueetieContentType.BlogComment:
                    contentTypeAuthoredBy = "Blog Comment by <a href=\"/members/profile.aspx?u=" +
                                            favoriteContent.AuthorUserID + "\">" +
                                            favoriteContent.DisplayName + "</a> in the " + favoriteContent.ApplicationName;
                    break;
                case (int)SueetieContentType.ForumTopic:
                    contentTypeAuthoredBy = "Forum Topic started by <a href=\"/members/profile.aspx?u=" +
                                            favoriteContent.AuthorUserID + "\">" +
                                            favoriteContent.DisplayName + "</a> in the " + favoriteContent.ApplicationName;
                    break;
                case (int)SueetieContentType.ForumMessage:
                    contentTypeAuthoredBy = "Forum Message by <a href=\"/members/profile.aspx?u=" +
                                            favoriteContent.AuthorUserID + "\">" +
                                            favoriteContent.DisplayName + "</a> in the " + favoriteContent.ApplicationName;
                    break;

                default:
                    break;
            }
            return contentTypeAuthoredBy;
        }

        public static bool IsAFave(UserContent userContent)
        {
            var _isFave = false;
            var favoriteID = SueetieUsers.GetFavoriteID(userContent);
            if (favoriteID > 0)
                _isFave = true;
            return _isFave;
        }

        public static string GetMediaObjectUrl(int moid)
        {
            var host = HttpContext.Current.Request.Url.Host;
            var mediaPath = SueetieApplications.Get().MediaGallery.ApplicationKey;
            return string.Format("http://{0}{1}/default.aspx?moid={2}", host, mediaPath, moid);
        }

        public static string ToString(object obj, string defValue)
        {
            if ((obj != DBNull.Value) && (obj != null))
                return obj.ToString();
            return defValue;
        }

        public static string[] ToStringArray(StringCollection coll)
        {
            if (coll == null || coll.Count == 0)
            {
                return new string[0];
            }

            var strReturn = new string[coll.Count];
            coll.CopyTo(strReturn, 0);
            return strReturn;
        }

        public static string DoGroupName(string groupname)
        {
            if (groupname == "na")
                return "Not a Group";
            return groupname;
        }

        private static readonly Regex STRIP_HTML = new Regex("<[^>]*>", RegexOptions.Compiled);

        /// <summary>
        /// Strips all HTML tags from the specified string.
        /// </summary>
        /// <param name="html">The string containing HTML</param>
        /// <returns>A string without HTML tags</returns>
        public static string StripHtml(string html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            // Remove BBCode brackets
            html = html.Replace("[", "<");
            html = html.Replace("]", ">");

            return STRIP_HTML.Replace(html, string.Empty);
        }

        public static string RemoveDiacritics(string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            for (var i = 0; i < normalized.Length; i++)
            {
                var c = normalized[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString();
        }

        private static readonly Regex REGEX_LINE_BREAKS = new Regex(@"\n\s+", RegexOptions.Compiled);

        /// <summary>
        /// Removes the HTML whitespace.
        /// </summary>
        /// <param name="html">The HTML.</param>
        public static string RemoveHtmlWhitespace(string html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            html = REGEX_LINE_BREAKS.Replace(html, string.Empty);

            return html.Trim();
        }

        private const string _space = " ";

        private static string RemoveIllegalSearchBodyCharacters(string text)
        {
            text = text.Replace("&#0182;", _space);
            text = text.Replace("\n", _space);
            return text;
        }

        public static string CleanSearchBodyContent(string html)
        {
            var _cleanContent = StripHtml(html);
            _cleanContent = RemoveHtmlWhitespace(_cleanContent);
            _cleanContent = RemoveIllegalSearchBodyCharacters(_cleanContent);
            return _cleanContent;
        }

        public static string CleanTextForJScript(string text)
        {
            var _cleanContent = text.Replace("'", "\"");
            return _cleanContent;
        }

        private static string RemoveIllegalTagCharacters(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            text = text.Replace(":", string.Empty);
            text = text.Replace("/", string.Empty);
            text = text.Replace("?", string.Empty);
            text = text.Replace("#", string.Empty);
            text = text.Replace("[", string.Empty);
            text = text.Replace("]", string.Empty);
            text = text.Replace("@", string.Empty);
            text = text.Replace("*", string.Empty);
            text = text.Replace(".", string.Empty);
            text = text.Replace(",", string.Empty);
            text = text.Replace("\"", string.Empty);
            text = text.Replace("&", string.Empty);
            text = text.Replace("'", string.Empty);
            text = RemoveDiacritics(text);
            text = RemoveExtraHyphen(text);

            return HttpUtility.UrlEncode(text).Replace("%", string.Empty);
        }

        public static string PrepCategoryLink(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            text = text.Replace(":", string.Empty);
            text = text.Replace("/", string.Empty);
            text = text.Replace("?", string.Empty);
            text = text.Replace("#", string.Empty);
            text = text.Replace("[", string.Empty);
            text = text.Replace("]", string.Empty);
            text = text.Replace("@", string.Empty);
            text = text.Replace("*", string.Empty);
            text = text.Replace(".", string.Empty);
            text = text.Replace(",", string.Empty);
            text = text.Replace("\"", string.Empty);
            text = text.Replace("&", string.Empty);
            text = text.Replace("'", string.Empty);
            text = text.Replace(" ", "-");
            text = RemoveDiacritics(text);
            text = RemoveExtraHyphen(text);

            return HttpUtility.HtmlEncode(text).Replace("%", string.Empty);
        }

        public static string PrepareTag(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            var _cleanTag = RemoveIllegalTagCharacters(text);
            _cleanTag = _cleanTag.Replace(" ", "+");
            return _cleanTag;
        }

        private static string RemoveExtraHyphen(string text)
        {
            if (text.Contains("--"))
            {
                text = text.Replace("--", "-");
                return RemoveExtraHyphen(text);
            }

            return text;
        }

        public static DateTime MinDate()
        {
            return DateTime.Parse("6/9/1969").Date;
        }

        public static bool IsMinDate(DateTime _date)
        {
            if (_date.Year == 1969)
                return true;
            return false;
        }

        public static DateTime SafeMinDate(string _dateString)
        {
            if (string.IsNullOrEmpty(_dateString))
                return Convert.ToDateTime("6/9/1969");
            return Convert.ToDateTime(_dateString);
        }

        public static string ExtractNumbers(string expr)
        {
            return string.Join(null, Regex.Split(expr, "[^\\d]"));
        }
    }
}