// -----------------------------------------------------------------------
// <copyright file="SueetieBlog.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_vw_blog.
    /// </summary>
    [Serializable]
    public class SueetieBlog
    {
        public int BlogID { get; set; }
        public string ApplicationKey { get; set; }
        public string AppDescription { get; set; }
        public int GroupID { get; set; }
        public int ApplicationID { get; set; }
        public int CategoryID { get; set; }
        public string BlogOwnerRole { get; set; }
        public string BlogAccessRole { get; set; }
        public string BlogTitle { get; set; }
        public string BlogDescription { get; set; }
        public int PostCount { get; set; }
        public int CommentCount { get; set; }
        public int TrackbackCount { get; set; }
        public bool IncludeInAggregateList { get; set; }
        public bool IsActive { get; set; }
        public int ContentID { get; set; }
        public int PostAuthorID { get; set; }
        public string PostTitle { get; set; }
        public string PostDescription { get; set; }
        public string PostContent { get; set; }
        public DateTime PostDateCreated { get; set; }
        public string PostAuthorUserName { get; set; }
        public string PostAuthorEmail { get; set; }
        public string PostAuthorDisplayName { get; set; }
        public bool IsPublished { get; set; }
        public string Permalink { get; set; }
        public DateTime DateBlogCreated { get; set; }
        public bool RegisteredComments { get; set; }
        public int PostImageTypeID { get; set; }
        public int PostImageHeight { get; set; }
        public int PostImageWidth { get; set; }
        public int AnchorPositionID { get; set; }
        public int MediaAlbumID { get; set; }
        public string DefaultPostImage { get; set; }
    }
}