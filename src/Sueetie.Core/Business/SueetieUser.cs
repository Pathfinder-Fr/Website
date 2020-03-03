// -----------------------------------------------------------------------
// <copyright file="SueetieUser.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Web.UI;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_vw_user.
    /// </summary>
    [Serializable]
    public class SueetieUser : INamingContainer
    {
        public int UserID { get; set; }
        public Guid MembershipID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public byte[] AvatarImage { get; set; }
        public string AvatarImageType { get; set; }
        public string DisplayName { get; set; }
        public string AvatarThumbnailFilename { get; set; }
        public string AvatarFilename { get; set; }
        public int AvatarRoot { get; set; }
        public DateTime DateJoined { get; set; }
        public string Bio { get; set; }
        public int ForumUserID { get; set; }
        public int TimeZone { get; set; }
        public bool IsActive { get; set; }
        public string IP { get; set; }
        public DateTime LastActivity { get; set; }
        public bool IsFiltered { get; set; }

        public SueetieUserProfile Profile
        {
            get
            {
                if (SueetieContext.Current.User.UserID == this.UserID)
                    return SueetieContext.Current.UserProfile;
                return SueetieUsers.GetSueetieUserProfile(this.UserID);
            }
        }

        //private MembershipUser _aspnetInfo;
        //public MembershipUser AspnetInfo
        //{
        //    get
        //    {
        //        if (SueetieContext.Current.User.UserID == UserID)
        //            return SueetieContext.Current.User.AspnetInfo;
        //        else
        //        {
        //            return Membership.GetUser(UserName);
        //        }
        //    }
        //    set { _aspnetInfo = value; }
        //}

        public bool HasAvatarImage
        {
            get
            {
                var hasImage = true;

                if (this.AvatarImage == null || this.IsAnonymous)
                {
                    hasImage = false;
                }

                return hasImage;
            }
        }

        public bool IsBanned
        {
            get
            {
                var isBanned = false;
                if (SueetieUsers.IsIPBanned(this.IP))
                    isBanned = true;
                return isBanned;
            }
        }

        public bool IsAnonymous
        {
            get
            {
                var isAnonymous = true;
                if (this.UserID > 0)
                    isAnonymous = false;
                return isAnonymous;
            }
        }

        public bool IsInRole(string role)
        {
            var userRoles = SueetieRoles.GetRolesForUser(this.UserName);
            foreach (var userRole in userRoles)
            {
                if (string.Compare(role, userRole, true) == 0)
                    return true;
            }
            return false;
        }

        public bool IsRegistered
        {
            get { return this.IsInRole("Registered"); }
        }

        public bool IsSueetieAdministrator
        {
            get { return this.IsInRole("SueetieAdministrator"); }
        }

        public bool IsWikiAdministrator
        {
            get { return this.IsInRole("WikiAdministrator"); }
        }

        public bool IsMarketplaceAdministrator
        {
            get { return this.IsInRole("MarketplaceAdministrator"); }
        }

        public bool IsBlogAdministrator
        {
            get { return this.IsInRole("BlogAdministrator"); }
        }

        public bool IsContentAdministrator
        {
            get { return this.IsInRole("ContentAdministrator"); }
        }
    }
}