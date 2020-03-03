// -----------------------------------------------------------------------
// <copyright file="SueetieForumContent.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_ForumContent.
    /// </summary>
    [Serializable]
    public class SueetieForumContent
    {
        public int ContentID { get; set; }
        public int SourceID { get; set; }
        public int ContentTypeID { get; set; }
        public int ApplicationID { get; set; }
        public int GroupID { get; set; }
        public int SueetieUserID { get; set; }
        public string Permalink { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public bool IsRestricted { get; set; }
        public int BoardID { get; set; }
        public int ForumID { get; set; }
        public int TopicID { get; set; }
        public int MessageID { get; set; }
        public int YAFUserID { get; set; }
    }
}