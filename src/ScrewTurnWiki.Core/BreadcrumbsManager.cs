using ScrewTurn.Wiki.PluginFramework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ScrewTurn.Wiki
{

    /// <summary>
    /// Manages navigation Breadcrumbs.
    /// </summary>
    public class BreadcrumbsManager
    {

        private const int MaxPages = 10;
        private const string CookieName = "ScrewTurnWikiBreadcrumbs3";
        private const string CookieValue = "B";

        private readonly List<string> pageFullNames = new List<string>(MaxPages);

        /// <summary>
        /// Initializes a new instance of the <b>BreadcrumbsManager</b> class.
        /// </summary>
        public BreadcrumbsManager()
        {
            HttpCookie cookie = GetCookie();
            if (cookie != null && !string.IsNullOrEmpty(cookie.Values[CookieValue]))
            {
                try
                {
                    foreach (var p in cookie.Values[CookieValue].Split('|'))
                    {
                        pageFullNames.Add(p);
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Gets the cookie.
        /// </summary>
        /// <returns>The cookie, or <c>null</c>.</returns>
        private HttpCookie GetCookie()
        {
            if (HttpContext.Current.Request != null)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieName];
                return cookie;
            }

            else return null;
        }

        /// <summary>
        /// Updates the cookie.
        /// </summary>
        private void UpdateCookie()
        {
            HttpCookie cookie = GetCookie();
            if (cookie == null)
            {
                cookie = new HttpCookie(CookieName);
            }
            cookie.Path = Settings.CookiePath;

            var sb = new StringBuilder(MaxPages * 20);
            foreach (var page in pageFullNames)
            {
                if (sb.Length != 0)
                    sb.Append('|');

                sb.Append(page);
            }

            cookie.Values[CookieValue] = sb.ToString();
            if (HttpContext.Current.Response != null)
            {
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
            if (HttpContext.Current.Request != null)
            {
                HttpContext.Current.Request.Cookies.Set(cookie);
            }
        }

        /// <summary>
        /// Adds a Page to the Breadcrumbs trail.
        /// </summary>
        /// <param name="page">The Page to add.</param>
        public void AddPage(PageInfo page)
        {
            lock (this)
            {
                var index = FindPage(page);
                if (index != -1) pageFullNames.RemoveAt(index);

                pageFullNames.Add(page.FullName);

                if (pageFullNames.Count > MaxPages) pageFullNames.RemoveRange(0, pageFullNames.Count - MaxPages);

                UpdateCookie();
            }
        }

        /// <summary>
        /// Finds a page by name.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>The index in the collection.</returns>
        private int FindPage(PageInfo page)
        {
            lock (this)
            {
                if (pageFullNames.Count == 0) return -1;

                var comp = new PageNameComparer();

                for (var i = 0; i < pageFullNames.Count; i++)
                {
                    if (string.Equals(pageFullNames[i], page.FullName, System.StringComparison.OrdinalIgnoreCase)) return i;
                }

                return -1;
            }
        }

        /// <summary>
        /// Removes a Page from the Breadcrumbs trail.
        /// </summary>
        /// <param name="page">The Page to remove.</param>
        public void RemovePage(PageInfo page)
        {
            lock (this)
            {
                var index = FindPage(page);
                if (index >= 0) pageFullNames.RemoveAt(index);

                UpdateCookie();
            }
        }

        /// <summary>
        /// Clears the Breadcrumbs trail.
        /// </summary>
        public void Clear()
        {
            lock (this)
            {
                pageFullNames.Clear();

                UpdateCookie();
            }
        }

        /// <summary>
        /// Gets all the Pages in the trail that still exist.
        /// </summary>
        public List<PageInfo> LoadAllPages() => pageFullNames.Select(p => Pages.FindPage(p)).Where(p => p != null).ToList();
    }

}
