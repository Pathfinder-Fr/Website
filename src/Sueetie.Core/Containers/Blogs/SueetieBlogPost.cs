// -----------------------------------------------------------------------
// <copyright file="SueetieBlogPost.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_vw_blogpost.
    /// </summary>
    [Serializable]
    public class SueetieBlogPost
    {
        public int SueetiePostID { get; set; }
        public int UserID { get; set; }
        public Guid PostID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PostContent { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string Author { get; set; }
        public bool IsPublished { get; set; }
        public bool IsCommentEnabled { get; set; }
        public int Raters { get; set; }
        public float Rating { get; set; }
        public string Slug { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public int GroupID { get; set; }
        public string GroupKey { get; set; }
        public int ApplicationID { get; set; }
        public string ApplicationKey { get; set; }
        public int ApplicationTypeID { get; set; }
        public int ContentID { get; set; }
        public int ContentTypeID { get; set; }
        public string Permalink { get; set; }
        public bool IsRestricted { get; set; }
        public string GroupName { get; set; }
        public string ApplicationName { get; set; }
        public string BlogAccessRole { get; set; }
        public string BlogTitle { get; set; }
        public bool IncludeInAggregateList { get; set; }
        public bool IsActive { get; set; }
        public string Categories { get; set; }
        public string Tags { get; set; }

        public string PostImageUrl { get; set; }
    }
}