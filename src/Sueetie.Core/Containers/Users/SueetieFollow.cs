// -----------------------------------------------------------------------
// <copyright file="SueetieFollow.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a Sueetie_Follower.
    /// </summary>
    [Serializable]
    public class SueetieFollow
    {
        public int UserID { get; set; }
        public int FollowerUserID { get; set; }
        public int FollowingUserID { get; set; }
        public int ContentIDFollowed { get; set; }
        public DateTime DateTimeFollowed { get; set; }
        public bool IsAFriend { get; set; }
        public SueetieFollowType FollowType { get; set; }
        public string DisplayName { get; set; }
        public string ThumbnailFilename { get; set; }
    }
}