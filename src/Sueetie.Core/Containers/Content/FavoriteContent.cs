// -----------------------------------------------------------------------
// <copyright file="FavoriteContent.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_FaveContent.
    /// </summary>
    [Serializable]
    [DataContract]
    public class FavoriteContent
    {
        [DataMember]
        public int FavoriteID { get; set; }

        [DataMember]
        public int ContentID { get; set; }

        [DataMember]
        public int UserID { get; set; }

        [DataMember]
        public int AuthorUserID { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Permalink { get; set; }

        [DataMember]
        public int ApplicationID { get; set; }

        [DataMember]
        public int ContentTypeID { get; set; }

        [DataMember]
        public int GroupID { get; set; }

        [DataMember]
        public string GroupName { get; set; }

        [DataMember]
        public bool IsRestricted { get; set; }

        [DataMember]
        public DateTime DateTimeCreated { get; set; }

        [DataMember]
        public string ApplicationName { get; set; }

        [DataMember]
        public string ContentType { get; set; }

        [DataMember]
        public string ContentTypeAuthoredBy { get; set; }
    }
}