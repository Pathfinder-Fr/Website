// -----------------------------------------------------------------------
// <copyright file="SueetieForumMessage.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_vw_ForumMessage.
    /// </summary>
    [Serializable]
    [DataContract]
    public class SueetieForumMessage
    {
        [DataMember]
        public int MessageID { get; set; }

        [DataMember]
        public int TopicID { get; set; }

        [DataMember]
        public int UserID { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public int SueetieUserID { get; set; }

        [DataMember]
        public int ContentID { get; set; }

        [DataMember]
        public int ContentTypeID { get; set; }

        [DataMember]
        public int ApplicationID { get; set; }

        [DataMember]
        public bool IsRestricted { get; set; }

        [DataMember]
        public string Permalink { get; set; }

        [DataMember]
        public DateTime DateTimeCreated { get; set; }

        [DataMember]
        public int SourceID { get; set; }

        [DataMember]
        public string Topic { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string ApplicationDescription { get; set; }

        [DataMember]
        public int GroupID { get; set; }

        [DataMember]
        public string GroupName { get; set; }

        [DataMember]
        public int TopicSueetieUserID { get; set; }

        [DataMember]
        public string TopicDisplayName { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public int ApplicationTypeID { get; set; }

        [DataMember]
        public string ApplicationKey { get; set; }

        [DataMember]
        public string Forum { get; set; }

        [DataMember]
        public int ForumID { get; set; }

        [DataMember]
        public string GroupKey { get; set; }

        [DataMember]
        public DateTime Edited { get; set; }

        [DataMember]
        public string Tags { get; set; }
    }
}