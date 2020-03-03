// -----------------------------------------------------------------------
// <copyright file="UserLogActivity.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_Sueetie_UserLogActivity.
    /// </summary>
    [Serializable]
    public class UserLogActivity
    {
        public int UserLogID { get; set; }
        public int UserLogCategoryID { get; set; }
        public string UserLogCategoryCode { get; set; }
        public int UserID { get; set; }
        public string DisplayName { get; set; }
        public int ItemID { get; set; }
        public int ContentID { get; set; }
        public int ContentTypeID { get; set; }
        public int ApplicationID { get; set; }
        public int GroupID { get; set; }
        public string Permalink { get; set; }
        public int SourceID { get; set; }
        public string SourceDescription { get; set; }
        public int SourceParentID { get; set; }
        public string SourceParentDescription { get; set; }
        public string SourceParentPermalink { get; set; }
        public int ToUserID { get; set; }
        public string ToUserDisplayName { get; set; }
        public string GroupPath { get; set; }
        public string ApplicationPath { get; set; }
        public string ApplicationName { get; set; }
        public DateTime DateTimeActivity { get; set; }
        public string Email { get; set; }
        public bool IsDisplayed { get; set; }
        public bool IsSyndicated { get; set; }

        public bool ShowHeader { get; set; }
        public string ActivityClass { get; set; }
        public string Activity { get; set; }
    }
}