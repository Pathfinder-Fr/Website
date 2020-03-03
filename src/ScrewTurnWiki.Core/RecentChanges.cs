
using ScrewTurn.Wiki.PluginFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScrewTurn.Wiki
{

    /// <summary>
    /// Manages the Wiki's Recent Changes.
    /// </summary>
    public static class RecentChanges
    {

        /// <summary>
        /// Gets all the changes, sorted by date/time ascending.
        /// </summary>
        public static IEnumerable<RecentChange> GetAllChanges()
        {
            return Settings.Provider.GetRecentChanges();
        }

        /// <summary>
        /// Adds a new change.
        /// </summary>
        /// <param name="page">The page name.</param>
        /// <param name="title">The page title.</param>
        /// <param name="messageSubject">The message subject.</param>
        /// <param name="dateTime">The date/time.</param>
        /// <param name="user">The user.</param>
        /// <param name="change">The change.</param>
        /// <param name="descr">The description (optional).</param>
        public static void AddChange(string page, string title, string messageSubject, DateTime dateTime, string user, Change change, string descr)
        {
            var allChanges = Settings.Provider.GetRecentChanges();
            if (allChanges.Count > 0)
            {
                RecentChange lastChange = allChanges[allChanges.Count - 1];
                if (lastChange.Page == page && lastChange.Title == title &&
                    lastChange.MessageSubject == messageSubject + "" &&
                    lastChange.User == user &&
                    lastChange.Change == change &&
                    (dateTime - lastChange.DateTime).TotalMinutes <= 60)
                {

                    // Skip this change
                    return;
                }
            }

            Settings.Provider.AddRecentChange(page, title, messageSubject, dateTime, user, change, descr);
        }

    }

}
