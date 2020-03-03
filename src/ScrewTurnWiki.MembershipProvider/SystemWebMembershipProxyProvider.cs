namespace PathfinderFr.ScrewTurnWiki.MembershipProvider
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Profile;
    using System.Web.Security;
    using ScrewTurn.Wiki.PluginFramework;

    public class SystemWebMembershipProxyProvider : IUsersStorageProviderV30
    {
        /// <summary>
        /// Cache duration for items, in minutes.
        /// </summary>
        private const int CacheDuration = 20;

        private static readonly ComponentInformation info;

        private static readonly string helpHtml;

        private static readonly Dictionary<string, string[]> userForGroupCache = new Dictionary<string, string[]>();

        private static readonly object userForGroupCacheLock = new object();

        private static DateTime userForGroupCacheCreation;

        private static UserInfo[] usersCache = null;

        private static readonly object usersCacheLock = new object();

        private static DateTime usersCacheCreation;

        private static readonly ConcurrentDictionary<string, UserInfoCacheEntry> usersByUserNameCache = new ConcurrentDictionary<string, UserInfoCacheEntry>(StringComparer.OrdinalIgnoreCase);

        private class UserInfoCacheEntry
        {
            public UserInfoCacheEntry(UserInfo userInfo)
            {
                this.UserInfo = userInfo;
                this.ExpireOn = DateTime.Now.AddMinutes(CacheDuration);
            }

            public DateTime ExpireOn { get; }

            public UserInfo UserInfo { get; }
        }

        static SystemWebMembershipProxyProvider()
        {
            var type = typeof(SystemWebMembershipProxyProvider);

            info = new ComponentInformation("PF membership proxy", "Pathfinder-fr", type.Assembly.GetName().Version.ToString(), "http://www.pathfinder-fr.org", null);

            using (var reader = new StreamReader(type.Assembly.GetManifestResourceStream(type.Namespace + ".HelpHtml.htm")))
            {
                helpHtml = reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Initializes the Storage Provider
        /// </summary>
        /// <param name="host"></param>
        /// <param name="config">configuration data, if any</param>
        public void Init(IHostV30 host, string config)
        {
            if (host == null) throw new ArgumentNullException(nameof(host));
            if (config == null) throw new ArgumentNullException(nameof(config));

            var membershipProvider = Membership.Provider;
            if (membershipProvider.RequiresQuestionAndAnswer)
            {
                host.LogEntry(this.Information.Name + " does not support the RequiresQuestionAndAnswer option on the current Membership Provider.", LogEntryType.Error, null, "SystemWebMembershipProxyProvider");
                throw new InvalidConfigurationException(this.Information.Name + " does not support the RequiresQuestionAndAnswer option on the current Membership Provider.");
            }

            // For UserInfo
            VerifyProfileField(host, "DisplayName");
            // For User Profile
            VerifyProfileField(host, "Culture");
            VerifyProfileField(host, "Timezone");
            // For Email Notifications
            VerifyProfileField(host, "PageChanges");
            VerifyProfileField(host, "DiscussionMessages");
            VerifyProfileField(host, "NamespacePageChanges");
            VerifyProfileField(host, "NamespaceDiscussionMessages");
        }

        private void VerifyProfileField(IHostV30 host, string profileFieldName)
        {
            var profileProperties = ProfileBase.Properties;
            var profileProperty = profileProperties[profileFieldName];
            if (profileProperty == null)
            {
                host.LogEntry(this.Information.Name + " requires a '" + profileFieldName + "' profile field defined in the current Profile Provider.", LogEntryType.Error, null, "SystemWebMembershipProxyProvider");
                throw new InvalidConfigurationException(this.Information.Name + " requires a '" + profileFieldName + "' profile field defined in the current Profile Provider.");
            }

            if (profileProperty.PropertyType != typeof(string))
            {
                host.LogEntry(this.Information.Name + " requires that the '" + profileFieldName + "' profile field is configured with type=\"String\".", LogEntryType.Error, null, "SystemWebMembershipProxyProvider");
                throw new InvalidConfigurationException(this.Information.Name + " requires that the '" + profileFieldName + "' profile field is configured with type=\"String\".");
            }

            if (profileProperty.IsReadOnly)
            {
                host.LogEntry(this.Information.Name + " requires that the '" + profileFieldName + "' profile field is configured with readOnly=\"false\".", LogEntryType.Error, null, "SystemWebMembershipProxyProvider");
                throw new InvalidConfigurationException(this.Information.Name + " requires that the '" + profileFieldName + "' profile field is configured with readOnly=\"false\".");
            }
            //if (!(bool)profileProperty.Attributes["AllowAnonymous"])
            //  throw new InvalidConfigurationException(this.Information.Name + " requires that the '" + profileFieldName + "' profile field is configured with allowAnonymous=\"true\".");
        }

        /// <summary>
        /// Gets the Information about the Provider.
        /// </summary>
        public ComponentInformation Information
        {
            get { return info; }
        }

        //cleanup/disconnect
        //not guaranteed to ever be called.
        public void Shutdown() { }

        /// <summary>
        /// Gets a brief summary of the configuration string format.
        /// null if no configuration is needed
        /// </summary>
        public string ConfigHelpHtml
        {
            get
            {
                return helpHtml;
            }
        }

        public bool UserAccountsReadOnly
        {
            get { return false; }
        }

        public bool UserGroupsReadOnly
        {
            get { return false; }
        }

        public bool UsersDataReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether group membership is read-only.
        /// </summary>
        /// <remarks>
        /// If UserAccountsReadOnly is false, then this property must also be false.
        /// If this property is true, the provider should return membership data compatible with default user groups.
        /// </remarks>
        public bool GroupMembershipReadOnly
        {
            get { return false; }
        }

        #region Users

        /// <summary>
        /// Adds a new user to the system.
        /// </summary>
        /// <param name="displayName">name to display for the user, can be null</param>
        /// <param name="active">is the account active</param>
        /// <param name="created">account creation datetime</param>
        /// <returns></returns>
        public UserInfo AddUser(string username, string displayName, string password, string email, bool active, DateTime created)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));

            MembershipCreateStatus createStatus;
            var membershipUser = Membership.CreateUser(username, password, email, null, null, active, out createStatus);

            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateEmail:
                case MembershipCreateStatus.DuplicateProviderUserKey:
                case MembershipCreateStatus.DuplicateUserName:
                case MembershipCreateStatus.UserRejected:
                    return null;
            }

            var profile = ProfileBase.Create(membershipUser.UserName);
            profile["DisplayName"] = displayName;
            profile.Save();

            return ToUserInfo(membershipUser);
        }

        /// <summary>
        /// get a user account
        /// </summary>
        /// <param name="username"></param>
        /// <returns>null if no matching user was found</returns>
        public UserInfo GetUser(string username)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException(nameof(username));

            var cacheEntry = usersByUserNameCache.GetOrAdd(username, CreateUserCacheEntry);

            if (cacheEntry != null && cacheEntry.ExpireOn > DateTime.Now)
            {
                return cacheEntry.UserInfo;
            }

            if (usersByUserNameCache.TryRemove(username, out _))
            {
                // recursive
                return usersByUserNameCache.GetOrAdd(username, CreateUserCacheEntry)?.UserInfo;
            }
            else
            {
                return ToUserInfo(Membership.GetUser(username));
            }
        }

        private UserInfoCacheEntry CreateUserCacheEntry(string username)
        {
            var membershipUser = Membership.GetUser(username);
            if (membershipUser == null) return null;
            return new UserInfoCacheEntry(ToUserInfo(membershipUser));
        }

        /// <summary>
        /// get a user account
        /// </summary>
        /// <param name="email"></param>
        /// <returns>null if no matching user was found</returns>
        public UserInfo GetUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));

            var users = Membership.FindUsersByEmail(email).OfType<MembershipUser>();
            return ToUserInfo(users.SingleOrDefault());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>All users, sorted by username</returns>
        public IEnumerable<UserInfo> GetUsers()
        {
            lock (usersCacheLock)
            {
                UserInfo[] result = usersCache;

                bool expired = usersCacheCreation < DateTime.Now.AddMinutes(-CacheDuration);

                if (!expired && result != null)
                {
                    return result;
                }

                if (expired)
                {
                    usersCacheCreation = DateTime.Now;

                }

                usersCache = Membership.GetAllUsers().OfType<MembershipUser>().OrderBy(u => u.UserName).Select(u => ToUserInfo(u, false, false)).ToArray();

                return usersCache;
            }
        }

        /// <summary>
        /// Gets all the users that have the specified element in their data.
        /// </summary>
        /// <param name="key">key of the data</param>
        /// <returns>the users and the data</returns>
        public IDictionary<UserInfo, string> GetUsersWithData(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            //HACK: don't return any users.
            return new Dictionary<UserInfo, string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user">user to modify</param>
        /// <param name="newDisplayName">new display name (can be null)</param>
        /// <param name="newPassword">new password (null or empty to keep current password)</param>
        /// <param name="newEmail"></param>
        /// <param name="newActive"></param>
        /// <returns>The Modified UserInfo instance</returns>
        public UserInfo ModifyUser(UserInfo user, string newDisplayName, string newPassword, string newEmail, bool newActive)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            var membershipUser = Membership.GetUser(user.Username, false);
            if (membershipUser == null)
                throw new ArgumentException("Could not locate user '" + user.Username + "' in Membership Provider.", nameof(user));

            if (!string.IsNullOrEmpty(newPassword))
            {
                membershipUser.ChangePassword(membershipUser.GetPassword(), newPassword);
            }

            membershipUser.Email = newEmail;
            membershipUser.IsApproved = newActive;
            Membership.UpdateUser(membershipUser);

            var profile = GetProfile(membershipUser);
            profile["DisplayName"] = newDisplayName;
            profile.Save();

            return ToUserInfo(membershipUser);
        }

        /// <returns>true if the user was removed successfuly</returns>
        public bool RemoveUser(UserInfo user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Membership.DeleteUser(user.Username);
        }

        #endregion

        #region Login/out
        /// <summary>
        /// Notifies the provider that a user has logged in through the authentication cookie.
        /// </summary>
        /// <param name="user"></param>
        public void NotifyCookieLogin(UserInfo user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            Membership.GetUser(true);
        }

        public void NotifyLogout(UserInfo user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (HttpContext.Current == null) return;
            switch (HttpContext.Current.User.Identity.AuthenticationType)
            {
                case "Forms":
                    FormsAuthentication.SignOut();
                    break;
            }
        }

        public bool TestAccount(UserInfo user, string password)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Membership.ValidateUser(user.Username, password);
        }

        public UserInfo TryAutoLogin(System.Web.HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (HttpContext.Current != null &&
                HttpContext.Current.User.Identity.IsAuthenticated)
                return ToUserInfo(Membership.GetUser(true));
            return null;
        }

        public UserInfo TryManualLogin(string username, string password)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException(nameof(username));
            //if (String.IsNullOrEmpty(password)) throw new ArgumentNullException("password");

            if (Membership.ValidateUser(username, password))
                return GetUser(username);
            return null;
        }
        #endregion

        #region UserGroups/Roles
        /// <summary>
        /// Adds a user group (role)
        /// </summary>
        /// <param name="description">ignored by the System Role Providers</param>
        public UserGroup AddUserGroup(string name, string description)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            //if (description == null)
            //  throw new ArgumentNullException("description");

            Roles.CreateRole(name);
            return new MembershipUserGroup(name, string.Empty, this, name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>All user groups (Roles), sorted by name</returns>
        public IEnumerable<UserGroup> GetUserGroups()
        {
            var roles = Roles.GetAllRoles();
            Array.Sort<string>(roles);

            return roles.OrderBy(r => r).Select(r => new MembershipUserGroup(r, string.Empty, this, r) { Users = GetUsersForGroup(r) }).ToArray();
        }

        private string[] GetUsersForGroup(string roleName)
        {
            lock (userForGroupCacheLock)
            {
                string[] result;
                bool expired = userForGroupCacheCreation < DateTime.Now.AddMinutes(-CacheDuration);

                if (!expired && userForGroupCache.TryGetValue(roleName, out result))
                {
                    return result;
                }

                if (expired)
                {
                    userForGroupCacheCreation = DateTime.Now;
                    userForGroupCache.Clear();
                }

                result = Roles.GetUsersInRole(roleName);
                userForGroupCache[roleName] = result;
                return result;
            }
        }

        public UserGroup ModifyUserGroup(UserGroup group, string description)
        {
            if (group == null) throw new ArgumentNullException(nameof(group));
            //if (description == null) throw new ArgumentNullException("description");

            // System Role Provider does not support role descriptions
            return group;
        }

        public bool RemoveUserGroup(UserGroup group)
        {
            if (group == null) throw new ArgumentNullException(nameof(group));

            return Roles.DeleteRole(group.Name);
        }

        public UserInfo SetUserMembership(UserInfo user, IEnumerable<string> groups)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var currentRoles = Roles.GetRolesForUser(user.Username);

            var removedRoles = currentRoles.Where(g => !groups.Contains(g)).ToArray();
            if (removedRoles.Length != 0)
            {
                Roles.RemoveUserFromRoles(user.Username, removedRoles);
            }

            var addedRoles = groups.Where(g => !currentRoles.Any(r => r == g)).ToList();
            if (addedRoles.Count != 0)
            {
                Roles.AddUserToRoles(user.Username, addedRoles.ToArray());
            }

            var ret = GetUser(user.Username);
            ret.Groups = Roles.GetRolesForUser(user.Username);

            return ret;
        }
        #endregion

        #region User Data
        public IDictionary<string, string> RetrieveAllUserData(UserInfo user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var ret = new Dictionary<string, string>();
            var profile = GetProfile(user.Username);

            foreach (var p in ProfileBase.Properties.OfType<SettingsProperty>())
            {
                if (p.PropertyType == typeof(string))
                    ret.Add(p.Name, (string)profile[p.Name]);
                else if (profile[p.Name] != null)
                    ret.Add(p.Name, profile[p.Name].ToString());
                else
                    ret.Add(p.Name, null);

                return ret;
            }

            return ret;
        }

        public string RetrieveUserData(UserInfo user, string key)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            key = key.ToLowerInvariant();
            var profile = GetProfile(user.Username);
            return profile[key] as string;
        }

        public bool StoreUserData(UserInfo user, string key, string value)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            try
            {
                var profile = GetProfile(user.Username);
                profile[key] = value;
                profile.Save();
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Internal Utilities
        private MembershipUserInfo ToUserInfo(MembershipUser membershipUser, bool loadRoles = true, bool loadProfile = true)
        {
            if (membershipUser == null) return null;

            var displayName = membershipUser.UserName;

            if (loadProfile)
            {
                var profile = GetProfile(membershipUser);
                if (profile["DisplayName"] == null)
                    profile["DisplayName"] = membershipUser.UserName;
                displayName = (string)profile["DisplayName"];
                profile.Save();
            }

            var userInfo = new MembershipUserInfo(membershipUser.UserName, displayName, membershipUser.Email, membershipUser.IsApproved, membershipUser.CreationDate, this, membershipUser.ProviderUserKey);

            if (loadRoles)
            {
                userInfo.Groups = Roles.GetRolesForUser(membershipUser.UserName);
            }
            else
            {
                userInfo.Groups = Enumerable.Empty<string>();
            }

            return userInfo;
        }

        private static ProfileBase GetProfile(string username)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException(nameof(username));

            return ProfileBase.Create(username);
        }

        private static ProfileBase GetProfile(MembershipUser user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return GetProfile(user.UserName);
        }
        #endregion
    }
}
