// -----------------------------------------------------------------------
// <copyright file="SueetieForumTopic.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_vw_forumTopic.
    /// </summary>
    [Serializable]
    public class SueetieForumTopic
    {
        public int TopicID { get; set; }
        public int ForumID { get; set; }
        public int UserID { get; set; }
        public string Topic { get; set; }
        public int SueetieUserID { get; set; }
        public int ContentID { get; set; }
        public int SourceID { get; set; }
        public int ContentTypeID { get; set; }
        public int ApplicationID { get; set; }
        public string Permalink { get; set; }
        public bool IsRestricted { get; set; }
        public int GroupID { get; set; }
        public string DisplayName { get; set; }
        public string ApplicationKey { get; set; }
        public string GroupName { get; set; }
        public string Forum { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public bool IsDeleted { get; set; }
        public string Tags { get; set; }
        public string SueetieUserIDs { get; set; }
        public string LeadTopicMessageID { get; set; }
    }
}