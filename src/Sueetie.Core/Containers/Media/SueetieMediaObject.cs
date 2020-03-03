// -----------------------------------------------------------------------
// <copyright file="SueetieMediaObject.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This object represents the properties and methods of a z_sueetie_vw_mediaobject.
    /// </summary>
    [Serializable]
    [DataContract]
    public class SueetieMediaObject
    {
        [DataMember]
        public int MediaObjectID { get; set; }

        [DataMember]
        public int AlbumID { get; set; }

        [DataMember]
        public int ContentID { get; set; }

        [DataMember]
        public int SourceID { get; set; }

        [DataMember]
        public int ContentTypeID { get; set; }

        [DataMember]
        public string ContentTypeDescription { get; set; }

        [DataMember]
        public int SueetieUserID { get; set; }

        [DataMember]
        public string Permalink { get; set; }

        [DataMember]
        public DateTime DateTimeCreated { get; set; }

        [DataMember]
        public bool IsRestricted { get; set; }

        [DataMember]
        public int ApplicationID { get; set; }

        [DataMember]
        public string ApplicationKey { get; set; }

        [DataMember]
        public string ApplicationDescription { get; set; }

        [DataMember]
        public string MediaObjectTitle { get; set; }

        [DataMember]
        public string AlbumTitle { get; set; }

        [DataMember]
        public int GroupID { get; set; }

        [DataMember]
        public string GroupKey { get; set; }

        [DataMember]
        public string GroupName { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public string MediaObjectUrl { get; set; }

        [DataMember]
        public int GalleryID { get; set; }

        [DataMember]
        public string GalleryName { get; set; }

        [DataMember]
        public string MediaObjectDescription { get; set; }

        [DataMember]
        public bool InDownloadReport { get; set; }

        [DataMember]
        public int ApplicationTypeID { get; set; }

        [DataMember]
        public string Abstract { get; set; }

        [DataMember]
        public string Authors { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public string Series { get; set; }

        [DataMember]
        public string DocumentType { get; set; }

        [DataMember]
        public string Keywords { get; set; }

        [DataMember]
        public string Misc { get; set; }

        [DataMember]
        public string Number { get; set; }

        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public string Organization { get; set; }

        [DataMember]
        public string Conference { get; set; }

        [DataMember]
        public string ISxN { get; set; }

        [DataMember]
        public string PublicationDate { get; set; }

        [DataMember]
        public string Publisher { get; set; }

        [DataMember]
        public bool IsAlbum { get; set; }

        [DataMember]
        public string OriginalFilename { get; set; }

        [DataMember]
        public string CreatedBy { get; set; }

        [DataMember]
        public DateTime DateAdded { get; set; }

        [DataMember]
        public string LastModifiedBy { get; set; }

        [DataMember]
        public DateTime DateLastModified { get; set; }

        [DataMember]
        public int ThumbnailWidth { get; set; }

        [DataMember]
        public int ThumbnailHeight { get; set; }

        [DataMember]
        public bool IsImage { get; set; }

        [DataMember]
        public string Tags { get; set; }
    }
}