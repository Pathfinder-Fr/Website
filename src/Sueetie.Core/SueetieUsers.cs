// -----------------------------------------------------------------------
// <copyright file="SueetieUsers.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI;

    /// <summary>
    /// The proxy class for communication between the business objects and the providers.
    /// </summary>
    public static class SueetieUsers
    {
        public static SueetieUser GetUser(int userID)
        {
            return GetUser(userID, true);
        }

        public static SueetieUser GetUser(int userID, bool getCached)
        {
            var sueetieUser = new SueetieUser();
            var provider = SueetieDataProvider.LoadProvider();

            var userCacheKey = UserCacheKey(userID);
            if (getCached)
            {
                sueetieUser = SueetieCache.Current[userCacheKey] as SueetieUser;
                if (sueetieUser == null)
                {
                    sueetieUser = provider.GetUser(userID);
                    SueetieCache.Current.InsertMinutes(userCacheKey, sueetieUser, 2);
                }
            }
            else
                sueetieUser = provider.GetUser(userID);
            return sueetieUser;
        }

        public static SueetieUser GetUser(string username)
        {
            var sueetieUser = new SueetieUser();
            var provider = SueetieDataProvider.LoadProvider();

            var userCacheKey = UserCacheKey(username);
            sueetieUser = SueetieCache.Current[userCacheKey] as SueetieUser;
            if (sueetieUser == null)
            {
                sueetieUser = provider.GetUser(username);
                SueetieCache.Current.InsertMinutes(userCacheKey, sueetieUser, 2);
            }
            return sueetieUser;
        }

        public static SueetieUser GetSueetieUserByEmail(string email)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetSueetieUserByEmail(email);
        }

        public static string UserCacheKey(int userID)
        {
            return string.Format("SueetieUserID-{0}", userID);
        }

        public static string UserCacheKey(string username)
        {
            return string.Format("SueetieUserName-{0}", username.ToLower());
        }

        public static void ClearUserCache(int userID)
        {
            var username = GetUser(userID).UserName.ToLower();
            SueetieCache.Current.Remove(UserCacheKey(userID));
            SueetieCache.Current.Remove(UserCacheKey(username));
        }

        public static int CreateSueetieUser(SueetieUser sueetieUser)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.CreateSueetieUser(sueetieUser);
        }

        public static void UpdateSueetieUser(SueetieUser sueetieUser)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.UpdateSueetieUser(sueetieUser);
        }

        public static void UpdateSueetieUserBio(SueetieUser sueetieUser)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.UpdateSueetieUserBio(sueetieUser);
        }

        public static void UpdateDisplayName(SueetieUser sueetieUser)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.UpdateDisplayName(sueetieUser);
        }

        public static void DeactivateUser(string _username)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.DeactivateUser(_username);
        }

        public static List<SueetieUser> GetSueetieUserList(SueetieUserType sueetieUserType)
        {
            return GetSueetieUserList(sueetieUserType, true);
        }

        public static List<SueetieUser> GetSueetieUserList(SueetieUserType sueetieUserType, bool useCachedUserList)
        {
            var key = SueetieUserListCacheKey(sueetieUserType);

            var sueetieUsers = SueetieCache.Current[key] as List<SueetieUser>;
            if (sueetieUsers == null || !useCachedUserList)
            {
                var provider = SueetieDataProvider.LoadProvider();
                sueetieUsers = provider.GetSueetieUserList(sueetieUserType);
                SueetieCache.Current.InsertMinutes(key, sueetieUsers, 5);
            }

            return sueetieUsers;
        }

        public static string SueetieUserListCacheKey(SueetieUserType sueetieUserType)
        {
            return string.Format("SueetieUserList-{0}-{1}", SueetieConfiguration.Get().Core.SiteUniqueName, (int)sueetieUserType);
        }

        public static List<SueetieAspnetUser> GetUnapprovedUserList()
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetUnapprovedUserList();
        }

        public static List<SueetieAspnetUser> GetInactiveUserList()
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetInactiveUserList();
        }

        public static void ClearSueetieUserListCache(SueetieUserType sueetieUserType)
        {
            SueetieCache.Current.Remove(SueetieUserListCacheKey(sueetieUserType));
        }

        public static SueetieUserProfile GetSueetieUserProfile(string username)
        {
            var userProfileCacheKey = UserProfileCacheKey(username);
            var sueetieUserProfile = SueetieCache.Current[userProfileCacheKey] as SueetieUserProfile;
            if (sueetieUserProfile == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                sueetieUserProfile = provider.GetSueetieUserProfile(username);
                SueetieCache.Current.InsertMinutes(userProfileCacheKey, sueetieUserProfile, 5);
            }
            return sueetieUserProfile;
        }

        public static string UserProfileCacheKey(string username)
        {
            return string.Format("SueetieUserProfileByUsername-{0}", username);
        }

        public static void ClearUserProfileCache(string username)
        {
            SueetieCache.Current.Remove(UserProfileCacheKey(username));
        }

        public static SueetieUserProfile GetSueetieUserProfile(int userID)
        {
            var userProfileCacheKey = UserProfileCacheKey(userID);
            var sueetieUserProfile = SueetieCache.Current[userProfileCacheKey] as SueetieUserProfile;
            if (sueetieUserProfile == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                sueetieUserProfile = provider.GetSueetieUserProfile(userID);
                SueetieCache.Current.InsertMinutes(userProfileCacheKey, sueetieUserProfile, 5);
            }
            return sueetieUserProfile;
        }

        public static string UserProfileCacheKey(int userID)
        {
            return string.Format("SueetieUserProfileByID-{0}", userID);
        }

        public static void ClearUserProfileCache(int userID)
        {
            SueetieCache.Current.Remove(UserProfileCacheKey(userID));
        }

        public static Pair GenerateProfileKeyValues(List<Pair> _keyValuePairs)
        {
            var _keys = string.Empty;
            var _values = string.Empty;
            var startingDigit = 0;
            foreach (var _item in _keyValuePairs)
            {
                startingDigit = _values.Length;
                _keys += _item.First + ":S:" + startingDigit + ":" + _item.Second.ToString().Length + ":";
                _values += _item.Second.ToString();
            }
            var _PropertyKeyValues = new Pair
            {
                First = _keys,
                Second = _values
            };

            return _PropertyKeyValues;
        }

        public static void UpdateSueetieUserProfile(Pair _keyValuePair, int userID)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.UpdateSueetieUserProfile(_keyValuePair, userID);
        }

        public static void UpdateSueetieUserAvatar(SueetieUserAvatar sueetieUserAvatar)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.UpdateSueetieUserAvatar(sueetieUserAvatar);

            var cookie = HttpContext.Current.Request.Cookies["SueetieUserProfile"];
            if (cookie != null)
            {
                cookie["AvatarRoot"] = sueetieUserAvatar.UserID.ToString();
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static void DeleteAvatar(int _userID)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.DeleteAvatar(_userID);

            var cookie = HttpContext.Current.Request.Cookies["SueetieUserProfile"];
            if (cookie != null)
            {
                cookie["AvatarRoot"] = "0";
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }


        public static string GetUserAvatarUrl(int userID)
        {
            return GetUserAvatarUrl(userID, false);
        }


        public static string GetUserAvatarUrl(int userID, bool useOriginalImage)
        {
            return GetUserAvatarUrl(userID, false, true);
        }

        public static string GetUserAvatarUrl(int userID, bool useOriginalImage, bool useCachedAvatarRoot)
        {
            var _avatarRoot = 0;
            var sueetieUser = GetSueetieUserList(SueetieUserType.RegisteredUser).Find(u => u.UserID == userID);

            if (!useCachedAvatarRoot)
                sueetieUser = GetUser(userID, false);

            if (sueetieUser != null)
            {
                _avatarRoot = sueetieUser.AvatarRoot;
            }
            else
                _avatarRoot = 0;

            var _extension = "t.jpg";
            if (useOriginalImage)
                _extension = ".jpg";
            return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/" +
                   SueetieConfiguration.Get().AvatarSettings.AvatarFolderPath.Replace("\\", "/") + _avatarRoot + _extension;
        }

        public static SueetieUser GetThinSueetieUser(int userID)
        {
            var _sueetieUser = GetSueetieUserList(SueetieUserType.RegisteredUser).Find(u => u.UserID == userID);
            if (_sueetieUser == null)
                _sueetieUser = GetSueetieUserList(SueetieUserType.AllUsers).Find(u => u.UserID == userID);
            return _sueetieUser;
        }

        public static SueetieUser GetThinSueetieUserFromEmail(string email)
        {
            return GetSueetieUserList(SueetieUserType.RegisteredUser).Find(u => u.Email.ToLower() == email.ToLower());
        }

        public static SueetieUser GetThinSueetieUser(string username)
        {
            return GetSueetieUserList(SueetieUserType.RegisteredUser).Find(u => u.UserName.ToLower() == username.ToLower());
        }

        public static int GetUserID(string username)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetUserID(username);
        }

        public static string GetUserDisplayName(string username)
        {
            return GetSueetieUserProfile(GetUserID(username)).DisplayName;
        }

        public static string GetUserDisplayName(int userID)
        {
            return GetUserDisplayName(userID, true);
        }

        public static string GetUserDisplayName(int userID, bool useCached)
        {
            return GetUser(userID, useCached).DisplayName;
        }

        public static bool IsNewUsername(string username)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.IsNewUsername(username);
        }

        public static bool IsNewEmailAddress(string email)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.IsNewEmailAddress(email);
        }

        public static bool IsNewDisplayName(string displayName)
        {
            var isNew = false;
            var _sueetieUser = GetSueetieUserList(SueetieUserType.RegisteredUser).Find(u => u.DisplayName.Match() == displayName.Match());
            if (_sueetieUser == null)
                isNew = true;
            return isNew;
        }

        public static void CreateUpdateUserProfileCookie(SueetieUser sueetieUser)
        {
            var cookie = HttpContext.Current.Request.Cookies["SueetieUserProfile"];
            if (cookie == null)
                cookie = new HttpCookie("SueetieUserProfile");

            cookie["UserID"] = sueetieUser.UserID.ToString();
            cookie["UserName"] = sueetieUser.UserName;
            cookie["Email"] = sueetieUser.Email;
            cookie["DisplayName"] = sueetieUser.DisplayName;

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static SueetieUser GetAnonymousUser()
        {
            var sueetieUser = new SueetieUser();
            sueetieUser.UserID = -1;
            sueetieUser.UserName = "anonymous";
            sueetieUser.Email = "na@sueetie.com";
            SueetieCache.Current.Insert(UserCacheKey(-1), sueetieUser);
            return sueetieUser;
        }

        public static SueetieUserProfile GetAnonymousUserProfile()
        {
            var sueetieUserProfile = new SueetieUserProfile();
            sueetieUserProfile.DisplayName = "Guest";
            return sueetieUserProfile;
        }

        public static int FollowUser(SueetieFollow sueetieFollow)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.FollowUser(sueetieFollow);
        }

        public static void UnFollowUser(SueetieFollow sueetieFollow)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.UnFollowUser(sueetieFollow);
        }

        public static int GetFollowID(SueetieFollow sueetieFollow)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetFollowID(sueetieFollow);
        }

        public static List<SueetieFollow> GetSueetieFollowList(int userID, int followTypeID)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetSueetieFollowList(userID, followTypeID);
        }

        public static List<FavoriteContent> GetFavoriteContentList(int userID)
        {
            var key = FavoriteContentListCacheKey(userID);

            var favoriteContents = SueetieCache.Current[key] as List<FavoriteContent>;
            if (favoriteContents == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                favoriteContents = provider.GetFavoriteContentList(userID);
                SueetieCache.Current.Insert(key, favoriteContents);
            }

            return favoriteContents;
        }

        public static string FavoriteContentListCacheKey(int userID)
        {
            return string.Format("FavoriteContentList-{0}", userID);
        }

        public static void ClearFavoriteContentListCache(int userID)
        {
            SueetieCache.Current.Remove(FavoriteContentListCacheKey(userID));
        }

        public static int CreateFavorite(UserContent userContent)
        {
            var provider = SueetieDataProvider.LoadProvider();
            var favoriteID = provider.CreateFavorite(userContent);
            ClearFavoriteContentListCache(userContent.UserID);
            return favoriteID;
        }

        public static int GetFavoriteID(UserContent userContent)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetFavoriteID(userContent);
        }

        public static UserContent DeleteFavorite(int favoriteID)
        {
            var provider = SueetieDataProvider.LoadProvider();
            var userContent = provider.DeleteFavorite(favoriteID);
            ClearFavoriteContentListCache(userContent.UserID);
            return userContent;
        }

        public static UserContent DeleteFavorite(UserContent userContent)
        {
            var provider = SueetieDataProvider.LoadProvider();
            var _userContent = provider.DeleteFavorite(userContent);
            ClearFavoriteContentListCache(userContent.UserID);
            return _userContent;
        }

        public static bool IsIPBanned(string IP)
        {
            var sueetieBannedIPList = GetSueetieBannedIPList();
            var isBanned = false;
            foreach (var banned in sueetieBannedIPList)
            {
                if (SueetieIPHelper.IsBanned(banned.Mask, IP))
                    isBanned = true;
            }
            return isBanned;
        }

        public static List<SueetieBannedIP> GetSueetieBannedIPList()
        {
            var key = SueetieBannedIPListCacheKey();

            var sueetieBannedIP = SueetieCache.Current[key] as List<SueetieBannedIP>;
            if (sueetieBannedIP == null)
            {
                var provider = SueetieDataProvider.LoadProvider();
                sueetieBannedIP = provider.GetSueetieBannedIPList();
                SueetieCache.Current.Insert(key, sueetieBannedIP);
            }

            return sueetieBannedIP;
        }

        public static string SueetieBannedIPListCacheKey()
        {
            return string.Format("SueetieBannedIPList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static void BanIP(string ip)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.BanIP(ip);
            SueetieCache.Current.Remove(SueetieBannedIPListCacheKey());
        }

        public static void UpdateBannedIP(int bannedID, string ip)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.UpdateBannedIP(bannedID, ip);
            SueetieCache.Current.Remove(SueetieBannedIPListCacheKey());
        }

        public static void RemoveBannedIP(int bannedID)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.RemoveBannedIP(bannedID);
            SueetieCache.Current.Remove(SueetieBannedIPListCacheKey());
        }

        public static void RemoveBannedIP(string ip)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.RemoveBannedIP(ip);
            SueetieCache.Current.Remove(SueetieBannedIPListCacheKey());
        }

        public static void UpdateSueetieUserIP(SueetieUser sueetieUser)
        {
            var provider = SueetieDataProvider.LoadProvider();
            provider.UpdateSueetieUserIP(sueetieUser);
            SueetieCache.Current.Remove(SueetieBannedIPListCacheKey());
        }

        public static List<SueetieSubscriber> GetSueetieSubscriberList()
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.GetSueetieSubscriberList();
        }
    }
}