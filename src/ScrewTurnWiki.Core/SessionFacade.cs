
using ScrewTurn.Wiki.PluginFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ScrewTurn.Wiki
{

    /// <summary>
    /// Exposes in a strongly-typed fashion the Session variables.
    /// </summary>
    public static class SessionFacade
    {

        /// <summary>
        /// Gets the current Session object.
        /// </summary>
        private static HttpSessionState Session
        {
            get
            {
                if (HttpContext.Current == null) return null;
                else return HttpContext.Current.Session;
            }
        }

        /// <summary>
        /// Gets or sets the Login Key.
        /// </summary>
        public static string LoginKey
        {
            get { return Session != null ? (string)Session["LoginKey"] : null; }
            set { if (Session != null) Session["LoginKey"] = value; }
        }

        /// <summary>
        /// Gets or sets the current username, or <c>null</c>.
        /// </summary>
        public static string CurrentUsername
        {
            get { return Session != null ? Session["Username"] as string : null; }
            set { if (Session != null) Session["Username"] = value; }
        }

        /// <summary>
        /// Gets the current user, if any.
        /// </summary>
        /// <returns>The current user, or <c>null</c>.</returns>
        public static UserInfo GetCurrentUser()
        {
            if (Session == null)
            {
                return null;
            }

            var sessionId = Session.SessionID;

            UserInfo current = SessionCache.GetCurrentUser(sessionId);
            if (current != null)
            {
                return current;
            }

            var un = CurrentUsername;
            if (string.IsNullOrEmpty(un))
            {
                return null;
            }

            if (un == AnonymousUsername)
            {
                return Users.GetAnonymousAccount();
            }

            current = Users.FindUser(un);
            if (current == null)
            {
                // Username is invalid
                Session.Clear();
                Session.Abandon();
                return null;
            }

            SessionCache.SetCurrentUser(sessionId, current);
            return current;

        }

        /// <summary>
        /// The username for anonymous users.
        /// </summary>
        public const string AnonymousUsername = "|";

        /// <summary>
        /// Gets the current username for security checks purposes only.
        /// </summary>
        /// <returns>The current username, or <b>AnonymousUsername</b> if anonymous.</returns>
        public static string GetCurrentUsername()
        {
            var un = CurrentUsername;
            if (string.IsNullOrEmpty(un)) return AnonymousUsername;
            else return un;
        }

        private static UserGroup[] anonymousGroups;

        /// <summary>
        /// Gets the current user groups.
        /// </summary>
        public static IEnumerable<UserGroup> GetCurrentGroups()
        {
            if (Session == null)
            {
                return Array.Empty<UserGroup>();
            }

            var sessionId = Session.SessionID;

            var groups = SessionCache.GetCurrentGroups(sessionId);
            if (groups != null && groups.Length != 0)
            {
                return groups;
            }

            var current = GetCurrentUser();
            if (current == null)
            {
                return anonymousGroups ?? (anonymousGroups = new UserGroup[] { Users.FindUserGroup(Settings.AnonymousGroup) });
            }

            // This check is necessary because after group deletion the session might contain outdated data
            groups = current.Groups.Select(g => Users.FindUserGroup(g)).Where(u => u != null).ToArray();
            SessionCache.SetCurrentGroups(sessionId, groups);
            return groups;
        }

        /// <summary>
        /// Gets the current group names.
        /// </summary>
        /// <returns>The group names.</returns>
        public static IEnumerable<string> GetCurrentGroupNames() => GetCurrentGroups().Select(x => x.Name);

        /// <summary>
        /// Gets the Breadcrumbs Manager.
        /// </summary>
        public static BreadcrumbsManager Breadcrumbs
        {
            get { return new BreadcrumbsManager(); }
        }

    }

    /// <summary>
    /// Implements a session data cache whose lifetime is only limited to one request.
    /// </summary>
    public static class SessionCache
    {

        private static Dictionary<string, UserInfo> currentUsers = new Dictionary<string, UserInfo>(100);
        private static Dictionary<string, UserGroup[]> currentGroups = new Dictionary<string, UserGroup[]>(100);

        /// <summary>
        /// Gets the current user, if any, of a session.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <returns>The current user, or <c>null</c>.</returns>
        public static UserInfo GetCurrentUser(string sessionId)
        {
            lock (currentUsers)
            {
                UserInfo result = null;
                currentUsers.TryGetValue(sessionId, out result);
                return result;
            }
        }

        /// <summary>
        /// Sets the current user of a session.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <param name="user">The user.</param>
        public static void SetCurrentUser(string sessionId, UserInfo user)
        {
            lock (currentUsers)
            {
                currentUsers[sessionId] = user;
            }
        }

        /// <summary>
        /// Gets the current groups, if any, of a session.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <returns>The groups, or <b>null.</b></returns>
        public static UserGroup[] GetCurrentGroups(string sessionId)
        {
            lock (currentGroups)
            {
                UserGroup[] result = null;
                currentGroups.TryGetValue(sessionId, out result);
                return result;
            }
        }

        /// <summary>
        /// Sets the current groups of a session.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <param name="groups">The groups.</param>
        public static void SetCurrentGroups(string sessionId, UserGroup[] groups)
        {
            lock (currentGroups)
            {
                currentGroups[sessionId] = groups;
            }
        }

        /// <summary>
        /// Clears all cached data of a session.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        public static void ClearData(string sessionId)
        {
            lock (currentUsers)
            {
                currentUsers.Remove(sessionId);
            }
            lock (currentGroups)
            {
                currentGroups.Remove(sessionId);
            }
        }

    }

}
