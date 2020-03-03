// -----------------------------------------------------------------------
// <copyright file="SueetieBlogComment.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_vw_Blogcomment.
    /// </summary>
    [Serializable]
    [DataContract]
    public class SueetieBlogComment
    {
        [DataMember]
        public int SueetieCommentID { get; set; }

        [DataMember]
        public int UserID { get; set; }

        [DataMember]
        public Guid PostCommentID { get; set; }

        [DataMember]
        public Guid PostID { get; set; }

        [DataMember]
        public DateTime CommentDate { get; set; }

        [DataMember]
        public string Author { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Website { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string Ip { get; set; }

        [DataMember]
        public bool IsApproved { get; set; }

        [DataMember]
        public Guid ParentCommentID { get; set; }

        [DataMember]
        public string ApplicationKey { get; set; }

        [DataMember]
        public int GroupID { get; set; }

        [DataMember]
        public string GroupKey { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public int ContentID { get; set; }

        [DataMember]
        public int ContentTypeID { get; set; }

        [DataMember]
        public string Permalink { get; set; }

        [DataMember]
        public int ApplicationID { get; set; }

        [DataMember]
        public DateTime DateTimeCreated { get; set; }

        [DataMember]
        public bool IsRestricted { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public int PostUserID { get; set; }

        [DataMember]
        public string PostDisplayName { get; set; }
    }
}