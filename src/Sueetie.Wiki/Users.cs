
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Configuration;

namespace Sueetie.Wiki
{

    /// <summary>
    /// Implements a Users Storage Provider.
    /// </summary>
    [Serializable]
    public static class WikiUsers
    {

        /// <summary>
        /// Gets all the Users.
        /// </summary>
        /// <remarks>The array is unsorted.</remarks>
        public static UserInfo[] AllUsers(string groupName)
        {
            string tmp;
            tmp = Tools.LoadFile(SueetieSettings.UsersFile(groupName)).Replace("\r", "");
            string[] lines = tmp.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            UserInfo[] result = new UserInfo[lines.Length];
            string[] fields;
            for (int i = 0; i < lines.Length; i++)
            {
                fields = lines[i].Split('|');
                // Structure:
                // Username|PasswordHash|Email|Active-Inactive|DateTime|Admin-User
                result[i] = new UserInfo(fields[0], fields[2]);
            }
            return result;

        }

        /// <summary>
        /// Searches for a User.
        /// </summary>
        /// <param name="user">The User to search for.</param>
        /// <returns>True if the User already exists.</returns>
        private static bool Exists(UserInfo user, string groupName)
        {
            UserInfo[] users = AllUsers(groupName);
            UsernameComparer comp = new UsernameComparer();
            for (int i = 0; i < users.Length; i++)
            {
                if (comp.Compare(users[i], user) == 0) return true;
            }
            return false;
        }

        /// <summary>
        /// Adds a new User to a Screwturn Wiki 3.0 RC1 Site.
        /// </summary>
        public static bool AddUser(string username, string email, string groupName, string displayName)
        {
            if (Exists(new UserInfo(username, email), groupName)) return false;

            StringBuilder sb = new StringBuilder();
            sb.Append(username);
            sb.Append("|");
            sb.Append(WebConfigurationManager.AppSettings["SUEETIE.WikiLoginKey"].ToString());
            sb.Append("|");
            sb.Append(email);
            sb.Append("|");
            sb.Append("ACTIVE");
            sb.Append("|");
            sb.Append(DateTime.Now.ToString("yyyy'/'MM'/'dd' 'HH':'mm':'ss"));
            sb.Append("|");
            sb.Append(displayName);
            sb.Append("\r\n"); // Important
            Tools.AppendFile(SueetieSettings.UsersFile(groupName), sb.ToString());
            return true;
        }

        /// <summary>
        /// Adds a new User to a Screwturn Wiki 2.0.35 Site.
        /// </summary>
        public static bool AddUser(string username, string email, string groupName)
        {
            if (Exists(new UserInfo(username, email), groupName)) return false;

            StringBuilder sb = new StringBuilder();
            sb.Append("\r\n"); // Important
            sb.Append(username);
            sb.Append("|");
            sb.Append(WebConfigurationManager.AppSettings["SUEETIE.WikiLoginKey"].ToString());
            sb.Append("|");
            sb.Append(email);
            sb.Append("|");
            sb.Append("ACTIVE");
            sb.Append("|");
            sb.Append(DateTime.Now.ToString("yyyy'/'MM'/'dd' 'HH':'mm':'ss"));
            sb.Append("|");
            sb.Append("USER");
            Tools.AppendFile(SueetieSettings.UsersFile(groupName), sb.ToString());
            return true;
        }
    }
}        
